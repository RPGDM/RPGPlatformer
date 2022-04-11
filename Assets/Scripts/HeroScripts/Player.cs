using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int health;
    private int currentLvl;
    private Attributes attributes = new Attributes();
    [SerializeField] MovingScript _movingController;
    [SerializeField] CombatScript _combatController;
    public Attributes PlayersAttributes
    {
        get
        {
            return attributes;
        }
    }
    private void Update()
    {
        _movingController.Moving();
        _combatController.attack();
    }
    private void Start()
    {
        //attributes.Strength = 10;
        //attributes.Agility = 10;

    }
}
