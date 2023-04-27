using System.Collections;
using DG.Tweening;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace EcsRunner.Systems {
    public class RotateSystem : IEcsRunSystem {
        private readonly EcsCustomInject<Configuration> _configuration = default;
        private readonly EcsCustomInject<SceneData> _sceneData = default;
        
        private readonly EcsFilterInject<Inc<TilesAndColsContainerIdentificator, RotationX>> _filterIdentificator = default;

        public void Run(IEcsSystems systems) {
            if (Input.GetMouseButtonDown(0) && DOTween.PlayingTweens() == null) {
                int configurationDirection = 360 / _configuration.Value.NumberOfSides;

                int direction = Input.mousePosition.x < Screen.width / 2
                    ? configurationDirection
                    : -configurationDirection;

                if (_filterIdentificator.Value.GetEntitiesCount() > 0) {
                    foreach (int entity in _filterIdentificator.Value) {
                        ref var rotationX = ref _filterIdentificator.Pools.Inc2.Get(entity);
                        
                        if (rotationX.value == 0 && direction > 0) {
                            rotationX.value = 360 - direction;
                        }
                        else {
                            rotationX.value = (rotationX.value - direction) % 360;
                        }
                        
                        _sceneData.Value.StartCoroutine(
                            DoRotating(_filterIdentificator.Pools.Inc1.Get(entity).value, direction));
                    }
                }
                
            }
        }

        IEnumerator DoRotating(GameObject modelView, int direction) {
            //Tween myTween = modelView.gameObject.transform.DOBlendableRotateBy(new Vector3(direction, 0, 0), 0.5f).SetLink(modelView.gameObject, LinkBehaviour.KillOnDisable);
            Tween myTween = modelView.gameObject.transform.DOBlendableLocalRotateBy(new Vector3(direction, 0, 0), 0.5f)
                .SetLink(modelView.gameObject, LinkBehaviour.KillOnDisable);
            yield return myTween.WaitForCompletion();
            // This log will happen after the tween has completed
        }
    }
}