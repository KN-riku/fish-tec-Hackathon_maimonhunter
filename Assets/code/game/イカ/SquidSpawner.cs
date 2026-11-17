using UnityEngine;

public class SquidSpawner : MonoBehaviour
{
    [Header("イカのプレハブ（複数登録可能）")]
    public GameObject[] squidPrefabs;

    [Header("出現のタイミング")]
    public float spawnInterval = 2.0f;

    [Header("出現範囲の設定（インスペクターで調整）")]
    public float xPos = 12f;      // 左右の出現位置（絶対値）
    public float yMin = -8.0f;    // 最も深い出現位置
    public float yMax = -1.5f;    // 最も浅い出現位置

    void Start()
    {
        // 指定した間隔でSpawn関数を繰り返し実行
        InvokeRepeating("Spawn", 1.0f, spawnInterval);
    }

    void Spawn()
    {
        // 1. プレハブ配列が空の場合は処理しない
        if (squidPrefabs == null || squidPrefabs.Length == 0) return;

        // 2. 登録されたプレハブの中からランダムに1つ選ぶ
        int prefabIndex = Random.Range(0, squidPrefabs.Length);
        GameObject selectedPrefab = squidPrefabs[prefabIndex];

        // 3. 左右どちらから出すか決定
        int side = Random.value > 0.5f ? 1 : -1;

        // 4. 高さをインスペクターで設定した範囲からランダムに決定
        float yPos = Random.Range(yMin, yMax);

        // 5. 生成位置を計算
        Vector3 spawnPos = new Vector3(xPos * side, yPos, 0);

        // 6. イカを生成
        GameObject squid = Instantiate(selectedPrefab, spawnPos, Quaternion.identity);

        // 7. イカの進行方向をセット（出現した側と逆方向へ向かわせる）
        Squid squidScript = squid.GetComponent<Squid>();
        if (squidScript != null)
        {
            squidScript.direction = -side;
        }
    }
}