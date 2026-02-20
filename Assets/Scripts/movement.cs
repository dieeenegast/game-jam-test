using UnityEngine;

public class movement : MonoBehaviour
{
    Rigidbody rb;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A)) 
        {
            rb.AddForce(transform.up);
        
        
        }

    }
}
