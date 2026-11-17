using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class collect : MonoBehaviour
{
    public void GoScene()
    {
        SceneManager.LoadScene("map");
    }
}
