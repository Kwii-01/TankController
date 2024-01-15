using System.Collections;
using System.Collections.Generic;

using Entities;

using UnityEngine;

using Weapons;

namespace Vehicles {
    public class Turret : MonoBehaviour {
        [SerializeField] private Weapon _weapon;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private Vector2 _clampX;
        private Quaternion _targetRotation;
        //   private Quaternion _targetWeaponRotation;

        private void Start() {
            this._targetRotation = this.transform.localRotation;
        }

        private void Update() {
            this.transform.localRotation = Quaternion.RotateTowards(this.transform.localRotation, this._targetRotation, this._rotationSpeed * Time.deltaTime);
        }

        public void Rotate(Vector3 direction) {

            // float angle = Quaternion.Angle(Quaternion.Euler(0f, direction.y, 0f), this.transform.localRotation);
            // if (angle > this._clampX.x && angle < this._clampX.y) {
            //     _targetWeaponRotation = Quaternion.Euler(0f, direction.y, 0f);
            // }
            this._targetRotation = Quaternion.Euler(0, 8f, Quaternion.LookRotation(direction.normalized).eulerAngles.y);
        }

        public void Setup(Entity entity) {
            this._weapon.SetUser(entity);
        }

        public void BeginShooting() {
            this._weapon.BeginUse();
        }

        public void StopShooting() {
            this._weapon.EndUse();
        }
    }
}