using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CanController : MonoBehaviour
{
    [Header("Movement (A=+Z, D=-Z | W=+X, S=-X)")]
    public float kickForce = 12f;
    public float torqueForce = 8f;
    public float jumpForce = 7f;
    public float maxSpeed = 25f;

    [Header("Spam & Cooldown")]
    public float moveCooldown = 0.12f;
    private float nextMoveTime;

    [Header("Can Physics Customization")]
    [Tooltip("Offset for the push. Y > 0 makes the can tumble/flop.")]
    public Vector3 kickOffset = new Vector3(0, 0.8f, 0);
    public ForceMode moveMode = ForceMode.Impulse;

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Drag settings help the can 'settle' after tumbling
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

        // A/D mapped to Z-axis (A is now the "Forward/Larger Z" key)
        if (Input.GetKey(KeyCode.A)) inputDir += Vector3.forward; // Increases Z
        if (Input.GetKey(KeyCode.D)) inputDir += Vector3.back;    // Decreases Z

        // W/S mapped to X-axis
        if (Input.GetKey(KeyCode.W)) inputDir += Vector3.right;   // Increases X
        if (Input.GetKey(KeyCode.S)) inputDir += Vector3.left;    // Decreases X

        if (inputDir != Vector3.zero)
        {
            // Speed cap to keep the physics stable while spamming
            if (rb.linearVelocity.magnitude < maxSpeed)
            {
                ApplyCanPhysics(inputDir.normalized);
            }

            nextMoveTime = Time.time + moveCooldown;
        }
    }

    void ApplyCanPhysics(Vector3 dir)
    {
        // AddForceAtPosition makes it "flop" because the push point is offset from the center
        rb.AddForceAtPosition(dir * kickForce, transform.position + kickOffset, moveMode);

        // This adds a rolling torque perpendicular to the movement direction
        Vector3 rollAxis = new Vector3(dir.z, 0, -dir.x);
        rb.AddTorque(rollAxis * torqueForce, moveMode);
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    private void OnCollisionStay(Collision col)
    {
        // Simple ground check based on the angle of the surface hit
        if (col.contacts[0].normal.y > 0.4f) isGrounded = true;
    }

    private void OnCollisionExit(Collision col)
    {
        isGrounded = false;
    }
}