using UnityEngine;

[RequireComponent(typeof(MouseLook), typeof(MovementRecorder))]
public class Attack : MonoBehaviour
{
    private MouseLook _look;
    private MovementRecorder _recorder;
    public GameObject bulletPrefab;
    public GameObject bulletSpawnPos;
    public bool lazerOn = false;

    public LineRenderer lineRenderer;

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

        if (lazerOn)
        {

        }
        else
        {
            var bullet = Instantiate(bulletPrefab,
                               bulletSpawnPos.transform.position,
                               transform.rotation
                               );
        }
     
    }

    public void EnableLazer()
    {
        lineRenderer.enabled = true;
    }

    public void DisableLazer()
    {
        lineRenderer = false;
    }
}