using UnityEngine;

[RequireComponent(typeof(MouseLook), typeof(MovementRecorder))]
public class Attack : MonoBehaviour
{
    private MouseLook _look;
    private MovementRecorder _recorder;
    public GameObject bulletPrefab;
    public GameObject bulletSpawnPos;

    private void Start()
    {
        _look = GetComponent<MouseLook>();
        _recorder = GetComponent<MovementRecorder>();
    }

    private void Update()
    {
        fire();
    }

    public void fire()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        _recorder.Attacked();

        var bullet = Instantiate(bulletPrefab,
                                 bulletSpawnPos.transform.position,
                                 transform.rotation
                                 );

    }
}