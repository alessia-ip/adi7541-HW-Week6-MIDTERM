using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class WASDController : MonoBehaviour
{
    private GameObject embark;
    private GameObject ship;

    public int contacts = 0;


    private ContactPoint2D contactPoint2D;
    
    // Start is called before the first frame update
    void Start()
    {
        embark = GameObject.FindWithTag("MainCamera").transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
        
        if (Input.GetKeyDown(KeyCode.E) && embark.activeInHierarchy == true)
        {
            ship.GetComponent<PolygonCollider2D>().isTrigger = false;
            embark.SetActive(false);
            ship.GetComponent<BoatController>().enabled = true;
            GameObject.FindWithTag("MainCamera").GetComponent<FollowPlayer>().player = ship.gameObject;
            this.gameObject.SetActive(false);
        }

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

        
        if (other.name.Contains("Ship"))
        {
            embark.SetActive(true);
            ship = other.gameObject;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {

        
        if (other.name.Contains("Ship"))
        {
            embark.SetActive(false);
        }

        /*if (contacts == 0)
        {
            transform.position = previousPositions[previousPositions.Count - 20];
        }*/
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        contacts++;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
       contactPoint2D = other.GetContact(0);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        contacts--;
        
        Debug.Log("YEEEEET");
        if (contacts == 0)
        {
            transform.position = contactPoint2D.point;
            Debug.Log("ANGERY");
        }
    }
}
