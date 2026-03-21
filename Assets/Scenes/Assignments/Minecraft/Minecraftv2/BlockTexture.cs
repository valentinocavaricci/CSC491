using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(BoxCollider))]
public class CustomBlockUV : MonoBehaviour
{
    public Material material;

    void Start()
    {
        Mesh mesh = new Mesh();
        mesh.name = "CustomBlock";

        Vector3[] vertices = new Vector3[]
        {
           
            new Vector3(0, 0, 1),
            new Vector3(1, 0, 1),
            new Vector3(1, 1, 1),
            new Vector3(0, 1, 1),

           
            new Vector3(1, 0, 0),
            new Vector3(0, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(1, 1, 0),

           
            new Vector3(0, 0, 0),
            new Vector3(0, 0, 1),
            new Vector3(0, 1, 1),
            new Vector3(0, 1, 0),

            
            new Vector3(1, 0, 1),
            new Vector3(1, 0, 0),
            new Vector3(1, 1, 0),
            new Vector3(1, 1, 1),

           
            new Vector3(0, 1, 1),
            new Vector3(1, 1, 1),
            new Vector3(1, 1, 0),
            new Vector3(0, 1, 0),

           
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(1, 0, 1),
            new Vector3(0, 0, 1)
        };



        int[] triangles = new int[]
        {
            1, 2, 0, 2, 3, 0,
            5, 6, 4, 6, 7, 4,
            9, 10, 8, 10, 11, 8,
            13, 14, 12, 14, 15, 12,
            17, 18, 16, 18, 19, 16,
            21, 22, 20, 22, 23, 20
        };

        float tileSize = 1f / 16f;

        Vector2[] dirtUV = GetTileUV(1, 0, tileSize);
        Vector2[] grassTopUV = GetTileUV(0, 0, tileSize);

        Vector2[] uvs = new Vector2[24];

        CopyFaceUVs(uvs, 0, dirtUV);

        CopyFaceUVs(uvs, 4, dirtUV);

        CopyFaceUVs(uvs, 8, dirtUV);

        CopyFaceUVs(uvs, 12, dirtUV);

        CopyFaceUVs(uvs, 16, grassTopUV);

      
        CopyFaceUVs(uvs, 20, dirtUV);

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        MeshFilter mf = GetComponent<MeshFilter>();
        mf.mesh = mesh;

        MeshRenderer mr = GetComponent<MeshRenderer>();
        mr.material = material;

        BoxCollider bc = GetComponent<BoxCollider>();
        bc.center = new Vector3(0.5f, 0.5f, 0.5f);
        bc.size = new Vector3(1f, 1f, 1f);
    }

    Vector2[] GetTileUV(int x, int yFromTop, float tileSize)
    {
        float padding = 0.002f;

        float uMin = x * tileSize + padding;
        float uMax = (x + 1) * tileSize - padding;

        float vMax = 1f - (yFromTop * tileSize) - padding;
        float vMin = 1f - ((yFromTop + 1) * tileSize) + padding;

        return new Vector2[]
        {
            new Vector2(uMin, vMin),
            new Vector2(uMax, vMin),
            new Vector2(uMax, vMax),
            new Vector2(uMin, vMax)
        };
    }

    void CopyFaceUVs(Vector2[] allUVs, int startIndex, Vector2[] faceUVs)
    {
        allUVs[startIndex + 0] = faceUVs[0];
        allUVs[startIndex + 1] = faceUVs[1];
        allUVs[startIndex + 2] = faceUVs[2];
        allUVs[startIndex + 3] = faceUVs[3];
    }
}