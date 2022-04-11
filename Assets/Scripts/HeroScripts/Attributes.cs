using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attributes
{
    private int strength = 0;
    private int agility = 0;
    public int Strength
    {
        get
        {
            return strength;
        }
        set
        {
            strength = value;
        }
    }
    public int Agility
    {
        get
        {
            return agility;
        }
        set
        {
            agility = value;
        }
    }
    public Attributes()
    {
        strength = 1;
        agility = 1;
    }
}
