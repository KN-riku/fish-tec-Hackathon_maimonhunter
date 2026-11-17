using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimationEnd : MonoBehaviour
{
    public float duration = 8f;
    public string nextScene;

    void Start()
    {
        Invoke("LoadNext", duration);
    }

    void LoadNext()
    {
        SceneManager.LoadScene(nextScene);
    }
}