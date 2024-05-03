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
    public LayerMask obstacleLayer;

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
        Collider2D collider = Physics2D.OverlapBox(transform.position, region_box, 0f, detectedLayer);

        if (collider) {
            Vector2 collider_center = collider.transform.position + (Vector3)collider.offset;
            Vector2 object_center =
                transform.parent.position + (Vector3)transform.parent.GetComponent<Collider2D>().offset;
            Vector2 toward = collider_center - object_center;

            RaycastHit2D hit = Physics2D.Raycast(object_center, toward, toward.magnitude, obstacleLayer);
            if (!hit) { return collider; }
        }
        return null;
    }
}
