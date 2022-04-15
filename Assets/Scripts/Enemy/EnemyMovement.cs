using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private Transform groundChecker;
    [SerializeField] private Transform wallChecker;
    [SerializeField] private Transform jumpUpChecker;
    private Collider2D enemyCollider;
    private Rigidbody2D enemyRigidbody;
    private Vector2 movingVector;
    private MovementDirection direction = MovementDirection.Left;
    private float attackSpeed = -3;
    private float walkSpeed = -1;
    private float jumpForce = 5;
    private bool patrolling;
    private bool isJumping = false;
    private bool canJumpUp;
    private bool moving = true;
    public void Patrol()
    {
        enemyRigidbody.velocity = new Vector2(walkSpeed, enemyRigidbody.velocity.y);
        if ((!Physics2D.OverlapCircle(groundChecker.position, 0.1f, groundMask) && !isJumping) ||
        Physics2D.OverlapCircle(wallChecker.position, 0.1f, groundMask))
        {
            Reflect();
        }

    }
    private void JumpUP()
    {
        if (!Physics2D.OverlapCircle(jumpUpChecker.position, 0.1f, groundMask))
        {
            Jump();
        }
    }
    public MovementDirection Direction
    {
        get
        {
            return direction;
        }
    }
    private void Reflect()
    {
        transform.localScale *= new Vector2(-1, 1);
        walkSpeed *= -1;
        attackSpeed *= -1;
        if (direction == MovementDirection.Left)
        {
            direction = MovementDirection.Right;
        }
        else
        {
            direction = MovementDirection.Left;
        }
    }
    private void FacePlayer(Vector2 playerPosition)
    {
        if ((playerPosition.x < GetComponentInParent<Enemy>().GetPosition().x && direction == MovementDirection.Right) ||
        (playerPosition.x > GetComponentInParent<Enemy>().GetPosition().x && direction == MovementDirection.Left))
        {
            Reflect();
        }
        else
        {
            return;
        }
    }
    public bool PlayerSearch(float agrRange)
    {
        if (Physics2D.OverlapCircle(GetComponentInParent<Enemy>().GetPosition(), agrRange, playerMask))
        {
            return true;
        }
        return false;
    }
    public void AgressivePursuit(float argRange)
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(GetComponentInParent<Enemy>().GetPosition(), argRange, playerMask);
        if (playerCollider != null)
        {
            FacePlayer(playerCollider.transform.position);
        }
        if (Physics2D.OverlapCircle(wallChecker.position, 0.1f, groundMask))
        {
            JumpUP();
        }
        if (playerCollider != null)
        {
            if (enemyCollider.Distance(playerCollider).distance > 2)
            {
                enemyRigidbody.velocity = new Vector2(walkSpeed, enemyRigidbody.velocity.y);
            }
        }
    }
    public void Attack()
    {
        enemyRigidbody.velocity = new Vector2(attackSpeed, enemyRigidbody.velocity.y);
    }
    private void StopMoving()
    {
        moving = false;
    }
    private void Jump()
    {
        enemyRigidbody.velocity = new Vector2(enemyRigidbody.velocity.x, 1 * jumpForce);
        isJumping = true;
        StartCoroutine(WaitingJump());
    }
    private void Start()
    {
        enemyRigidbody = GetComponentInParent<Rigidbody2D>();
        enemyCollider = GetComponentInParent<BoxCollider2D>();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(groundChecker.position, 0.1f);
        Gizmos.DrawWireSphere(wallChecker.position, 0.1f);
        Gizmos.DrawWireSphere(jumpUpChecker.position, 0.1f);
    }
    private IEnumerator WaitingJump()
    {
        yield return new WaitForSeconds(1);
        isJumping = false;
    }
    private IEnumerator startRunning()
    {
        yield return new WaitForSeconds(1);
        moving = true;
    }
    public enum MovementDirection
    {
        Left,
        Right
    }
}
