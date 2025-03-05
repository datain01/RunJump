using UnityEngine;

public class ButtonSettings : MonoBehaviour
{
    public GameObject panelSettings; // 🔹 설정 패널 (Inspector에서 할당)

    void Start()
    {
        if (panelSettings != null)
        {
            panelSettings.SetActive(false); // 🔹 게임 시작 시 패널 비활성화
        }
    }

    public void ToggleSettings()
    {
        if (panelSettings != null)
        {
            bool isActive = panelSettings.activeSelf;
            panelSettings.SetActive(!isActive); // 🔹 설정 패널 ON/OFF 전환
        }
    }
}
