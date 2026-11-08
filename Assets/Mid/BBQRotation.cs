
using UnityEngine;

public class BBQRotation_S10 : MonoBehaviour
{
    public float rotationSpeed = 20f;

    private float angle = 0f;
    private Quaternion initialRotation;

    void Start()
    {
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        // 시간에 따라 회전 각도 증가
        angle += rotationSpeed * Time.deltaTime;
        angle %= 360f;

        // S10: TRS 행렬 생성
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(
            Vector3.zero,
            Quaternion.Euler(angle, 0f, 0f),
            Vector3.one
        );

        // 행렬에서 회전된 축 추출
        Vector3 forward =
            rotationMatrix.GetColumn(2);

        Vector3 up =
            rotationMatrix.GetColumn(1);

        // 회전 결과를 Transform에 적용
        Quaternion rotation = Quaternion.LookRotation(
            forward,
            up
        );

        transform.localRotation = initialRotation * rotation;
    }
}
