using UnityEngine;

public class MaxHPItemMover : MonoBehaviour
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
        transform.position += Vector3.left * GameManager.instance.speed * Time.deltaTime;

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
