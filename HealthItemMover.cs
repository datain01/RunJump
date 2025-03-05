using UnityEngine;

public class HealthItemMover : MonoBehaviour
{
    private float resetPosition;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        CalculateResetPosition();
    }

    void Update()
    {
        // GameManager에서 속도를 가져와서 이동 (장애물과 동일한 속도)
        transform.position += Vector3.left * GameManager.instance.speed * Time.deltaTime;

        // 화면 밖으로 나가면 아이템 삭제
        if (transform.position.x <= resetPosition)
        {
            Destroy(gameObject);
        }
    }

    void CalculateResetPosition()
    {
        float cameraLeftEdge = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        resetPosition = cameraLeftEdge - 1f; // 장애물과 동일한 삭제 기준 적용
    }
}
