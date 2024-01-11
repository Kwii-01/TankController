using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Vehicles {
    public class TankChain : MonoBehaviour {
        [SerializeField] private Wheel[] _wheels;

        public void Apply(float speed) {
            foreach (Wheel wheel in this._wheels) {
                wheel.CalculateForces(speed);
            }
            Vector3 position = this.transform.localPosition;
            float averageOffset = this._wheels.Sum(w => w.Offset) / this._wheels.Length;
            position.y = Mathf.Min(.065f, averageOffset);
            this.transform.localPosition = position;
        }

        public bool IsGrounded() {
            foreach (Wheel wheel in this._wheels) {
                if (wheel.IsGrounded == false) {
                    return false;
                }
            }
            return true;
        }
    }
}