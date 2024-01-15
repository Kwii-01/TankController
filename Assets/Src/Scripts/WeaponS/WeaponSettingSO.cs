using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Weapons {
    public abstract class WeaponSettingSO : ScriptableObject {
        public string Name;
        public float UseSpeed;
        public float UseDelay;
        public float Range;
    }
}