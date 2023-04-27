using UnityEngine;

namespace EcsRunner {
    [CreateAssetMenu]
    public class Configuration : ScriptableObject {
        public int GeneratorChainLength = 5;
        public TileView TileView;
        public ColumnView ColumnView;
        public CoinView CoinView;
        public float Speed = 0.005f;
        public float StaticForcePointX = 28f;
        public float ModelDeletePointX = 50f;
        public float Force = 0.05f;
        public int NumberOfSides = 4;
        public float deleteTimeRemaining = 3;
    }
}