using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbovePlayerCorrection : MonoBehaviour
{
    void Update()
    {
        var parentRot = transform.parent.rotation;
        var parentPos = transform.parent.position;
        transform.rotation = Quaternion.Euler(0f, 0f, -parentRot.z);
        transform.position = transform.parent.position + Vector3.up * 0.5f;
    }
}
