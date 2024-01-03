using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace YsoCorp {
    public class Wheel : MonoBehaviour {
        public Rigidbody rbVehicle;
        public bool control;
        [Header("Suspension")]
        public float restDistance;
        public float strength;
        public float damping;
        [Header("Steering")]
        public float gripFactor;
        public float tireMass;
        [Header("Visuals")]
        public Transform model;
        public float markThreshold;
        public TrailRenderer trail;
        public bool isGrounded { get; private set; } = true;

        public void CalculateForces(float speed) {
            if (Physics.Raycast(this.transform.position, -this.transform.up, out RaycastHit hitInfo, this.restDistance)) {
                Vector3 tireVelocity = this.rbVehicle.GetPointVelocity(this.transform.position);
                //suspension
                float upVelocity = Vector3.Dot(this.transform.up, tireVelocity);
                float offset = this.restDistance - hitInfo.distance;
                float force = (offset * this.strength) - (upVelocity * this.damping);
                Vector3 springForce = this.transform.up * force;

                // //steering
                float steeringVel = Vector3.Dot(this.transform.right, tireVelocity);
                float velChange = -(steeringVel * this.gripFactor) / Time.fixedDeltaTime * this.tireMass;
                Vector3 steeringForce = this.transform.right * velChange;

                this.rbVehicle.AddForceAtPosition(springForce + steeringForce + this.transform.forward * speed, this.transform.position);

                //  this.model.localPosition = Vector3.up * (offset - this.transform.localPosition.y);
                //  this.trail.emitting = Mathf.Abs(velChange) > this.markThreshold;
                this.isGrounded = true;
            } else {
                this.isGrounded = false;
                //  this.model.localPosition = Vector3.up * -this.transform.localPosition.y;
                //  this.trail.emitting = false;
            }
        }
    }
}