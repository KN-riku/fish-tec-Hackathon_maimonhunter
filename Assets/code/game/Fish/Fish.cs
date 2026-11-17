using UnityEngine;
using UnityEngine.SceneManagement;

public class Fish : MonoBehaviour
{
    public string fishName;
    public int basePoint;
    public Season[] availableSeasons;

    bool isCaught = false;

    public void CatchFish()
    {
        if (isCaught) return;
        isCaught = true;

        float multiplier = FishingMethodManager.Instance.GetMultiplier();
        int finalPoint = Mathf.RoundToInt(basePoint * multiplier);

        // ScoreManagerではなく、GameManagerを探してスコアを加算する
        GameManager gm = FindFirstObjectByType<GameManager>();
        if (gm != null)
        {
            gm.AddScore(finalPoint);
        }

        if (GAME_SE.instance != null)
            GAME_SE.instance.PlayCatchSE();

        Destroy(gameObject);
    }
}