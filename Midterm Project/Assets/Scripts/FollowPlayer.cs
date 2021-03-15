using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    //this can be the ship OR the pirate, depending on what the player is currently doing
    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        //the new position should be the player's x & y, but the camera's z pos
        var newPos = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        transform.position = newPos; //then move the camera to the new position
    }



}
