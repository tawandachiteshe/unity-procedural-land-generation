using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public enum DrawMode { NoiseMap, ColorMap, Mesh }

    public DrawMode drawMode;
    public int mapWidth;
    public int mapHeight;

    public int seed;
    public float noiseScale;
    public int octaves;

    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public Vector2 offset;
    public float meshHeightMultiplier;

    public AnimationCurve meshHeightCurve;

    public TerrainType[] regions;
    public bool autoUpdate;

    public void GenerateMap() {


            float[,] noiseMap = Noise.GenerateMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);

            Color[] colorMap = new Color[mapWidth * mapHeight];

            for (int y = 0; y < mapWidth; y++)
            {
                for (int x = 0; x < mapHeight; x++)
                {
                    float currentHeight = noiseMap[x, y];

                    for (int i = 0; i < regions.Length; i++)
                    {
                        if(currentHeight <= regions[i].Height)
                        {
                            colorMap[y * mapWidth + x] = regions[i].color;
                            break;
                        }
                        
                    }
                }
            }

            MapDisplay display = FindObjectOfType<MapDisplay>();

            if(drawMode == DrawMode.NoiseMap)
                display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
            else if(drawMode == DrawMode.ColorMap)
                display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));
            else if(drawMode == DrawMode.Mesh)
                display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve), TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));

            

    }

    void OnValidate() {

    
        if(mapWidth < 1)
            mapWidth = 1;
        
        if(mapHeight < 1)
            mapHeight = 1;

        if(lacunarity < 1)
            lacunarity = 1;
        
        if(octaves < 0)
            octaves = 1;

    }
}

[System.Serializable]
public struct TerrainType {

    public string name;
    public float Height;
    public Color color;

}
