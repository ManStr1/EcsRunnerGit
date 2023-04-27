using System.Collections;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using EcsRunner.Systems;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;

namespace EcsRunner {
    sealed class EcsStartup : MonoBehaviour {
        EcsWorld _world;
        IEcsSystems _systems;
        public Configuration Configuration;
        public SceneData SceneData;

        void Start() {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);
            _systems
                .Add(new InitUISystem())
                .Add(new InitPlayerSystem())
                .Add(new InitTilesSystem())

                .Add(new StartGameSystem())
                
                .Add(new ControlGenerateSystem())
                
                .Add(new GenerateTileSystem())
                .Add(new GenerateColumnSystem())
                
                .Add(new CreateModelViewSystem())
                
                .Add(new MoveModelSystem())
                .Add(new MoveModelViewSystem())
                
                .Add(new RotateSystem())
                
                .Add(new DoesPlayerLoseWhileColsRotatingSystem())
                .Add(new PlayerMovementSystem())

                .Add(new CheckPlayerFallPositionSystem())
                .Add(new CheckPlayerCoinPosition())
                
                
                .Add(new CheckPlayerLoseSystem())
                
                .Add(new DeleteModelSystem())
                
                .Add(new EndGamePresetDeleteSystem())
                .Add(new EndGameDeleteSystem())

                .Add(new UISystem())
                // register additional worlds here, for example:
                // .AddWorld (new EcsWorld (), "events")
#if UNITY_EDITOR
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Inject(_world)
                .Inject(Configuration)
                .Inject(SceneData)
                .Init();
        }

        void Update() {
            // process systems here.
            _systems?.Run();
        }

        void OnDestroy() {
            if (_systems != null) {
                // list of custom worlds will be cleared
                // during IEcsSystems.Destroy(). so, you
                // need to save it here if you need.
                _systems.Destroy();
                _systems = null;
            }

            // cleanup custom worlds here.

            // cleanup default world.
            if (_world != null) {
                _world.Destroy();
                _world = null;
            }
        }
    }

    public class InitUISystem : IEcsInitSystem {
        private readonly EcsCustomInject<SceneData> _sceneData = default;

        private readonly EcsPoolInject<Starter> _starterPool = default;

        private readonly EcsFilterInject<Inc<TilesAndColsContainerIdentificator>, Exc<Starter>> _filterGameStarter = default;


        public void Init(IEcsSystems systems) {
            SaveData.LoadFile();

            #region GamePlay Menu 

            _sceneData.Value.SceneCoins = SaveData.currentCoins;
            _sceneData.Value.SceneDiamonds = SaveData.currentDiamonds;
            _sceneData.Value.SceneBestScore = SaveData.currentBestScore;
            _sceneData.Value.SceneSkinNumber = SaveData.currentSkinNumber;
            _sceneData.Value.SceneBadgeNumber = SaveData.currentBadgeNumber;

            _sceneData.Value.gameUI.MainMenuUI.PlayerInfoPanelUI.DiamondsContainerUI.DiamondsText.text = _sceneData.Value.SceneDiamonds + "";

            foreach (GameObject view in _sceneData.Value.Player.Views) {
                if (int.Parse(view.name) == _sceneData.Value.SceneSkinNumber) {
                    view.SetActive(true);
                } else {
                    view.SetActive(false);
                }
            }

            #endregion

            #region Main Menu

            _sceneData.Value.gameUI.MainMenuUI.gameObject.SetActive(true);

            _sceneData.Value.gameUI.MainMenuUI.StartButton.onClick.AddListener(() => {

                foreach (int entity in _filterGameStarter.Value) {
                    _starterPool.Value.Add(entity);
                }

                _sceneData.Value.gameUI.MainMenuUI.Ad2RewardButton.gameObject.SetActive(false);
                _sceneData.Value.gameUI.MainMenuUI.StartButton.gameObject.SetActive(false);
                _sceneData.Value.gameUI.MainMenuUI.ButtonPanelUI.gameObject.SetActive(false);

            });

            _sceneData.Value.gameUI.MainMenuUI.ButtonPanelUI.ProfileButton.onClick.AddListener(() => {
                _sceneData.Value.gameUI.MainMenuUI.gameObject.SetActive(false);
                _sceneData.Value.gameUI.ProfileMenuUI.gameObject.SetActive(true);
            });

            _sceneData.Value.gameUI.MainMenuUI.ButtonPanelUI.MissionButton.onClick.AddListener(() => {
                _sceneData.Value.gameUI.MainMenuUI.gameObject.SetActive(false);
                _sceneData.Value.gameUI.MissionsMenuUI.gameObject.SetActive(true);
            });

            _sceneData.Value.gameUI.MainMenuUI.ButtonPanelUI.StoreButton.onClick.AddListener(() => {
                _sceneData.Value.gameUI.MainMenuUI.gameObject.SetActive(false);
                _sceneData.Value.gameUI.StoreMenuUI.gameObject.SetActive(true);
            });

            _sceneData.Value.gameUI.MainMenuUI.PlayerInfoPanelUI.CoinsContainerUI.CoinsText.text = _sceneData.Value.SceneCoins + "";
            _sceneData.Value.gameUI.MainMenuUI.PlayerInfoPanelUI.DiamondsContainerUI.DiamondsText.text = _sceneData.Value.SceneDiamonds + "";


            #endregion

            #region Profile Menu

            _sceneData.Value.gameUI.ProfileMenuUI.ProfileMenuPlayerInfoUI.BackButton.onClick.AddListener(() => {
                _sceneData.Value.gameUI.ProfileMenuUI.gameObject.SetActive(false);
                _sceneData.Value.gameUI.MainMenuUI.gameObject.SetActive(true);
            });

            _sceneData.Value.gameUI.ProfileMenuUI.ProfileMenuPlayerInfoUI.ProfileMenuPlayerTextContainerUI.BestScoreText.text = "Best score \n" + _sceneData.Value.SceneBestScore;


            // Skin list
            _sceneData.Value.gameUI.ProfileMenuUI.ProfileMenuSkinsUI.ShowCaseImage.sprite =
                _sceneData.Value.gameUI.ProfileMenuUI.ProfileMenuSkinsUI.SkinScrollContainerUI.SkinList.Skins[_sceneData.Value.SceneSkinNumber].GetComponent<Image>().sprite;

            _sceneData.Value.gameUI.MainMenuUI.ButtonPanelUI.ProfileButton.onClick.AddListener(() => {
                foreach (SkinUI skin in _sceneData.Value.gameUI.ProfileMenuUI.ProfileMenuSkinsUI.SkinScrollContainerUI.SkinList.Skins) {

                    skin.Availability = SaveData.currentAvailableSkins[int.Parse(skin.name)];

                    if (skin.Availability) {
                        skin.GetComponent<Image>().color = Color.white;
                        skin.GetComponent<Button>().onClick.AddListener(() => {
                            _sceneData.Value.SceneSkinNumber = int.Parse(skin.name);
                            _sceneData.Value.gameUI.ProfileMenuUI.ProfileMenuSkinsUI.ShowCaseImage.sprite = skin.GetComponent<Image>().sprite;
                        });
                    } else {
                        skin.GetComponent<Image>().color = Color.red;
                    }
                }
            });

            // Skin Select button
            _sceneData.Value.gameUI.ProfileMenuUI.ProfileMenuSkinsUI.SelectButton.onClick.AddListener(() => {

                SaveData.currentSkinNumber = _sceneData.Value.SceneSkinNumber;
                SaveData.SaveFile();

                foreach (GameObject view in _sceneData.Value.Player.Views) {
                    if (int.Parse(view.name) == _sceneData.Value.SceneSkinNumber) {
                        view.SetActive(true);
                    } else {
                        view.SetActive(false);
                    }
                }
            });


            // Badges
            _sceneData.Value.gameUI.ProfileMenuUI.ProfileMenuPlayerInfoUI.AvatarContainerUI.ActiveBadgeImage.sprite =
                _sceneData.Value.gameUI.ProfileMenuUI.ProfileMenuBadgesUI.BadgeScrollContainerUI.BadgeListUI.Badges[_sceneData.Value.SceneBadgeNumber].GetComponent<Image>().sprite;
            foreach (BadgeUI badge in _sceneData.Value.gameUI.ProfileMenuUI.ProfileMenuBadgesUI.BadgeScrollContainerUI.BadgeListUI.Badges) {
                badge.GetComponent<Button>().onClick.AddListener(() => {
                    _sceneData.Value.SceneBadgeNumber = int.Parse(badge.name);
                    SaveData.currentBadgeNumber = _sceneData.Value.SceneBadgeNumber;
                    SaveData.SaveFile();
                    _sceneData.Value.gameUI.ProfileMenuUI.ProfileMenuPlayerInfoUI.AvatarContainerUI.ActiveBadgeImage.sprite = badge.GetComponent<Image>().sprite;
                });
            }


            #endregion

            #region Mission Menu

            _sceneData.Value.gameUI.MissionsMenuUI.BackButton.onClick.AddListener(() => {
                _sceneData.Value.gameUI.MissionsMenuUI.gameObject.SetActive(false);
                _sceneData.Value.gameUI.MainMenuUI.gameObject.SetActive(true);
            });

            #endregion

            #region Store Menu

            _sceneData.Value.gameUI.StoreMenuUI.BackButton.onClick.AddListener(() => {
                _sceneData.Value.gameUI.StoreMenuUI.gameObject.SetActive(false);
                _sceneData.Value.gameUI.StoreMenuUI.BuyButton.gameObject.SetActive(false);
                _sceneData.Value.gameUI.MainMenuUI.gameObject.SetActive(true);
            });
            
            // Store skin list
            foreach (SkinUI skin in _sceneData.Value.gameUI.StoreMenuUI.SkinScrollContainerUI.SkinList.Skins) {
                skin.GetComponent<Button>().onClick.AddListener(() => {

                    _sceneData.Value.gameUI.StoreMenuUI.BuyButton.gameObject.SetActive(true);

                    foreach (SkinUI skinLoop in _sceneData.Value.gameUI.StoreMenuUI.SkinScrollContainerUI.SkinList.Skins) { 
                        if (skinLoop == skin) {
                            skin.GetComponent<Image>().color = Color.grey;
                        } else {
                            skinLoop.GetComponent<Image>().color = Color.white;
                        }
                    }

                    skin.Availability = SaveData.currentAvailableSkins[int.Parse(skin.name)];

                    if (skin.Availability == false) {
                        _sceneData.Value.gameUI.StoreMenuUI.AvailableImage.gameObject.SetActive(false);
                    } else {
                        _sceneData.Value.gameUI.StoreMenuUI.AvailableImage.gameObject.SetActive(true);
                    }

                    _sceneData.Value.SceneSkinStoreNumber = int.Parse(skin.name);
                });
            }

            // Store buy button
            _sceneData.Value.gameUI.StoreMenuUI.BuyButton.onClick.AddListener(() => {
                if (_sceneData.Value.SceneCoins >= 10) {
                    _sceneData.Value.SceneCoins -= 10;
                    SaveData.currentCoins = _sceneData.Value.SceneCoins;
                    SaveData.currentAvailableSkins[_sceneData.Value.SceneSkinStoreNumber] = true;

                    _sceneData.Value.gameUI.StoreMenuUI.SkinScrollContainerUI.SkinList.Skins[_sceneData.Value.SceneSkinStoreNumber].Availability = true;
                    _sceneData.Value.gameUI.StoreMenuUI.AvailableImage.gameObject.SetActive(true);

                    SaveData.SaveFile();
                }
            });


            #endregion
        }

    }
}