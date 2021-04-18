using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public float fireRate = 0.2f;
    public Transform firingPoint;
    public GameObject bulletPrefab;
    public GameObject orbPrefab;
    float timeUntilFire;


    public bool begin;
    public float timerForOrb = 5;
    Player pm;

    private void Start()
    {
        pm = gameObject.GetComponent<Player>();       
    }

    private void Update()
    {
        timerForOrb += Time.deltaTime;
        //Check ButtonDown, check direction of the player, create bullet.
        if (Input.GetButtonDown("Fire3") && timeUntilFire < Time.time)
        {
            float angle = pm.isFacingRight ? 0f : 180f;
            Instantiate(bulletPrefab, firingPoint.position, Quaternion.Euler(new Vector3(0f, 0f, angle)));
            timeUntilFire = Time.time + fireRate;
        }

        if (Input.GetButtonDown("Fire2") && timerForOrb >= 5)
        {
            float angle = pm.isFacingRight ? 0f : 180f;
            Instantiate(orbPrefab, firingPoint.position, Quaternion.Euler(new Vector3(0f, 0f, angle)));
            timeUntilFire = Time.time + fireRate;
            begin = true;
        }

        if (begin == true)
        {
            timerForOrb = 0;
            begin = false;
        }
    }        
}
