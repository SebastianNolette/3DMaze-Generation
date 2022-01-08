using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement3D : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float speed;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Jump");
        float z = Input.GetAxisRaw("Vertical");

        if (y >0)
        {
            rb.useGravity = false;
        }
        else
        {
            rb.useGravity = true;
        }

        Vector3 moveBy = transform.right * x + transform.forward * z + transform.up * y;
        rb.MovePosition(transform.position + moveBy.normalized * speed * Time.deltaTime);
    }
}
