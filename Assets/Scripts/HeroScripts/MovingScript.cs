using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingScript : MonoBehaviour
{
    [SerializeField] private Player _hero;
    [SerializeField] private Rigidbody2D _rigidBody;
    [SerializeField] private Animator _animationController;
    [SerializeField] private Transform _groundCheker;
    [SerializeField] private LayerMask _ground;
    private float WalkSpeed = 6;
    private float JumpForce = 4;
    private Vector2 movingVector;
    private DirectionState directionState = DirectionState.Right;
    private bool onGround;
    private float checkGroundRadius = 0.1f;
    private float jumpTimeCounter;
    private float JumpTime = 0.35f;
    private bool isJumping = false;
    private bool secondJump = false;
    public bool OnGround
    {
        get
        {
            return onGround;
        }
    }
    public DirectionState GetDirectionState
    {
        get
        {
            return directionState;
        }
    }
    public void Moving()
    {
        MovingHorizontal();
        Jump();
    }
    private void MovingHorizontal()
    {
        movingVector.x = Input.GetAxis("Horizontal");
        _animationController.SetFloat("walking", Mathf.Abs(movingVector.x));
        _rigidBody.velocity = new Vector2(movingVector.x * WalkSpeed, _rigidBody.velocity.y);
        Reflect();
    }
    private void Jump()
    {
        if (CheckGround() && Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("1");
            isJumping = true;
            jumpTimeCounter = JumpTime;
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, 1 * JumpForce);
            secondJump = true;
        }
        if (Input.GetKey(KeyCode.W) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, 1 * JumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            isJumping = false;
        }
        if (Input.GetKeyDown(KeyCode.W) && !CheckGround() && secondJump)
        {
            Debug.Log("2");
            isJumping = true;
            jumpTimeCounter = JumpTime;
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, 1 * JumpForce);
            secondJump = false;
        }
        if (Input.GetKey(KeyCode.W) && isJumping && secondJump)
        {
            if (jumpTimeCounter > 0)
            {
                _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, 1 * JumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
    }
    private void Reflect()
    {
        if ((movingVector.x > 0 && directionState == DirectionState.Left) || (movingVector.x < 0 && directionState == DirectionState.Right))
        {
            transform.localScale *= new Vector2(-1, 1);
            if (directionState == DirectionState.Left)
            {
                directionState = DirectionState.Right;
            }
            else
            {
                directionState = DirectionState.Left;
            }
        }
    }
    private bool CheckGround()
    {
        _animationController.SetBool("onGround", onGround);
        return onGround = Physics2D.OverlapCircle(_groundCheker.position, checkGroundRadius, _ground);
    }
    public enum DirectionState
    {
        Right,
        Left
    }
}
