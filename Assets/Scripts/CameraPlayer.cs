using UnityEngine;

public class CameraPlayer : MonoBehaviour
{
    public GameObject followPlayer;
    public Vector2 minCamPosition;
    public Vector2 maxCamPosition;
    public float smoothTime;

    private Vector2 velocity;

    private void FixedUpdate()
    {
        float posCamX = Mathf.SmoothDamp(transform.position.x, followPlayer.transform.position.x, ref velocity.x, smoothTime);
        float posCamY = Mathf.SmoothDamp(transform.position.y, followPlayer.transform.position.y, ref velocity.y, smoothTime);

        transform.position = new Vector3(Mathf.Clamp(posCamX, minCamPosition.x, maxCamPosition.x), Mathf.Clamp(posCamY, minCamPosition.y, maxCamPosition.y), transform.position.z);
    }




}
