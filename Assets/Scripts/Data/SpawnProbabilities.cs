using UnityEngine;


namespace JigiJumper.Data
{
    [CreateAssetMenu(fileName = "Probability Data", menuName = "Data/SpawnProbability")]
    public class SpawnProbabilities : ScriptableObject
    {
        [Header("Planet Controller")]
        public PlanetType planetType = PlanetType.Large; // todo make a list of them based on probability

        [Header("Oscillator Controller")]
        [Range(0f, 1f)]
        public float oscillationSpeed = 0f;

        [Header("Destructor Controller")]
        public float selfDestructionTimer = 5f;
        public float rotationSpeed = 50f;
        public State state = State.None;
    }
}