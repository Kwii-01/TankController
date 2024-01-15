using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Weapons {
    public class Range : Weapon {
        [SerializeField] private Transform _firePoint;
        [SerializeField] private RangeWeaponSettingSO _settingSO;

        protected override void Use() {
            base.Use();
        }

        protected override void UseEnded() {
            base.UseEnded();
        }

        public override T GetSettings<T>() {
            return this._settingSO as T;
        }
    }
}