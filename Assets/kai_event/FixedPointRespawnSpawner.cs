using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedPointRespawnSpawner : MonoBehaviour
{
    [Header("Prefab assets from Project (NOT Hierarchy objects)")]
    public GameObject[] itemPrefabs;

    [Header("Fixed spawn positions (world coords)")]
    public Vector2[] spawnPositions = new Vector2[]
    {
        new Vector2(-2f, -2f),
        new Vector2(-1f, -3f),
        new Vector2(-0.5f, -1.5f),
        new Vector2(1f, -2.5f),
        new Vector2(2.5f, -1.1f),
        new Vector2(1.8f, -2.7f),
    };

    [Header("Initial spawn count")]
    public int initialSpawnCount = 3;

    [Header("Respawn cooldown (seconds)")]
    public float respawnMin = 1.5f;
    public float respawnMax = 4.0f;

    [Header("Auto fill empty points until all 6 are filled")]
    public bool autoFillUntilFull = true;
    public float fillIntervalMin = 0.8f;
    public float fillIntervalMax = 2.0f;

    private GameObject[] spawnedAtPoint;

    private void Awake()
    {
        spawnedAtPoint = new GameObject[spawnPositions.Length];
    }

    private void Start()
    {
        SpawnInitial();

        if (autoFillUntilFull)
            StartCoroutine(FillEmptyUntilFull());
    }

    private void SpawnInitial()
    {
        int count = Mathf.Clamp(initialSpawnCount, 0, spawnPositions.Length);

        List<int> indices = new List<int>();
        for (int i = 0; i < spawnPositions.Length; i++)
            indices.Add(i);

        for (int i = 0; i < indices.Count; i++)
        {
            int j = Random.Range(i, indices.Count);
            (indices[i], indices[j]) = (indices[j], indices[i]);
        }

        for (int k = 0; k < count; k++)
            SpawnAt(indices[k]);
    }

    public void SpawnAt(int pointIndex)
    {
        if (!IsSetupValid()) return;
        if (pointIndex < 0 || pointIndex >= spawnPositions.Length) return;

        // 同地点に複数防止
        if (spawnedAtPoint[pointIndex] != null) return;

        GameObject prefab = GetRandomPrefab();
        if (prefab == null) return;

        Vector2 pos = spawnPositions[pointIndex];
        GameObject instance = Instantiate(prefab, pos, Quaternion.identity);

        var item = instance.GetComponent<SpawnedItem>();
        if (item == null) item = instance.AddComponent<SpawnedItem>();

        item.Initialize(this, pointIndex);

        spawnedAtPoint[pointIndex] = instance;
    }

    public void NotifyPickedUp(int pointIndex, GameObject pickedInstance)
    {
        if (pointIndex < 0 || pointIndex >= spawnedAtPoint.Length) return;

        // Destroy済み参照事故防止：一致してたらクリア
        if (spawnedAtPoint[pointIndex] == pickedInstance)
            spawnedAtPoint[pointIndex] = null;

        float ct = Random.Range(respawnMin, respawnMax);
        StartCoroutine(RespawnAfter(pointIndex, ct));
    }

    private IEnumerator RespawnAfter(int pointIndex, float seconds)
    {
        yield return new WaitForSeconds(seconds);

        if (spawnedAtPoint[pointIndex] != null) yield break;

        SpawnAt(pointIndex);
    }

    // 放置で空き地点を埋めていき、最終的に全点(6)埋まったら終了
    private IEnumerator FillEmptyUntilFull()
    {
        while (true)
        {
            bool anyEmpty = false;
            for (int i = 0; i < spawnedAtPoint.Length; i++)
            {
                if (spawnedAtPoint[i] == null)
                {
                    anyEmpty = true;
                    break;
                }
            }
            if (!anyEmpty) yield break;

            float wait = Random.Range(fillIntervalMin, fillIntervalMax);
            yield return new WaitForSeconds(wait);

            List<int> empties = new List<int>();
            for (int i = 0; i < spawnedAtPoint.Length; i++)
                if (spawnedAtPoint[i] == null) empties.Add(i);

            if (empties.Count > 0)
            {
                int idx = empties[Random.Range(0, empties.Count)];
                SpawnAt(idx);
            }
        }
    }

    private GameObject GetRandomPrefab()
    {
        if (itemPrefabs == null || itemPrefabs.Length == 0) return null;

        // null(Missing)混入してても落ちない
        for (int attempt = 0; attempt < 10; attempt++)
        {
            var p = itemPrefabs[Random.Range(0, itemPrefabs.Length)];
            if (p != null) return p;
        }
        return null;
    }

    private bool IsSetupValid()
    {
        if (itemPrefabs == null || itemPrefabs.Length == 0)
        {
            Debug.LogError("[Spawner] itemPrefabs is empty.");
            return false;
        }
        if (spawnPositions == null || spawnPositions.Length == 0)
        {
            Debug.LogError("[Spawner] spawnPositions is empty.");
            return false;
        }
        return true;
    }
}