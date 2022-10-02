using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MouseLook), typeof(MovementRecorder))]
public class Attack : MonoBehaviour
{
    private MouseLook _look;
    private MovementRecorder _recorder;
    public GameObject bulletPrefab;
    public GameObject bulletSpawnPos;
    public GameObject lazerPrefab;
    public CoolDownTimer coolDownTimer;

    [System.NonSerialized]
    public long coolDownStartTime = 0;

    public float coolDown = 5;

    private bool lazerReady = true;

    private void Start()
    {
        _look = GetComponent<MouseLook>();
        _recorder = GetComponent<MovementRecorder>();

    }

    private void Update()
    {
        fire();
        coolDownTimer.updateSlider(GameManager.getEpochTime() - coolDownStartTime);
    }

    public void fire()
    {
        if (!GameManager.instance.canMove) return;

        if (Input.GetMouseButtonDown(0)) // fire bullet
        {
            InstantiateProjectile(bulletPrefab);
        }

        if (Input.GetMouseButtonDown(1) && lazerReady) // fire lazer
        {
            InstantiateProjectile(lazerPrefab);
            lazerReady = false;
            coolDownStartTime = GameManager.getEpochTime();
            StartCoroutine(EndRoutine());
        }
    }


    private void InstantiateProjectile(GameObject obj)
    {
        _recorder.Attacked();

        var bullet = Instantiate(obj,
                           bulletSpawnPos.transform.position,
                           transform.rotation
                           );
    }


    IEnumerator EndRoutine()
    {
        yield return new WaitForSeconds(coolDown);

        lazerReady = true;
    }

}
