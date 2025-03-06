using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game Settings")]
    public float speed = 2f;
    public float speedIncreaseRate = 0.1f;
    public float maxSpeed = 10f;
    public float timeToIncreaseSpeed = 5f;

    [Header("UI Elements")]
    public GameObject startButton;
    public GameObject settingsButton;
    public GameObject panelScore;
    public GameObject textScore;
    public GameObject player;
    public GameObject[] followers;
    public HPBarManager hpBarManager; // ✅ 인스펙터에서 직접 할당

    [Header("Score Settings")]
    public TextMeshProUGUI scoreText;
    private int score = 0;
    private bool isGameOver = false;
    private bool isGameStarted = false;
    private Coroutine scoreCoroutine;
    private Coroutine speedCoroutine;

    [Header("Debugging")]
    [SerializeField] private int autoScoreIncrement;

    void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        Time.timeScale = 0f;
        ShowStartScreen();
        UpdateScoreUI();
    }

    public void StartGame()
    {
        if (isGameStarted) return;

        isGameStarted = true;
        isGameOver = false;
        Time.timeScale = 1f;
        score = 0;
        speed = 2f;

        StartCoroutines();

        SetActiveObjects(false, startButton, settingsButton, panelScore);
        SetActiveObjects(true, player, textScore);
        hpBarManager?.ShowHPBar(true); // ✅ 체력바 표시
        ToggleFollowers(true);
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        isGameStarted = false;

        StopCoroutines();
        SaveScores();

        Debug.Log("Game Over!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ShowStartScreen()
    {
        SetActiveObjects(true, startButton, settingsButton, panelScore);
        SetActiveObjects(false, player, textScore);
        hpBarManager?.ShowHPBar(false); // ✅ 체력바 숨김
        ToggleFollowers(false);

        if (panelScore)
        {
            TextMeshProUGUI scoreTextComponent = panelScore.GetComponentInChildren<TextMeshProUGUI>();
            if (scoreTextComponent != null)
            {
                int lastScore = PlayerPrefs.GetInt("LastScore", 0);
                int highScore = PlayerPrefs.GetInt("HighScore", 0);
                scoreTextComponent.text = $"이전 스코어: {lastScore:D3}\n최고 스코어: {highScore:D3}";
            }
        }
    }

    private void StartCoroutines()
    {
        if (scoreCoroutine != null) StopCoroutine(scoreCoroutine);
        if (speedCoroutine != null) StopCoroutine(speedCoroutine);

        scoreCoroutine = StartCoroutine(IncreaseScore());
        speedCoroutine = StartCoroutine(IncreaseSpeedOverTime());
    }

    private void StopCoroutines()
    {
        if (scoreCoroutine != null) StopCoroutine(scoreCoroutine);
        if (speedCoroutine != null) StopCoroutine(speedCoroutine);
    }

    IEnumerator IncreaseScore()
    {
        while (!isGameOver)
        {
            yield return new WaitForSeconds(1f);
            score += CalculateScoreIncrement();
            UpdateScoreUI();
        }
    }

    IEnumerator IncreaseSpeedOverTime()
    {
        while (!isGameOver)
        {
            yield return new WaitForSeconds(timeToIncreaseSpeed);
            if (speed < maxSpeed)
            {
                speed = Mathf.Min(speed + speedIncreaseRate, maxSpeed);
                Debug.Log($"속도 증가: {speed}");
            }
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText) scoreText.text = score.ToString();
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    private void SaveScores()
    {
        PlayerPrefs.SetInt("LastScore", score);
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (score > highScore) PlayerPrefs.SetInt("HighScore", score);
        PlayerPrefs.Save();
    }

    private int CalculateScoreIncrement()
    {
        float currentSpeed = speed;
        int additionalPoints = Mathf.FloorToInt((currentSpeed - 2.0f) / 0.5f) * 50;
        autoScoreIncrement = 50 + additionalPoints;
        return autoScoreIncrement;
    }

    private void SetActiveObjects(bool isActive, params GameObject[] objects)
    {
        foreach (GameObject obj in objects)
        {
            if (obj) obj.SetActive(isActive);
        }
    }

    private void ToggleFollowers(bool isActive)
    {
        if (followers == null) return;
        foreach (GameObject follower in followers)
        {
            if (follower) follower.SetActive(isActive);
        }
    }
}
