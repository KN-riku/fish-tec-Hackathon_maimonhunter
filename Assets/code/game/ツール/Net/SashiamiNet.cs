using UnityEngine;

public class SashiamiNet : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        Fish fish = col.GetComponent<Fish>();

        if (fish != null)
        {
            fish.CatchFish();
        }
    }
}