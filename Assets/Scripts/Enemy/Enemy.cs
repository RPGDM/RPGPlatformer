using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int maxHealth = 20;
    [SerializeField] private Rigidbody2D _rigidBody;
    [SerializeField] private Behavior enemyBehavior;
    [SerializeField] private Transform agreCircle;
    [SerializeField] private EnemyMovement movementController;
    [SerializeField] private float agreRange = 4.3f;
    [SerializeField] private float activeAgreRange = 5f;
    [SerializeField] private GameObject dropSoul;
    [SerializeField] private int damage = 20;
    [SerializeField] private float verticalKnockback;
    [SerializeField] private float horizontalKnockback;
    [SerializeField] private float cancelMovementTime;
    private GameObject[] player;
    private Behavior startEnemyBehavior;
    private int currentHealth;
    private bool isTakingDamage;
    private bool isDead;
    public GameObject GetPlayer
    {
        get
        {
            try
            {
            return player[0];
            }
            catch(System.IndexOutOfRangeException ex)
            {
                Debug.Log("Player not found" + ex);
                throw;
            }

        }
    }
    public Behavior EnemyBehavior
    {
        get
        {
            return enemyBehavior;
        }
        set
        {
            enemyBehavior = value;
        }
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
        if (!isTakingDamage && currentHealth > 0)
        {
            enemyBehavior = Behavior.takingDamage;
            isTakingDamage = true;
            movementController.IsTakingDamage = true;
            currentHealth -= damage;
            KnockBack();
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                die();
                return;
            }
        }
    }
    private void KnockBack()
    {
        _rigidBody.AddForce(Vector2.up * verticalKnockback);
        if (player[0] != null)
        {
            if (transform.position.x < player[0].transform.position.x)
            {
                _rigidBody.AddForce(Vector2.left * horizontalKnockback);
            }
            else
            {
                _rigidBody.AddForce(Vector2.right * horizontalKnockback);
            }
            if (!isDead)
            {
                Invoke("EnableMovement", cancelMovementTime);
            }
        }
    }
    private void EnableMovement()
    {
        isTakingDamage = false;
    }
    private void die()
    {
        SpawnSoul(Random.Range(0, 3));
        isDead = true;
        gameObject.SetActive(false);
    }
    private void SpawnSoul(int amountOfSouls)
    {
        for (int i = 0; i <= amountOfSouls; i++)
        {
            Instantiate(dropSoul, transform.position, transform.rotation);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            collision.collider.GetComponent<CombatScript>().TakeDamage(damage, this);
            movementController.GoalAchieved = true;
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
                if (movementController.AgressivePursuit(activeAgreRange) && !isTakingDamage)
                {
                    enemyBehavior = Behavior.attack;
                }
                else
                {
                    if (!movementController.PlayerSearch(activeAgreRange))
                    {
                        enemyBehavior = startEnemyBehavior;
                    }
                }
                break;
            case Behavior.waitingToPlayer:
                if (movementController.PlayerSearch(agreRange))
                {
                    enemyBehavior = Behavior.agressive;
                }
                break;
            case Behavior.attack:
                StartCoroutine(movementController.Attack());
                break;
                case Behavior.takingDamage:
                if(!isTakingDamage)
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
        player = GameObject.FindGameObjectsWithTag("Player");
    }
    private void Update()
    {
        EnemyAction();
    }
    public enum Behavior
    {
        waitingToPlayer,
        agressive,
        patrol,
        attack,
        takingDamage
    }
}
