using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace EcsRunner.Systems {
    public class CheckPlayerFallPositionSystem : IEcsRunSystem {
        private readonly EcsCustomInject<Configuration> _configuration = default;

        private readonly EcsPoolInject<Loser> _loserPool = default;

        private readonly EcsFilterInject<Inc<Player, Position>, Exc<Loser>> _filterPlayer = default;

        public void Run(IEcsSystems systems) {
            foreach (int playerEntity in _filterPlayer.Value) {
                ref var gamePosition = ref _filterPlayer.Pools.Inc2.Get(playerEntity);
                ref var player = ref _filterPlayer.Pools.Inc1.Get(playerEntity);

                if (gamePosition.value.x >= _configuration.Value.StaticForcePointX) {
                    player.value.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    player.value.GetComponent<BoxCollider>().enabled = false;

                    Debug.Log("FALLED!");
                    _loserPool.Value.Add(playerEntity);
                }
            }
        }
    }
}