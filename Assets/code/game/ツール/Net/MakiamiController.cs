using UnityEngine;
using System.Collections;

public class MakiamiController : MonoBehaviour
{
    public GameObject netPrefab;

    public float cooldownTime = 3f;
    public float swipeDuration = 1f;   // ← 追加（最大スワイプ時間）
    public float seaSurfaceY = 0f;

    bool isSwiping = false;
    bool isCoolingDown = false;

    GameObject currentNet;

    void Update()
    {
        if (FishingMethodManager.Instance.currentMethod
            != FishingMethod.Maki)
            return;

        if (isCoolingDown) return;

        if (Input.GetMouseButtonDown(0) && !isSwiping)
        {
            Vector3 pos =
                Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;

            if (pos.y > seaSurfaceY) return;

            currentNet =
                Instantiate(netPrefab, pos, Quaternion.identity);

            isSwiping = true;

            StartCoroutine(SwipeTimer());   // ← 自動終了タイマー
        }

        if (Input.GetMouseButton(0) && isSwiping)
        {
            Vector3 pos =
                Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;

            if (pos.y > seaSurfaceY)
                pos.y = seaSurfaceY;

            currentNet.transform.position = pos;
        }

        if (Input.GetMouseButtonUp(0) && isSwiping)
        {
            EndSwipe();
        }
    }

    void EndSwipe()
    {
        isSwiping = false;

        if (currentNet != null)
            Destroy(currentNet, 0.2f);

        StartCoroutine(Cooldown());
    }

    IEnumerator SwipeTimer()
    {
        yield return new WaitForSeconds(swipeDuration);

        if (isSwiping)
        {
            EndSwipe();
        }
    }

    IEnumerator Cooldown()
    {
        isCoolingDown = true;
        yield return new WaitForSeconds(cooldownTime);
        isCoolingDown = false;
    }
}
