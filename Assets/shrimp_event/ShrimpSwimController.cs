 using UnityEngine;

public class ShrimpSwimController : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float swimSpeed = 1.2f;

    [Header("Noise")]
    [SerializeField] private float noiseFrequency = 0.25f;

    [Header("Screen Margin")]
    [SerializeField] private float horizontalMargin = 0.3f;
    [SerializeField] private float verticalMargin = 0.2f;

    [Header("Vertical Range")]
    [SerializeField][Range(0f, 1f)] private float verticalRangeRatio = 0.333f;
    [SerializeField][Range(0f, 1f)] private float verticalCenterRatio = 0.25f; // 0=最下部 0.5=中央 1=最上部

    // internals
    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;

    private float noiseOffsetX;
    private float noiseOffsetY;
    private float noiseTime;

    // 全域まんべんなく巡回するためのドリフトオフセット
    private float driftOffsetX;
    private float driftOffsetY;
    private float driftTime;
    private float driftFrequency = 0.07f; // ゆっくり大きく動く低周波

    private Vector2 velocity = Vector2.zero;
    private bool facingRight = true;

    private float camLeft, camRight, camBottom, camTop;

    // 壁跳ね返り用：滑らかな反転速度
    private Vector2 reflectVelocity = Vector2.zero;
    private float reflectSmoothTime = 0.4f;

    void Start()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();

        noiseOffsetX = Random.Range(0f, 100f);
        noiseOffsetY = Random.Range(0f, 100f);
        driftOffsetX = Random.Range(0f, 100f);
        driftOffsetY = Random.Range(0f, 100f);

        UpdateCameraBounds();
    }

    void Update()
    {
        UpdateCameraBounds();

        noiseTime += Time.deltaTime * noiseFrequency;
        driftTime += Time.deltaTime * driftFrequency;

        // 高周波ノイズ：細かいランダム方向
        float nx = Mathf.PerlinNoise(noiseOffsetX + noiseTime, 0f) * 2f - 1f;
        float ny = Mathf.PerlinNoise(0f, noiseOffsetY + noiseTime) * 2f - 1f;

        // 低周波ドリフト：画面全域をゆっくり巡回させる
        float dx = Mathf.PerlinNoise(driftOffsetX + driftTime, 50f) * 2f - 1f;
        float dy = Mathf.PerlinNoise(50f, driftOffsetY + driftTime) * 2f - 1f;

        // 合成（ドリフトを強めに効かせて全域巡回を促す）
        Vector2 moveDir = new Vector2(nx * 0.4f + dx * 0.6f, ny * 0.4f + dy * 0.6f);
        moveDir = moveDir.normalized;

        Vector2 pos = transform.position;

        // --- 壁処理：震えを防ぐため「反射」方式に変更 ---
        // 壁に当たったら進行方向を反転させ、クランプはしない
        float nextX = pos.x + moveDir.x * swimSpeed * Time.deltaTime;
        float nextY = pos.y + moveDir.y * swimSpeed * Time.deltaTime;

        if (nextX < camLeft || nextX > camRight)
        {
            moveDir.x *= -1f;
            // ノイズ位相もX方向を反転（同じ壁に何度も当たらないように）
            noiseOffsetX += 50f;
            driftOffsetX += 50f;
        }
        if (nextY < camBottom || nextY > camTop)
        {
            moveDir.y *= -1f;
            noiseOffsetY += 50f;
            driftOffsetY += 50f;
        }

        Vector2 newPos = pos + moveDir * swimSpeed * Time.deltaTime;

        // 保険クランプ（反射後もはみ出た場合のみ）
        newPos.x = Mathf.Clamp(newPos.x, camLeft, camRight);
        newPos.y = Mathf.Clamp(newPos.y, camBottom, camTop);

        velocity = newPos - pos;
        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);

        UpdateFacing();
    }

    private void UpdateCameraBounds()
    {
        if (mainCamera == null) return;

        float halfH = mainCamera.orthographicSize;
        float halfW = halfH * mainCamera.aspect;
        Vector3 cp = mainCamera.transform.position;

        float left = cp.x - halfW + horizontalMargin;
        float right = cp.x + halfW - horizontalMargin;
        float bottom = cp.y - halfH + verticalMargin;
        float top = cp.y + halfH - verticalMargin;

        float fullH = top - bottom;
        float halfRange = (fullH * verticalRangeRatio) / 2f;
        // 中心を画面下端から1/3の位置に（海底近く）
        float centerY = bottom + fullH * verticalCenterRatio;

        camLeft = left;
        camRight = right;
        camBottom = Mathf.Max(bottom, centerY - halfRange);
        camTop = Mathf.Min(top, centerY + halfRange);
    }

    private void UpdateFacing()
    {
        if (spriteRenderer == null) return;
        if (velocity.x > 0.001f && !facingRight) { facingRight = true; spriteRenderer.flipX = false; }
        if (velocity.x < -0.001f && facingRight) { facingRight = false; spriteRenderer.flipX = true; }
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        float w = camRight - camLeft;
        float h = camTop - camBottom;
        Vector3 center = new Vector3((camLeft + camRight) / 2f, (camBottom + camTop) / 2f, 0);
        Gizmos.color = new Color(0f, 0.8f, 1f, 0.35f);
        Gizmos.DrawWireCube(center, new Vector3(w, h, 0));
    }
}