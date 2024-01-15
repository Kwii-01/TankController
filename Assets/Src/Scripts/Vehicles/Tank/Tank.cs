using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Vehicles {
    public class Tank : Vehicle {
        private static readonly float DEFAULT_SPEED_MULTIPLIER = 1f;
        [Header("Tank")]
        [SerializeField] private float _accelerationTime;
        [SerializeField] private AnimationCurve _accelerationCurve;
        [SerializeField] private Turret _turret;
        public Turret Turret => this._turret;

        [Header("Chains")]
        [SerializeField] private TankChain _leftChain;
        [SerializeField] private TankChain _rightChain;


        private bool _steering;
        private bool _speeding;

        private float _power;
        private float _accelerationTimer;
        private float _leftSpeedMultiplier;
        private float _rightSpeedMultiplier;
        private float _currentSpeed;
        private float _normalizedSpeed;

        private void Start() {
            this._turret.Setup(this);
            this._leftSpeedMultiplier = DEFAULT_SPEED_MULTIPLIER;
            this._rightSpeedMultiplier = DEFAULT_SPEED_MULTIPLIER;
            this._accelerationTimer = this._accelerationTime;
        }

        private void Update() {
            if (this._speeding) {
                if (this._accelerationTimer < this._accelerationTime) {
                    this._accelerationTimer += Time.deltaTime;
                }
            }
        }

        protected void FixedUpdate() {
            this.ApplyForcesOnWheels();
            this.ClampSpeed();
        }

        public override void Move(float direction) {
            if (this.isBlackout == false) {
                this._power = this.statBehaviour.Get(StatSystem.StatType.Acceleration) * direction;
                if (this._speeding == false && direction != 0) {// JUST BEGAN TO SPEED
                    this._accelerationTimer = 0f;
                }
                this._speeding = direction != 0f;
                if (this._speeding == false) {
                    this.Stop();
                }
            }
        }

        public override void Stop() {
            this._power = 0f;
            this._speeding = false;
            this._accelerationTimer = this._accelerationTime;
            this._leftSpeedMultiplier = DEFAULT_SPEED_MULTIPLIER;
            this._rightSpeedMultiplier = DEFAULT_SPEED_MULTIPLIER;
        }

        public override void Steer(float direction) {
            if (this._power == 0f) {
                this.SetSteer(DEFAULT_SPEED_MULTIPLIER, -DEFAULT_SPEED_MULTIPLIER, direction);
            } else {
                this.SetSteer(2f, DEFAULT_SPEED_MULTIPLIER, direction);
            }
        }

        private void SetSteer(float steerSuperior, float steerInferior, float direction) {
            if (direction > 0) {
                this._steering = true;
                this._leftSpeedMultiplier = steerSuperior;
                this._rightSpeedMultiplier = steerInferior;
            } else if (direction < 0) {
                this._steering = true;
                this._leftSpeedMultiplier = steerInferior;
                this._rightSpeedMultiplier = steerSuperior;
            } else {
                this._steering = false;
                this._leftSpeedMultiplier = DEFAULT_SPEED_MULTIPLIER;
                this._rightSpeedMultiplier = DEFAULT_SPEED_MULTIPLIER;
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
            if (this._steering && this._speeding == false) {
                speed = this.statBehaviour.Get(StatSystem.StatType.Acceleration) / 1.5f;
            }
            this._leftChain.Apply(speed * this._leftSpeedMultiplier);
            this._rightChain.Apply(speed * this._rightSpeedMultiplier);
        }

        private void ClampSpeed() {
            Vector3 velocity = this.transform.InverseTransformDirection(this.rb.velocity);
            float maxSpeed = this.statBehaviour.Get(StatSystem.StatType.MoveSpeed);
            velocity.z = Mathf.Clamp(velocity.z, -maxSpeed, maxSpeed);
            this.rb.velocity = this.transform.TransformDirection(velocity);
        }

        private float CalculateSpeed() {
            this._currentSpeed = Vector3.Dot(this.transform.forward, this.rb.velocity);
            this._normalizedSpeed = Mathf.Clamp01(Mathf.Abs(this._currentSpeed) / this.statBehaviour.Get(StatSystem.StatType.MoveSpeed));
            if (this._speeding) {
                return this._accelerationCurve.Evaluate(this._accelerationTime / this._accelerationTimer) * this._power;
            }
            return -this._currentSpeed * this.rb.mass * 2f;
        }

    }
}