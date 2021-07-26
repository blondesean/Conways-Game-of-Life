using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{

    public bool isAlive;

    //Constructor is alive or not
    public Square(bool aliveStatus)
    {
        isAlive = aliveStatus;
    }

    //Print if alive or not
    public string AliveStatus()
    {
        if (isAlive)
        {
            return "alive";
        }
        else
        {
            return "dead";
        }
    }

    //Use for print loop if alive, prints either X or nothing
    public void PrintCurrentGen()
    {
        if (isAlive)
        {
            print("X");
        }
        else
        {
            print(".");
        }
    }

    //Check to see if cell will be alive next round
    public bool livesOn(bool neighbor1, bool neighbor2, bool neighbor3, bool neighbor4, bool neighbor5, bool neighbor6, bool neighbor7, bool neighbor8, bool debugIt = false)
    {
        int aliveNeighbors = 0;

        //every alive neighbor = 1 point, we determine what to do depending on how many there are 
        if (neighbor1) { aliveNeighbors++; }
        if (neighbor2) { aliveNeighbors++; }
        if (neighbor3) { aliveNeighbors++; }
        if (neighbor4) { aliveNeighbors++; }
        if (neighbor5) { aliveNeighbors++; }
        if (neighbor6) { aliveNeighbors++; }
        if (neighbor7) { aliveNeighbors++; }
        if (neighbor8) { aliveNeighbors++; }

        if (debugIt)
        {
            print("There are " + aliveNeighbors + ".");
        }

        if (isAlive)
        {
            switch (aliveNeighbors)
            {
                case 0:
                    return false;
                case 1:
                    return false;
                case 2:
                    return true;
                case 3:
                    return true;
                default:
                    return false;
            }
        }
        else if (aliveNeighbors == 3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
