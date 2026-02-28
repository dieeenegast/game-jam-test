using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CanController : MonoBehaviour
{
    [Header("Ground Movement")]
    public float kickForce = 12f;
    public float torqueForce = 8f;
    public float jumpForce = 7f;
    public float maxSpeed = 25f;
    public float moveCooldown = 0.12f;

    [Header("Air Movement (Ground-Like)")]
    public float airPushForce = 12f;
    public float airTorqueForce = 8f; // Now it will rotate in the air!

    [Header("Can Physics Customization")]
    public Vector3 kickOffset = new Vector3(0, 0.8f, 0);
    public ForceMode moveMode = ForceMode.Impulse;

    private Rigidbody rb;
    private bool isGrounded;
    private float nextMoveTime;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = 0.8f;
        rb.angularDamping = 0.8f;
    }

    void Update()
    {
        if (Time.time >= nextMoveTime)
        {
            HandleInput();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void HandleInput()
    {
        Vector3 inputDir = Vector3.zero;

        if (Input.GetKey(KeyCode.A)) inputDir += Vector3.forward;
        if (Input.GetKey(KeyCode.D)) inputDir += Vector3.back;
        if (Input.GetKey(KeyCode.W)) inputDir += Vector3.right;
        if (Input.GetKey(KeyCode.S)) inputDir += Vector3.left;

        if (inputDir != Vector3.zero)
        {
            ApplyCanPhysics(inputDir.normalized);
            nextMoveTime = Time.time + moveCooldown;
        }
    }

    void ApplyCanPhysics(Vector3 dir)
    {
        if (rb.linearVelocity.magnitude > maxSpeed) return;

        // Common Torque calculation for both ground and air
        Vector3 rollAxis = new Vector3(dir.z, 0, -dir.x);

        if (isGrounded)
        {
            // GROUND: Standard "kick" with offset to make it flop/tumble
            rb.AddForceAtPosition(dir * kickForce, transform.position + kickOffset, moveMode);
            rb.AddTorque(rollAxis * torqueForce, moveMode);
        }
        else
        {
            // AIR: Directional push (from center) + Pure Torque (spinning)
            // This moves the can forward AND rotates it without the "lever" effect
            rb.AddForce(dir * airPushForce, moveMode);
            rb.AddTorque(rollAxis * airTorqueForce, moveMode);
        }
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    private void OnCollisionStay(Collision col)
    {
        if (col.contacts[0].normal.y > 0.4f) isGrounded = true;
    }

    private void OnCollisionExit(Collision col)
    {
        isGrounded = false;
    }
}