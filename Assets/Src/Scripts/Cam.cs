using System.Collections;
using System.Collections.Generic;

using Cinemachine;

using UnityEngine;

namespace TankController {
    public class Cam : MonoBehaviour {
        [field: SerializeField] public Camera Camera { get; private set; } = default;
        private CinemachineBrain _brain;
        private CinemachineBasicMultiChannelPerlin _perlinNoise;

        public void DoShake(float duration, float amplitude = 1f, float frequency = 10f) {

        }
    }
}