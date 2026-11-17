using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SpawnedItem : MonoBehaviour
{
    private FixedPointRespawnSpawner spawner;
    private int pointIndex;
    private bool reserved;

    [Header("設定")]
    [SerializeField] private int itemPoint = 10; // このアイテムの点数

    public void Initialize(FixedPointRespawnSpawner spawner, int pointIndex)
    {
        this.spawner = spawner;
        this.pointIndex = pointIndex;
        reserved = false;
    }

    private void Reset()
    {
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (reserved) return;

        var diver = other.GetComponent<chara_move>();
        if (diver == null) return;
        if (!diver.CanPickupNow()) return;

        reserved = true;

        var col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        bool started = diver.TriggerGet(() =>
        {
            // --- スコア加算 ---
            GameManager gm = Object.FindFirstObjectByType<GameManager>();
            if (gm != null)
            {
                gm.AddScore(itemPoint);
            }

            // --- SE再生 ---
            if (GAME_SE.instance != null)
            {
                GAME_SE.instance.PlayCatchSE();
            }

            if (spawner != null)
                spawner.NotifyPickedUp(pointIndex, gameObject);

            Destroy(gameObject);
        });

        if (!started)
        {
            reserved = false;
            if (col != null) col.enabled = true;
        }
    }
}