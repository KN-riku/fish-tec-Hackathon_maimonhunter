using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class side_title : MonoBehaviour
{
    public void GoScene()
    {
        SceneManager.LoadScene("title");
    }
}
