using System.Collections;
using System.Collections.Generic;

using Entities;

using UnityEditor;

using UnityEngine;

namespace Weapons {
    public abstract class Weapon : MonoBehaviour {
        protected float _useTimer;
        public bool Using { get; private set; } = false;
        protected Entity entity;

        protected virtual void Update() {
            if (this.entity) {
                this.UpdateUseTimer();
            }
        }

        public void BeginUse() {
            this.Using = true;
            if (this._useTimer <= 0f) {
                this.Use();
            }
        }

        public void EndUse() {
            this.Using = false;
            this.UseEnded();
        }

        public virtual void SetUser(Entity entity) {
            this.entity = entity;
        }

        protected virtual void Use() {
            WeaponSettingSO settingSO = this.GetSettings<WeaponSettingSO>();
            this._useTimer = settingSO.UseSpeed;
        }

        protected virtual void UseEnded() {

        }

        public abstract T GetSettings<T>() where T : WeaponSettingSO;

        private void UpdateUseTimer() {
            if (this._useTimer > 0f) {
                this._useTimer -= Time.deltaTime;
            }
            if (this.Using && this._useTimer <= 0f) {
                this.Use();
            }
        }

    }
}