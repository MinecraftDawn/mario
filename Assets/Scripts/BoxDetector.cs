using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDetector : MonoBehaviour
{
    public float boxWidth = 1f;
    public float boxHight = 1f;
    public Color undetectedColor;
    public Color detectedColor;
    public LayerMask detectedLayer;
    private bool detected = false;

    void FixedUpdate()
    {
        Vector2 region_box = Vector3.zero;
        region_box.x = boxWidth;
        region_box.y = boxHight;
        Collider2D collide = Physics2D.OverlapBox(transform.position, region_box, 0f, detectedLayer);
        detected = collide;
    }

    private void OnDrawGizmos()
    {
        if (detected) {
            Gizmos.color = detectedColor;
        } else {
            Gizmos.color = undetectedColor;
        }
        Vector3 region_box = Vector3.zero;
        region_box.x = boxWidth;
        region_box.y = boxHight;
        Gizmos.DrawCube(transform.position, region_box);
    }
}
