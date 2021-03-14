﻿
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
            goldTXT.text = "Current gold:\n" + goldAMT + "\n \nTreasure remaining today:\n" + dailySpots;
        }
    }

    public TextMeshProUGUI goldTXT; 
    
    //tile offsets
    public float xOffset;
    public float yOffset;
    
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

    private Vector2 playerStartPos;

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
        Random.seed = System.DateTime.Now.Second;
        
        PATH_TO_TERRAIN = Application.dataPath + DIR + FILE_NAME_TERRAIN;
        PATH_TO_TERRAIN = PATH_TO_TERRAIN.Replace("Num", levelNum + "");
        
        PATH_TO_OBJECTIVES = Application.dataPath + DIR + FILE_NAME_OBJECTIVES;
        PATH_TO_OBJECTIVES = PATH_TO_OBJECTIVES.Replace("Num", Random.Range(1,3) + "");

        PATH_TO_GOLD = Application.dataPath + DIR_LOGS + FILE_NAME_SAVE_GOLD;
        
        FILE_PATH_JSON = Application.dataPath + "/" + name + ".json";
        if (File.Exists(FILE_PATH_JSON))
        {
            string jsonText = File.ReadAllText(FILE_PATH_JSON);
            Vector3 savedPos = JsonUtility.FromJson<Vector3>(jsonText);
            playerStartPos = savedPos; 
        }

        
        ShoreParser();
        
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




    void ShoreParser()
    {
        GameObject level = new GameObject();
        level.name = "Level";

        //terrain
        
        string[] terrain = File.ReadAllLines(PATH_TO_TERRAIN);

        for (int y = 0; y < terrain.Length; y++)
        {
            string currentLine = terrain[y]; //this is the current line of the ASCII file
            char[] characters = currentLine.ToCharArray(); //we want to get every character of that file

            for (int x = 0; x < characters.Length; x++)
            {

                GameObject newObj;
                char c = characters[x];

                switch (c)
                {
                    case 'P': //pirate ship
                        newObj = Instantiate<GameObject>(playerBoatBlue);
                        break;
                    case 'p': //pirate
                        newObj = Instantiate<GameObject>(pirate);
                        break;
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
                    case 'R':
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
                    newObj.transform.position = new Vector2(x * tileSize + xOffset, -y * tileSize + yOffset);
                    if (newObj.name.Contains("Ship") && File.Exists(FILE_PATH_JSON))
                    {
                        newObj.transform.position = playerStartPos;
                    }

                    newObj.GetComponent<SpriteRenderer>().sortingOrder = 0; //set the sort order so these are on the bottom
                    if (newObj.name.Contains("Ship") || newObj.name.Contains("Pirate"))
                    {
                        newObj.GetComponent<SpriteRenderer>().sortingOrder = 2;
                    }
                    newObj.transform.parent = level.transform;
                }

            }
        }
        
        //objectives
        
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
                    case '#':
                        newObj = Instantiate<GameObject>(gold);
                        break;
                    default:
                        newObj = null;
                        break;
                }
                
                if (newObj != null)
                {
                    newObj.transform.position = new Vector2(x * tileSize + xOffset, -y * tileSize + yOffset);
                    newObj.GetComponent<SpriteRenderer>().sortingOrder = 1; //set the sort order so these are on the bottom
                    newObj.transform.parent = level.transform;
                }
                
            }
        }


    }

    private void OnApplicationQuit()
    {
        Debug.Log("QUIT");
        string jsonPosition = JsonUtility.ToJson(GameObject.FindWithTag("Player").transform.position, true);
        Debug.Log(jsonPosition);
        File.WriteAllText(FILE_PATH_JSON, jsonPosition);
    }
}
