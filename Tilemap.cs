using System.Numerics;
using ldtk;
using Raylib_cs;

public class Tilemap
{
    public LdtkJson LdtkJson;
    public Texture2D[] TileTextures;
    
    public Tilemap(string jsonPath)
    {
        LdtkJson = LdtkJson.FromJson(File.ReadAllText(Directory.GetCurrentDirectory() + "/resource/" + jsonPath));

        // TileTextures = new Texture2D[LdtkJson.Levels[0].LayerInstances.Length];
        // for (int i = 0; i < LdtkJson.Levels[0].LayerInstances.Length; i++)
        // {
        //     string name = Path.GetFileName(LdtkJson.Levels[0].LayerInstances[i].TilesetRelPath);
        //     TileTextures[i] = Raylib.LoadTexture(Directory.GetCurrentDirectory() + "/resource/" + name);
        // }
        
        // This is the same as commented above, but funnier for it's abuse of C# conveniences
        TileTextures = LdtkJson.Levels[0].LayerInstances.Select(e => { return Raylib.LoadTexture(Directory.GetCurrentDirectory() + "/resource/" + Path.GetFileName(e.TilesetRelPath)); }).ToArray();
    }

    public void Draw()
    {
        for (int i = LdtkJson.Levels[0].LayerInstances.Length-1; i >= 0; i--) // sneaky reverse for loop!
        {
            foreach (TileInstance? tile in LdtkJson.Levels[0].LayerInstances[i].AutoLayerTiles)
            {
                int tID = (int)tile.T;
                int columns = TileTextures[i].Width / 16;
                Raylib.DrawTextureRec(TileTextures[i], new Rectangle((tID % columns) * 16, (tID / columns) * 16, 16, 16), new Vector2((int)tile.Px[0], (int)tile.Px[1]), Color.White);
            }
        }
    }

    ~Tilemap() // destructor, prevents gpu memory leak if game loads levels more than once.
    {
        foreach (Texture2D tex in TileTextures)
        {
            Raylib.UnloadTexture(tex);
        }
    }
}