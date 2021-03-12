
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using File = System.IO.File;

public class ASCIITileParser : MonoBehaviour
{
    private int levelNum = 1;

    public float tileSize = 1; //this is based on the pixel size of the tile
    
    //tile offsets
    public float xOffset;
    public float yOffset;
    
    private string FILE_NAME_TERRAIN = "TerrainNum.txt";
    private string FILE_NAME_OBJECTIVES = "ObjectiveNum.txt";
    private const string DIR = "/ASCII/";
    private string PATH_TO_TERRAIN;
    private string PATH_TO_OBJECTIVES;
    
    public GameObject instance;
    
    [Header("Terrain Prefabs - Sand")] 
    public GameObject sandbarTopLeft; //char 1
    public GameObject sandbarTopMiddle; //char 2
    public GameObject sandbarTopRight; //char 3   
    public GameObject sandbarCenterLeft; //char 4
    public GameObject sandbarCenterMiddle; //char 5
    public GameObject sandbarCenterRight; //char 6   
    public GameObject sandbarBottomLeft; //char 7
    public GameObject sandbarBottomMiddle; //char 8
    public GameObject sandbarBottomRight; //char 9
    
    [Header("Terrain Prefabs - Grass")] 
    public GameObject grassTopLeft; //a
    public GameObject grassTopMiddle; //b
    public GameObject grassTopMiddle2; //c
    public GameObject grassTopRight; //d
    public GameObject grassCenterLeft; //e
    public GameObject grassCenterLeft2; //f
    public GameObject grassCenterMiddle; //g
    public GameObject grassCenterMiddle2; //h
    public GameObject grassCenterMiddle3; //i
    public GameObject grassCenterMiddle4; //j
    public GameObject grassCenterRight; //k
    public GameObject grassCenterRight2; //l 
    public GameObject grassBottomLeft; //m
    public GameObject grassBottomMiddle; //n
    public GameObject grassBottomMiddle2; //o
    public GameObject grassBottomRight; //q

    [Header("Player Prefab")] 
    public GameObject playerBoatBlue;
    public GameObject playerBoatRed;
    public GameObject playerBoatYellow;

    [Header("Object Prefabs")] 
    public GameObject gold;


    // Start is called before the first frame update
    void Start()
    {
        //set instance
        if (instance != null)
        {
            instance = this.gameObject;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        PATH_TO_TERRAIN = Application.dataPath + DIR + FILE_NAME_TERRAIN;
        PATH_TO_TERRAIN = PATH_TO_TERRAIN.Replace("Num", levelNum + "");
        
        PATH_TO_OBJECTIVES = Application.dataPath + DIR + FILE_NAME_OBJECTIVES;
        PATH_TO_OBJECTIVES = PATH_TO_OBJECTIVES.Replace("Num", levelNum + "");

        
        ShoreParser();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShoreParser()
    {
        string[] terrain = File.ReadAllLines(PATH_TO_TERRAIN);
        
        for (int y = 0; y < terrain.Length; y++)
        {
            string currentLine = terrain[y]; //this is the current line of the ASCII file
            char[] characters = currentLine.ToCharArray(); //we want to get every character of that file

            for (int x = 0; x < characters.Length; x++)
            {
                GameObject level = new GameObject();
                
                GameObject newObj;
                char c = characters[x];

                switch (c)
                {
                    case '1': 
                        newObj = Instantiate<GameObject>(sandbarTopLeft);
                        break;
                    case '2':  
                        newObj = Instantiate<GameObject>(sandbarTopMiddle);
                        break;                    
                    case '3':
                        newObj = Instantiate<GameObject>(sandbarTopRight);
                        break;
                    case '4': 
                        newObj = Instantiate<GameObject>(sandbarCenterLeft);
                        break;
                    case '5': 
                        newObj = Instantiate<GameObject>(sandbarCenterMiddle);
                        break;                    
                    case '6': 
                        newObj = Instantiate<GameObject>(sandbarCenterRight);
                        break;
                    case '7': 
                        newObj = Instantiate<GameObject>(sandbarBottomLeft);
                        break;
                    case '8': 
                        newObj = Instantiate<GameObject>(sandbarBottomMiddle);
                        break;                    
                    case '9': 
                        newObj = Instantiate<GameObject>(sandbarBottomRight);
                        break;
                    case 'a':
                        newObj = Instantiate<GameObject>(grassTopLeft);
                        break;
                    case 'b':
                        newObj = Instantiate<GameObject>(grassTopMiddle);
                        break;
                    case 'c':
                        newObj = Instantiate<GameObject>(grassTopMiddle2);
                        break;
                    case 'd':
                        newObj = Instantiate<GameObject>(grassTopRight);
                        break;
                    default:
                        newObj = null;
                        break;
                }

                if (newObj != null)
                {
                    newObj.transform.position = new Vector2(x * tileSize + xOffset, -y * tileSize + yOffset);
                    newObj.GetComponent<SpriteRenderer>().sortingOrder = 0; //set the sort order so these are on the bottom
                    newObj.transform.parent = level.transform;
                }
                
            }
        }
        
    }
    
}
