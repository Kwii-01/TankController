using System.Collections;
using System.Collections.Generic;

using Entities;

using UnityEngine;

using DG.Tweening;
using TankController;

namespace Weapons {
    public class Range : Weapon {
        [SerializeField] private Transform _firePoint;
        [SerializeField] private RangeWeaponSettingSO _settingSO;
        private Rigidbody _userRigidbody;
        private Animator _animator;

        public override void SetUser(Entity entity) {
            base.SetUser(entity);
            this._userRigidbody = entity.GetComponent<Rigidbody>();
            this._animator = entity.GetComponentInChildren<Animator>();
        }

        protected override void Use() {
            base.Use();
            this._animator.SetTrigger("Shoot");
            DOVirtual.DelayedCall(this._settingSO.UseDelay, this.Shoot);
        }

        protected override void UseEnded() {
            base.UseEnded();
        }

        public override T GetSettings<T>() {
            return this._settingSO as T;
        }

        protected void Shoot() {
            this.ShootRecoil();
            this.ShootCamShake();
            this.ShootVFX();
            if (this._settingSO.InstantiatedProjectile == false) {
                this.SimulateShoot();
            }
        }

        protected void ShootRecoil() {
            this._userRigidbody.AddExplosionForce(this._settingSO.RecoilForce, this._firePoint.position + this._settingSO.RecoilOffset, this._settingSO.RecoilRadius);
        }

        protected void ShootVFX() {
            if (this._settingSO.ShootVFX) {
                Instantiate(this._settingSO.ShootVFX, this._firePoint.position, this._firePoint.rotation);
            }
        }

        protected void ShootCamShake() {
            Cam.Instance.DoShake(this._settingSO.CamShakeDuration, this._settingSO.CamShakeAmplitude, this._settingSO.CamShakeFrequency);
        }

        /// <summary>
        /// This is a temporary way to shoot at mouse position
        /// </summary>
        protected void SimulateShoot() {
            Ray targetPoint = Cam.Instance.Camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(targetPoint, out RaycastHit hitInfo, this._settingSO.Range)) {
                Vector3 direction = hitInfo.point - this._firePoint.position;
                if (Physics.SphereCast(this._firePoint.position, .25f, direction.normalized, out RaycastHit hitInfo1)) {
                    Instantiate(this._settingSO.HitVFX, hitInfo1.point, Quaternion.LookRotation(hitInfo1.normal, Vector3.forward) * Quaternion.Euler(90f, 0f, 0f));
                }
            }
        }
    }
}