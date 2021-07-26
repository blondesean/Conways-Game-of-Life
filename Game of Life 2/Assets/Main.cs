using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Main : MonoBehaviour
{
    const int gridLength = 100;
    public GameObject[,] grid = new GameObject[gridLength, gridLength];
    public bool[,] futureGrid = new bool[gridLength, gridLength];
    public GameObject square;
    public int k = 1;
    public float pauseSeconds = 1f;
    public bool manualInputMode = false;
    public bool slowMode = false;

    // Use this for initialization
    void Start()
    {

        int i;
        int j;
        bool createLife;

        //for manual input
        bool alive = true; bool dead = false;
        bool[,] futureGrid = new bool[10, 10]
            {
                { dead, dead, dead, dead, dead, dead, dead, dead, dead, dead },
                { dead, dead, dead, dead, dead, dead, dead, dead, dead, dead },
                { dead, alive, alive, alive, dead, dead, dead, dead, dead, dead },
                { dead, dead, dead, dead, dead, dead, dead, dead, dead, dead },
                { dead, dead, dead, dead, dead, dead, dead, dead, dead, dead },
                { dead, dead, dead, dead, dead, dead, dead, dead, dead, dead },
                { dead, dead, dead, dead, dead, dead, dead, dead, dead, dead },
                { dead, dead, dead, dead, dead, dead, dead, dead, dead, dead },
                { dead, dead, dead, dead, dead, dead, dead, dead, dead, dead },
                { dead, dead, dead, dead, dead, dead, dead, dead, dead, dead },
            };

        //set seed for consistent results while testing
        //if (!manualInputMode) { UnityEngine.Random.InitState(21); };

        //create the alive status for the grid, set location & spawn object
        print("Spawning Grids & determining alive status");
        for (j = 0; j < gridLength; j++)
        {
            for (i = 0; i < gridLength; i++)
            {
                if (manualInputMode)
                {
                    spawnEnemy(i, j, gridLength, futureGrid[gridLength - 1 - j, i]);
                }
                else //randomly generate grid
                {
                    if (UnityEngine.Random.Range(0, 2) == 1) { createLife = true; } else { createLife = false; };
                    spawnEnemy(i, j, gridLength, createLife);
                }
            }
        }

        //This is the repeated calculation of the conways game of life grid
        StartCoroutine(LoopingGridCalc());
    }

    private void spawnEnemy(int xInt, int yInt, int gridSize, bool aliveOrDead)
    {

        //calculations for spawn location
        float xVal = (float)xInt + 0.5f - (float)gridSize / 2;
        float yVal = (float)yInt + .5f - (float)gridSize / 2;

        //create the object
        GameObject a = Instantiate(square) as GameObject;
        a.transform.position = new Vector2(xVal, yVal);
        a.GetComponent<Square>().isAlive = aliveOrDead;

        //add the object to our grid
        grid[xInt, yInt] = a;

        //color dead objects appropriately
        //print("spawning square at (" + xVal + "," + yVal + ") - " + a.GetComponent<Square>().AliveStatus());
        if (a.GetComponent<Square>().AliveStatus() == "dead")
        {
            grid[xInt, yInt].gameObject.GetComponent<SpriteRenderer>().color = new Color(245 / 255f, 119 / 255f, 50 / 255f, 130 / 255f);
        }
    }

    //need this method so when we check with squares not on the map we treat those as dead squares
    static bool neighborStatus(GameObject[,] check, int x, int y, string pos)
    {
        try
        {
            //print("---" + pos + ":Neighbor at (" + x + "," + y + ") is ");
            //print(check[x, y].GetComponent<Square>().isAlive + " - (" + x + "," + y + ")\n");
            return check[x, y].GetComponent<Square>().isAlive;

        }
        catch (IndexOutOfRangeException)
        {
            //print("a border - (" + x + ", " + y + ")\n");
            return false;
        }

    }

    private IEnumerator LoopingGridCalc()
    {
        while (true)
        {
            //needed for cycling through grid
            int i;
            int j;
            int gridLength = grid.GetLength(0); //size of the grid (x position, should match y tho)

            //Pause if desired
            //print("Starting pause");
            if (!slowMode) { pauseSeconds = 0f; };
            yield return new WaitForSeconds(pauseSeconds);

            //store variables for neighbor status
            bool neighborN; bool neighborS; bool neighborE; bool neighborW;
            bool neighborNE; bool neighborNW; bool neighborSE; bool neighborSW;

            //update the new color of the squares
            //print("Updating alive colors");
            for (j = 0; j < gridLength; j++)
            {
                for (i = 0; i < gridLength; i++)
                {
                    //keep original color if alive, otherwise assume dead, make dark
                    if (grid[i, j].gameObject.GetComponent<Square>().isAlive == true)
                        grid[i, j].gameObject.GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
                    else
                    {
                        grid[i, j].gameObject.GetComponent<SpriteRenderer>().color = new Color(245 / 255f, 119 / 255f, 50 / 255f, 130 / 255f);
                    }
                }
            }

            //print("running alive status update");
            //update squares alive status based on neighbors positions
            for (j = 0; j < gridLength; j++)
            {
                for (i = 0; i < gridLength; i++)
                {
                    //print("Square at (" + i + "," + j + ") = " + grid[i, j].gameObject.GetComponent<Square>().AliveStatus());

                    //get values for neighbors
                    neighborN = neighborStatus(grid, i, j + 1, "N");
                    neighborE = neighborStatus(grid, i + 1, j, "E");
                    neighborS = neighborStatus(grid, i, j - 1, "S");
                    neighborW = neighborStatus(grid, i - 1, j, "W");
                    neighborNE = neighborStatus(grid, i + 1, j + 1, "NE");
                    neighborSE = neighborStatus(grid, i + 1, j - 1, "SE");
                    neighborSW = neighborStatus(grid, i - 1, j - 1, "SW");
                    neighborNW = neighborStatus(grid, i - 1, j + 1, "NW");

                    //Store alive status for next iteration
                    //print("cell at (" + i + "," + j + ") N:" + neighborN + " NE:" + neighborNE + " E:" + neighborE + " SE:" + neighborSE  + " S:" + neighborS + " SW:" + neighborSW  + " W:" + neighborW  + " NW:" + neighborNW + " | Result for next iteration = " + grid[i, j].GetComponent<Square>().livesOn(neighborN, neighborS, neighborE, neighborW, neighborNE, neighborNW, neighborSE, neighborSW, false));
                    futureGrid[i, j] = grid[i, j].GetComponent<Square>().livesOn(neighborN, neighborS, neighborE, neighborW, neighborNE, neighborNW, neighborSE, neighborSW, false);
                }
            }

            //update the new status of the squares
            //print("Copying alive statuses of new grid");
            for (j = 0; j < gridLength; j++)
            {
                for (i = 0; i < gridLength; i++)
                {
                    grid[i, j].GetComponent<Square>().isAlive = futureGrid[i, j];
                }
            }

            //pause for a bit of time
            print("End of Simulation Round : " + k);
            k++;
        }

    }
}
