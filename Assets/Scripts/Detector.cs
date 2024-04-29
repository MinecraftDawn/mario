using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Detector : MonoBehaviour
{
    protected bool _detected = false;
    protected GameObject detectedObject = null;

    void FixedUpdate()
    {
        Collider2D collide = Detect();
        _detected = collide;
        detectedObject = _detected ? collide.gameObject : null;
    }

    protected virtual Collider2D Detect() { return null; }

    public bool IsDetected() { return _detected; }
    public GameObject GetDetectedObject() { return detectedObject; }
}
