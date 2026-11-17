using UnityEngine;
using UnityEngine.UI;

public class AllButtonSE : MonoBehaviour
{
    public AudioClip clickSE;

    void Start()
    {
        Button[] buttons = FindObjectsOfType<Button>();

        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() =>
            {
                SEManager.instance.PlaySE(clickSE);
            });
        }
    }
}