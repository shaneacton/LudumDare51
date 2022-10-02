using UnityEngine;

public class GhostAttack : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject lazerPrefab;
    public GameObject bulletSpawnPos;

    public void fire()
    {
        var bullet = Instantiate(bulletPrefab,
                                 bulletSpawnPos.transform.position,
                                 transform.rotation
                                );
    }

    public void lazer()
    {
        var bullet = Instantiate(lazerPrefab,
                                 bulletSpawnPos.transform.position,
                                 transform.rotation
                                );
    }
}
