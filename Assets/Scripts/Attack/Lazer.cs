using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Lazer : MonoBehaviour
{
    private float x_scale;
    public float scaleFactor = 5;
    public float timeout = 5;

    private void Start()
    {
        x_scale = transform.localScale.x;
    }

    public void IncreaseBeamSize()
    {
        x_scale += scaleFactor * Time.deltaTime;
        transform.localScale = new Vector3(x_scale,
                                            transform.localScale.y,
                                            transform.localScale.z);
    }

    public void DisableBeam()
    {
        StartCoroutine(EndRoutine());
    }

    IEnumerator EndRoutine()
    {
        yield return new WaitForSeconds(timeout);

        Destroy(gameObject);
    }
}
