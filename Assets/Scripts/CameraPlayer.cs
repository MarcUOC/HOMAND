using UnityEngine;

public class CameraPlayer : MonoBehaviour
{
    public Transform target;
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
    }
}
