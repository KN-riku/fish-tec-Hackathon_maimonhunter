using UnityEngine;

public class ButtonClickSE : MonoBehaviour
{
    public AudioClip clickSE;

    public void PlayClickSE()
    {
        SEManager.instance.PlaySE(clickSE);
    }
}