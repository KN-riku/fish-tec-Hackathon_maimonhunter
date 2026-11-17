using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class side_collect : MonoBehaviour
{
    public void GoScene()
    {
        SceneManager.LoadScene("collect");
    }
}
