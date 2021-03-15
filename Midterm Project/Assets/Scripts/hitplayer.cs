using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class hitplayer : MonoBehaviour
{
    public Sprite destroyed;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name.Contains("Ship"))
        {
            var currentGold = GameObject.FindWithTag("Game manager").GetComponent<ASCIITileParser>().GoldAmt;

            if (currentGold > 0)
            {
                //you lose gold if you hit a pirate ship
                GameObject.FindWithTag("Game manager").GetComponent<ASCIITileParser>().GoldAmt =
                    GameObject.FindWithTag("Game manager").GetComponent<ASCIITileParser>().GoldAmt - 10 * Random.Range(1, 3);
                if (currentGold < 0) //we can't have less than 0 gold!
                {
                    //you lose gold if you hit a pirate ship
                    GameObject.FindWithTag("Game manager").GetComponent<ASCIITileParser>().GoldAmt = 0;
                }
            }

            //then change the pirate sprite to be destroyed - you'll only lose the gold once
            this.gameObject.GetComponent<SpriteRenderer>().sprite = destroyed;
            this.gameObject.GetComponent<hitplayer>().enabled = false;
        }
    }
}
