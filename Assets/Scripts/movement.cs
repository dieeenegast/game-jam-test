using UnityEngine;

public class movement : MonoBehaviour
{
    Rigidbody rb;
    public float force = 40f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    public float moveCooldown = 0.5f;
    private float lastMoveTime;

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.A) && Time.time > lastMoveTime)
        {
            rb.AddForce(Vector3.left * force, ForceMode.Impulse);
            lastMoveTime = Time.time;
        }
    }
}
