using System;
using UnityEngine;
using UnityEngine.UI;

public class season : MonoBehaviour // クラス名を判別しやすいように変更（任意）
{
    // 他のスクリプトから参照する用
    public static int CurrentSeasonIndex;

    [Header("UI要素")]
    public Image seasonIcon;
    public Sprite[] seasonSprites; // 0:春, 1:夏, 2:秋, 3:冬

    [Header("マップ上の演出")]
    public GameObject[] seasonAnims;

    void Update()
    {
        UpdateSeasonVisuals();
    }

    void UpdateSeasonVisuals()
    {
        // 現在の「分」を取得
        int minute = DateTime.Now.Minute;

        // ---------------------------------------------------------
        // 【改造ポイント】
        // minute % 4 は 0, 1, 2, 3 分 のサイクルを作ります。
        // これにより 1分ごとに 季節が切り替わります。
        // 0分:春, 1分:夏, 2分:秋, 3分:冬, 4分:春... とループします。
        // ---------------------------------------------------------
        int index = minute % 4;

        CurrentSeasonIndex = index; // 現在の季節を静的変数に保存

        // アイコンの切り替え
        if (seasonIcon != null && seasonSprites.Length > index)
        {
            seasonIcon.sprite = seasonSprites[index];
        }

        // アニメーション/演出オブジェクトの切り替え
        for (int i = 0; i < seasonAnims.Length; i++)
        {
            if (seasonAnims[i] != null)
            {
                // 現在のインデックスと一致するオブジェクトだけを有効化
                seasonAnims[i].SetActive(i == index);
            }
        }
    }
}