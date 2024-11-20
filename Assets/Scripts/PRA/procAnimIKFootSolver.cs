using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class procAnimIKFootSolver : MonoBehaviour
{
    [SerializeField] private LayerMask terrainLayer = default;
    [SerializeField] private Transform body = default;
    [SerializeField] private procAnimIKFootSolver[] otherFoot = default;
    [SerializeField] private float speed = 1;
    [SerializeField] private float stepDistance = 4;
    [SerializeField] private float stepLength = 4;
    [SerializeField] private float stepHeight = 1;
    [SerializeField] private Vector3 footOffset = default;

    //float footSpacing;
    Vector3 footSpacing;
    Vector3 oldPos, currentPos, newPos;
    Vector3 oldNormal, currentNormal, newNormal;
    float lerp;

    // Start is called before the first frame update
    void Start()
    {
        //footSpacing = transform.localPosition.x;
        footSpacing = transform.localPosition;
        currentPos = newPos = oldPos = transform.position;
        currentNormal = newNormal = oldNormal = transform.up;
        lerp = 1;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = currentPos;
        transform.up = currentNormal;

        //Ray ray = new Ray(body.position + (body.right * footSpacing), Vector3.down);
        Ray ray = new Ray(body.position + footSpacing, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit info, 10, terrainLayer.value))
        {
            for (int i = 0; i <= 6; i++)
            {
                if (Vector3.Distance(newPos, info.point) > stepDistance && !otherFoot[i].IsMoving() && lerp >= 1)
                {
                    lerp = 0;
                    int direction = body.InverseTransformPoint(info.point).z > body.InverseTransformPoint(newPos).z ? 1 : -1;
                    newPos = info.point + (body.forward * stepLength * direction) + footOffset;
                    newNormal = info.normal;
                }
            }
            //if (Vector3.Distance(newPos, info.point) > stepDistance && !otherFoot.IsMoving() && lerp >= 1)
            //{
            //    lerp = 0;
            //    int direction = body.InverseTransformPoint(info.point).z > body.InverseTransformPoint(newPos).z ? 1 : -1;
            //    newPos = info.point + (body.forward * stepLength * direction) + footOffset;
            //    newNormal = info.normal;
            //}

            //stepDistance += Random.Range(-0.1f, 0.1f);

            //if (Vector3.Distance(newPos, info.point) > stepDistance && lerp >= 1)
            //{
            //    //stepLength += Random.Range(-0.2f, 0.2f);

            //    lerp = 0;
            //    int direction = body.InverseTransformPoint(info.point).z > body.InverseTransformPoint(newPos).z ? 1 : -1;
            //    newPos = info.point + (body.forward * stepLength * direction) + footOffset;
            //    newNormal = info.normal;
            //    //Debug.Log(newNormal);
            //}
        }

        if (lerp < 1)
        {
            Vector3 tempPos = Vector3.Lerp(oldPos, newPos, lerp);
            tempPos.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            Debug.Log(lerp);

            currentPos = tempPos;
            //currentNormal = Vector3.Lerp(oldNormal, newNormal, lerp);

            lerp += Time.deltaTime * speed;
        }
        else
        {
            oldPos = newPos;
            oldNormal = newNormal;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(newPos, 0.2f);
    }

    public bool IsMoving()
    {
        return lerp < 1;
    }
}
