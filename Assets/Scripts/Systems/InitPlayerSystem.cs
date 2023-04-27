using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace EcsRunner.Systems {
    public class InitPlayerSystem : IEcsInitSystem {
        private readonly EcsWorldInject _world = default;

        private readonly EcsCustomInject<SceneData> _sceneData = default;

        private readonly EcsPoolInject<Player> _playerPool = default;
        private readonly EcsPoolInject<Position> _positionPool = default;

        public void Init(IEcsSystems systems) {
            int entity = _world.Value.NewEntity();

            ref Player player = ref _playerPool.Value.Add(entity);
            player.value = _sceneData.Value.Player;

            ref Position position = ref _positionPool.Value.Add(entity);
            position.value = new Vector3(25f, 1.1f, 0);
        }
    }
}