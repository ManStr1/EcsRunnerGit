using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace EcsRunner.Systems {
    public class MoveModelSystem : IEcsRunSystem {
        private readonly EcsCustomInject<Configuration> _configuration = default;
        private readonly EcsCustomInject<SceneData> _sceneData = default;

        private readonly EcsFilterInject<Inc<Position, Movable>> _filterMovable = default;
        private readonly EcsFilterInject<Inc<Position, HasScorePoint>> _filterHasScorePoint = default;

        public void Run(IEcsSystems systems) {
            foreach (int entity in _filterMovable.Value) {
                ref Vector3 position = ref _filterMovable.Pools.Inc1.Get(entity).value;

                if (position.x >= _configuration.Value.StaticForcePointX) {

                    if (_filterHasScorePoint.Pools.Inc2.Has(entity)) {

                        _sceneData.Value.SceneScore++;
                        _filterHasScorePoint.Pools.Inc2.Del(entity);

                    }

                    position += new Vector3(_configuration.Value.Speed + _configuration.Value.Force, 0, 0);
                }
                else {
                    position += new Vector3(_configuration.Value.Speed, 0, 0);
                }
            }
        }
    }
}