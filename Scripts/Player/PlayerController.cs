using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpVelocity;
    [SerializeField] private float footRadius;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float jumpBuffer;
    [SerializeField] private bool direction;
    [SerializeField] private Transform feetPos;
    [SerializeField] private LayerMask groundType;


    private float _inputDir;
    private float _coyoteTimer;
    private float _jumpTimer;
    private bool _isGrounded;
    private Rigidbody2D _rb2d;
    private SpriteRenderer _sprite;
    private Animator _anim;
    private IPlayerState state;

    public string playerstate;
    public IdleState idlestate = new IdleState();
    public WalkingState walkingstate = new WalkingState();
    public JumpingState jumpingstate = new JumpingState();
    public FallingState fallingstate = new FallingState();
    public Vector2 velocity;


    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();


        state = idlestate;
    }

    void Update()
    {
        velocity = new Vector2(0, 0);
        _isGrounded = Physics2D.OverlapCircle(feetPos.position, footRadius, groundType);

        state = state.DoState(this);

    }

    private void FixedUpdate()
    {
        _rb2d.velocity = new Vector2(_inputDir * speed, _rb2d.velocity.y);
    }

    public void setAnimState(int stateid)
    {
        _anim.SetInteger("state", stateid);
    }

    public void doAnim()
    {
        _sprite.flipX = direction;
    }

    public void handleInput()
    {

        _inputDir = Input.GetAxisRaw("Horizontal");

        if (_inputDir > 0) 
            direction = true;
        else if (_inputDir < 0)
            direction = false;

        velocity = new Vector2(_inputDir * speed, _rb2d.velocity.y);

    }

    public void initJump()
    {
        _jumpTimer = jumpBuffer;
    }

    public void handleJump()
    {
        if(!Input.GetButtonDown("Jump"))
            _jumpTimer -= Time.deltaTime;

        if (_jumpTimer > 0f && _coyoteTimer > 0f)
        {
            _rb2d.velocity = new Vector2(_rb2d.velocity.x, jumpVelocity);
            _jumpTimer = 0f;
        }

        if (Input.GetButtonUp("Jump") && _rb2d.velocity.y > 0f)
        {
            _rb2d.velocity = new Vector2(_rb2d.velocity.x, _rb2d.velocity.y * 0.5f);
            _coyoteTimer = 0f;
        }
    }

    public bool isGrounded()
    {
        return _isGrounded;
    }

    public void death()
    {
        if (_rb2d.position.y < -7f)
        {
            _rb2d.position = new Vector2(0, 0);
        }
    }

    public void doCoyoteTime()
    {
        if (_isGrounded)
            _coyoteTimer = coyoteTime;
        else
            _coyoteTimer -= Time.deltaTime;
    }

    public float getCoyoteTime()
    {
        return _coyoteTimer;
    }

}


