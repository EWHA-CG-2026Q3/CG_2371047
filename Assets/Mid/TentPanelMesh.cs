
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TentPanelMesh : MonoBehaviour
{
    void Start()
    {
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(-1f, 0f, 0f), // 0 왼쪽 아래
            new Vector3(-1f, 2f, 0f), // 1 왼쪽 위
            new Vector3( 1f, 2f, 0f), // 2 오른쪽 위
            new Vector3( 1f, 0f, 0f)  // 3 오른쪽 아래
        };

        int[] triangles = new int[]
        {
            0, 1, 2,
            0, 2, 3
        };

        Mesh mesh = new Mesh();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
    }
}
