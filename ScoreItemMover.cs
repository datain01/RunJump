using UnityEngine;

public class ScoreItemMover : MonoBehaviour
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
        // GameManager에서 속도를 가져와서 이동
        transform.position += Vector3.left * GameManager.instance.speed * Time.deltaTime;

        // 아이템이 화면 밖으로 나가면 삭제
        if (transform.position.x <= resetPosition)
        {
            Destroy(gameObject);
        }
    }

    void CalculateResetPosition()
    {
        float cameraLeftEdge = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        resetPosition = cameraLeftEdge - 1f;
    }
}
