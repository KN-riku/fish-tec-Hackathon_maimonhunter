using UnityEngine;

public class GAME_SE : MonoBehaviour
{
    public static GAME_SE instance;
    private AudioSource audioSource;

    [Header("SE Clips")]
    public AudioClip catchSE;      // 通常の捕獲音

    void Awake()
    {
        // シングルトンの設定
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // AudioSourceの取得（なければ自動で追加してエラーを防ぐ）
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // 通常の捕獲音を再生
    public void PlayCatchSE()
    {
        if (catchSE != null && audioSource != null)
        {
            audioSource.PlayOneShot(catchSE);
        }
    }
}