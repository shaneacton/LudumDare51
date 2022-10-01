using UnityEngine;

public class GhostAttack : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject bulletSpawnPos;

    public void fire()
    {
        var bullet = Instantiate(bulletPrefab,
                                 bulletSpawnPos.transform.position,
                                 transform.rotation
                                );
    }
}
