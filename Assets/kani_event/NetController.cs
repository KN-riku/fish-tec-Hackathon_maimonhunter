using UnityEngine;

public class NetController : MonoBehaviour
{
    [Header("Start Position (World)")]
    public Vector2 startPos = new Vector2(0f, 2.5f);

    [Header("Move Range (World)")]
    public float minX = -2.5f;
    public float maxX = 2.5f;
    public float minY = -1.5f;
    public float maxY = 4.0f;

    [Header("Follow")]
    public float followSpeed = 25f;

    private Camera cam;
    private float fixedZ;

    void Start()
    {
        cam = Camera.main;

        // 初期位置
        transform.position = new Vector3(startPos.x, startPos.y, transform.position.z);
        fixedZ = transform.position.z;
    }

    void Update()
    {
        // 押している間だけ追従（網自体は常に表示）
        if (!Input.GetMouseButton(0)) return;

        float zDist = Mathf.Abs(cam.transform.position.z - fixedZ);
        Vector3 world = cam.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, zDist)
        );

        float x = Mathf.Clamp(world.x, minX, maxX);
        float y = Mathf.Clamp(world.y, minY, maxY);

        Vector3 targetPos = new Vector3(x, y, fixedZ);

        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
    }
}