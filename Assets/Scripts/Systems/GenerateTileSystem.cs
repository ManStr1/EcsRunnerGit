using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace EcsRunner.Systems {
    public class GenerateTileSystem : IEcsRunSystem {
        private readonly EcsWorldInject _world = default;

        private readonly EcsCustomInject<Configuration> _configuration = default;
        private readonly EcsCustomInject<SceneData> _sceneData = default;

        private readonly EcsPoolInject<Tile> _tilePool = default;
        private readonly EcsPoolInject<Position> _positionPool = default;
        private readonly EcsPoolInject<IsFreeFromObstacles> _obstaclesMarkerPool = default;
        private readonly EcsPoolInject<ViewTypeRef> _viewTypeRefPool = default;
        private readonly EcsPoolInject<DeleteOrder> _deleteOrderPool = default;
        private readonly EcsPoolInject<HasScorePoint> _hasScorePointPool = default;
        private readonly EcsPoolInject<LastTile> _lastTilePool = default;

        private readonly EcsFilterInject<Inc<Tile, Position, SpawnDetector>> _filterSpawnDetector = default;

        public void Run(IEcsSystems systems) {
            if ( /* _filter_tiles.Value.GetEntitiesCount() == 26 || */
                _filterSpawnDetector.Value.GetEntitiesCount() > 0) {
                foreach (int entity in _filterSpawnDetector.Value) {
                    _filterSpawnDetector.Pools.Inc3.Del(entity);
                }

                for (int tile = 0; tile < _configuration.Value.GeneratorChainLength; tile++) {
                    int tileEntity = _world.Value.NewEntity();
                    _tilePool.Value.Add(tileEntity);

                    Vector3 position = new Vector3(-tile, 0, 0);
                    ref Position p1 = ref _positionPool.Value.Add(tileEntity);
                    p1.value = position;

                    _obstaclesMarkerPool.Value.Add(tileEntity);

                    ref ViewTypeRef v1 = ref _viewTypeRefPool.Value.Add(tileEntity);
                    v1.value = ViewType.Tile;

                    ref DeleteOrder dO = ref _deleteOrderPool.Value.Add(tileEntity);
                    dO.value = _sceneData.Value.EndDeleteOrderInt;

                    _hasScorePointPool.Value.Add(tileEntity);

                    if (tile == _configuration.Value.GeneratorChainLength - 1) {
                        _lastTilePool.Value.Add(tileEntity);
                    }

                    _sceneData.Value.EndDeleteOrderInt++;
                }
            }
        }
    }
}