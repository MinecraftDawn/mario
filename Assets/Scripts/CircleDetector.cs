using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CircleDetector : Detector
{
    [SerializeField]
    private float _fromDegree = 0f;
    [SerializeField]
    private float _toDegree = 180f;
    [SerializeField]
    private float _radius = 5f;
    public Color undetectedColor;
    public Color detectedColor;
    public LayerMask detectLayer;
    public Vector2 offset = Vector2.zero;
    public int numArcLines = 0;

    void OnDrawGizmos()
    {
        Vector3 center = transform.position + (Vector3)offset;
        Gizmos.color = _detected ? detectedColor : undetectedColor;
        Gizmos.DrawWireSphere(center, _radius);
        Gizmos.color = Color.red;
        Vector3 from = Quaternion.Euler(0, 0, _fromDegree) * Vector3.right;
        Vector3 to = Quaternion.Euler(0, 0, _toDegree) * Vector3.right;
        Gizmos.DrawLine(center, center + from * _radius);
        Gizmos.DrawLine(center, center + to * _radius);
        for (int i = 0; i < numArcLines; i++) {
            float degree = (_toDegree - _fromDegree) * (i + 1) / (numArcLines + 1) + _fromDegree;
            Vector3 toward = Quaternion.Euler(0, 0, degree) * Vector3.right;
            Gizmos.DrawLine(center, center + toward * _radius);
        }
    }

    protected override Collider2D Detect()
    {
        Vector3 center = transform.position + (Vector3)offset;
        Collider2D collider = Physics2D.OverlapCircle(center, _radius, detectLayer);
        if (!collider) { return null; }
        Debug.Log("player in circle");
        Vector2 to_player = collider.transform.position - center;
        float angle = Vector2.SignedAngle(Vector2.right, to_player);
        if (angle < 0) { angle = 360 + angle; }
        if (angle < _fromDegree || angle > _toDegree) { return null; }
        return collider;
    }

}
