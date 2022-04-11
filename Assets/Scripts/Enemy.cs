using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rigidBody;
    private Vector2 direction;
    private int currentHealth;
    private bool damageable = true;
    private float invulnerabilityTime = .2f;//u cant spam attack
    private bool hit = false;
    private void Start()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(int damage)
    {
        if (damageable && !hit && currentHealth > 0)
        {
            hit = true;
            currentHealth -= damage;
            direction = Vector2.left;
            _rigidBody.AddForce(direction * 300);
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                die();
            }
        }
        else
        {
            hit = false;
        }
    }
    private void die()
    {
        //_animator.SetTrigger("death");
        gameObject.SetActive(false);
    }
    private IEnumerator TurnOffHit()
    {
        yield return new WaitForSeconds(invulnerabilityTime);
        hit = false;
    }
}
