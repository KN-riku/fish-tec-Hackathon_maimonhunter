using UnityEngine;

public class Squid : MonoBehaviour
{
    public float normalSpeed = 1.0f;
    public float attractSpeed = 2.5f;
    private bool isInLight = false;
    private Transform targetLight;
    public int direction = 1; // 1:右へ, -1:左へ

    void Update()
    {
        if (isInLight && targetLight != null)
        {
            // 特定の高さに固定せず、ライトの座標(targetLight.position)へ直接向かう
            // ライトが下にあればイカは降りてきます
            transform.position = Vector2.MoveTowards(
                transform.position,
                targetLight.position,
                attractSpeed * Time.deltaTime
            );
        }
        else
        {
            // ライトの外にいるときは横に泳ぐ
            transform.Translate(Vector2.right * direction * normalSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInLight = true;
            targetLight = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInLight = false;
        }
    }
}