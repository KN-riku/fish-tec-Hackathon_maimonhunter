using UnityEngine;

/// <summary>
/// 底引き網をマウスで操作し、円の部分に1秒当たったら捕獲する
/// 捕獲時にスコア加算とSE再生を行います。
/// </summary>
public class NetCon : MonoBehaviour
{
    [Header("Move Range (World)")]
    [SerializeField] private float minX = -2.7f;
    [SerializeField] private float maxX = 2.7f;
    [SerializeField] private float minY = -4.0f;
    [SerializeField] private float maxY = 1.25f;

    [Header("Follow")]
    [SerializeField] private float followSpeed = 25f;

    [Header("Catch Settings")]
    [SerializeField] private float catchTime = 1.0f;
    [SerializeField] private int scorePerShrimp = 50; // 加算するスコア

    [Header("Net Circle")]
    [SerializeField] private Vector2 circleOffset = new Vector2(-1.1f, 0f);
    [SerializeField] private float circleRadius = 0.4f;

    private Camera mainCamera;
    private float contactTimer = 0f;
    private GameObject contactShrimp = null;

    // GameManagerへの参照（Startで自動取得）
    private GameManager gameManager;

    void Start()
    {
        mainCamera = Camera.main;

        // シーン内にあるGameManagerを探して取得
        gameManager = Object.FindFirstObjectByType<GameManager>();

        var box = GetComponent<BoxCollider2D>();
        if (box != null) box.enabled = false;
    }

    void Update()
    {
        MoveNetToMouse();
        CheckCircleOverlap();
    }

    private void MoveNetToMouse()
    {
        if (mainCamera == null) return;
        Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;
        float clampedX = Mathf.Clamp(mouseWorld.x, minX, maxX);
        float clampedY = Mathf.Clamp(mouseWorld.y, minY, maxY);
        Vector3 target = new Vector3(clampedX, clampedY, 0f);
        transform.position = Vector3.Lerp(transform.position, target, followSpeed * Time.deltaTime);
    }

    private void CheckCircleOverlap()
    {
        Vector2 circleCenter = (Vector2)transform.position + circleOffset;
        Collider2D hit = Physics2D.OverlapCircle(circleCenter, circleRadius);

        if (hit != null && hit.CompareTag("Shrimp"))
        {
            if (contactShrimp == hit.gameObject)
            {
                contactTimer += Time.deltaTime;
                if (contactTimer >= catchTime)
                {
                    // --- 捕獲成功時の処理を追加 ---
                    CatchSuccess();
                }
            }
            else
            {
                contactShrimp = hit.gameObject;
                contactTimer = 0f;
            }
        }
        else
        {
            contactShrimp = null;
            contactTimer = 0f;
        }
    }

    private void CatchSuccess()
    {
        // 1. スコアを加算
        if (gameManager != null)
        {
            gameManager.AddScore(scorePerShrimp);
        }

        // 2. SEを再生 (GAME_SEのシングルトンを利用)
        if (GAME_SE.instance != null)
        {
            GAME_SE.instance.PlayCatchSE();
        }

        // 3. オブジェクトを削除
        Destroy(contactShrimp);

        // 後片付け
        contactShrimp = null;
        contactTimer = 0f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.5f);
        Vector2 center = (Vector2)transform.position + circleOffset;
        Gizmos.DrawWireSphere(new Vector3(center.x, center.y, 0f), circleRadius);
    }
}