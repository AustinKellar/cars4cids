using UnityEngine;

public class TerrainController : MonoBehaviour
{
    [Range(0.01f, 1.0f)]
    public float mixFraction = 0.7f;

    [Range(10.0f, 200.0f)]
    public int heightScale = 25;

    [Range(0.0f, 206.0f)]
    public int perlinXStart = 10;
    [Range(0.0f, 206.0f)]
    public int perlinYStart = 10;

    [Range(0.0f, 50.0f)]
    public int perlinXSpan = 20;
    [Range(0.0f, 50.0f)]
    public int perlinYSpan = 20;

    [Range(0.0f, 6.0f)]
    public int octaves = 4;

    [Range(0.0f, 1.0f)]
    public float persistence = 0.8f;

    public int edgeSize = 256;
    public int plantHeight = 40;
    public Texture2D goalTexture;
    public Transform tree1;
    public Transform tree2;
    public Transform tree3;
    public Transform tree4;
    public Transform tree5;

    private float[,] heightField;

    [SerializeField]
    private Terrain terrain;

    public void Start()
    {
        heightField = new float[edgeSize, edgeSize];
        Terrain terrain = GetComponent<Terrain>();
        GenerateTerrainGuidanceTexture(terrain.terrainData, goalTexture, mixFraction);
        AssignSplatMap(terrain.terrainData);
        generateTrees(terrain.terrainData);
        generatePlants(terrain.terrainData);
    }

    TerrainData GenerateTerrainGuidanceTexture(TerrainData terrainData, Texture2D guideTexture, float mixFraction)
    {
        terrainData.size = new Vector3(edgeSize, heightScale, edgeSize);
        terrainData.heightmapResolution = edgeSize;

        GenerateHeightGuidanceTexture(guideTexture, mixFraction);
        terrainData.SetHeights(0, 0, heightField);
        return terrainData;
    }

    void GenerateHeightGuidanceTexture(Texture2D guideTexture, float mixFraction)
    {
        for (int y = 0; y < edgeSize; y++)
        {
            for(int x = 0; x < edgeSize; x++)
            {
                heightField[y, x] = CalculateHeightGuidanceTexture(guideTexture, y, x, mixFraction);
            }
        }
    }

    float CalculateHeightGuidanceTexture(Texture2D guideTex, int y, int x, float mixFraction)
    {
        float xfrac, yfrac;
        float grayScaleVal;
        float noiseVal = 0.0f;

        xfrac = (float)x / (float)edgeSize;
        yfrac = (float)y / (float)edgeSize;
        grayScaleVal = guideTex.GetPixelBilinear(xfrac, yfrac).grayscale;

        noiseVal = CalculateHeightOctaves(y, x);

        return (grayScaleVal * mixFraction) + noiseVal * (1 - mixFraction);

    }

    void AssignSplatMap(TerrainData terrainData)
    {
        float[,,] splatmapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];
        for (int y = 0; y < terrainData.alphamapHeight; y++)
        {
            for (int x = 0; x < terrainData.alphamapWidth; x++)
            {
                float normX = x * 1.0f / (terrainData.alphamapWidth - 1);
                float normY = y * 1.0f / (terrainData.alphamapHeight - 1);

                var angle = terrainData.GetSteepness(normY, normX);
                var frac = angle / 90;
                var height = terrainData.GetHeight(Mathf.RoundToInt(normY * terrainData.heightmapHeight), Mathf.RoundToInt(normX * terrainData.heightmapWidth));
                if (height >= plantHeight - 12 && height <= plantHeight && frac < .7)
                {
                    splatmapData[x, y, 2] = (float)1;
                } else
                {
                    splatmapData[x, y, 0] = (float)(1 - frac);
                    splatmapData[x, y, 1] = (float)(frac);
                }
            }
        }
        terrainData.SetAlphamaps(0, 0, splatmapData);
    }

    void generateTrees(TerrainData terrainData)
    {
        for (int y = 0; y < terrainData.alphamapHeight; y += 12)
        {
            for (int x = 0; x < terrainData.alphamapWidth; x += 12)
            {
                float normX = x * 1.0f / (terrainData.alphamapWidth - 1);
                float normY = y * 1.0f / (terrainData.alphamapHeight - 1);

                var angle = terrainData.GetSteepness(normY, normX);
                var frac = angle / 90;
                var height = terrainData.GetHeight(Mathf.RoundToInt(normY * terrainData.heightmapHeight), Mathf.RoundToInt(normX * terrainData.heightmapWidth));
                if (height >= 160)
                {
                    if (frac > .1)
                    {
                        var random = (Random.Range(1.0f, 30.0f) / 100.0f);
                        var spacing = Random.Range(1.0f, 20.0f);
                        var treePick = (int)Random.Range(0.0f, 3.0f);
                        if (random > .1f)
                        {
                            if (treePick == 0)
                            {
                                Instantiate(tree1, new Vector3((y * 2) + spacing, height, (x * 2) + spacing), tree1.rotation);
                            }
                            if (treePick == 1)
                            {
                                Instantiate(tree2, new Vector3((y * 2) + spacing, height, (x * 2) + spacing), tree2.rotation);
                            }
                            if (treePick == 2)
                            {
                                Instantiate(tree3, new Vector3((y * 2) + spacing, height, (x * 2) + spacing), tree3.rotation);
                            }
                        }
                    }
                }
            }
        }
    }

    void generatePlants(TerrainData terrainData)
    {
        for (int y = 0; y < terrainData.alphamapHeight; y += 7)
        {
            for (int x = 0; x < terrainData.alphamapWidth; x += 7)
            {
                float normX = x * 1.0f / (terrainData.alphamapWidth - 1);
                float normY = y * 1.0f / (terrainData.alphamapHeight - 1);

                var angle = terrainData.GetSteepness(normY, normX);
                var frac = angle / 90;
                var height = terrainData.GetHeight(Mathf.RoundToInt(normY * terrainData.heightmapHeight), Mathf.RoundToInt(normX * terrainData.heightmapWidth));
                if (height >= plantHeight - 12 && height <= plantHeight && frac < .5) 
                {
                    if (frac > .1)
                    {
                        var random = (Random.Range(1.0f, 30.0f) / 100.0f);
                        var spacing = Random.Range(1.0f, 10.0f);
                        var treePick = (int)Random.Range(0.0f, 3.0f);
                        if (random > .1f)
                        {
                            if (treePick == 0)
                            {
                                Instantiate(tree4, new Vector3((y * 2) + spacing, height, (x * 2) + spacing), tree4.rotation);
                            }
                            if (treePick == 1)
                            {
                                Instantiate(tree5, new Vector3((y * 2) + spacing, height, (x * 2) + spacing), tree5.rotation);
                            }
                        }
                    }
                }
            }
        }
    }

    float CalculateHeightOctaves(float x, float y)
    {
        float noiseVal = 0.0f;
        float frequency = 1.0f;
        float amplitude = 1.0f;
        float maxValue = 0.0f;

        for (int i = 0; i < octaves; i++)
        {
            float perlinX = perlinXStart + ((float)x / (float)edgeSize) * perlinXSpan * frequency;
            float perlinY = perlinYStart + ((float)y / (float)edgeSize) * perlinYSpan * frequency;

            noiseVal += Mathf.PerlinNoise(perlinX, perlinY) * amplitude;

            maxValue += amplitude;
            amplitude *= persistence;
            frequency *= 2;
        }

        return noiseVal / maxValue;
    }
}
