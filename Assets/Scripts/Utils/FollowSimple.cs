using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowSimple : MonoBehaviour
{
    [Header("Refs")]
    public Transform followTarget;

    [Header("Mode")]
    public FollowMode followMode = FollowMode.LateUpdate;

    [Header("Position")]
    public bool followPosition = true;
    public bool offsetEnabled = true;
    public bool followLerp = false;
    public float followFactor = 1;

    [Header("Rotation")]
    public bool followRotation = false;
    public bool rotationLerp = false;
    public float rotationFactor = 1;

    private float timeDif;
    public enum FollowMode
    {
        Update, LateUpdate, FixedUpdate
    }

    Vector3 offset;
    void Awake()
    {
        if (followTarget != null)
            offset = offsetEnabled ? transform.position - followTarget.position : Vector3.zero;
    }

    private void LateUpdate()
    {
        if (followMode != FollowMode.LateUpdate || followTarget ==null) return;
        TimeCalc();
        FollowPosition();
        FollowRotation();
    }


    public void Update()
    {
        if (followMode != FollowMode.Update || followTarget == null) return;
        TimeCalc();
        FollowPosition();
        FollowRotation();

    }

    public void FixedUpdate()
    {
        if (followMode != FollowMode.FixedUpdate || followTarget == null) return;
        TimeCalc();
        FollowPosition();
        FollowRotation();
    }

    public void SetFollowOffset(Vector3 newOffset)
    {
        offsetEnabled = true;
        offset = newOffset;
    }
    private void TimeCalc()
    {
        timeDif = followMode switch { FollowMode.FixedUpdate => Time.fixedDeltaTime, _ => Time.deltaTime };
    }


    private void FollowRotation()
    {
        if (!followRotation) return;

        if (rotationLerp)
            transform.rotation = Quaternion.Lerp(transform.rotation, followTarget.rotation, timeDif * rotationFactor);
        else
            transform.rotation = followTarget.rotation;
    }



    private void FollowPosition()
    {
        if (!followPosition) return;

        Vector3 dir = transform.position - followTarget.position;
        dir.Normalize();

        //Vector3 finalPos = offsetEnabled && followRotation ? followTarget.position + (followTarget.rotation * offset) : followTarget.position;
        Vector3 finalPos = followTarget.position;

        if (offsetEnabled)
            finalPos += offset;

        if (followLerp)
            transform.position = Vector3.Lerp(transform.position, finalPos, timeDif * followFactor);
        else
            transform.position = finalPos;
    }
}
