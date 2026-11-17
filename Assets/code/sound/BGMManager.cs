using UnityEngine;

public class BGMManager : MonoBehaviour
{
    private static BGMManager instance;
    private AudioSource audioSource;

    [SerializeField] private AudioClip sceneBGM; // インスペクターでシーンごとの曲を指定

    void Awake()
    {
        if (instance != null)
        {
            // すでにマネージャーが存在する場合、新しいシーンの「曲」だけを古いマネージャーに渡す
            instance.ChangeBGM(sceneBGM);
            Destroy(gameObject); // 自分（新しい方のオブジェクト）は消える
            return;
        }

        instance = this;
        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);

        // 最初のシーンの曲を再生
        PlayBGM(sceneBGM);
    }

    // 曲をチェックして、違えば再生するメソッド
    public void ChangeBGM(AudioClip nextClip)
    {
        // 今鳴っている曲と同じなら何もしない（途切れさせないため）
        if (audioSource.clip == nextClip) return;

        PlayBGM(nextClip);
    }

    private void PlayBGM(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}