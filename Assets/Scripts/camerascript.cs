using UnityEngine;

public class CameraFollowZ : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;        // Drag your Can/Block here
    public float smoothSpeed = 0.125f; // How "snappy" the follow is (0 to 1)

    [Header("Offset")]
    public Vector3 offset;          // The distance between camera and block

    private void Start()
    {
        // Automatically calculate the initial offset if not set
        if (offset == Vector3.zero && target != null)
        {
            offset = transform.position - target.position;
        }
    }

    // LateUpdate is best for cameras so the target moves first
    void LateUpdate()
    {
        if (target == null) return;

        // We keep our current X and Y, but take the Target's Z + Offset
        Vector3 desiredPosition = new Vector3(transform.position.x, transform.position.y, target.position.z + offset.z);

        // Smoothly slide to that position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;
    }
}