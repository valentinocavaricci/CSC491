using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public Material blockMaterial;

    public int width = 10;
    public int depth = 10;

    void Start()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                SpawnBlock(new Vector3(x, 0, z));
            }
        }
    }

    void SpawnBlock(Vector3 position)
    {
        GameObject block = new GameObject("Block");

        block.transform.position = position;

        MeshFilter mf = block.AddComponent<MeshFilter>();
        mf.mesh = Meshgen.mesh;

        MeshRenderer mr = block.AddComponent<MeshRenderer>();
        mr.material = blockMaterial;

        block.AddComponent<BoxCollider>();
    }
}