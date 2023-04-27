using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace EcsRunner.Systems {
    public class PlayerMovementSystem : IEcsRunSystem {

        private readonly EcsFilterInject<Inc<Player, Position>> _filterPlayer = default;

        public void Run(IEcsSystems systems) {
            foreach (int entity in _filterPlayer.Value) {
                ref var gamePosition = ref _filterPlayer.Pools.Inc2.Get(entity);
                ref var player = ref _filterPlayer.Pools.Inc1.Get(entity);
                var scenePosition = player.value.transform;

                if (scenePosition.position.y < -20) {
                    Object.Destroy(player.value.gameObject);
                    _filterPlayer.Pools.Inc2.Del(entity);
                }

                gamePosition.value = scenePosition.position;
            }
        }
    }
}