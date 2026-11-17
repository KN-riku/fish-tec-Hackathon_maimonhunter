using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float timeLimit = 60f;
    private float currentTime;

    public int score = 0;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public GameObject endText;

    public static int finalScore;   // ← スコア保持用
    public static int selectedLocation; // 追加：0:金沢, 1:輪島, 2:七尾

    private bool isGameOver = false;

    void Start()
    {
        currentTime = timeLimit;
        UpdateScoreUI();
        endText.SetActive(false);
    }

    void Update()
    {
        if (isGameOver) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            EndGame();
        }

        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        timerText.text = Mathf.CeilToInt(currentTime).ToString();
    }

    public void AddScore(int point)
    {
        if (isGameOver) return;

        score += point;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score : " + score;
    }

    void EndGame()
    {
        isGameOver = true;
        endText.SetActive(true);

        finalScore = score;  // ← スコア保存

        StartCoroutine(GoToScoreScene());
    }

    System.Collections.IEnumerator GoToScoreScene()
    {
        yield return new WaitForSeconds(3f); // 3秒待つ

        SceneManager.LoadScene("score");  // ← シーン名 score
    }
}

