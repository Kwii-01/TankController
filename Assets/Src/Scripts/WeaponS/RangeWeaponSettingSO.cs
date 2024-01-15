using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Weapons {
    [CreateAssetMenu(menuName = "Weapons/Range")]
    public class RangeWeaponSettingSO : WeaponSettingSO {
        // PROJECTILE ??
        public float Damages;

        [Header("Shoot recoil")]
        public float RecoilForce;
        public float RecoilRadius;
        public Vector3 RecoilOffset;

        [Header("Cam shake")]
        public float CamShakeDuration;
        public float CamShakeAmplitude;
        public float CamShakeFrequency;

        [Header("Projectile")]
        public bool InstantiatedProjectile;
        public Projectile ProjectilePrefab;

        [Header("VFX")]
        public GameObject ShootVFX;
        public GameObject HitVFX;
        public GameObject HitDecalVFX;
    }
}