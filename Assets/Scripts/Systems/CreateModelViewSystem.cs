using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace EcsRunner.Systems {
    public class CreateModelViewSystem : IEcsRunSystem {
        private readonly EcsCustomInject<Configuration> _configuration = default;
        private readonly EcsCustomInject<SceneData> _sceneData = default;

        private readonly EcsPoolInject<View> _viewPool = default;
        private readonly EcsPoolInject<Movable> _movablePool = default;
        private readonly EcsPoolInject<RotationX> _rotationXPool = default;

        private readonly EcsFilterInject<Inc<Position, ViewTypeRef>, Exc<View>> _filterViewTypeRefWithoutView = default;

        public void Run(IEcsSystems systems) {
            foreach (int entity in _filterViewTypeRefWithoutView.Value) {
                ref ViewTypeRef obViewTypeRef = ref _filterViewTypeRefWithoutView.Pools.Inc2.Get(entity);
                ref Position position = ref _filterViewTypeRefWithoutView.Pools.Inc1.Get(entity);
                ModelView view;

                switch (obViewTypeRef.value) {
                    case ViewType.Tile:
                        view = GameObject.Instantiate(_configuration.Value.TileView,
                            _sceneData.Value.TilesAndColsContainer, false);
                        break;
                    case ViewType.Column:
                        view = GameObject.Instantiate(_configuration.Value.ColumnView,
                            _sceneData.Value.TilesAndColsContainer, false);

                        if (_rotationXPool.Value.Has(entity)) {
                            ref var rotationX = ref _rotationXPool.Value.Get(entity);
                            view.transform.Rotate(new Vector3(rotationX.value, 0, 0));
                        }
                        
                        break;
                    case ViewType.Coin:
                        view = GameObject.Instantiate(_configuration.Value.CoinView,
                            _sceneData.Value.TilesAndColsContainer, false);

                        if (_rotationXPool.Value.Has(entity)) {
                            ref var rotationX = ref _rotationXPool.Value.Get(entity);
                            view.transform.Rotate(new Vector3(rotationX.value, 0, 0));
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                _movablePool.Value.Add(entity);
                view.transform.position = new Vector3(position.value.x, position.value.y, position.value.z);

                ref View c1 = ref _viewPool.Value.Add(entity);
                c1.value = view;
            }
        }
    }
}