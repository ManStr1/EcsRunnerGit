using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

namespace EcsRunner {
    public class SaveData : MonoBehaviour {

        public static int currentBestScore = 0;
        public static int currentCoins = 0;
        public static int currentDiamonds = 0;
        public static int currentSkinNumber = 0;
        public static int currentBadgeNumber = 0;
        public static bool[] currentAvailableSkins;

        public static void SaveFile() {
            string destination = Application.persistentDataPath + "/save.dat";
            FileStream file;

            if (File.Exists(destination)) file = File.OpenWrite(destination);
            else file = File.Create(destination);

            GameData data = new GameData(currentBestScore, currentCoins, currentDiamonds, currentSkinNumber, currentBadgeNumber, currentAvailableSkins);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, data);
            file.Close();
        }

        public static void LoadFile() {
            string destination = Application.persistentDataPath + "/save.dat";
            FileStream file;

            if (File.Exists(destination)) file = File.OpenRead(destination);
            else {
                //Debug.LogError("File not found");
                currentAvailableSkins = new bool[6];
                currentAvailableSkins[0] = true;
                SaveFile();
                return;
            }

            BinaryFormatter bf = new BinaryFormatter();
            GameData data = (GameData)bf.Deserialize(file);
            file.Close();

            currentBestScore = data.bestScore;
            currentCoins = data.coins;
            currentDiamonds = data.diamonds;
            currentSkinNumber = data.skinNumber;
            currentBadgeNumber = data.badgeNumber;
            currentAvailableSkins = data.availableSkins;
        }

    }
}