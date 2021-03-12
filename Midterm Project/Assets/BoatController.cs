using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rb.rotation += 0.2f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            rb.rotation -= 0.2f;
        }

       
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Debug.Log("W");
            rb.AddForce(-transform.up * 0.1f * Time.deltaTime);
            Debug.Log(rb.velocity);
        }
        
    }
}
