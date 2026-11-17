using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CatchImageManager : MonoBehaviour
{
    [System.Serializable]
    public class CrabImageData
    {
        public CrabID crabID;
        public Sprite image;
    }

    [Header("UI")]
    public Image displayImage;

    [Header("Image List")]
    public CrabImageData[] crabImages;

    [Header("Display Time")]
    public float showTime = 1.5f;

    private Coroutine currentCoroutine;

    void Start()
    {
        if (displayImage != null)
        {
            displayImage.gameObject.SetActive(false);
        }
    }

    public void ShowCatchImage(CrabID id)
    {
        Sprite sprite = GetSpriteByID(id);
        if (sprite == null || displayImage == null) return;

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        currentCoroutine = StartCoroutine(ShowImageRoutine(sprite));
    }

    Sprite GetSpriteByID(CrabID id)
    {
        foreach (var data in crabImages)
        {
            if (data.crabID == id)
            {
                return data.image;
            }
        }
        return null;
    }

    IEnumerator ShowImageRoutine(Sprite sprite)
    {
        displayImage.sprite = sprite;
        displayImage.gameObject.SetActive(true);

        yield return new WaitForSeconds(showTime);

        displayImage.gameObject.SetActive(false);
    }
}