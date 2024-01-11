using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Vehicles {
    public class Tank : Vehicle {
        [SerializeField] private AnimationCurve _accelerationCurve;
        [SerializeField] private AnimationCurve _dragCurve;
        [SerializeField] private TankChain _leftChain;
        [SerializeField] private TankChain _rightChain;


        private bool _steering;
        private float _power;
        private float _leftMultiplier = 1f;
        private float _rightMultiplier = 1f;
        private float _currentSpeed;
        private float _normalizedSpeed;


        protected void FixedUpdate() {
            this.ApplyForcesOnWheels();
        }

        private void Update() {
            if (Input.GetKey(KeyCode.Z)) {
                this.Move(1f);
            } else if (Input.GetKey(KeyCode.S)) {
                this.Move(-1f);
            } else {
                this.Stop();
            }

            if (Input.GetKey(KeyCode.Q)) {
                this.Steer(-1f);
            } else if (Input.GetKey(KeyCode.D)) {
                this.Steer(1f);
            } else {
                Steer(0f);
            }
        }

        public override void Move(float direction) {
            if (this.isBlackout == false) {
                this._power = this.statBehaviour.Get(StatSystem.StatType.Acceleration) * direction;
                if (this._power == 0f) {
                    this.Stop();
                }
            }
        }

        public override void Stop() {
            this._power = 0f;
            this._leftMultiplier = 1f;
            this._rightMultiplier = 1f;
        }

        public override void Steer(float direction) {
            _leftMultiplier = 1f;
            _rightMultiplier = 1f;
            this._steering = direction != 0f;
            if (this._power == 0f) {
                if (direction > 0) {
                    _leftMultiplier = 1f;
                    _rightMultiplier = -1f;
                } else if (direction < 0) {
                    _leftMultiplier = -1f;
                    _rightMultiplier = 1f;
                }
            } else {
                if (direction > 0) {
                    _leftMultiplier = 2f;
                    _rightMultiplier = 1f;
                } else if (direction < 0) {
                    _leftMultiplier = 1f;
                    _rightMultiplier = 2f;
                }
            }
        }

        public bool IsGrounded() {
            return this._leftChain.IsGrounded() && this._rightChain.IsGrounded();
        }

        public override float GetNormalizedSpeed() {
            return this._normalizedSpeed;
        }

        private void ApplyForcesOnWheels() {
            float speed = this.CalculateSpeed();
            if (this._steering && this._power == 0f) {
                speed = this.statBehaviour.Get(StatSystem.StatType.Acceleration) / 2f;
            }
            this._leftChain.Apply(speed * this._leftMultiplier);
            this._rightChain.Apply(speed * this._rightMultiplier);
        }

        private float CalculateSpeed() {
            this._currentSpeed = Vector3.Dot(this.transform.forward, this.rb.velocity);
            this._normalizedSpeed = Mathf.Clamp01(Mathf.Abs(this._currentSpeed) / this.statBehaviour.Get(StatSystem.StatType.MoveSpeed));
            if (this._power != 0f) {
                return this._accelerationCurve.Evaluate(this._normalizedSpeed) * this._power;
            }
            return -this._currentSpeed * this.rb.mass;
            //return this._dragCurve.Evaluate(1 - this._normalizedSpeed) * -this._currentSpeed * this.rb.mass;
        }

    }
}
