using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace YsoCorp {
    public class Car : Vehicle {
        private const float MAX_STEER = 30f;
        [Header("Car")]
        public float maxSpeed;
        public float acceleration;
        public float braking;
        public AnimationCurve speedCurve;
        public AnimationCurve dragCurve;
        public Wheel[] wheels;

        [Header("Air control")]
        public float maxSpeedAir;
        public float accelerationAir;
        public AnimationCurve speedAirCurve;
        public AnimationCurve dragAirCurve;
        //  public CarVFX vfxs;

        private float _power;
        private float _powerAC;
        private float _targetSteer;
        private float _steerSpeedFactor;
        private float _currentSpeed;
        private float _normalizedSpeed;

        protected override void Awake() {
            base.Awake();
            this.rb.maxAngularVelocity = this.maxSpeedAir;
        }

        protected void FixedUpdate() {
            this.ApplyForcesOnWheels();
            //  this.ApplyAirControl();
            //   this.vfxs.SetSpeedIntensity(this._normalizedSpeed);
        }

        public override void Move(float direction) {
            if (this.isBlackout == false) {
                if (direction > 0) {
                    //  this.vfxs.SetBoost(true);
                    this._power = this.acceleration;
                } else if (direction < 0) {
                    //    this.vfxs.SetBoost(false);
                    this._power = this.braking;
                }
            }
        }

        public override void Stop() {
            //     this.vfxs.SetBoost(false);
            this._power = 0f;
        }

        public override void Steer(float direction) {
            this._steerSpeedFactor = 2f;
            if (direction > 0) {
                this._targetSteer = MAX_STEER;
                this._powerAC = this.accelerationAir;
            } else if (direction < 0) {
                this._targetSteer = -MAX_STEER;
                this._powerAC = -this.accelerationAir;
            } else {
                this._powerAC = 0f;
                this._targetSteer = 0f;
                this._steerSpeedFactor = 4f;
            }
        }

        public bool IsGrounded() {
            foreach (Wheel wheel in this.wheels) {
                if (wheel.isGrounded == false) {
                    return false;
                }
            }
            return true;
        }

        public override float GetNormalizedSpeed() {
            return this._normalizedSpeed;
        }

        private void ApplyForcesOnWheels() {
            float speed = this.CalculateSpeed();
            foreach (Wheel wheel in this.wheels) {
                if (wheel.control == true) {
                    wheel.transform.localRotation = Quaternion.RotateTowards(wheel.transform.localRotation, Quaternion.AngleAxis(this._targetSteer, Vector3.up), this._steerSpeedFactor);
                }
                wheel.CalculateForces(speed);
            }
        }

        // private void ApplyAirControl() {
        //     if (this.IsGrounded() == false) {
        //         if (this._powerAC != 0f) {
        //             this.rb.AddTorque(this.CalculateACInDirection(this.transform.up, this._powerAC));
        //         } else {
        //             this.rb.AddTorque(this.rb.angularVelocity.normalized * this.CalculateAngularDrag());
        //         }
        //     }
        // }

        private float CalculateSpeed() {
            this._currentSpeed = Vector3.Dot(this.transform.forward, this.rb.velocity);
            this._normalizedSpeed = Mathf.Clamp01(Mathf.Abs(this._currentSpeed) / this.maxSpeed);
            if (this._power != 0f) {
                return this.speedCurve.Evaluate(this._normalizedSpeed) * this._power;
            }
            return this.dragCurve.Evaluate(1 - this._normalizedSpeed) * -this._currentSpeed * this.rb.mass;
        }

        // private float CalculateAngularDrag() {
        //     float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(this.rb.angularVelocity.magnitude) / this.maxSpeedAir);
        //     return this.dragAirCurve.Evaluate(1 - normalizedSpeed) * -this.rb.angularVelocity.magnitude * this.rb.mass;
        // }

        // private Vector3 CalculateACInDirection(Vector3 direction, float acceleration) {
        //     float speed = Vector3.Dot(direction, this.rb.angularVelocity);
        //     float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(speed) / this.maxSpeedAir);
        //     return direction * this.speedAirCurve.Evaluate(normalizedSpeed) * acceleration;
        // }
    }
}