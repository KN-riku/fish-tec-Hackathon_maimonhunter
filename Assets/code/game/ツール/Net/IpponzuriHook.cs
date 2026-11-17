using UnityEngine;

public class IpponzuriHook : MonoBehaviour
{
    LineRenderer line;
    float seaSurfaceY;

    void Start()
    {
        line = GetComponent<LineRenderer>();

        // 海面位置を取得
        seaSurfaceY = FindObjectOfType<IpponzuriController>().seaSurfaceY;
    }

    void Update()
    {
        if (line == null) return;

        // 糸の始点（海面）
        Vector3 startPos =
            new Vector3(transform.position.x, seaSurfaceY, 0);

        // 終点（針）
        Vector3 endPos = transform.position;

        line.SetPosition(0, startPos);
        line.SetPosition(1, endPos);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Fish fish = col.GetComponent<Fish>();
        if (fish != null)
        {
            fish.CatchFish();
            Destroy(gameObject);
        }
    }
}