using UnityEngine;

public class BackgroundMover : MonoBehaviour
{
    private float resetPosition;
    private float startPosition;
    private Camera mainCamera;
    private float backgroundWidth;

    [SerializeField] private float speedModifier = 0f; // 인스펙터에서 조정할 수 있는 속도 가감 변수

    void Start()
    {
        mainCamera = Camera.main;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        backgroundWidth = spriteRenderer ? spriteRenderer.bounds.size.x : 10f;
        CalculatePositions();
    }

    void Update()
    {
        // GameManager에서 속도를 가져와서 speedModifier를 적용
        float moveSpeed = GameManager.instance.speed + speedModifier;
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        // 배경이 화면 밖으로 나가면 위치 리셋
        if (transform.position.x <= resetPosition)
        {
            transform.position = new Vector3(startPosition, transform.position.y, transform.position.z);
        }
    }

    void CalculatePositions()
    {
        float cameraLeftEdge = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        resetPosition = cameraLeftEdge - (backgroundWidth / 2);

        float cameraRightEdge = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        startPosition = cameraRightEdge + (backgroundWidth / 2);
    }
}
