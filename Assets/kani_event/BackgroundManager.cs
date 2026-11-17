using UnityEngine;
using System.Collections;

public class BackgroundScrollEvent : MonoBehaviour
{
    [Header("背景全体をまとめた親オブジェクト")]
    public Transform backgroundRoot;

    [Header("通常時の位置")]
    public Vector3 normalPosition = new Vector3(0f, 0f, 0f);

    [Header("深海イベント時の位置")]
    public Vector3 deepPosition = new Vector3(0f, 10f, 0f);

    [Header("移動時間")]
    public float moveDuration = 2f;

    private bool isMoving = false;

    void Start()
    {
        if (backgroundRoot != null)
        {
            backgroundRoot.position = normalPosition;
        }
    }

    public void MoveToDeepSea()
    {
        if (isMoving || backgroundRoot == null) return;
        StartCoroutine(MoveBackground(deepPosition));
    }

    public void MoveToNormalSea()
    {
        if (isMoving || backgroundRoot == null) return;
        StartCoroutine(MoveBackground(normalPosition));
    }

    private IEnumerator MoveBackground(Vector3 targetPos)
    {
        isMoving = true;

        Vector3 startPos = backgroundRoot.position;
        float time = 0f;

        while (time < moveDuration)
        {
            time += Time.deltaTime;
            float t = time / moveDuration;
            t = Mathf.SmoothStep(0f, 1f, t);

            backgroundRoot.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        backgroundRoot.position = targetPos;
        isMoving = false;
    }
}