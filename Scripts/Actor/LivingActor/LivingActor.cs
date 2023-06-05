using Actors;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace LivingActors
{
    public class LivingActor : Actor
    {
        protected IState _state;
        public string state;
        public Vector2 velocity;

        public IdleState idlestate = new IdleState();
        public WalkingState walkingstate = new WalkingState();
        public JumpingState jumpingstate = new JumpingState();
        public FallingState fallingstate = new FallingState();
        public DashingState dashingstate = new DashingState();
        public DyingState dyingstate = new DyingState();
        public WanderingState wanderingstate = new WanderingState();

        protected void InitLivingActor()
        {
            InitActor();
            this._state = this.idlestate;
        }
        protected void ChangeVelocity(float x, float y) { 
            this._rb2d.velocity = new Vector2(x, y);
        }
        protected void MoveTowardsPosition(float x, float y)
        {
            this._rb2d.MovePosition(new Vector2(math.round(x/16), math.round(y /16)));
        }
        protected bool IsGrounded() { return this._isGrounded; }
        protected void DoFlip() { this._sprite.flipX = this.direction; }
        protected virtual void UseAI() {}
        protected virtual void Death() { }
        public virtual void SetAnimState(int stateid)
        {
            this._anim.SetInteger("state", stateid);
        }
        public virtual IState DoIdleState() { return null; }
        public virtual IState DoWalkingState() { return null; }
        public virtual IState DoJumpingState() { return null; }
        public virtual IState DoFallingState() { return null; }
        public virtual IState DoDashingState() { return null; }
        public virtual IState DoDyingState() { return null; }
        public virtual IState DoWanderingState() { return null; }



    }
}

