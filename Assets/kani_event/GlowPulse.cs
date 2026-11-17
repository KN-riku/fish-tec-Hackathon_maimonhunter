using UnityEngine;

public class GlowPulse : MonoBehaviour
{
    [Header("Pulse Settings")]
    public float pulseSpeed = 2f;      // –¬‘Å‚Â‘¬‚³
    public float pulseAmount = 0.2f;   // ‘å‚«‚³•Ï‰»—Ê

    private Vector3 baseScale;

    void Start()
    {
        baseScale = transform.localScale;
    }

    void Update()
    {
        float scale = 1 + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        transform.localScale = baseScale * scale;
    }
}