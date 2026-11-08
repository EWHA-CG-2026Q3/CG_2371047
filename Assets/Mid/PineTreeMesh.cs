using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PineTreeMesh : MonoBehaviour
{
    void Start()
    {
        // 원뿔 밑면을 구성하는 정점 개수
        int n = 8;

        // 나뭇잎 3단의 크기
        float[] radii = { 1.2f, 0.9f, 0.6f };
        float[] heights = { 1.5f, 1.3f, 1.1f };
        float[] baseY = { 1.2f, 2.0f, 2.8f };

        // 각 원뿔: 밑면 n개 + 꼭대기 1개 + 밑면 중심 1개
        Vector3[] vertices = new Vector3[3 * (n + 2)];

        // 각 원뿔: 옆면 n개 + 밑면 n개
        // 삼각형 하나당 인덱스 3개
        int[] triangles = new int[3 * n * 6];

        for (int layer = 0; layer < 3; layer++)
        {
            int vertexStart = layer * (n + 2);
            int triangleStart = layer * n * 6;

            float radius = radii[layer];
            float height = heights[layer];
            float y = baseY[layer];

            // 1. 원뿔 밑면의 정점 생성
            for (int i = 0; i < n; i++)
            {
                float angle = 2f * Mathf.PI * i / n;

                float x = radius * Mathf.Cos(angle);
                float z = radius * Mathf.Sin(angle);

                vertices[vertexStart + i] =
                    new Vector3(x, y, z);
            }

            // 2. 원뿔 꼭대기 정점
            int top = vertexStart + n;

            vertices[top] =
                new Vector3(0f, y + height, 0f);

            // 3. 원뿔 밑면 중심 정점
            int center = vertexStart + n + 1;

            vertices[center] =
                new Vector3(0f, y, 0f);

            // 4. 삼각형 인덱스 생성
            for (int i = 0; i < n; i++)
            {
                int current = vertexStart + i;
                int next = vertexStart + (i + 1) % n;

                int t = triangleStart + i * 6;

                // 옆면 삼각형
                triangles[t] = current;
                triangles[t + 1] = top;
                triangles[t + 2] = next;

                // 밑면 삼각형
                triangles[t + 3] = center;
                triangles[t + 4] = current;
                triangles[t + 5] = next;
            }
        }

        // S03, S04에서 사용했던 Mesh 생성 코드
        Mesh mesh = new Mesh();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
    }
}
