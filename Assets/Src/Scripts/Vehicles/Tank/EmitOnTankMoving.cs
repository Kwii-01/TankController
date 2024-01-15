using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Vehicles;

namespace TankController {
    public class EmitOnTankMoving : MonoBehaviour {
        [SerializeField] private Tank _tank;
        [SerializeField] private ParticleSystem _vfx;

        private void Update() {
            if (this._vfx.isEmitting == false) {
                if (this._tank.Speeding || this._tank.Steering) {
                    this._vfx.Play();
                }
            } else {
                if (this._tank.Speeding == false && this._tank.Steering == false) {
                    this._vfx.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                }
            }
        }
    }
}