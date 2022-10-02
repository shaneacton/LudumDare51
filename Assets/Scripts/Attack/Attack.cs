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

    public bool lazerReady = true;

    private void Start()
    {
        _look = GetComponent<MouseLook>();
        _recorder = GetComponent<MovementRecorder>();
        coolDownTimer.SetMax(coolDown);

    }

    private void Update()
    {
        fire();

        if (!lazerReady)
        {
            coolDownTimer.UpdateSlider(GameManager.getEpochTime() - coolDownStartTime);
        }
        else
            coolDownTimer.UpdateSlider(coolDown);
    }

    public void fire()
    {
        if (!GameManager.instance.canMove) return;

        if (Input.GetMouseButtonDown(0)) // fire bullet
        {
            _recorder.Attacked(AttackType.Shoot);
            AudioManager.Play("Pistol");
            InstantiateProjectile(bulletPrefab);
        }

        if (Input.GetMouseButtonDown(1) && lazerReady) // fire lazer
        {
            _recorder.Attacked(AttackType.Lazer);
            InstantiateProjectile(lazerPrefab);
            lazerReady = false;
            coolDownStartTime = GameManager.getEpochTime();
            StartCoroutine(EndRoutine());
        }
    }


    private void InstantiateProjectile(GameObject obj)
    {

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
