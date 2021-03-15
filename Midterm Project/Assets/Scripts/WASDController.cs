using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Random = UnityEngine.Random;

public class WASDController : MonoBehaviour
{
    
    //this is specifically to control the pirate character on land! 
    
    //this is just the text to let you know how to embark 
    private GameObject embark;
    
    //this is the gameobject for the ship
    private GameObject ship;

    //this is the number of contact points with the land the person has
    public int contacts = 0;

    //this is the actual point of contact between two colliders
    private ContactPoint2D contactPoint2D;
    
    // Start is called before the first frame update
    void Start()
    {
        embark = GameObject.FindWithTag("MainCamera").transform.GetChild(1).gameObject; //find this gameobject automatically in the scene
    }

    // Update is called once per frame
    void Update()
    {
        
        //if embark is active (which only happens near the ship)
        //AND the player presses the designated key (e for embark)
        //get back on the ship
        if (Input.GetKeyDown(KeyCode.E) && embark.activeInHierarchy == true)
        {
            ship.GetComponent<PolygonCollider2D>().isTrigger = false; //set the ship to no longer be a trigger
            embark.SetActive(false); //turn off embark
            ship.GetComponent<BoatController>().enabled = true; //re-enable the boat controller
            GameObject.FindWithTag("MainCamera").GetComponent<FollowPlayer>().player = ship.gameObject; //change the camera follow to follow the ship again
            this.gameObject.SetActive(false); //turn off this object
        }

        //the below controls are all to move the player around using WASD
        if (Input.GetKey(KeyCode.W))
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + 1f * Time.deltaTime);
        }
        
        if (Input.GetKey(KeyCode.S))
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - 1f * Time.deltaTime);
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            transform.position = new Vector2(transform.position.x - 1f * Time.deltaTime, transform.position.y );
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            transform.position = new Vector2(transform.position.x + 1f * Time.deltaTime, transform.position.y);
        }

    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {

        //if the object you've collided with is the ship
        if (other.name.Contains("Ship"))
        {
            embark.SetActive(true); //turn the embark text on
            ship = other.gameObject; //set the reference to the ship gameobject
        }


        //if it's an 'x' marks the spot, we want to collect the treasure
        if (other.name.Contains("Spot"))
        {
            //increase the gold amount for the player from between 10 - 30 gold
            GameObject.FindWithTag("Game manager").GetComponent<ASCIITileParser>().GoldAmt =
                GameObject.FindWithTag("Game manager").GetComponent<ASCIITileParser>().GoldAmt + 10 * Random.Range(1, 3);
            Destroy(other.gameObject); //then destroy the treasure spot
        }
        
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {

        //another ship contact script
        if (other.name.Contains("Ship"))
        {
            embark.SetActive(false);
        }

        /*if (contacts == 0)
        {
            transform.position = previousPositions[previousPositions.Count - 20];
        }*/
    }

    //with contacts ++, we are keeping track how many pieces of terrain we are touching
    private void OnCollisionEnter2D(Collision2D other)
    {
        contacts++;
    }

    //we are setting the contact point so we can get the exact coordinates of the point
    private void OnCollisionStay2D(Collision2D other)
    {
       contactPoint2D = other.GetContact(0);
    }

    //
    private void OnCollisionExit2D(Collision2D other)
    {
        //contacts -- is to also help keep track of the terrain we are standing on
        contacts--;
        
        //Debug.Log("YEEEEET");
        
        
        //If we are at 0 contact points, we know we are in the water, where we shouldn't be!!!
        //in that case, we want to move the player back onto land
        if (contacts == 0) 
        {
            transform.position = contactPoint2D.point; //put the player back on the last spot where they were touching the ground
            //Debug.Log("ANGERY");
        }
    }
}
