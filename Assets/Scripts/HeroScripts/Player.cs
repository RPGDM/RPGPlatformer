using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int maxHealth = 100;
    private int currentHealth;
    private int currentLvl;
    private Attributes attributes = new Attributes();
    [SerializeField] MovingScript _movingController;
    [SerializeField] CombatScript _combatController;
    [SerializeField] private SoulsController soulsController;
    private int balance = 0;
    public Attributes PlayersAttributes
    {
        get
        {
            return attributes;
        }
    }
    public int Balance
    {
        get
        {
            return balance;
        }
        set
        {
            balance = value;
        }
    }
    public int CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            currentHealth = value;
        }
    }
    private void Update()
    {
        _movingController.Moving();
        _combatController.attack();
    }
    private void Start()
    {
        currentHealth = maxHealth;
        //attributes.Strength = 10;
        //attributes.Agility = 10;

    }
}
