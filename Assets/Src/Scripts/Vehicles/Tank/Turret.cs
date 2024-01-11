using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Vehicles {
    public class Turret : MonoBehaviour {
        [SerializeField] private Transform _weaponTransform;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private Vector2 _clampX;
        private Quaternion _targetRotation;
        private Quaternion _targetWeaponRotation;

        private void Start() {
            this._targetRotation = this.transform.rotation;
        }

        private void Update() {
            this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, this._targetRotation, this._rotationSpeed * Time.deltaTime);
        }

        public void Rotate(Vector3 direction) {

            float angle = Quaternion.Angle(Quaternion.Euler(0f, direction.y, 0f), this.transform.localRotation);
            if (angle > this._clampX.x && angle < this._clampX.y) {
                _targetWeaponRotation = Quaternion.Euler(0f, direction.y, 0f);
            }
            // ONLY Y ROTATION
            direction.y = 0f;
            this._targetRotation = Quaternion.LookRotation(direction.normalized);
        }

        public void BeginShooting() {

        }

        public void StopShooting() {

        }
    }
}