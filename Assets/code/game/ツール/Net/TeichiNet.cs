using UnityEngine;

public class TeichiNet : MonoBehaviour
{
    public float radius = 3f;
    public float suctionPower = 5f;
    public float lifeTime = 3f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        Collider2D[] hits =
            Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D col in hits)
        {
            Fish fish = col.GetComponent<Fish>();
            if (fish == null) continue;

            // íÜêSÇ÷à¯Ç´äÒÇπÇÈ
            fish.transform.position =
                Vector2.MoveTowards(
                    fish.transform.position,
                    transform.position,
                    suctionPower * Time.deltaTime
                );
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Fish fish = col.GetComponent<Fish>();
        if (fish != null)
        {
            fish.CatchFish();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}