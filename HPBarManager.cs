using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPBarManager : MonoBehaviour
{
    public Slider hpSlider; // HP 바
    public TextMeshProUGUI hpText; // HP 텍스트 (현재 HP / 최대 HP)
    public GameObject hpBarContainer; // ✅ HP 바 전체를 감싸는 UI 오브젝트

    public void UpdateHPUI(int currentHP, int maxHP)
    {
        hpSlider.maxValue = maxHP;
        hpSlider.value = currentHP;
        hpText.text = $"{currentHP}";
    }

    public void ShowHPBar(bool isActive)
    {
        if (hpBarContainer != null)
        {
            hpBarContainer.SetActive(isActive);
        }
    }
}
