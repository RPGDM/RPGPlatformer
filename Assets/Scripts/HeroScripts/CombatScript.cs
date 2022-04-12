using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CombatScript : MonoBehaviour
{
    [SerializeField] Animator _animationController;
    [SerializeField] Weapon _weapon;
    private Player _player;
    private Collider2D _weaponCollider;
    private UnityEvent<bool> attackPressed;
    private bool attacks = false;
    public void attack()
    {
        if (Input.GetMouseButtonDown(0) && !attacks)
        {
            attacks = true;
            _animationController.SetInteger("attackIndex", 0);
            _animationController.SetTrigger("attack");
            _weaponCollider.enabled = !_weaponCollider.enabled;
            StartCoroutine(AttackSpeedController());
        }
    }
    private int dealingDamage()
    {
        int damage = (int)_weapon.DefaultDamage;
        if (_weapon.AttributeOfWeapon == Weapon.AttributesWeapon.Agility)
        {
            damage = (int)(_weapon.DefaultDamage * (_player.PlayersAttributes.Agility * 0.4));
        }
        else
        {
            if (_weapon.AttributeOfWeapon == Weapon.AttributesWeapon.Strength)
            {
                damage = (int)(_weapon.DefaultDamage * (_player.PlayersAttributes.Strength * 0.4));
            }
        }
        return damage;
    }
    private IEnumerator AttackSpeedController()
    {
        yield return new WaitForSeconds(_weapon.AttackSpeed);
        attacks = false;
        _weaponCollider.enabled = !_weaponCollider.enabled;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(attacks.ToString());
        if (collision.GetComponent<Enemy>() && attacks)
        {
            collision.GetComponent<Enemy>().TakeDamage(dealingDamage());
        }
    }
    private void Start()
    {
        _player = GetComponentInParent<Player>();
        _weaponCollider = GetComponentInParent<CapsuleCollider2D>();
    }
}
