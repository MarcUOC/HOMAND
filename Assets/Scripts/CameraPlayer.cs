using UnityEngine;

public class CameraPlayer : MonoBehaviour
{
    /*public Transform target;
    public Vector3 cameraOffset;
    public float followSpeed = 10f;
    private float xMin = 0f;
    Vector3 velocity = Vector3.zero;



    private void FixedUpdate()
    {
        //Smooth camera and follow player
        transform.position = target.position + cameraOffset;
        Vector3 targetPos = target.position + cameraOffset;
        Vector3 clampedPos = new Vector3(Mathf.Clamp(targetPos.x, xMin, float.MaxValue), targetPos.y, targetPos.z);
        Vector3 smoothPos = Vector3.SmoothDamp(transform.position, clampedPos, ref velocity, followSpeed * Time.deltaTime);
        transform.position = smoothPos;
    }*/

    public Transform player;
    public Vector3 offset;

    void Update()
    {
        transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, offset.z); 
    }

    /*public GameObject followPlayer;
    public Vector2 followOffset;
    public float speed;
    private Vector2 threshold;
    private Rigidbody2D rb;

    void Start()
    {
        threshold = calculateThreshold();
        rb = followPlayer.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 follow = followPlayer.transform.position;
        float xDiferrence = Vector2.Distance(Vector2.right * transform.position.x, Vector2.right * follow.x);
        float yDiferrence = Vector2.Distance(Vector2.up * transform.position.y, Vector2.up * follow.y);

        Vector3 newPosition = transform.position;
        if (Mathf.Abs(xDiferrence) >= threshold.x)
        {
            newPosition.x = follow.x;
        }
        if (Mathf.Abs(yDiferrence) >= threshold.y)
        {
            newPosition.y = follow.y;
        }
        float moveSpeed = rb.velocity.magnitude > speed ? rb.velocity.magnitude : speed;
        transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);
    }

    private Vector3 calculateThreshold()
    {
        Rect aspect = Camera.main.pixelRect;
        Vector2 t = new Vector2(Camera.main.orthographicSize * aspect.width / aspect.height, Camera.main.orthographicSize);
        t.x -= followOffset.x;
        t.y -= followOffset.y;
        return t;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector2 border = calculateThreshold();
        Gizmos.DrawWireCube(transform.position, new Vector3(border.x * 2, border.y * 2, 1));
    }*/

}
