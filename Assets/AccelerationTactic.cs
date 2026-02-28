using System;

public interface AccelerationTactic {
    public float DeltaSpeed(float deltaTime);

    [Serializable]
    public struct NoAcceleration : AccelerationTactic {
        public float DeltaSpeed(float deltaTime) {
            return 0;
        }
    }

    [Serializable]
    public struct LinearAcceleration : AccelerationTactic {
        public float acceleration;

        public float DeltaSpeed(float deltaTime) {
            return acceleration * deltaTime;
        }
    }
}