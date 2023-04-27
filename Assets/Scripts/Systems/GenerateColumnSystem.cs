using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace EcsRunner.Systems {
    public class GenerateColumnSystem : IEcsRunSystem {
        private readonly EcsWorldInject _world = default;
        
        private readonly EcsCustomInject<Configuration> _configuration = default;

        private readonly EcsPoolInject<Column> _columnPool = default;
        private readonly EcsPoolInject<Coin> _coinPool = default;
        private readonly EcsPoolInject<Position> _positionPool = default;
        private readonly EcsPoolInject<ViewTypeRef> _viewTypeRefPool = default;
        private readonly EcsPoolInject<RotationX> _rotationXPool = default;

        private readonly EcsFilterInject<Inc<Tile, Position, LastTile>> _filterLastTile = default;
        private readonly EcsFilterInject<Inc<Tile, Position, DeleteOrder, IsFreeFromObstacles>> _filterFreeFromObstacles = default;

        public void Run(IEcsSystems systems) {
            if (_filterLastTile.Value.GetEntitiesCount() > 0) {
                
                int configurationDirection = 360 / _configuration.Value.NumberOfSides;
                
                foreach (int tileEntity in _filterFreeFromObstacles.Value) {
                    ref var position = ref _filterFreeFromObstacles.Pools.Inc2.Get(tileEntity);
                    ref var deleteOrder = ref _filterFreeFromObstacles.Pools.Inc3.Get(tileEntity);

                    int count = 1;
                    int type = Random.Range(0, 2);
                    //int col = 1;

                    for (int j = 0; j < count; j++) {
                        int entity = _world.Value.NewEntity();

                        ref ViewTypeRef v1 = ref _viewTypeRefPool.Value.Add(entity);

                        switch (type) {
                            case 0:
                                _columnPool.Value.Add(entity);
                                v1.value = ViewType.Column;
                                break;
                            case 1:
                                _coinPool.Value.Add(entity);
                                v1.value = ViewType.Coin;
                                break;
                        }
                        
                        ref Position p1 = ref _positionPool.Value.Add(entity);
                        p1.value = new Vector3(position.value.x, 0, position.value.z);

                        int rnd = Random.Range(0, 4);
                        ref var rotationX = ref _rotationXPool.Value.Add(entity);
                        rotationX.value = rnd * configurationDirection;

                        ref DeleteOrder dO = ref _filterFreeFromObstacles.Pools.Inc3.Add(entity);
                        dO.value = deleteOrder.value;
                    }


                    _filterFreeFromObstacles.Pools.Inc4.Del(tileEntity);
                }
            }
        }

    }
}