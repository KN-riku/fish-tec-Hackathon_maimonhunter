using UnityEngine;

// 既存のSeason列挙型
public enum Season
{
    Spring,
    Summer,
    Autumn,
    Winter
}

public class SeasonManager : MonoBehaviour
{
    public Season currentSeason;

    void Update()
    {
        // マップ画面のseasonクラスが持っている静的変数(CurrentSeasonIndex)を直接参照
        // これでマップとゲームシーンの季節が完全に同期します
        int seasonIdx = season.CurrentSeasonIndex;

        // int型をSeason列挙型に変換して代入
        currentSeason = (Season)seasonIdx;
    }
}