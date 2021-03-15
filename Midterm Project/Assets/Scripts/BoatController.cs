using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{

    public GameObject disembark; //the text to let the player know to disembark
    private ContactPoint2D _contactPoint; //a contact point I am using to know exactly where the boat is touching the land
    private GameObject pirate; //the pirate player character avatar
    
    
    float speed = 0f; //boat speed
    private float maxSpeed = 0.3f; //maximum speed
    float accel = .01f; //acceleration
    
    private Rigidbody2D rb; //rigidbody reference

    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>(); //set the rigidbody to this rigidbody
        GameObject.FindWithTag("MainCamera").GetComponent<FollowPlayer>().player = this.gameObject; //set the camera to follow this gameobject
        disembark = GameObject.FindWithTag("MainCamera").transform.GetChild(0).gameObject; //get the disembark obj, which is a child of camera
        pirate = GameObject.FindWithTag("Disembark"); //get the pirate gameobject
        pirate.SetActive(false); //set the pirate to not be active - this does not need to be active unless the player is on land
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

        //x is to go on land - only when disembark is ON (which happens when the boat has contact with the shore)
        if (Input.GetKeyDown(KeyCode.X) && disembark.activeInHierarchy == true)
        {
            GetComponent<PolygonCollider2D>().isTrigger = true; //set the collider to a trigger so it doesn't get pushed away
            pirate.transform.position = _contactPoint.point; //move the player to the exact position where the boat touched the shore
            pirate.SetActive(true); //turn the player on
            disembark.SetActive(false); //turn off the disembark text
            GameObject.FindWithTag("MainCamera").GetComponent<FollowPlayer>().player = pirate.gameObject; //make the camera follow the pirate instead of the boat
            rb.velocity = Vector2.zero; //set the velocity to 0, 0, so the boat doesn't start to drift off
            GetComponent<BoatController>().enabled = false; //turn off this controller - the player should only be able to control the pirate until they re-embark
        }
        
        
    }

    
    
    private void FixedUpdate()
    {
        //based on the sprite direction, I need a -maxspeed and -speed insteadof the + vals, which go backward
        if (Input.GetKey(KeyCode.W))
        {
            if (speed != -maxSpeed) //if the speed isn't over the max
            {
                speed -= accel; //accelerate further
                rb.AddForce((transform.up * speed)); //then add speed to the boat!
            }
        }
        else
        {
            speed = 0; //otherwise set speed back to zero on release - this also lets the boat slow
        }

        //adding a horizontal torque
        // using positive torque would work more like a rudder (press d to go left)
        //but negative torque is more intuitive (d to go right)
        //this rotates the boat on the pivot at the set speed
        //i can't use transform.rotation because it doesn't change the force direction of the movement
        rb.AddTorque(Input.GetAxis("Horizontal") * -0.1f);

        //this next line is to normalize the direction of the force
        //without this, the boat continues to drift in the previous direction, even when you turn
        //Vector2.Dot show the dot product of the two vectors
        //in this case, the current velocity, and the forward velocity (based on sprite direction)
        Debug.Log("RB: " + rb.velocity);
        Debug.Log("RB DOT: " + Vector2.Dot(rb.velocity, transform.up));
        Debug.Log("RB TRANSFORM: " + transform.up * Vector2.Dot(rb.velocity, transform.up));
        //this points the velocity in the forward direction again! 
        rb.velocity = transform.up * Vector2.Dot(rb.velocity, transform.up);
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Shore")
        {
            disembark.SetActive(true);
            _contactPoint = other.GetContact(0); //this is the point where the ship touches the shore. We set this so we know where to put the pirate! 
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
