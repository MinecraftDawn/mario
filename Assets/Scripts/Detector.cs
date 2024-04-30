using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public float detectInterval = 0.03f;
    protected bool _detected = false;
    protected GameObject detectedObject = null;
    private float timer = 0.0f;

    void Start()
    {
        StartCoroutine(DetectCoroutine());
    }

    protected virtual IEnumerator DetectCoroutine()
    {
        WaitForSeconds wait = new WaitForSeconds(detectInterval);
        while (true) {
            yield return wait;
            Debug.Log("Check");
            timer = detectInterval;
            Collider2D collide = Detect();
            _detected = collide;
            detectedObject = _detected ? collide.gameObject : null;
        }
    }

    protected virtual Collider2D Detect() { return null; }

    public bool IsDetected() { return _detected; }
    public GameObject GetDetectedObject() { return detectedObject; }
}
