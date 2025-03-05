using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public static SettingManager instance;

    public Button bgmMuteButton;
    public Button sfxMuteButton;
    public Button vibrationButton; // 🔹 진동 설정 버튼
    public Sprite muteOnSprite;
    public Sprite muteOffSprite;
    public Sprite vibrationOnSprite;
    public Sprite vibrationOffSprite;

    private bool isBGMMuted;
    private bool isSFXMuted;
    private bool isVibrationOn;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // 🔹 설정 값 불러오기
        isBGMMuted = PlayerPrefs.GetInt("BGMMute", 0) == 1;
        isSFXMuted = PlayerPrefs.GetInt("SFXMute", 0) == 1;
        isVibrationOn = PlayerPrefs.GetInt("Vibration", 1) == 1; // 기본값: ON

        ApplyAudioSettings();
        UpdateButtonSprites();
    }

    public void ToggleBGMMute()
    {
        isBGMMuted = !isBGMMuted;
        PlayerPrefs.SetInt("BGMMute", isBGMMuted ? 1 : 0);
        PlayerPrefs.Save();

        ApplyAudioSettings();
        UpdateButtonSprites();
    }

    public void ToggleSFXMute()
    {
        isSFXMuted = !isSFXMuted;
        PlayerPrefs.SetInt("SFXMute", isSFXMuted ? 1 : 0);
        PlayerPrefs.Save();

        ApplyAudioSettings();
        UpdateButtonSprites();
    }

    public void ToggleVibration()
    {
        isVibrationOn = !isVibrationOn;
        PlayerPrefs.SetInt("Vibration", isVibrationOn ? 1 : 0);
        PlayerPrefs.Save();
        UpdateButtonSprites();
    }

    void ApplyAudioSettings()
    {
        GameObject bgmObject = GameObject.FindWithTag("BGM");
        if (bgmObject != null)
        {
            AudioSource bgmAudio = bgmObject.GetComponent<AudioSource>();
            if (bgmAudio != null)
            {
                bgmAudio.mute = isBGMMuted;
            }
        }

        GameObject[] sfxObjects = GameObject.FindGameObjectsWithTag("SFX");
        foreach (GameObject sfxObject in sfxObjects)
        {
            AudioSource sfxAudio = sfxObject.GetComponent<AudioSource>();
            if (sfxAudio != null)
            {
                sfxAudio.mute = isSFXMuted;
            }
        }
    }

    void UpdateButtonSprites()
    {
        if (bgmMuteButton != null)
        {
            Image bgmImage = bgmMuteButton.GetComponent<Image>();
            if (bgmImage != null)
            {
                bgmImage.sprite = isBGMMuted ? muteOnSprite : muteOffSprite;
            }
        }

        if (sfxMuteButton != null)
        {
            Image sfxImage = sfxMuteButton.GetComponent<Image>();
            if (sfxImage != null)
            {
                sfxImage.sprite = isSFXMuted ? muteOnSprite : muteOffSprite;
            }
        }

        if (vibrationButton != null)
        {
            Image vibrationImage = vibrationButton.GetComponent<Image>();
            if (vibrationImage != null)
            {
                vibrationImage.sprite = isVibrationOn ? vibrationOnSprite : vibrationOffSprite;
            }
        }
    }

    public bool IsVibrationOn()
    {
        return isVibrationOn;
    }
}
