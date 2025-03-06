using UnityEngine;
using TMPro; // TextMeshPro 사용

public class BackgroundColor : MonoBehaviour
{
    private Camera mainCamera;

    [Header("Speed Ranges and Colors")]
    public Color dayColor = new Color(129f / 255f, 190f / 255f, 250f / 255f); // 81BEFA (낮 하늘색)
    public Color sunsetColor = new Color(255f / 255f, 150f / 255f, 100f / 255f); // 석양색
    public Color nightColor = new Color(10f / 255f, 10f / 255f, 40f / 255f); // 밤하늘색

    [Header("Text Color Settings")]
    public TextMeshProUGUI textScore; // ✅ 점수 텍스트 (TextMeshPro)
    public Color textDayColor = Color.black; // 기본 검은색
    public Color textNightColor = Color.white; // ✅ 밤에는 흰색으로 변경

    private void Start()
    {
        mainCamera = Camera.main;
        UpdateBackgroundColor(); // 게임 시작 시 초기 색상 설정
    }

    private void Update()
    {
        if (GameManager.instance == null) return; // GameManager가 없을 경우 예외 방지
        UpdateBackgroundColor(); // 매 프레임마다 색상 업데이트
    }

    private void UpdateBackgroundColor()
    {
        float speed = GameManager.instance.speed; // 현재 게임 속도 가져오기
        Color newBackgroundColor;
        Color newTextColor;

        if (speed < 4f)
        {
            newBackgroundColor = dayColor;
            newTextColor = textDayColor;
        }
        else if (speed < 7f)
        {
            newBackgroundColor = sunsetColor;
            newTextColor = textDayColor;
        }
        else if (speed < 9.9f)
        {
            newBackgroundColor = nightColor;
            newTextColor = textNightColor;
        } else {
            newBackgroundColor = dayColor;
            newTextColor = textDayColor;
        }

        mainCamera.backgroundColor = newBackgroundColor; // 배경색 변경
        if (textScore != null)
        {
            textScore.color = newTextColor; // ✅ TextScore 색상 변경
        }
    }
}
