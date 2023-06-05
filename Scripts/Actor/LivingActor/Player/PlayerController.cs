using LivingActors;
using Unity.Mathematics;
using UnityEngine;

namespace Player
{
    public class PlayerController : LivingActor
    {
        [SerializeField] private float speed;
        [SerializeField] private float jumpVelocity;
        [SerializeField] private float dashVelocity;
        [SerializeField] private float footRadius;
        [SerializeField] private float coyoteTime;
        [SerializeField] private float dashTime;
        [SerializeField] private float jumpBuffer;
        [SerializeField] private float dashCooldown;
        [SerializeField] private Transform feetPos;
        [SerializeField] private LayerMask groundType;

        private float _inputDir;
        private float _coyoteTimer;
        private float _jumpTimer;
        private float _dashTimer;
        private float _dashCooldownTimer;
        private float _savedYpos;

        private void Start()
        {
            InitLivingActor();
        }

        private void Update()
        {
            this.velocity = new Vector2(0, 0);
            this._dashCooldownTimer -= Time.deltaTime;
            this._isGrounded = Physics2D.OverlapCircle(this.feetPos.position, this.footRadius, this.groundType);

            this._state = this._state.DoState(this);
        }

        private void FixedUpdate()
        {
            this._rb2d.velocity = new Vector2(this._inputDir * this.speed, this._rb2d.velocity.y);
        }

        // general input and misc //
        private void HandleInput()
        {

            this._inputDir = Input.GetAxisRaw("Horizontal");

            if (this._inputDir > 0)
                this.direction = true;
            else if (this._inputDir < 0)
                this.direction = false;

            this.velocity = new Vector2(this._inputDir * this.speed, this._rb2d.velocity.y);

        }

        // dash //
        private void InitDash()
        {
            this._dashTimer = this.dashTime;
            this._dashCooldownTimer = this.dashCooldown;
            this._savedYpos = this._rb2d.position.y;
        }

        private bool HandleDash()
        {
            this._dashTimer -= Time.deltaTime;

            if (this.direction) 
                this._inputDir = this.dashVelocity;
            else
                this._inputDir = -1 * this.dashVelocity;

            this._rb2d.velocity = new Vector2(this._rb2d.velocity.x, 0);
            this._rb2d.position = new Vector2(this._rb2d.position.x, _savedYpos);

            return !(this._dashTimer > 0f);

        }

        private bool CheckDashCooldown()
        {
            return (this._dashCooldownTimer < 0f);
        }

        private void ExitDash()
        {
            this._rb2d.velocity = new Vector2(this._rb2d.velocity.x, -2);
        }

        // jump //
        private void InitJump()
        {
            this._jumpTimer = this.jumpBuffer;
        }
        private void HandleJump()
        {
            if (!Input.GetButtonDown("Jump"))
                this._jumpTimer -= Time.deltaTime;

            if (this._jumpTimer > 0f && this._coyoteTimer > 0f)
            {
                this._rb2d.velocity = new Vector2(this._rb2d.velocity.x, this.jumpVelocity);
                this._jumpTimer = 0f;
            }

            if (Input.GetButtonUp("Jump") && this._rb2d.velocity.y > 0f)
            {
                this._rb2d.velocity = new Vector2(this._rb2d.velocity.x, this._rb2d.velocity.y * 0.5f);
                this._coyoteTimer = 0f;
            }
        }
        private void DoCoyoteTime()
        {
            if (this._isGrounded)
                this._coyoteTimer = this.coyoteTime;
            else
                this._coyoteTimer -= Time.deltaTime;
        }

        private float GetCoyoteTime()
        {
            return this._coyoteTimer;
        }

        protected override void Death()
        {
            if (this._rb2d.position.y < -7f)
                this._rb2d.position = new Vector2(0, 0);
        }

        // handle states //
        public override IState DoIdleState() 
        {
            // state responsibilities
            DoCoyoteTime();
            HandleInput();
            DoFlip();
            Death();

            // change state
            if (Input.GetKeyDown(KeyCode.LeftShift) && CheckDashCooldown())
            {
                InitDash();
                return this.dashingstate;
            }
            else if (Input.GetButtonDown("Jump"))
            {
                InitJump();
                return this.jumpingstate;
            }
            else if (!IsGrounded() && GetCoyoteTime() <= 0f)
                return this.fallingstate;
            else if (math.abs(this.velocity.x) > 0f)
                return this.walkingstate;
            else
                return this.idlestate;
        }
        
        public override IState DoWalkingState() 
        {
            // state responsibilities
            DoCoyoteTime();
            HandleInput();
            DoFlip();
            Death();

            // change state
            if (Input.GetKeyDown(KeyCode.LeftShift) && CheckDashCooldown())
            {
                InitDash();
                return this.dashingstate;
            }
            else if (Input.GetButtonDown("Jump"))
            {
                InitJump();
                return this.jumpingstate;
            }
            else if (!IsGrounded() && GetCoyoteTime() <= 0f)
                return this.fallingstate;
            else if (math.abs(this.velocity.x) > 0f)
                return this.walkingstate;
            else
                return this.idlestate;
        }
        
        public override IState DoJumpingState() 
        {
            // state responsibilities
            DoCoyoteTime();
            HandleJump();
            HandleInput();
            DoFlip();
            Death();

            // change state
            if (Input.GetKeyDown(KeyCode.LeftShift) && CheckDashCooldown())
            {
                InitDash();
                return this.dashingstate;
            }
            else if (this.velocity.y < 0f)
                return this.fallingstate;
            else
                return this.jumpingstate;
        }
        
        public override IState DoFallingState() 
        {
            // state responsibilities
            DoCoyoteTime();
            HandleInput();
            DoFlip();
            Death();

            // change state
            if (Input.GetKeyDown(KeyCode.LeftShift) && CheckDashCooldown())
            {
                InitDash();
                return this.dashingstate;
            }
            else if (Input.GetButtonDown("Jump"))
            {
                InitJump();
                return this.jumpingstate;
            }
            else if (this.velocity.y == 0f || IsGrounded())
                return this.idlestate;
            else
                return this.fallingstate;
        }
        
        public override IState DoDashingState() 
        {
            bool dashEnd = false;

            // state responsibilities
            dashEnd = HandleDash();
            Death();

            // change state
            if (dashEnd)
            {
                ExitDash();
                return this.idlestate;
            }
            else
                return this.dashingstate;
        }
    }
}

