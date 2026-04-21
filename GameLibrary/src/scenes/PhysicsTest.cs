using System.Numerics;
using Box2D.NET;
using Ldtk;
using static Box2D.NET.B2Joints;
using static Box2D.NET.B2Ids;
using static Box2D.NET.B2Types;
using static Box2D.NET.B2MathFunction;
using static Box2D.NET.B2Bodies;
using static Box2D.NET.B2Shapes;
using static Box2D.NET.B2Worlds;
using static Box2D.NET.B2Geometries;
using static Box2D.NET.B2Diagnostics;
using Raylib_cs;

namespace GameLibrary;

public class PhysicsTest : Scene
{
    private B2WorldId WorldId;
    private static B2DebugDraw _debugDraw = Box2dDebugDrawRaylib.Create();
    private Texture2D _ballTex;
    float timeStep = 1.0f / 60.0f;
    int subStepCount = 4;
    private Tilemap _tilemap;
    
    private List<B2BodyId> _balls = new List<B2BodyId>();
    
    public PhysicsTest(Tilemap tilemap)
    {
        _tilemap = tilemap;
        _ballTex = Resources.Sprites["woman"];
        
        B2WorldDef worldDef = b2DefaultWorldDef();
        
        worldDef.gravity = new B2Vec2(0.0f, 100.0f);
        WorldId = b2CreateWorld(worldDef);
        
        foreach (var tile in tilemap.LdtkJson.Levels[0].LayerInstances[0].AutoLayerTiles)
        {
            B2BodyDef groundBodyDef = b2DefaultBodyDef();
            groundBodyDef.position = new B2Vec2(tile.Px[0]+8, tile.Px[1]+8);
        
            B2BodyId groundId = b2CreateBody(WorldId, groundBodyDef);
        
            B2Polygon groundBox = b2MakeBox(8.0f, 8.0f);
        
            B2ShapeDef groundShapeDef = b2DefaultShapeDef();
            b2CreatePolygonShape(groundId, groundShapeDef, groundBox);
        }
    }
    
    public void ClearBalls()
    {
        foreach (B2BodyId ball in _balls)
        {
            b2DestroyBody(ball);
        }
        _balls.Clear();
    }
    
    public override void Update()
    {
        Raylib.ClearBackground(Color.Black);

        _tilemap.Draw();
        
        if (Raylib.IsMouseButtonPressed(0) || Raylib.IsKeyDown(KeyboardKey.Space))
        {
            B2BodyDef bodyDef = b2DefaultBodyDef();
            bodyDef.type  = B2BodyType.b2_dynamicBody;
            bodyDef.position = new B2Vec2(0, 4);
            B2BodyId bodyId = b2CreateBody(WorldId, bodyDef);
        
            B2Circle dynamicCircle;
            dynamicCircle.center = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), Game.GetActiveCamera()).ToB2();
            dynamicCircle.radius = 6;
        
            B2ShapeDef shapeDef = b2DefaultShapeDef();
            shapeDef.density = 1.0f;
            shapeDef.material.friction = 0.3f;
            shapeDef.material.restitution = 0.75f;
        
            b2CreateCircleShape(bodyId, shapeDef, dynamicCircle);
            _balls.Add(bodyId);
        }
        
        b2World_Step(WorldId, timeStep, subStepCount);
        
        foreach (B2BodyId ball in _balls)
        {
            B2Vec2 pos = b2Body_GetWorldCenterOfMass(ball);
            float rot = b2Rot_GetAngle(b2Body_GetRotation(ball)) * 180 / MathF.PI;
            Raylib.DrawTexturePro(_ballTex, new Rectangle(0, 0, _ballTex.Dimensions), new Rectangle(pos.ToVec2(), _ballTex.Dimensions/2), _ballTex.Dimensions/4, rot, Color.LightGray);
        }
        
        if (Raylib.IsKeyDown(KeyboardKey.V))
            b2World_Draw(WorldId, _debugDraw);
        
        Raylib.EndMode2D();

        if (ImGui.Button("Clear", 0, 0))
        {
            ClearBalls();
        }

        if (Raylib.IsKeyDown(KeyboardKey.Escape)) Game.ActiveScene = new MainMenu();

    }
    
    public void Destroy()
    {
        b2DestroyWorld(WorldId);
    }
    
    static class Box2dDebugDrawRaylib
    {
        public static B2DebugDraw Create()
        {
            var debugDraw = b2DefaultDebugDraw();
            debugDraw.DrawPolygonFcn = DrawPolygon;
            debugDraw.DrawSolidPolygonFcn = DrawSolidPolygon;
            debugDraw.DrawCircleFcn = DrawCircle;
            debugDraw.DrawSolidCircleFcn = DrawSolidCircle;
            debugDraw.DrawSolidCapsuleFcn = DrawSolidCapsule;
            debugDraw.drawLineFcn = DrawSegment;
            debugDraw.DrawTransformFcn = DrawTransform;
            debugDraw.DrawPointFcn = DrawPoint;
            debugDraw.DrawStringFcn = DrawString;

            debugDraw.drawShapes = true;
            debugDraw.drawJoints = true;
            debugDraw.drawJointExtras = true;
            debugDraw.drawBounds = true;
            debugDraw.drawMass = true;
            debugDraw.drawBodyNames = true;
            debugDraw.drawGraphColors = true;
            debugDraw.drawContactNormals = true;
            // debugDraw.drawContactForces = true;
            // debugDraw.drawContactFeatures = true;
            // debugDraw.drawFrictionForces = true;
            debugDraw.drawIslands = true;

            return debugDraw;
        }
        
        private static void DrawPolygon(ReadOnlySpan<B2Vec2> vertices, int vertexCount, B2HexColor color, object o)
        {
            for (int i = 0; i < vertexCount; i++)
            {
                Raylib.DrawLineV(vertices[i].ToVec2(), vertices[(i + 1) % vertexCount].ToVec2(), color.ToRaylib());
            }
        }
        
        private static void DrawSolidPolygon(in B2Transform transform, ReadOnlySpan<B2Vec2> vertices, int vertexCount, float radius, B2HexColor color, object o)
        {
            Vector2[] verts = vertices.ToArray().Select(v => v.ToVec2()).ToArray();

            for (var i = 0; i < verts.Length; i++)
            {
                verts[i] = new Vector2(verts[i].X * transform.q.c - verts[i].Y * transform.q.s, verts[i].X * transform.q.s + verts[i].Y * transform.q.c);
                verts[i] += transform.p.ToVec2();
            }
            for (int i = 0; i < vertexCount; i++)
            {
                Raylib.DrawLineV(verts[i], verts[(i + 1) % vertexCount], color.ToRaylib());
            }
        }
        
        private static void DrawCircle(in B2Vec2 center, float radius, B2HexColor color, object o)
        {
            Raylib.DrawCircleLinesV(center.ToVec2(), radius, color.ToRaylib());
        }
        
        private static void DrawSolidCircle(in B2Transform transform, float radius, B2HexColor color, object o)
        {
            Vector2 center = transform.p.ToVec2();
            Raylib.DrawCircleV(center, radius, Raylib.ColorAlpha(color.ToRaylib(), 0.25f));
            Raylib.DrawCircleLinesV(center, radius, color.ToRaylib());
            Raylib.DrawLineV(center, center + new Vector2(transform.q.c, transform.q.s) * radius, color.ToRaylib());
        }
        
        private static void DrawSolidCapsule(in B2Vec2 point1, in B2Vec2 point2, float radius, B2HexColor color, object o)
        {
            Vector2 p1 = point1.ToVec2();
            Vector2 p2 = point2.ToVec2();
            Raylib.DrawCircleV(p1, radius, color.ToRaylib());
            Raylib.DrawCircleV(p2, radius, color.ToRaylib());
            Raylib.DrawLineEx(p1, p2, radius, color.ToRaylib());
        }
        
        private static void DrawSegment(in B2Vec2 point1, in B2Vec2 point2, B2HexColor color, object o)
        {
            Vector2 p1 = point1.ToVec2();
            Vector2 p2 = point2.ToVec2();
            Raylib.DrawLineV(p1, p2, color.ToRaylib());
        }
        
        private static void DrawTransform(in B2Transform transform, object o)
        {
            DrawSolidCircle(transform, 4, B2HexColor.b2_colorAqua, o);
        }
        
        private static void DrawPoint(in B2Vec2 point, float size, B2HexColor color, object o)
        {
            Raylib.DrawCircleV(point.ToVec2(), size, color.ToRaylib());
        }
        
        private static void DrawString(in B2Vec2 point, string s, B2HexColor color, object o)
        {
            Vector2 p = point.ToVec2();
            Raylib.DrawText(s, (int)p.X, (int)p.Y, 4, color.ToRaylib());
        }
    }
}