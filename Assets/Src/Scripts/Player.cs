using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Vehicles;

namespace TankController {
    public class Player : MonoBehaviour {
        [SerializeField] private Cam _cam;
        [SerializeField] private Tank _tank;

        private void Update() {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            this._tank.Move(vertical);
            this._tank.Steer(horizontal);
            Plane plane = new Plane(Vector3.down, this._tank.transform.position.y);
            Ray ray = this._cam.Camera.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out float enter)) {
                Vector3 lookDirection = Vector3.ProjectOnPlane(ray.GetPoint(enter) - this._tank.transform.position, Vector3.up);
                this._tank.Turret.Rotate(this._tank.transform.InverseTransformDirection(lookDirection));
            }

        }

    }
}