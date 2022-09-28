using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(MouseLook))]
public class Attack : MonoBehaviour
{

    private MouseLook _look;
    public GameObject bulletPrefab;
    public GameObject bulletSpawnPos;

    private void Start()
    {
        _look = GetComponent<MouseLook>();
    }

    private void Update()
    {
        fire();
    }

    private void fire()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        var bullet = Instantiate(bulletPrefab,
                                 bulletSpawnPos.transform.position,
                                 transform.rotation,
                                 transform);

    }
}