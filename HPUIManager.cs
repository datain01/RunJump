using UnityEngine;
using UnityEngine.UI;

public class HPUIManager : MonoBehaviour
{
    public static HPUIManager instance;

    public Image[] heartImages; // 하트 이미지 배열
    public Sprite fullHeart; // 정상 하트 이미지
    public Sprite brokenHeart; // 깨진 하트 이미지

    // HP UI 업데이트
    public void UpdateHPUI(int hp)
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i < hp)
                heartImages[i].sprite = fullHeart; // 정상 하트
            else
                heartImages[i].sprite = brokenHeart; // 깨진 하트
        }
    }
}
