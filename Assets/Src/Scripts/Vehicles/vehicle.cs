using System.Collections;
using System.Collections.Generic;

using Entities;

using UnityEngine;

namespace YsoCorp {
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Vehicle : Entity {
        public Rigidbody rb;
        protected bool isBlackout;

        protected override void Reset() {
            base.Reset();
            this.rb = this.GetComponent<Rigidbody>();
        }

        public abstract void Steer(float range);// between -1 / 1
        public abstract void Move(float range);// between -1 / 1
        public abstract void Stop();
        public abstract float GetNormalizedSpeed();
    }
}