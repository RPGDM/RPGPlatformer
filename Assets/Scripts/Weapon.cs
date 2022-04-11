using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private float defaultDamage = 20;
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private AttributesWeapon attributeOfWeapon;
    public float AttackRange
    {
        get
        {
            return attackRange;
        }
    }
    public float DefaultDamage
    {
        get
        {
            return defaultDamage;
        }
    }
    public float AttackSpeed
    {
        get
        {
            return attackSpeed;
        }
    }
    public AttributesWeapon AttributeOfWeapon
    {
        get
        {
            return attributeOfWeapon;
        }
    }
    public enum AttributesWeapon
    {
        Strength,
        Agility
    }
}
