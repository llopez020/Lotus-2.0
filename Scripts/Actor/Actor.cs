using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actors
{
    public class Actor : MonoBehaviour
    {
        [SerializeField] protected bool direction;

        protected bool _isGrounded;
        protected SpriteRenderer _sprite;
        protected Rigidbody2D _rb2d;
        protected Animator _anim;

        protected void ChangePosition(float x, float y)
        {
            this._rb2d.position = new Vector2(x, y);
        }

        protected void InitActor()
        {
            this._rb2d = GetComponent<Rigidbody2D>();
            this._sprite = GetComponent<SpriteRenderer>();
            this._anim = GetComponent<Animator>();
        }
    }
}
