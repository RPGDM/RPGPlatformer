using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rigidBody;
    [SerializeField] private Behavior enemyBehavior;
    [SerializeField] private Transform agreCircle;
    [SerializeField] private EnemyMovement movementController;
    [SerializeField] private float agreRange = 4.3f;
    [SerializeField] private float activeAgreRange = 5f;
    [SerializeField] private GameObject dropSoul;
    [SerializeField] private int damage = 20;
    private Behavior startEnemyBehavior;
    private Vector2 direction;
    private int currentHealth;
    private bool damageable = true;
    private float invulnerabilityTime = .2f;//u cant spam attack
    private bool hit = false;
    private bool hitPlayer;
    private int enemyValue;
    public Behavior EnemyBehavior()
    {
        return enemyBehavior;
    }
    public Vector2 GetPosition()
    {
        return agreCircle.position;
    }
    public float getAgrRange()
    {
        return agreRange;
    }
    public void TakeDamage(int damage)
    {
        if (damageable && !hit && currentHealth > 0)
        {
            hit = true;
            currentHealth -= damage;
            Debug.Log(movementController.Direction);
            if (movementController.Direction == EnemyMovement.MovementDirection.Left)
            {
                direction = new Vector2(10, 3);
            }
            else
            {
                direction = new Vector2(-10, 3);
            }
            _rigidBody.velocity = new Vector2(0, 0);
            _rigidBody.velocity = direction;
            Debug.Log(damage);
            Debug.Log(currentHealth + "HP");
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                die();
                return;
            }
            StartCoroutine(TurnOffHit());
        }
        else
        {
            Debug.Log("Pishow nahoy");
            //StartCoroutine(TurnOffHit());
        }
    }
    private void die()
    {
        SpawnSoul(Random.Range(0, 3));
        gameObject.SetActive(false);
    }
    private void SpawnSoul(int amountOfSouls)
    {
        for (int i = 0; i <= amountOfSouls; i++)
        {
            Instantiate(dropSoul, transform.position, transform.rotation);
        }
    }
    private IEnumerator TurnOffHit()
    {
        yield return new WaitForSeconds(invulnerabilityTime);
        hit = false;
    }
    private IEnumerator HitPlayerAgain()
    {
        yield return new WaitForSeconds(1f);
        hitPlayer = false;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            collision.collider.GetComponent<CombatScript>().TakeDamage(damage, this);
            hitPlayer = true;
            StartCoroutine(HitPlayerAgain());
        }
    }
    private void EnemyAction()
    {
        switch (enemyBehavior)
        {
            case Behavior.patrol:
                movementController.Patrol();
                if (movementController.PlayerSearch(agreRange))
                {
                    enemyBehavior = Behavior.agressive;
                }
                break;
            case Behavior.agressive:
                if (hitPlayer)
                {
                    movementController.AgressivePursuit(activeAgreRange);
                }
                else
                {
                    movementController.Attack();
                }
                if (!movementController.PlayerSearch(activeAgreRange))
                {
                    enemyBehavior = startEnemyBehavior;
                    hitPlayer = true;
                }
                break;
            case Behavior.waitingToPlayer:
                if (movementController.PlayerSearch(agreRange))
                {
                    enemyBehavior = Behavior.agressive;
                }
                break;
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (agreCircle != null)
        {
            if (enemyBehavior != Behavior.agressive)
            {
                Gizmos.DrawWireSphere(agreCircle.position, agreRange);
            }
            else
            {
                Gizmos.DrawWireSphere(agreCircle.position, activeAgreRange);
            }
        }
    }
    private void Start()
    {
        currentHealth = maxHealth;
        startEnemyBehavior = enemyBehavior;
    }
    private void Update()
    {
        EnemyAction();
    }
    public enum Behavior
    {
        waitingToPlayer,
        agressive,
        patrol
    }
}
