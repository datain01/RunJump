using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float speed = 2f;
    public float speedIncreaseRate = 0.1f;
    public float maxSpeed = 10f;
    public float timeToIncreaseSpeed = 5f;

    public GameObject startButton;
    public GameObject settingsButton;
    public GameObject panelScore;
    public GameObject panelHP;   // HP UI 추가
    public GameObject textScore; // 점수 텍스트 추가
    public GameObject player;    // 플레이어 추가
    public GameObject[] followers; // 여러 Follower 관리

    public TextMeshProUGUI scoreText;
    private int score = 0;
    private bool isGameOver = false;
    private bool isGameStarted = false;
    private Coroutine scoreCoroutine;
    private Coroutine speedCoroutine;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
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
        speed = 2f; // 속도 초기화

        if (scoreCoroutine == null)
        {
            scoreCoroutine = StartCoroutine(IncreaseScore());
        }

        if (speedCoroutine == null)
        {
            speedCoroutine = StartCoroutine(IncreaseSpeedOverTime());
        }

        // UI 및 오브젝트 활성화
        startButton?.SetActive(false);
        settingsButton?.SetActive(false);
        panelScore?.SetActive(false);

        player?.SetActive(true);
        panelHP?.SetActive(true);
        textScore?.SetActive(true);

        // 모든 Follower 활성화
        if (followers != null)
        {
            foreach (GameObject follower in followers)
            {
                follower?.SetActive(true);
            }
        }
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        isGameStarted = false;

        SaveScores();
        Debug.Log("Game Over!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void ShowStartScreen()
    {
        startButton?.SetActive(true);
        settingsButton?.SetActive(true);
        panelScore?.SetActive(true);

        // 게임 시작 전 UI 및 오브젝트 비활성화
        player?.SetActive(false);
        panelHP?.SetActive(false);
        textScore?.SetActive(false);

        // 모든 Follower 비활성화
        if (followers != null)
        {
            foreach (GameObject follower in followers)
            {
                follower?.SetActive(false);
            }
        }

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

    IEnumerator IncreaseScore()
    {
        while (!isGameOver)
        {
            yield return new WaitForSeconds(1f);
            score += 100;
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
                speed += speedIncreaseRate;
                speed = Mathf.Min(speed, maxSpeed);
                Debug.Log($"속도 증가: {speed}");
            }
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    void SaveScores()
    {
        PlayerPrefs.SetInt("LastScore", score);
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
        PlayerPrefs.Save();
    }
}
