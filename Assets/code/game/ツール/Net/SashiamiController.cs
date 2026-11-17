using UnityEngine;
using System.Collections;

public class SashiamiController : MonoBehaviour
{
    public GameObject netPrefab;
    public float activeTime = 4f;
    public float seaSurfaceY = 0f;

    GameObject currentNet;

    void Update()
    {
        if (FishingMethodManager.Instance.currentMethod
            != FishingMethod.Sashi)
            return;

        if (Input.GetMouseButtonDown(0) && currentNet == null)
        {
            Vector3 pos =
                Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;

            if (pos.y > seaSurfaceY) return;

            currentNet =
                Instantiate(netPrefab, pos, Quaternion.identity);

            StartCoroutine(NetLife());
        }
    }

    IEnumerator NetLife()
    {
        yield return new WaitForSeconds(activeTime);

        if (currentNet != null)
        {
            Destroy(currentNet);
            currentNet = null;
        }
    }
}