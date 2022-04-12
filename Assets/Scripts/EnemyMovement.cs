using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private Transform groundCheker;
    private Rigidbody2D enemyRigidbody;
    private Vector2 movingVector;
    private MovementDirection direction = MovementDirection.Left;
    private float WalkSpeed = -1;
    private bool patrolling;
    public void Patrol()
    {
        enemyRigidbody.velocity = new Vector2(WalkSpeed, enemyRigidbody.velocity.y);
        if (!Physics2D.OverlapCircle(groundCheker.position, 0.1f, groundMask))
        {
            Reflect();
        }
    }
    private void Reflect()
    {
        transform.localScale *= new Vector2(-1, 1);
        WalkSpeed *= -1;
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
        enemyRigidbody.velocity = new Vector2(WalkSpeed, enemyRigidbody.velocity.y);
    }
    public void waitingToPlayer()
    {

    }
    private void Start()
    {
        enemyRigidbody = GetComponentInParent<Rigidbody2D>();
    }
    public enum MovementDirection
    {
        Left,
        Right
    }
}
