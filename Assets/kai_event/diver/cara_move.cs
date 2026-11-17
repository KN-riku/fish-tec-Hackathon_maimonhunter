using UnityEngine;

public class chara_move : MonoBehaviour
{
    [Header("Move Speed")]
    public float speedX = 2f;
    public float speedY = 2.5f;

    [Header("Screen Areas")]
    [Range(0.1f, 0.6f)]
    public float diveAreaHeightRatio = 0.30f;

    [Header("Horizontal Limit (} from start X)")]
    public float xLimit = 2.5f;

    [Header("Depth")]
    public float surfaceY = 0.5f;    // šŠC–ÊYãŒÀŒÅ’è
    public float minDepthY = -4f;

    [Header("Get (direction=5)")]
    public float getDuration = 0.5f;

    private Animator animator;
    private SpriteRenderer sr;

    private float baseX;

    private enum State { Surface, Diving, Rising }
    private State state = State.Surface;

    private bool isGetting = false;
    private float getTimer = 0f;
    private System.Action onGetComplete;

    private bool hasContactStop = false;
    private float contactStopY = 0f;

    private bool pickedThisDive = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        // š‰ŠúˆÊ’u‚ğŒÅ’è (0, 0.5)
        transform.position = new Vector3(0f, 0.5f, transform.position.z);

        baseX = transform.position.x;

        pickedThisDive = false;
        state = State.Surface;
    }

    void Update()
    {
        Vector3 pos = transform.position;

        // ===== Ìæ’† =====
        if (isGetting)
        {
            getTimer -= Time.deltaTime;
            animator.SetInteger("direction", 5);

            if (getTimer <= 0f)
            {
                isGetting = false;
                hasContactStop = false;

                var cb = onGetComplete;
                onGetComplete = null;
                cb?.Invoke();
            }
            return;
        }

        bool holdDiveArea = IsHoldingDiveArea();

        // ===== ó‘Ô‘JˆÚ =====
        if (state == State.Surface)
        {
            if (holdDiveArea)
            {
                state = State.Diving;
                pickedThisDive = false;
            }
        }
        else if (state == State.Diving)
        {
            if (!holdDiveArea) state = State.Rising;
        }
        else if (state == State.Rising)
        {
            if (pos.y >= surfaceY)
            {
                pos.y = surfaceY;
                state = State.Surface;
                pickedThisDive = false;
            }
        }

        int direction = 0;

        // ===== ˆÚ“®ˆ— =====
        if (state == State.Surface)
        {
            pos.y = surfaceY; // šãŒÀŒÅ’è

            int horizontal = GetHoldHorizontal();
            if (horizontal != 0)
            {
                pos.x += horizontal * speedX * Time.deltaTime;
                direction = (horizontal == -1) ? 3 : 4;
                sr.flipX = (horizontal == -1);
            }
            else
            {
                direction = 0;
            }
        }
        else if (state == State.Diving)
        {
            pos.y -= speedY * Time.deltaTime;

            if (hasContactStop)
                pos.y = Mathf.Max(pos.y, contactStopY);
            else
                pos.y = Mathf.Max(pos.y, minDepthY);

            direction = 2;
        }
        else // Rising
        {
            pos.y += speedY * Time.deltaTime;
            direction = 1;
        }

        // šãŒÀ§ŒÀi0.5ˆÈã‚És‚©‚È‚¢j
        pos.y = Mathf.Min(pos.y, surfaceY);

        pos.x = Mathf.Clamp(pos.x, baseX - xLimit, baseX + xLimit);

        transform.position = pos;
        animator.SetInteger("direction", direction);
    }

    public bool CanPickupNow()
    {
        if (isGetting) return false;
        if (state == State.Surface) return false;
        if (pickedThisDive) return false;
        return true;
    }

    public bool TriggerGet(System.Action onComplete = null)
    {
        if (!CanPickupNow()) return false;

        pickedThisDive = true;

        // š‹­§‹AŠÒ
        state = State.Rising;

        hasContactStop = true;
        contactStopY = transform.position.y;

        isGetting = true;
        getTimer = getDuration;

        onGetComplete = onComplete;
        return true;
    }

    bool IsHoldingDiveArea()
    {
        float thresholdY = Screen.height * diveAreaHeightRatio;

        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began || t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary)
                return (t.position.y <= thresholdY);
            return false;
        }

        if (Input.GetMouseButton(0))
            return (Input.mousePosition.y <= thresholdY);

        return false;
    }

    int GetHoldHorizontal()
    {
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began || t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary)
                return (t.position.x < Screen.width * 0.5f) ? -1 : 1;
            return 0;
        }

        if (Input.GetMouseButton(0))
            return (Input.mousePosition.x < Screen.width * 0.5f) ? -1 : 1;

        return 0;
    }
}