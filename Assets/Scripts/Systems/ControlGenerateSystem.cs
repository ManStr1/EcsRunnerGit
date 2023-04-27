using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace EcsRunner.Systems {
    public class ControlGenerateSystem : IEcsRunSystem {
        private readonly EcsPoolInject<SpawnDetector> _spawnDetectorPool = default;

        private readonly EcsFilterInject<Inc<Tile, Position, LastTile>> _filterLastTile = default;

        public void Run(IEcsSystems systems) {
            if (_filterLastTile.Value.GetEntitiesCount() > 0) {
                foreach (int entity in _filterLastTile.Value) {
                    ref var position = ref _filterLastTile.Pools.Inc2.Get(entity).value;

                    if ((float)System.Math.Round(position.x, 2) == 1) {
                        _spawnDetectorPool.Value.Add(entity);
                        _filterLastTile.Pools.Inc3.Del(entity);
                    }
                }
            }
        }
    }
}