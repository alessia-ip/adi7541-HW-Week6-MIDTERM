
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using File = System.IO.File;
using Random = UnityEngine.Random;


public class ASCIITileParser : MonoBehaviour
{
    private int levelNum = 1;

    public float tileSize = 1; //this is based on the pixel size of the tile

    public int goldAMT;

    //keeps track of the player's collected gold! 
    public int GoldAmt
    {
        get
        {
            return goldAMT;
        }
        set
        {
            goldAMT = value;
            File.WriteAllText(PATH_TO_GOLD, goldAMT + "");
            var dailySpots = GameObject.FindGameObjectsWithTag("GOLD").Length;
            goldTXT.text = "Current gold: " + goldAMT + "\n \nTreasure remaining today: " + dailySpots;
        }
    }

    //UI element to print out gold claimed text!
    public TextMeshProUGUI goldTXT; 
    
    //I think i probably didn't use this anywhere
    //tile offsets
    public float xOffset;
    public float yOffset;
    
    //All strings for file names/important files
    //didn't use the time one I think. 
    //TODO add in a time tracker for how long the player has played for
    private string FILE_NAME_TERRAIN = "TerrainNum.txt";
    private string FILE_NAME_OBJECTIVES = "ObjectiveNum.txt";
    private const string DIR = "/ASCII/";
    private string PATH_TO_TERRAIN;
    private string PATH_TO_OBJECTIVES;

    private string FILE_NAME_SAVE_TIME = "Time.txt";
    private string FILE_NAME_SAVE_GOLD = "Gold.txt";
    private const string DIR_LOGS = "/Logs/";
    private string PATH_TO_GOLD;
    
    private string FILE_PATH_JSON;

    //wherever the player starts the level
    private Vector2 playerStartPos;

    //These are all prefabs used in the tile editor
    //There are a lot
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

    [Header("Terrain Prefabs - Grass Sand")]
    public GameObject grassSandTR; //w
    public GameObject grassSandTL; //x
    public GameObject grassSandBR; //y
    public GameObject grassSandBL; //z
    
    [Header("Player Prefab")] 
    public GameObject playerBoatBlue;
    public GameObject playerBoatRed;
    public GameObject playerBoatYellow;
    public GameObject pirate;

    [Header("Object Prefabs")] 
    public GameObject gold;
    public GameObject rock1;
    public GameObject rock2;
    public GameObject rock3;

    // Start is called before the first frame update
    void Start()
    {
        //I wanted a truly random seed and this was as close as I could get using time in seconds
        Random.seed = System.DateTime.Now.Second;
        
        //this is the terrain path
        //this only ended up being one level so the 'Num' doesn't do much now
        PATH_TO_TERRAIN = Application.dataPath + DIR + FILE_NAME_TERRAIN;
        PATH_TO_TERRAIN = PATH_TO_TERRAIN.Replace("Num", levelNum + "");
        
        //There are 3 possible files with objectives in it, and one of these is loaded at random 
        PATH_TO_OBJECTIVES = Application.dataPath + DIR + FILE_NAME_OBJECTIVES;
        PATH_TO_OBJECTIVES = PATH_TO_OBJECTIVES.Replace("Num", Random.Range(1,3) + "");

        //getting the player's previous gold
        PATH_TO_GOLD = Application.dataPath + DIR_LOGS + FILE_NAME_SAVE_GOLD;
        
        //player position is extracted from here, if it exists
        FILE_PATH_JSON = Application.dataPath + "/" + name + ".json";
        if (File.Exists(FILE_PATH_JSON))
        {
            string jsonText = File.ReadAllText(FILE_PATH_JSON);
            Vector3 savedPos = JsonUtility.FromJson<Vector3>(jsonText);
            playerStartPos = savedPos; 
        }

        //now execute this to get the tiles
        ShoreParser();
       
        //get the actual gold value, if it exists
        //otherwise, just set it to zero. if the player closes with still 0, it won't break (but it does break if null)
        if (!File.Exists(PATH_TO_GOLD))
        {
            File.Create(PATH_TO_GOLD);
            GoldAmt = 0;
            File.WriteAllText(PATH_TO_GOLD, GoldAmt + "");
        }
        else
        {
            GoldAmt = Int32.Parse(File.ReadAllText(PATH_TO_GOLD));
        }
        
    }



//function to get all tiles
    void ShoreParser()
    {
        //level gameobject to save things under to keep scene cleaner
        GameObject level = new GameObject();
        level.name = "Level";

        //terrain
        
        //first file we look at is for the terrain
        string[] terrain = File.ReadAllLines(PATH_TO_TERRAIN);

        for (int y = 0; y < terrain.Length; y++)
        {
            string currentLine = terrain[y]; //this is the current line of the ASCII file
            char[] characters = currentLine.ToCharArray(); //we want to get every character of that file

            for (int x = 0; x < characters.Length; x++)
            { 
//x and y become x and y coords, based on where they are in the ascii file
                GameObject newObj;
                char c = characters[x]; //the current character

                //long switch statement for the terrain tiles
                switch (c)
                {
                    case 'P': //pirate ship -> this is the player
                        newObj = Instantiate<GameObject>(playerBoatBlue);
                        break;
                    case 'p': //pirate -> this is the player on land
                        newObj = Instantiate<GameObject>(pirate);
                        break;
                    case '1': //here down is all land tile types
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
                    case 'e':
                        newObj = Instantiate<GameObject>(grassCenterLeft);
                        break;
                    case 'f':
                        newObj = Instantiate<GameObject>(grassCenterLeft2);
                        break;
                    case 'g':
                        newObj = Instantiate<GameObject>(grassCenterMiddle);
                        break;
                    case 'h':
                        newObj = Instantiate<GameObject>(grassCenterMiddle2);
                        break;
                    case 'i':
                        newObj = Instantiate<GameObject>(grassCenterMiddle3);
                        break;
                    case 'j':
                        newObj = Instantiate<GameObject>(grassCenterMiddle4);
                        break;
                    case 'k':
                        newObj = Instantiate<GameObject>(grassCenterRight);
                        break;
                    case 'l':
                        newObj = Instantiate<GameObject>(grassCenterRight2);
                        break;
                    case 'm':
                        newObj = Instantiate<GameObject>(grassBottomLeft);
                        break;
                    case 'n':
                        newObj = Instantiate<GameObject>(grassBottomMiddle);
                        break;
                    case 'o':
                        newObj = Instantiate<GameObject>(grassBottomMiddle2);
                        break;
                    case 'q':
                        newObj = Instantiate<GameObject>(grassBottomRight);
                        break;
                    case 'w':
                        newObj = Instantiate<GameObject>(grassSandTR);
                        break;
                    case 'x':
                        newObj = Instantiate<GameObject>(grassSandTL);
                        break;
                    case 'y':
                        newObj = Instantiate<GameObject>(grassSandBR);
                        break;
                    case 'z':
                        newObj = Instantiate<GameObject>(grassSandBL);
                        break;
                    case 'R': //rocks should be one of three type of rock sprite, picked randomly
                        var rock = Random.Range(1, 3);
                        Debug.Log(rock);
                        switch (rock)
                        {
                            case 1:
                                Debug.Log("Switch");
                                newObj = Instantiate<GameObject>(rock1);
                                break;
                            case 2:
                                newObj = Instantiate<GameObject>(rock2);
                                break;
                            case 3:
                                newObj = Instantiate<GameObject>(rock3);
                                break;
                            default:
                                newObj = Instantiate<GameObject>(rock1);
                                break;
                        }

                        break;
                    default:
                        newObj = null;
                        break;
                }

                if (newObj != null)
                {
                    //this should be the position of most objects
                    newObj.transform.position = new Vector2(x * tileSize + xOffset, -y * tileSize + yOffset);
                    
                    //if the object just made is the player ship, set that instead to the last position the player was at when the game was closed
                    //this only happens if the JSON exists
                    if (newObj.name.Contains("Ship") && File.Exists(FILE_PATH_JSON))
                    {
                        newObj.transform.position = playerStartPos;
                    }

                    newObj.GetComponent<SpriteRenderer>().sortingOrder = 0; //set the sort order so these are on the bottom
                   
                    //the ship and the pirate should be the top items visisble
                    if (newObj.name.Contains("Ship") || newObj.name.Contains("Pirate"))
                    {
                        newObj.GetComponent<SpriteRenderer>().sortingOrder = 2;
                    }
                    newObj.transform.parent = level.transform;
                }

            }
        }
        
        //objectives
        //we're repeating the above to place treasure markers on land
        string[] treasures = File.ReadAllLines(PATH_TO_OBJECTIVES);
        for (int y = 0; y < treasures.Length; y++)
        {
            string currentLine = treasures[y]; //this is the current line of the ASCII file
            char[] characters = currentLine.ToCharArray(); //we want to get every character of that file

            for (int x = 0; x < characters.Length; x++)
            {

                GameObject newObj;
                char c = characters[x];
                switch (c)
                {
                    case '#': //x marks the spot tiles
                        newObj = Instantiate<GameObject>(gold);
                        break;
                    default:
                        newObj = null;
                        break;
                }
                
                if (newObj != null)
                {
                    newObj.transform.position = new Vector2(x * tileSize + xOffset, -y * tileSize + yOffset);
                    newObj.GetComponent<SpriteRenderer>().sortingOrder = 1; //set the sort order so these are above the terrain, but below the player
                    newObj.transform.parent = level.transform;
                }
                
            }
        }


    }

    private void OnApplicationQuit()
    {
        Debug.Log("QUIT");
        //when quitting, save the player's last position in a JSON file
        string jsonPosition = JsonUtility.ToJson(GameObject.FindWithTag("Player").transform.position, true);
        Debug.Log(jsonPosition);
        File.WriteAllText(FILE_PATH_JSON, jsonPosition);
    }
}
