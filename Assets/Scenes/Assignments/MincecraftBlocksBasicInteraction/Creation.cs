using UnityEngine;

public class Creation : MonoBehaviour
{
    public GameObject blockPrefab;

    public int width = 10;
    public int length = 10;
    public int height = 3;
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
                for (int y = 0; y < height; y++)
                {
                    Vector3 position = new Vector3(x * blockSpacing, y * blockSpacing, z * blockSpacing);
                    Instantiate(blockPrefab, position, Quaternion.identity, transform);
                }
            }
        }
    }
}