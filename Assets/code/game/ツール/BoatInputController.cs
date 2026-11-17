using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoatInputController : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public GameObject leftUpIcon;
    public GameObject leftUp2Icon;
    public GameObject rightUpIcon;
    public GameObject rightUp2Icon;

    private Image leftUpImg;
    private Image leftUp2Img;
    private Image rightUpImg;
    private Image rightUp2Img;

    private Vector2 startPos;
    private bool isSelecting = false;
    private float swipeThreshold = 50f;

    // 基準角度（好きに調整可能）
    private float angleLeftUp1 = 135f;
    private float angleLeftUp2 = 165f;
    private float angleRightUp1 = 45f;
    private float angleRightUp2 = 75f;

    private FishingMethod methodLeftUp1 = FishingMethod.Teichi;
    private FishingMethod methodLeftUp2 = FishingMethod.Maki;
    private FishingMethod methodRightUp1 = FishingMethod.Ippon;
    private FishingMethod methodRightUp2 = FishingMethod.Sashi;
    void Start()
    {
        leftUpImg = leftUpIcon.GetComponent<Image>();
        leftUp2Img = leftUp2Icon.GetComponent<Image>();
        rightUpImg = rightUpIcon.GetComponent<Image>();
        rightUp2Img = rightUp2Icon.GetComponent<Image>();

        HideIcons();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isSelecting = true;
        startPos = eventData.position;

        ShowIcons();
        ResetColors();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isSelecting) return;

        Vector2 dir = eventData.position - startPos;
        if (dir.magnitude < swipeThreshold)
        {
            ResetColors();
            return;
        }

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360f;

        HighlightClosest(angle);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isSelecting) return;

        Vector2 dir = eventData.position - startPos;

        if (dir.magnitude >= swipeThreshold)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360f;

            FishingMethod selected = GetClosestMethod(angle);
            FishingMethodManager.Instance.SetMethod(selected);
        }

        isSelecting = false;
        HideIcons();
    }

    void HighlightClosest(float angle)
    {
        ResetColors();

        FishingMethod method = GetClosestMethod(angle);

        switch (method)
        {
            case FishingMethod.Teichi:
                leftUpImg.color = Color.yellow;
                break;

            case FishingMethod.Maki:
                leftUp2Img.color = Color.yellow;
                break;

            case FishingMethod.Ippon:
                rightUpImg.color = Color.yellow;
                break;

            case FishingMethod.Sashi:
                rightUp2Img.color = Color.yellow;
                break;
        }
    }

    FishingMethod GetClosestMethod(float angle)
    {
        float d1 = Mathf.Abs(Mathf.DeltaAngle(angle, angleLeftUp1));
        float d2 = Mathf.Abs(Mathf.DeltaAngle(angle, angleLeftUp2));
        float d3 = Mathf.Abs(Mathf.DeltaAngle(angle, angleRightUp1));
        float d4 = Mathf.Abs(Mathf.DeltaAngle(angle, angleRightUp2));

        float min = Mathf.Min(d1, d2, d3, d4);

        if (min == d1) return methodLeftUp1;
        if (min == d2) return methodLeftUp2;
        if (min == d3) return methodRightUp1;
        return methodRightUp2;
    }

    void ShowIcons()
    {
        leftUpIcon.SetActive(true);
        leftUp2Icon.SetActive(true);
        rightUpIcon.SetActive(true);
        rightUp2Icon.SetActive(true);
    }

    void HideIcons()
    {
        leftUpIcon.SetActive(false);
        leftUp2Icon.SetActive(false);
        rightUpIcon.SetActive(false);
        rightUp2Icon.SetActive(false);
    }

    void ResetColors()
    {
        leftUpImg.color = Color.white;
        leftUp2Img.color = Color.white;
        rightUpImg.color = Color.white;
        rightUp2Img.color = Color.white;
    }
}