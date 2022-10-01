using UnityEngine;

[RequireComponent(typeof(MouseLook), typeof(MovementRecorder))]
public class Attack : MonoBehaviour
{
    private MouseLook _look;
    private MovementRecorder _recorder;
    public GameObject bulletPrefab;
    public GameObject bulletSpawnPos;
    public GameObject lazerPrefab;
    public bool lazerOn = false;


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
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }


        _recorder.Attacked();


        var projectilePrefab = lazerOn ? lazerPrefab : bulletPrefab;


        var bullet = Instantiate(projectilePrefab,
                           bulletSpawnPos.transform.position,
                           transform.rotation
                           );

    }

}