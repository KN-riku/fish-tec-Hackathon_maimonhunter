using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PageController : MonoBehaviour
{
    [System.Serializable]
    public struct LocationPages
    {
        public string locationName; // インスペクター上での識別用（金沢など）
        public Sprite[] images;     // その地域の画像リスト
    }

    [Header("Settings")]
    public Image displayImage;
    [Tooltip("地域ごとに画像をセットしてください")]
    public List<LocationPages> locationDataList;

    private int currentLocationIndex = 0; // 現在の地域 (0:金沢, 1:輪島...)
    private int currentPageIndex = 0;     // その地域の中の何枚目か

    void Start()
    {
        UpdatePage();
    }

    // 表示を更新する
    void UpdatePage()
    {
        if (locationDataList == null || locationDataList.Count == 0) return;

        // 現在の地域の画像配列を取得
        Sprite[] currentImages = locationDataList[currentLocationIndex].images;

        if (currentImages.Length > 0)
        {
            // インデックスが範囲外にならないよう調整
            currentPageIndex = Mathf.Clamp(currentPageIndex, 0, currentImages.Length - 1);
            displayImage.sprite = currentImages[currentPageIndex];
        }
    }

    // 地域を直接切り替える（ボタンから呼び出す用：引数に 0, 1, 2... を入れる）
    public void SetLocation(int index)
    {
        if (index < 0 || index >= locationDataList.Count) return;

        currentLocationIndex = index;
        currentPageIndex = 0; // 地域を変えたら最初のページに戻す
        UpdatePage();
    }

    // 次のページへ（右ボタン）
    public void NextPage()
    {
        Sprite[] currentImages = locationDataList[currentLocationIndex].images;

        currentPageIndex++;
        if (currentPageIndex >= currentImages.Length)
        {
            currentPageIndex = 0; // 最後まで行ったら最初に戻る
        }
        UpdatePage();
    }

    // 前のページへ（左ボタン）
    public void PrevPage()
    {
        Sprite[] currentImages = locationDataList[currentLocationIndex].images;

        currentPageIndex--;
        if (currentPageIndex < 0)
        {
            currentPageIndex = currentImages.Length - 1; // 最初から前に戻ったら最後に
        }
        UpdatePage();
    }
}