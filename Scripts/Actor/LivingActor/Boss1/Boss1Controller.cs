using LivingActors;
using Unity.Mathematics;
using UnityEngine;

namespace LivingActors
{
    public class Boss1Controller : LivingActor
    {
        [SerializeField] private float speed;
        [SerializeField] private float sinSpeed;
        [SerializeField] private float ceiling;
        [SerializeField] private float floor;
        [SerializeField] private float width;

        private float xLoc;
        private float yLoc;
        private bool yDir;
        private bool xDir;
        private bool _dirChange;
        private Transform _trans;

        public float x;

        void Start()
        {
            InitLivingActor();

            this._trans = GetComponent<Transform>();
            this.xLoc = this._rb2d.position.x;
            this.yDir = false;
            this.xDir = true;
            this._dirChange = false;
            this.yLoc = this.ceiling;
        }

        void FixedUpdate()
        {
            this._state = this._state.DoState(this);
        }

        public override IState DoIdleState()
        {
            float newX;

            if (this.yDir)
                this.yLoc += this.speed;
            else
                this.yLoc -= this.speed;

            newX = math.sin(this.xLoc);

            this.x = newX;

            ChangePosition(newX * this.width, this.yLoc);

            if (this.yLoc >= this.ceiling)
                this.yDir = false;
            else if (this.yLoc <= this.floor)
                this.yDir = true;

            if (math.abs(newX) > 0.96 && !this._dirChange)
            {
                this.xDir = (!this.xDir);
                _dirChange = true;
            }

            if (this.xDir)
            {
                _trans.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                _trans.localScale = new Vector3(1, 1, 1);
            }

            if (math.abs(newX) < 0.95)
                this._dirChange = false;

            this.xLoc += this.sinSpeed;

            return this.idlestate;
        }
    }
}
