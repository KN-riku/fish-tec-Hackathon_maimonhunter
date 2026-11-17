using UnityEngine;

public class LightController : MonoBehaviour
{
    [Header("移動制限の設定")]
    public float yMax = -1.0f; // これより上にはいかない
    public float yMin = -8.0f; // 必要であれば下の限界も設定
    public float xRange = 10.0f; // 左右の限界

    void Update()
    {
        // マウスの座標をワールド座標に変換
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        // Mathf.Clamp(現在の値, 最小値, 最大値) で座標を制限する
        float clampedX = Mathf.Clamp(worldPos.x, -xRange, xRange);
        float clampedY = Mathf.Clamp(worldPos.y, yMin, yMax);

        // 制限した座標を適用
        transform.position = new Vector3(clampedX, clampedY, 0);
    }
}