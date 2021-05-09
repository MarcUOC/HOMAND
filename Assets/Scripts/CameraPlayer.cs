using UnityEngine;

public class CameraPlayer : MonoBehaviour
{
    public GameObject followPlayer;
    public Vector2 minCamPositionZone1;
    public Vector2 maxCamPositionZone1;
    public Vector2 minCamPositionZone2;
    public Vector2 maxCamPositionZone2;
    public Vector2 minCamPositionZone3;
    public Vector2 maxCamPositionZone3;
    public float smoothTime;

    private Vector2 velocity;

    private void FixedUpdate()
    {
        float posCamX = Mathf.SmoothDamp(transform.position.x, followPlayer.transform.position.x, ref velocity.x, smoothTime);
        float posCamY = Mathf.SmoothDamp(transform.position.y, followPlayer.transform.position.y, ref velocity.y, smoothTime);

        if (followPlayer.transform.position.x < 76f)
        {
            transform.position = new Vector3(Mathf.Clamp(posCamX, minCamPositionZone1.x, maxCamPositionZone1.x), Mathf.Clamp(posCamY, minCamPositionZone1.y, maxCamPositionZone1.y), transform.position.z);
        }

        if (followPlayer.transform.position.x > 76f)
        {
            transform.position = new Vector3(Mathf.Clamp(posCamX, minCamPositionZone2.x, maxCamPositionZone2.x), Mathf.Clamp(posCamY, minCamPositionZone2.y, maxCamPositionZone2.y), transform.position.z);
        }

        if (followPlayer.transform.position.x > 174.5f)
        {
            transform.position = new Vector3(Mathf.Clamp(posCamX, minCamPositionZone3.x, maxCamPositionZone3.x), Mathf.Clamp(posCamY, minCamPositionZone3.y, maxCamPositionZone3.y), transform.position.z);
        }
    }
}
