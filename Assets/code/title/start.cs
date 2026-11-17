using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections; // コルーチンを使うために必要

public class start : MonoBehaviour
{
    public AudioClip clickSound;

    // ボタンに登録するのはこのメソッド
    public void OnClickButton()
    {
        // 1. SEを鳴らす
        if (SEManager.instance != null)
        {
            SEManager.instance.PlaySE(clickSound);
        }

        // 2. 「1秒待ってからシーン移動」という一連の処理（コルーチン）を開始
        StartCoroutine(WaitAndGoScene());
    }

    // 実際の待機と移動の処理
    private IEnumerator WaitAndGoScene()
    {
        // 1.0秒間、ここで処理を中断して待つ
        yield return new WaitForSeconds(1.0f);
        // 待機が終わったらシーン移動
        SceneManager.LoadScene("map");
    }
}
