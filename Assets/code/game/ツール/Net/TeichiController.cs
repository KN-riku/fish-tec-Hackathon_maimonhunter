using UnityEngine;
using System.Collections;

public class TeichiController : MonoBehaviour
{
    public GameObject teichiPrefab;
    public float seaSurfaceY = 0f;
    public float cooldownTime = 10f; // ’·‚¢

    bool isCoolingDown = false;

    void Update()
    {
        if (FishingMethodManager.Instance.currentMethod
            != FishingMethod.Teichi)
            return;

        if (isCoolingDown) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos =
                Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;

            if (pos.y > seaSurfaceY) return;

            Instantiate(teichiPrefab, pos, Quaternion.identity);

            StartCoroutine(Cooldown());
        }
    }

    IEnumerator Cooldown()
    {
        isCoolingDown = true;
        yield return new WaitForSeconds(cooldownTime);
        isCoolingDown = false;
    }
}