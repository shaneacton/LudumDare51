using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MouseLook), typeof(MovementRecorder))]
public class Attack : MonoBehaviour
{
    private MouseLook _look;
    private MovementRecorder _recorder;
    public GameObject bulletPrefab;
    public GameObject bulletSpawnPos;
    public GameObject lazerPrefab;
    public CoolDownTimer coolDownTimer;
    public Slider chargeUpBar;

    public Animator animator;

    [System.NonSerialized]
    public long coolDownStartTime = 0;

    public float coolDown = 5;

    public bool lazerReady = true;

    private float lastShotTime;

    public float lazerShootingTime = 0.35f;

    public float lazerChargeTime = 0.5f;

    private float timeCharging = -1;

    public CameraShake shake;

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
            animator.SetBool("isAttacking", true);
            lastShotTime = Time.time;
        }

        if (Input.GetMouseButtonDown(1) && lazerReady)
        {
            StartCoroutine(shake.Shake(lazerChargeTime, 0.1f));
        }

        bool holdingLazer = Input.GetMouseButton(1) && lazerReady;
        if (holdingLazer)
        {
            chargeUpBar.gameObject.SetActive(true);
            chargeUpBar.value = timeCharging / lazerChargeTime;
            chargeUpBar.gameObject.GetComponent<RectTransform>().position = transform.position;
            if (lazerReady && timeCharging > lazerChargeTime)
            {
                _recorder.Attacked(AttackType.Lazer);
                InstantiateProjectile(lazerPrefab);
                AudioManager.Play("Lazer");
                lazerReady = false;
                chargeUpBar.gameObject.SetActive(false);
                coolDownStartTime = GameManager.getEpochTime();
                StartCoroutine(EndRoutine());
                StartCoroutine(ShootingLazer());
                timeCharging = 0;
            }
            else
            {
                timeCharging += Time.deltaTime;
            }
        }
        else
        {
            chargeUpBar.gameObject.SetActive(false);
            timeCharging = 0;
        }

        if ((Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)) || Time.time - lastShotTime > 0.5f)
        {
            animator.SetBool("isAttacking", false);
        }
    }


    private GameObject InstantiateProjectile(GameObject obj)
    {

        var bullet = Instantiate(obj,
                           bulletSpawnPos.transform.position,
                           transform.rotation
                           );
        return bullet;
    }

    IEnumerator ShootingLazer()
    {
        GameManager.instance.canMove = false;
        yield return new WaitForSeconds(lazerShootingTime);
        GameManager.instance.canMove = true;
    }

    IEnumerator EndRoutine()
    {
        yield return new WaitForSeconds(coolDown);

        lazerReady = true;
    }

}
