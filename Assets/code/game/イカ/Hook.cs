using UnityEngine;
using System.Collections;

public class Hook : MonoBehaviour
{
    public float moveRange = 0.5f;
    public float moveSpeed = 2.5f;
    public float respawnTime = 3.0f;

    private Vector3 startPos;
    private bool isActive = true;
    private SpriteRenderer sr;
    private Collider2D col;

    void Start()
    {
        startPos = transform.position;
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (!isActive) return;
        // 上下運動
        float newY = startPos.y + Mathf.Sin(Time.time * moveSpeed) * moveRange;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 当たった相手がイカ（Squidタグ）で、針がアクティブな時
        if (isActive && other.CompareTag("Squid"))
        {
            // GAME_SEのインスタンスを通じて音を鳴らす
            if (GAME_SE.instance != null)
            {
                GAME_SE.instance.PlayCatchSE();
            }

            // スコア加算処理（GameManagerがある場合）
            Object.FindFirstObjectByType<GameManager>()?.AddScore(50);

            Destroy(other.gameObject); // イカを消す
            StartCoroutine(RechargeHook()); // 針を一時消失させる
        }
    }

    IEnumerator RechargeHook()
    {
        isActive = false;
        sr.enabled = false;
        col.enabled = false;
        yield return new WaitForSeconds(respawnTime);
        sr.enabled = true;
        col.enabled = true;
        isActive = true;
    }
}