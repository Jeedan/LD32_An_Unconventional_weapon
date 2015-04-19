using UnityEngine;
using System.Collections;

public class SmoothFollowCamera : MonoBehaviour
{

    public Transform target;
    public Vector3 offset = Vector3.zero;
    public float damping = 1.0f;

    private Vector3 targetOffset = Vector3.zero;

    void Start()
    {
        if (target)
        {
            targetOffset = transform.position - target.position;
            targetOffset += offset;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target)
        {
            Vector3 desiredPos = (target.position + targetOffset);

            desiredPos.y += offset.y;

            desiredPos.x += offset.x;

            desiredPos.z += offset.z;
            //      Vector3 lerpPos = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime * damping);
            transform.position = desiredPos;
            Quaternion lookRot = Quaternion.LookRotation(target.position - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRot, Time.deltaTime * 120f);
        }
    }
}
