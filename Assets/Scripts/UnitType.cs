using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//this class is supposed to help with, I don't actually know lol, I should probably work on other game before starting this again
public class UnitType
{
    string unitType;
    string movementType;
    string attackType;

    public void CreateUnit(string unitType)
    {
        this.unitType = unitType;
        switch (unitType)
        {
            case "flying": break;
            default: break;
        }
    }

}
