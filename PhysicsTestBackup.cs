// using System.Numerics;
// using System.Runtime.InteropServices;
// using Box2dNet;
// using Box2dNet.Interop;
// using Ldtk;
// using Raylib_cs;
// using Raylib_Game_CS_;
// using static Box2dNet.Interop.B2Api;
//
// namespace Raylib_Game_C_;
//
// public class PhysicsTest
// {
//     private b2WorldId WorldId;
//     private static b2DebugDraw _debugDraw = Box2dDebugDrawRaylib.Create();
//     private Texture2D _ballTex;
//     float timeStep = 1.0f / 60.0f;
//     int subStepCount = 4;
//     
//     private List<b2BodyId> _balls = new List<b2BodyId>();
//     
//     public PhysicsTest(Tilemap tilemap)
//     {
//         _ballTex = Raylib.LoadTexture(Directory.GetCurrentDirectory() + "/resource/woman.png");
//         
//         b2WorldDef worldDef = b2DefaultWorldDef();
//         
//         worldDef.gravity = new Vector2(0.0f, 100.0f);
//         WorldId = b2CreateWorld(worldDef);
//         
//         foreach (var tile in tilemap.LdtkJson.Levels[0].LayerInstances[0].AutoLayerTiles)
//         {
//             b2BodyDef groundBodyDef = b2DefaultBodyDef();
//             groundBodyDef.position = new Vector2(tile.Px[0]+8, tile.Px[1]+8);
//         
//             b2BodyId groundId = b2CreateBody(WorldId, groundBodyDef);
//         
//             b2Polygon groundBox = b2MakeBox(8.0f, 8.0f);
//         
//             b2ShapeDef groundShapeDef = b2DefaultShapeDef();
//             b2CreatePolygonShape(groundId, groundShapeDef, groundBox);
//         }
//         
//         // b2BodyDef bodyDef = b2DefaultBodyDef();
//         // bodyDef.type  = b2BodyType.b2_dynamicBody;
//         // bodyDef.position = new Vector2(0, 4);
//         // BodyId = b2CreateBody(WorldId, bodyDef);
//         //
//         // b2Polygon dynamicBox = b2MakeBox(30.0f, 30.0f);
//         //
//         // b2ShapeDef shapeDef = b2DefaultShapeDef();
//         // shapeDef.density = 1.0f;
//         // shapeDef.material.friction = 0.3f;
//         // shapeDef.material.restitution = 0.75f;
//         //
//         // b2CreatePolygonShape(BodyId, shapeDef, dynamicBox);
//     }
//     
//     public void Step()
//     {
//         if (Raylib.IsMouseButtonPressed(0) || Raylib.IsKeyDown(KeyboardKey.Space))
//         {
//             b2BodyDef bodyDef = b2DefaultBodyDef();
//             bodyDef.type  = b2BodyType.b2_dynamicBody;
//             bodyDef.position = new Vector2(0, 4);
//             b2BodyId bodyId = b2CreateBody(WorldId, bodyDef);
//         
//             b2Circle dynamicCircle;
//             dynamicCircle.center = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), Game.Camera2D);
//             dynamicCircle.radius = 6;
//         
//             b2ShapeDef shapeDef = b2DefaultShapeDef();
//             shapeDef.density = 1.0f;
//             shapeDef.material.friction = 0.3f;
//             shapeDef.material.restitution = 0.75f;
//         
//             b2CreateCircleShape(bodyId, shapeDef, dynamicCircle);
//             _balls.Add(bodyId);
//         }
//         
//         b2World_Step(WorldId, timeStep, subStepCount);
//         
//         foreach (b2BodyId ball in _balls)
//         {
//             Vector2 pos = b2Body_GetWorldCenterOfMass(ball);
//             float rot = b2Body_GetRotation(ball).GetAngle() * 180 / MathF.PI;
//             Raylib.DrawTexturePro(_ballTex, new Rectangle(0, 0, _ballTex.Dimensions), new Rectangle(pos, _ballTex.Dimensions/2), _ballTex.Dimensions/4, rot, Color.LightGray);
//         }
//         
//         if (!Raylib.IsKeyDown(KeyboardKey.V)) 
//             b2World_Draw(WorldId, ref _debugDraw);
//     }
//     
//     public void Destroy()
//     {
//         b2DestroyWorld(WorldId);
//     }
//     
//     static class Box2dDebugDrawRaylib
//     {
//         public static b2DebugDraw Create()
//         {
//             var debugDraw = b2DefaultDebugDraw();
//             debugDraw.DrawPolygon = Marshal.GetFunctionPointerForDelegate((DrawPolygon)DrawPolygon);
//             debugDraw.DrawSolidPolygon = Marshal.GetFunctionPointerForDelegate((DrawSolidPolygon)DrawSolidPolygon);
//             debugDraw.DrawCircle = Marshal.GetFunctionPointerForDelegate((DrawCircle)DrawCircle);
//             debugDraw.DrawSolidCircle = Marshal.GetFunctionPointerForDelegate((DrawSolidCircle)DrawSolidCircle);
//             debugDraw.DrawSolidCapsule = Marshal.GetFunctionPointerForDelegate((DrawSolidCapsule)DrawSolidCapsule);
//             debugDraw.DrawSegment = Marshal.GetFunctionPointerForDelegate((DrawSegment)DrawSegment);
//             debugDraw.DrawTransform = Marshal.GetFunctionPointerForDelegate((DrawTransform)DrawTransform);
//             debugDraw.DrawPoint = Marshal.GetFunctionPointerForDelegate((DrawPoint)DrawPoint);
//             debugDraw.DrawString = Marshal.GetFunctionPointerForDelegate((DrawString)DrawString);
//
//             debugDraw.drawShapes = true;
//             // debugDraw.drawJoints = true;
//             // debugDraw.drawJointExtras = true;
//             debugDraw.drawBounds = true;
//             // debugDraw.drawMass = true;
//             // debugDraw.drawBodyNames = true;
//             // debugDraw.contactDrawType = b2ContactDrawType.b2_drawContacts_Clip;
//             // debugDraw.drawGraphColors = true;
//             // debugDraw.drawContactNormals = true;
//             // debugDraw.drawContactForces = true;
//             // debugDraw.drawContactFeatures = true;
//             // debugDraw.drawFrictionForces = true;
//             debugDraw.drawIslands = true;
//
//             return debugDraw;
//         }
//         
//         private static void DrawPolygon(IntPtr vertices, int vertexCount, b2HexColor color, IntPtr context)
//         {
//             Vector2[] verts = vertices.NativeArrayToArray<Vector2>(vertexCount);
//             for (int i = 0; i < vertexCount; i++)
//             {
//                 Raylib.DrawLineV(verts[i], verts[(i + 1) % vertexCount], Color.Red);
//             }
//         }
//         
//         private static void DrawSolidPolygon(b2Transform transform, IntPtr vertices, int vertexCount, float radius, b2HexColor color, IntPtr context)
//         {
//             Vector2[] verts = vertices.NativeArrayToArray<Vector2>(vertexCount);
//             for (var i = 0; i < verts.Length; i++)
//             {
//                 verts[i] = new Vector2(verts[i].X * transform.q.c - verts[i].Y * transform.q.s, verts[i].X * transform.q.s + verts[i].Y * transform.q.c);
//                 verts[i] += transform.p;
//             }
//             for (int i = 0; i < vertexCount; i++)
//             {
//                 Raylib.DrawLineV(verts[i], verts[(i + 1) % vertexCount], Color.Red);
//             }
//         }
//         
//         private static void DrawCircle(Vector2 center, float radius, b2HexColor color, IntPtr context)
//         {
//             Raylib.DrawCircleLinesV(center, radius, Color.Red);
//         }
//         
//         private static void DrawSolidCircle(b2Transform transform, float radius, b2HexColor color, IntPtr context)
//         {
//             Raylib.DrawCircleV(transform.p, radius, Raylib.ColorAlpha(color.ToRaylib(), 0.25f));
//             Raylib.DrawCircleLinesV(transform.p, radius, color.ToRaylib());
//             Raylib.DrawLineV(transform.p, transform.p + new Vector2(transform.q.c, transform.q.s) * radius, color.ToRaylib());
//         }
//         
//         private static void DrawSolidCapsule(Vector2 p1, Vector2 p2, float radius, b2HexColor color, IntPtr context)
//         {
//             Raylib.DrawCircleV(p1, radius, color.ToRaylib());
//             Raylib.DrawCircleV(p2, radius, color.ToRaylib());
//             Raylib.DrawLineEx(p1, p2, radius, color.ToRaylib());
//         }
//         
//         private static void DrawSegment(Vector2 p1, Vector2 p2, b2HexColor color, IntPtr context)
//         {
//             Raylib.DrawLineV(p1, p2, color.ToRaylib());
//         }
//         
//         private static void DrawTransform(b2Transform transform, IntPtr context)
//         {
//             DrawSolidCircle(transform, 4, b2HexColor.b2_colorAqua, context);
//         }
//         
//         private static void DrawPoint(Vector2 p, float size, b2HexColor color, IntPtr context)
//         {
//             Raylib.DrawCircleV(p, size, color.ToRaylib());
//         }
//         
//         private static void DrawString(Vector2 p, IntPtr s, b2HexColor color, IntPtr context)
//         {
//             Raylib.DrawText(Marshal.PtrToStringAnsi(s), (int)p.X, (int)p.Y, 4, color.ToRaylib());
//         }
//     }
// }