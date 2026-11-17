using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class select : MonoBehaviour
{
    // --- 既存の変数 ---
    public Image previewDisplay;
    public Sprite[] previewSprites;
    public AudioClip selectSound;
    public AudioClip decisionSound;
    private int currentSelection = -1;

    [Header("イベント設定")]
    [Range(0, 100)] public int eventProbability = 50; // 各地域でイベントが起きる確率

    // 地域ごとのイベント発生フラグ
    private bool evKanazawa = false;
    private bool evWajima = false;
    private bool evNanao = false;

    [Header("イベントマーク(アニメ付きImage)")]
    public GameObject eventMarkKanazawa;
    public GameObject eventMarkWajima;
    public GameObject eventMarkNanao;

    void Start()
    {
        // シーン開始時に、地域ごとに「今回のイベント」を抽選する
        int seasonIdx = season.CurrentSeasonIndex;

        // 1. まず全フラグをリセット
        evKanazawa = evWajima = evNanao = false;

        // 2. 季節ごとの候補地に対して個別に確率判定を行う
        if (seasonIdx == 0) // 春: 七尾
        {
            evNanao = Random.Range(0, 100) < eventProbability;
        }
        else if (seasonIdx == 1) // 夏: 輪島, 七尾
        {
            evWajima = Random.Range(0, 100) < eventProbability;
            evNanao = Random.Range(0, 100) < eventProbability;
        }
        else if (seasonIdx == 2) // 秋: 金沢
        {
            evKanazawa = Random.Range(0, 100) < eventProbability;
        }
        else if (seasonIdx == 3) // 冬: 金沢, 輪島
        {
            evKanazawa = Random.Range(0, 100) < eventProbability;
            evWajima = Random.Range(0, 100) < eventProbability;
        }

        // 3. 判定結果に基づいてマークを表示
        ShowEventMarks();
    }

    void ShowEventMarks()
    {
        if (eventMarkKanazawa) eventMarkKanazawa.SetActive(evKanazawa);
        if (eventMarkWajima) eventMarkWajima.SetActive(evWajima);
        if (eventMarkNanao) eventMarkNanao.SetActive(evNanao);
    }

    // --- 中略：OnSelect系やPlaySelectSEなどは前のコードと同じ ---

    private IEnumerator WaitAndGoScene()
    {
        yield return new WaitForSeconds(0.8f);

        // --- 追加：選んだ場所をGameManagerに保存 ---
        GameManager.selectedLocation = currentSelection;

        int seasonIdx = season.CurrentSeasonIndex; // 0:春, 1:夏, 2:秋, 3:冬
        string sceneName = "";

        // --- 季節・場所・魚介類ごとのイベントシーン分岐 ---
        if (currentSelection == 0) // 金沢
        {
            if (seasonIdx == 2 && evKanazawa) sceneName = "autumnevent"; // 秋・金沢（エビ）
            else if (seasonIdx == 3 && evKanazawa) sceneName = "dive"; // 冬・金沢（カニ）
        }
        else if (currentSelection == 1) // 輪島
        {
            if (seasonIdx == 1 && evWajima) sceneName = "kai_summerevent"; // 夏・輪島（貝）
            else if (seasonIdx == 3 && evWajima) sceneName = "dive"; // 冬・輪島（カニ）
        }
        else if (currentSelection == 2) // 七尾
        {
            if (seasonIdx == 0 && evNanao) sceneName = "springevent"; // 春・七尾（鯛）
            else if (seasonIdx == 1 && evNanao) sceneName = "ika_summerevent"; // 夏・七尾（イカ）
        }

        // --- 通常のシーン遷移（イベントがない場合） ---
        if (string.IsNullOrEmpty(sceneName))
        {
            if (currentSelection == 0) sceneName = "kanazawa";
            else if (currentSelection == 1) sceneName = "wajima";
            else if (currentSelection == 2) sceneName = "nanao";
        }

        SceneManager.LoadScene(sceneName);
    }

    // 以下、必要な既存メソッドを維持
    public void OnSelectkanazawa() { currentSelection = 0; PlaySelectSE(); UpdatePreview(); }
    public void OnSelectwajima() { currentSelection = 1; PlaySelectSE(); UpdatePreview(); }
    public void OnSelectnanao() { currentSelection = 2; PlaySelectSE(); UpdatePreview(); }
    private void PlaySelectSE() { if (SEManager.instance != null && selectSound != null) SEManager.instance.PlaySE(selectSound); }
    void UpdatePreview() { if (previewDisplay == null) return; if (currentSelection >= 0 && currentSelection < previewSprites.Length) previewDisplay.sprite = previewSprites[currentSelection]; }
    public void OnClickOK() { if (currentSelection == -1) return; if (SEManager.instance != null && decisionSound != null) SEManager.instance.PlaySE(decisionSound); StartCoroutine(WaitAndGoScene()); }
}