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
    public int numArcLines = 0;

    void OnDrawGizmos()
    {
        Gizmos.color = _detected ? detectedColor : undetectedColor;
        Gizmos.DrawWireSphere(transform.position, _radius);
        Gizmos.color = Color.red;
        Vector3 from = Quaternion.Euler(0, 0, _fromDegree) * Vector3.right;
        Vector3 to = Quaternion.Euler(0, 0, _toDegree) * Vector3.right;
        Gizmos.DrawLine(transform.position, transform.position + from * _radius);
        Gizmos.DrawLine(transform.position, transform.position + to * _radius);
        for (int i = 0; i < numArcLines; i++) {
            float degree = (_toDegree - _fromDegree) * (i + 1) / (numArcLines + 1) + _fromDegree;
            Vector3 toward = Quaternion.Euler(0, 0, degree) * Vector3.right;
            Gizmos.DrawLine(transform.position, transform.position + toward * _radius);
        }
    }

    protected override Collider2D Detect()
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, _radius, detectLayer);
        if (!collider) { return null; }
        Vector2 to_player = collider.transform.position - transform.position;
        float angle = Vector2.SignedAngle(Vector2.right, to_player);
        if (angle < 0) { angle = 360 + angle; }
        if (angle < _fromDegree || angle > _toDegree) { return null; }
        return collider;
    }

}
