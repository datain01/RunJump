using UnityEngine;

public class BackgroundMover3 : MonoBehaviour
{
    private float backgroundWidth; 
    private Camera mainCamera;
    private Transform[] backgrounds;

    private float speedFactor = 1f / 3f; // ✅ 배경 이동 속도 조절 비율 (기본값 0.5, 필요시 조정)

    void Start()
    {
        mainCamera = Camera.main;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        backgroundWidth = spriteRenderer ? spriteRenderer.bounds.size.x : 10f;

        // 같은 태그를 가진 배경 오브젝트들을 찾아서 정렬
        backgrounds = new Transform[3];  
        for (int i = 0; i < 3; i++)
        {
            backgrounds[i] = GameObject.Find($"Background_{i}").transform; // "Background_0", "Background_1", "Background_2"
        }

        // 정확한 위치로 배경 배치
        ArrangeBackgrounds();
    }

    void Update()
    {
        float moveSpeed = GameManager.instance.speed * speedFactor; // ✅ 속도 조정
        foreach (var bg in backgrounds)
        {
            bg.position += Vector3.left * moveSpeed * Time.deltaTime;
        }

        // 배경 위치 재정렬
        CheckAndReposition();
    }

    void ArrangeBackgrounds()
    {
        float startX = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].position = new Vector3(startX + i * backgroundWidth, backgrounds[i].position.y, backgrounds[i].position.z);
        }
    }

    void CheckAndReposition()
    {
        foreach (var bg in backgrounds)
        {
            float cameraLeftEdge = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
            if (bg.position.x + backgroundWidth / 2 < cameraLeftEdge)
            {
                Transform lastBg = GetRightmostBackground();
                bg.position = new Vector3(lastBg.position.x + backgroundWidth, bg.position.y, bg.position.z);
            }
        }
    }

    Transform GetRightmostBackground()
    {
        Transform rightmost = backgrounds[0];
        foreach (var bg in backgrounds)
        {
            if (bg.position.x > rightmost.position.x)
            {
                rightmost = bg;
            }
        }
        return rightmost;
    }
}
