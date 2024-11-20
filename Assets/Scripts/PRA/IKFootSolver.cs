using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootSolver : MonoBehaviour
{
    [SerializeField] IKFootSolver otherFoot = default;
    [SerializeField] Transform body = default;
    [SerializeField] LayerMask terrainLayer = default;
    [SerializeField] float speed = 4f;
    [SerializeField] float stepDistance = 4f;
    [SerializeField] float overheadAmount = 1f;
    [SerializeField] float stepHeight = 1f;
    [SerializeField] float lerpTarget = 1f;

    [SerializeField] private Vector3 footOffset = default;

    Vector3 oldPosition, currentPosition, newPosition, targetPointPosition, footSpacing;
    Transform parent;
    Vector3 axis = Vector3.up;
    float lerp;
    int direction = 1;

    private Quaternion startRot;
    private Quaternion endRot;

    private void Start()
    {
        footSpacing = transform.position - body.position;
        footSpacing = new Vector3(footSpacing.x, 0f, footSpacing.z);

        currentPosition = newPosition = oldPosition = transform.position;
        lerp = lerpTarget;
        parent = transform.parent;

        startRot = transform.rotation;
        endRot = body.rotation;
    }

    void Update()
    {
        transform.position = currentPosition;
        //transform.rotation = playerObject.rotation;

        //Ray ray = new Ray(body.TransformPoint(footSpacing), Vector3.down);
        Ray ray = new Ray(body.position + footSpacing, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit info, 10, terrainLayer))
        {
            targetPointPosition = info.point;
            if (CheckTargetPointDistance(transform.position) > stepDistance && !otherFoot.IsMoving() && lerp >= lerpTarget)
            {
                lerp = 0;
                //direction = body.InverseTransformPoint(info.point).z > body.InverseTransformPoint(newPosition).z ? 1 : -1;
                direction = body.TransformDirection(info.point).z > body.TransformDirection(newPosition).z ? 1 : -1;

                //newPosition = info.point + (body.forward * direction * overheadAmount) + footOffset;
                newPosition = info.point + (body.position * direction * overheadAmount) + footOffset;
            }
        }

        if (lerp < lerpTarget)
        {
            Vector3 tempPosition = Vector3.Lerp(oldPosition, newPosition, lerp);
            tempPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            transform.rotation = Quaternion.Slerp(startRot, endRot, lerp);

            currentPosition = tempPosition;
            lerp += Time.deltaTime * speed;
        }
        else
        {
            oldPosition = newPosition;
        }

        //Debug.Log(transform.localPosition);
    }
    // private Vector3 PointRelativeToTheBody(Vector3 pivot, Vector3 angles)
    // {
    //     Vector3 point;
    //     var dir = parent.position - pivot; // get point direction relative to pivot
    //     dir = Quaternion.Euler(angles) * dir; // rotate it
    //     point = dir + pivot; // calculate rotated point
    //     return point; // return it
    // }

    private float CheckTargetPointDistance(Vector3 point)
    {
        Debug.DrawLine(point, targetPointPosition, Color.red);
        // Debug.Log(Vector3.Distance(targetPointPosition, point));
        // Debug.Log(lerp);
        return Vector3.Distance(targetPointPosition, point);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(targetPointPosition, 0.2f);
    }

    public bool IsMoving()
    {
        return lerp < lerpTarget;
    }
}