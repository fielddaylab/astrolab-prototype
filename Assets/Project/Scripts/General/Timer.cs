

using System;

namespace AstroLab {

    [Serializable]
    public struct Timer {
        public float Period;
        [NonSerialized] public bool Paused;
        [NonSerialized] public float Accumulator;
        [NonSerialized] public bool AdvancedRecently;

        public Timer(float period) {
            Period = period;
            Paused = false;
            Accumulator = 0;
            AdvancedRecently = false;
        }

        public bool Advance(float deltaTime) {
            if (Paused) return false;
            Accumulator += deltaTime;
            if (Accumulator >= Period) {
                AdvancedRecently = true;
                return true;
            } else {
                AdvancedRecently = false;
                return false;
            }
        }

        public float GetProgress() {
            return Accumulator / Period;
        }
    }
}