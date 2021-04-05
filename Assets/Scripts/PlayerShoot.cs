using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public float fireRate = 0.2f;
    public Transform firingPoint;
    public GameObject bulletPrefab;
    public GameObject orbPrefab;
    float timeUntilFire;
    public int ammoOrb;
    Player pm;

    private void Start()
    {
        pm = gameObject.GetComponent<Player>();
    }

    private void Update()
    {  
        //Check ButtonDown, check direction of the player, create bullet.
        if (Input.GetButtonDown("Fire3") && timeUntilFire < Time.time)
        {
            float angle = pm.isFacingRight ? 0f : 180f;
            Instantiate(bulletPrefab, firingPoint.position, Quaternion.Euler(new Vector3(0f, 0f, angle)));
            timeUntilFire = Time.time + fireRate;
        }

        //Check ButtonDown, check direction of the player, create orb.
        if (Input.GetButtonDown("Fire2") && ammoOrb == 1)
        {        
            float angle = pm.isFacingRight ? 0f : 180f;
            Instantiate(orbPrefab, firingPoint.position, Quaternion.Euler(new Vector3(0f, 0f, angle)));
            timeUntilFire = Time.time + fireRate;
            ammoOrb = 0;
            
        }

        //Recharge
        if (Input.GetButtonDown("Fire1") && ammoOrb == 0)
        {
            ammoOrb = 1;
        }
    }        
}
