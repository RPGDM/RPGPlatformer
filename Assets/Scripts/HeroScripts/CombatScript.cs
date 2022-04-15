using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CombatScript : MonoBehaviour
{
    [SerializeField] Animator _animationController;
    [SerializeField] Weapon _weapon;
    [SerializeField] private float horizontalKnockback;
    [SerializeField] private float verticalKnockback;
    [SerializeField] private float cancelMovementTime;
    [SerializeField] private float invulnerabilityTime;
    private Player _player;
    private Collider2D _weaponCollider;
    private MovingScript playerMovement;
    private Rigidbody2D rigidBody;
    private Enemy AttackingEnemy;
    private bool attacks = false;
    private float lastInputTime;
    private bool damageable = true;
    private bool isDead = false;


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
    private void KnockBack()
    {
        rigidBody.AddForce(Vector2.up * verticalKnockback);
        if (AttackingEnemy != null)
        {
            if (transform.position.x < AttackingEnemy.transform.position.x)
            {
                rigidBody.AddForce(Vector2.left * horizontalKnockback);
            }
            else
            {
                rigidBody.AddForce(Vector2.right * horizontalKnockback);
            }
            if (!isDead)
            {
                _animationController.SetTrigger("hit");
                Invoke("EnableMovement", cancelMovementTime);
                Invoke("CancelHit", invulnerabilityTime);
            }

        }
    }
    private void CancelHit()
    {
        attacks = false;
    }
    public void EnableMovement()
    {
        playerMovement.IsTakingDamage = false;
    }
    private IEnumerator AttackSpeedController()

    {
        yield return new WaitForSeconds(_weapon.AttackSpeed);
        attacks = false;
        _weaponCollider.enabled = !_weaponCollider.enabled;
    }
    public void TakeDamage(int damage, Enemy enemy)
    {
        if (damageable)
        {
            playerMovement.IsTakingDamage = true;
            AttackingEnemy = enemy;
            attacks = true;
            GetComponent<Player>().CurrentHealth -= damage;
            if (GetComponent<Player>().CurrentHealth <= 0)
            {
                isDead = true;
            }
            KnockBack();
        }
    }
    private IEnumerator TurnOffHit()
    {
        yield return new WaitForSeconds(0.4f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() && attacks)
        {
            collision.GetComponent<Enemy>().TakeDamage(dealingDamage());
        }
    }
    private void Start()
    {
        _player = GetComponentInParent<Player>();
        _weaponCollider = GetComponentInParent<CapsuleCollider2D>();
        rigidBody = GetComponentInParent<Rigidbody2D>();
        playerMovement = GetComponentInParent<MovingScript>();
    }
    private void FixedUpdate()
    {
    }
}
