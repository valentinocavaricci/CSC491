using UnityEngine;

public class Meshgen : MonoBehaviour
{
    public static Mesh mesh;

    void Awake()
    {
        if (mesh != null) return;

        mesh = new Mesh();

        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0,0,0), 
            new Vector3(1,0,0), 
            new Vector3(1,1,0), 
            new Vector3(0,1,0), 

            new Vector3(0,0,1), 
            new Vector3(1,0,1), 
            new Vector3(1,1,1), 
            new Vector3(0,1,1)  
        };

        int[] triangles = new int[]
        {
            
            0,2,1,
            0,3,2,

            
            5,6,4,
            6,7,4,

            4,7,0,
            7,3,0,

            1,2,5,
            2,6,5,

            3,7,2,
            7,6,2,

            4,0,5,
            0,1,5
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        MeshFilter mf = gameObject.AddComponent<MeshFilter>();
        mf.mesh = mesh;

        gameObject.AddComponent<MeshRenderer>();
    }
}