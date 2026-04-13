using UnityEngine;

public class Creation : MonoBehaviour
{
    public GameObject blockPrefab;

    public int width = 40;
    public int length = 40;
    public int maxHeight = 6;
    public float noiseScale = 0.08f;
    public float blockSpacing = 1f;

    void Start()
    {
        GenerateWorld();
    }



    void GenerateWorld()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < length; z++)
            {
                float noise = Mathf.PerlinNoise(x * noiseScale, z * noiseScale);
                int height = Mathf.FloorToInt(noise * maxHeight) + 1;

                for (int y = 0; y < height; y++)
                {
                    Vector3 position = new Vector3(
                        x * blockSpacing,
                        y * blockSpacing,
                        z * blockSpacing
                    );

                    GameObject block = Instantiate(blockPrefab, position, Quaternion.identity, transform);

                    CustomBlockUV blockUV = block.GetComponent<CustomBlockUV>();

                    if (blockUV != null)
                    {
                        if (y <= 1)
                        {
                            blockUV.Build(CustomBlockUV.BlockType.Stone);
                        }
                        else if (y == height - 1)
                        {
                            blockUV.Build(CustomBlockUV.BlockType.Grass);
                        }
                        else
                        {
                            blockUV.Build(CustomBlockUV.BlockType.Dirt);
                        }
                    }
                }
            }
        }
    }
}