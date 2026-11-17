using UnityEngine;

public class SEManager : MonoBehaviour
{
    public static SEManager instance;
    private AudioSource audioSource;

    void Awake()
    {
        // どこからでも呼べるようにする（シングルトン）
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    // SEを鳴らすためのメソッド
    public void PlaySE(AudioClip clip)
    {
        // PlayOneShotを使うと、音が重なっても途切れずに鳴る
        audioSource.PlayOneShot(clip);
    }
}
