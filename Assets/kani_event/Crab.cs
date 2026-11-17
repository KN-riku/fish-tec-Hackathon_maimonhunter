using UnityEngine;

public class Crab : MonoBehaviour
{
    [Header("References")]
    public Transform net;
    public CrabSpawner spawner;

    [Header("Crab Info")]
    public CrabID crabID;
    public int scorePoints; // Spawnerから設定される

    [Header("Move")]
    public float walkSpeed = 1.2f;
    public float escapeBaseSpeed = 2.5f;
    public float escapeSpeedByNetSpeed = 0.8f;
    public float escapeMaxSpeed = 6.0f;

    [Header("Move Area (World X)")]
    public float minX = -2.5f;
    public float maxX = 2.5f;

    [Header("Escape")]
    public float escapeDistance = 1.5f;
    [Range(0f, 1f)]
    public float escapeChance = 1.0f;

    [Header("Net Speed Smoothing")]
    [Range(0f, 1f)]
    public float netSpeedSmoothing = 0.85f;

    [Header("Catch")]
    public float catchTime = 1.0f;

    [Header("Life Time")]
    public float lifeTime = 10f;

    private float dir;
    private Vector3 lastNetPos;
    private float netSpeedSmoothed;

    private bool isTouchingNet = false;
    private float catchTimer = 0f;
    private float lifeTimer = 0f;
    private bool isRemoved = false;

    void Start()
    {
        dir = (Random.value < 0.5f) ? -1f : 1f;
        if (net == null)
        {
            GameObject obj = GameObject.Find("Net");
            if (obj == null) obj = GameObject.Find("NET");
            if (obj != null) net = obj.transform;
        }
        if (spawner == null) spawner = FindFirstObjectByType<CrabSpawner>();
        if (net != null)
        {
            lastNetPos = net.position;
            netSpeedSmoothed = 0f;
        }
    }

    void Update()
    {
        if (isRemoved) return;

        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lifeTime) { Despawn(); return; }

        if (transform.position.x <= minX) dir = 1f;
        if (transform.position.x >= maxX) dir = -1f;

        float netSpeed = 0f;
        if (net != null)
        {
            float dt = Mathf.Max(Time.deltaTime, 0.0001f);
            netSpeed = Vector3.Distance(net.position, lastNetPos) / dt;
            lastNetPos = net.position;
            netSpeedSmoothed = Mathf.Lerp(netSpeed, netSpeedSmoothed, netSpeedSmoothing);
        }

        float speed = walkSpeed;
        if (net != null)
        {
            float dist = Vector2.Distance(transform.position, net.position);
            if (dist < escapeDistance && Random.value <= escapeChance)
            {
                dir = (transform.position.x < net.position.x) ? -1f : 1f;
                speed = escapeBaseSpeed + netSpeedSmoothed * escapeSpeedByNetSpeed;
                speed = Mathf.Min(speed, escapeMaxSpeed);
            }
        }

        transform.Translate(Vector3.right * dir * speed * Time.deltaTime);
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);

        if (isTouchingNet)
        {
            catchTimer += Time.deltaTime;
            if (catchTimer >= catchTime) Catch();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Net")) isTouchingNet = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Net")) { isTouchingNet = false; catchTimer = 0f; }
    }

    void Catch()
    {
        if (isRemoved) return;
        isRemoved = true;

        // --- 1. スコア加算 ---
        GameManager gm = FindFirstObjectByType<GameManager>();
        if (gm != null) gm.AddScore(scorePoints);

        // --- 2. SE再生 (判別をなくし、一律で通常のCatchSEを再生) ---
        if (GAME_SE.instance != null)
        {
            GAME_SE.instance.PlayCatchSE();
        }

        // --- 3. 画像表示 (画像は引き続きレアの時だけ出す場合) ---
        bool isRare = (crabID == CrabID.Kagayaki || crabID == CrabID.IchibanOshi || crabID == CrabID.Kaguyahime);
        if (isRare)
        {
            CatchImageManager imageManager = FindFirstObjectByType<CatchImageManager>();
            if (imageManager != null) imageManager.ShowCatchImage(crabID);
        }

        if (spawner != null) spawner.OnCrabRemoved(this);
        Destroy(gameObject);
    }

    void Despawn()
    {
        if (isRemoved) return;
        isRemoved = true;
        if (spawner != null) spawner.OnCrabRemoved(this);
        Destroy(gameObject);
    }
}