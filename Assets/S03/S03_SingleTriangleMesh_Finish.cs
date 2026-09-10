using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class S03_SingleTriangleMesh_Finish : MonoBehaviour
{
    void Start()
    {
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0f, 0f, 0f), // 0
            new Vector3(1f, 0f, 0f), // 1
            new Vector3(1f, 0.5f, 1f), // 2
            new Vector3(0.5f, -0.5f, 1f), // 3
            new Vector3(0f, 0f, 1f), // 4
            new Vector3(1f, 0f, 1f), // 5
            new Vector3(0.5f, 1f, 1f), // 6
            new Vector3(0f, 0.5f, 1f), // 7
        };

        int[] triangles = new int[] { 4, 5, 6,
                                      7, 3, 2};

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
    }
}