using UnityEngine;
using System.Collections;

public class IpponzuriController : MonoBehaviour
{
    public GameObject hookPrefab;

    public float moveSpeed = 10f;
    public float cooldownTime = 4f;
    public float seaSurfaceY = 0f;
    public float maxDepth = -4f;

    bool isCoolingDown = false;
    GameObject currentHook;

    float previousMouseY;

    void Update()
    {
        if (FishingMethodManager.Instance.currentMethod
            != FishingMethod.Ippon)
            return;

        if (isCoolingDown) return;

        // タップで針生成
        if (Input.GetMouseButtonDown(0) && currentHook == null)
        {
            Vector3 pos =
                Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;

            if (pos.y > seaSurfaceY) return;

            pos.y = seaSurfaceY;

            currentHook =
                Instantiate(hookPrefab, pos, Quaternion.identity);

            previousMouseY = Input.mousePosition.y;

            StartCoroutine(Cooldown());
        }

        // 押してる間、上下操作
        if (Input.GetMouseButton(0) && currentHook != null)
        {
            float currentMouseY = Input.mousePosition.y;
            float deltaY = currentMouseY - previousMouseY;

            float moveAmount = deltaY * moveSpeed * Time.deltaTime;

            currentHook.transform.Translate(Vector2.up * moveAmount);

            // 海面より上に行かない
            if (currentHook.transform.position.y > seaSurfaceY)
                currentHook.transform.position =
                    new Vector3(currentHook.transform.position.x,
                                seaSurfaceY,
                                0);

            // 深く行きすぎない
            if (currentHook.transform.position.y < maxDepth)
                currentHook.transform.position =
                    new Vector3(currentHook.transform.position.x,
                                maxDepth,
                                0);

            previousMouseY = currentMouseY;
        }

        // 離したら回収
        if (Input.GetMouseButtonUp(0) && currentHook != null)
        {
            Destroy(currentHook);
        }
    }

    IEnumerator Cooldown()
    {
        isCoolingDown = true;
        yield return new WaitForSeconds(cooldownTime);
        isCoolingDown = false;
    }
}