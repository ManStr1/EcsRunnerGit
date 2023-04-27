using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System;
using System.Linq;
using UnityEngine;

namespace EcsRunner.Systems {
    public class InitTilesSystem : IEcsInitSystem {
        private readonly EcsWorldInject _world = default;

        private readonly EcsCustomInject<SceneData> _sceneData = default;

        private readonly EcsPoolInject<TilesAndColsContainerIdentificator> _tilesAndColsContainerIdentificatorPool = default;

        private readonly EcsPoolInject<Tile> _tilePool = default;
        private readonly EcsPoolInject<Position> _positionPool = default;
        private readonly EcsPoolInject<ViewTypeRef> _viewTypeRefPool = default;
        private readonly EcsPoolInject<LastTile> _lastTilePool = default;
        private readonly EcsPoolInject<View> _viewPool = default;
        private readonly EcsPoolInject<DeleteOrder> _deleteOrderPool = default;
        private readonly EcsPoolInject<HasScorePoint> _hasScorePointPool = default;

        public void Init(IEcsSystems systems) {
            TileView[] startTiles = GameObject.FindObjectsOfType<TileView>();
            TileView[] sortedTiles = new TileView[startTiles.Length];
            float[] listOfTranformTiles = new float[startTiles.Length];

            //Get all transforms
            for (int i = 0; i < startTiles.Length; i++) {
                listOfTranformTiles[i] = startTiles[i].transform.position.x;
            }

            //Sort all transforms
            Array.Sort(listOfTranformTiles);

            //Clone tiles in transform order
            for (int i = 0; i < sortedTiles.Length; i++) {
                for (int j = 0; j < startTiles.Length; j++) {
                    if (startTiles[j].transform.position.x == listOfTranformTiles[i]) {
                        sortedTiles[i] = startTiles[j];
                        break;
                    }
                }
            }

            //create container entity and set rotation
            int tilesAndColsContainerEntity = _world.Value.NewEntity();
            ref var val = ref _tilesAndColsContainerIdentificatorPool.Value.Add(tilesAndColsContainerEntity);
            val.value = _sceneData.Value.TilesAndColsContainer.gameObject;

            for (int tile = sortedTiles.Length - 1; tile >= 0; tile--) {
                int tileEntity = _world.Value.NewEntity();

                _tilePool.Value.Add(tileEntity);
                Vector3 position = sortedTiles[tile].transform.position;

                ref Position p1 = ref _positionPool.Value.Add(tileEntity);
                p1.value = position;

                ref ViewTypeRef v1 = ref _viewTypeRefPool.Value.Add(tileEntity);
                v1.value = ViewType.Tile;

                ref View c1 = ref _viewPool.Value.Add(tileEntity);
                c1.value = sortedTiles[tile];

                ref DeleteOrder dO = ref _deleteOrderPool.Value.Add(tileEntity);
                dO.value = _sceneData.Value.EndDeleteOrderInt;

                _hasScorePointPool.Value.Add(tileEntity);

                _sceneData.Value.EndDeleteOrderInt++;

                if (position.x == 0) _lastTilePool.Value.Add(tileEntity);
            }
        }
    }
}