using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreResultDisplay : MonoBehaviour
{
    [Header("1. スコア・ランク表示")]
    public TextMeshProUGUI scoreText;
    public Image rankImage;
    public Sprite rankS, rankA, rankB, rankC;

    [Header("2. 左側：季節のデータ画像")]
    public Image seasonDataImage;
    public Sprite[] seasonDataSprites; // 0:春, 1:夏, 2:秋, 3:冬 用の4枚

    [Header("3. 右側：地域別の料理画像")]
    public Image[] dishImages;          // 3つのUI枠をドラッグ＆ドロップ
    public Sprite[] kanazawaDishes;     // 金沢の料理リスト
    public Sprite[] wajimaDishes;       // 輪島の料理リスト
    public Sprite[] nanaoDishes;        // 七尾の料理リスト

    void Start()
    {
        // --- スコアとランクの表示 ---
        int score = GameManager.finalScore;
        scoreText.text = score.ToString();
        UpdateRankImage(score);

        // --- 季節データの更新（左側） ---
        UpdateSeasonData();

        // --- 地域料理の更新（右側） ---
        UpdateDishImagesByLocation();
    }

    void UpdateSeasonData()
    {
        // seasonスクリプトの静的変数から現在の季節を取得
        int seasonIdx = season.CurrentSeasonIndex;

        if (seasonDataSprites != null && seasonIdx < seasonDataSprites.Length)
        {
            seasonDataImage.sprite = seasonDataSprites[seasonIdx];
        }
    }

    void UpdateDishImagesByLocation()
    {
        // GameManagerに保存された場所(0:金沢, 1:輪島, 2:七尾)を取得
        int locationIdx = GameManager.selectedLocation;

        Sprite[] selectedSprites;
        switch (locationIdx)
        {
            case 0: selectedSprites = kanazawaDishes; break;
            case 1: selectedSprites = wajimaDishes; break;
            case 2: selectedSprites = nanaoDishes; break;
            default: return;
        }

        // 料理枠に画像をセット
        for (int i = 0; i < dishImages.Length; i++)
        {
            if (selectedSprites != null && i < selectedSprites.Length)
            {
                dishImages[i].sprite = selectedSprites[i];
                dishImages[i].gameObject.SetActive(true);
            }
            else
            {
                dishImages[i].gameObject.SetActive(false);
            }
        }
    }

    void UpdateRankImage(int score)
    {
        if (score >= 300) rankImage.sprite = rankS;
        else if (score >= 200) rankImage.sprite = rankA;
        else if (score >= 100) rankImage.sprite = rankB;
        else rankImage.sprite = rankC;
    }
}
