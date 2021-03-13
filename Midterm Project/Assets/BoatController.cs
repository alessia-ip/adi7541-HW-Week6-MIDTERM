using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{

    public GameObject disembark;
    private ContactPoint2D _contactPoint;
    private GameObject pirate;
    
    
    float speed = 0f;
    private float maxSpeed = 0.3f;
    float accel = .01f;
    
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        GameObject.FindWithTag("MainCamera").GetComponent<FollowPlayer>().player = this.gameObject;
        disembark = GameObject.FindWithTag("MainCamera").transform.GetChild(0).gameObject;
        pirate = GameObject.FindWithTag("Disembark");
        pirate.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKey(KeyCode.A))
        {
            rb.rotation += 0.2f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            rb.rotation -= 0.2f;
        }*/

        if (Input.GetKeyDown(KeyCode.X) && disembark.activeInHierarchy == true)
        {
            GetComponent<PolygonCollider2D>().isTrigger = true;
            pirate.transform.position = _contactPoint.point;
            pirate.SetActive(true);
            disembark.SetActive(false);
            GetComponent<BoatController>().enabled = false;
            rb.velocity = Vector2.zero;
        }
        
        
    }

    
    
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (speed != -maxSpeed)
            {
                speed -= accel;
                rb.AddForce((transform.up * speed));
            }
        }
        else
        {
            speed = 0;
        }

        rb.AddTorque(Input.GetAxis("Horizontal") * -0.1f);

        rb.velocity = transform.up * Vector2.Dot(rb.velocity, transform.up);
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Shore")
        {
            disembark.SetActive(true);
            _contactPoint = other.GetContact(0);
        }
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Shore")
        {
            disembark.SetActive(false);
        }
    }
    
}
