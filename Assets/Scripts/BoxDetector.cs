using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDetector : Detector
{
    public float boxWidth = 1f;
    public float boxHight = 1f;
    public Color undetectedColor;
    public Color detectedColor;
    public LayerMask detectedLayer;

    private void OnDrawGizmos()
    {
        Gizmos.color = _detected ? detectedColor : undetectedColor;
        Vector3 region_box = Vector3.zero;
        region_box.x = boxWidth;
        region_box.y = boxHight;
        Gizmos.DrawCube(transform.position, region_box);
    }

    protected override Collider2D Detect()
    {
        Vector2 region_box = Vector2.zero;
        region_box.x = boxWidth;
        region_box.y = boxHight;
        return Physics2D.OverlapBox(transform.position, region_box, 0f, detectedLayer);
    }
}
