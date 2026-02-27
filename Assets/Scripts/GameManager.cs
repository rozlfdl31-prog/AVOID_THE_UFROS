using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // UI 연결용
    public TextMeshProUGUI scoreText;
    public GameObject gameOverUI;
    public TextMeshProUGUI rankingText; // [추가] 랭킹 표시용

    private int score = 0;
    private bool isGameOver = false;
    private bool isGameStarted = false;
    private float gameStartTime = 0f;

    // [추가] 리스타트인지 구분 (static = 씬 다시 로드해도 유지!)
    private static bool isRestart = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // 리스타트면 타이틀 건너뛰고 바로 시작!
        if (isRestart)
        {
            isRestart = false; // 플래그 리셋
            StartGame();

            // 타이틀 화면 숨기기
            TitleScreenManager title = FindAnyObjectByType<TitleScreenManager>();
            if (title != null && title.titleScreenUI != null)
                title.titleScreenUI.SetActive(false);
        }
    }

    void Update()
    {
        if (!isGameStarted || isGameOver) return;

        score = Mathf.FloorToInt((Time.time - gameStartTime) * 10);

        if (scoreText != null) scoreText.text = "Score: " + score;
    }

    public void StartGame()
    {
        isGameStarted = true;
        gameStartTime = Time.time;

        if (scoreText != null)
            scoreText.gameObject.SetActive(true);

        // BGM 시작!
        if (SoundManager.instance != null)
            SoundManager.instance.PlayBGM();
    }

    public bool IsGameStarted()
    {
        return isGameStarted;
    }

    public void GameOver()
    {
        isGameOver = true;

        // BGM 정지
        if (SoundManager.instance != null)
            SoundManager.instance.StopBGM();

        // 랭킹 저장 & 표시
        SaveScore(score);
        if (rankingText != null)
            rankingText.text = GetRankingText();

        if (gameOverUI != null) gameOverUI.SetActive(true);

        Debug.Log("Game Over! Score: " + score);
    }

    public void RestartGame()
    {
        isRestart = true; // [핵심] "다음 씬은 리스타트다!" 표시
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // ===== 랭킹 시스템 (Top 5) =====

    void SaveScore(int newScore)
    {
        // 기존 랭킹 불러오기
        int[] scores = new int[5];
        for (int i = 0; i < 5; i++)
            scores[i] = PlayerPrefs.GetInt("Rank" + i, 0);

        // 새 점수가 들어갈 위치 찾기
        for (int i = 0; i < 5; i++)
        {
            if (newScore > scores[i])
            {
                // 뒤로 밀기
                for (int j = 4; j > i; j--)
                    scores[j] = scores[j - 1];

                scores[i] = newScore;
                break;
            }
        }

        // 저장
        for (int i = 0; i < 5; i++)
            PlayerPrefs.SetInt("Rank" + i, scores[i]);
        PlayerPrefs.Save();
    }

    string GetRankingText()
    {
        string result = "== TOP 5 ==\n";
        for (int i = 0; i < 5; i++)
        {
            int s = PlayerPrefs.GetInt("Rank" + i, 0);
            result += (i + 1) + "st  " + s + "\n";
        }
        return result;
    }
}