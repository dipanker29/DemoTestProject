using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CardMatch.Progress
{
    /// <summary>
    /// The SaveSystem class is responsible for saving and loading game progression data to and from a JSON file.
    /// </summary>
    public class SaveSystem : ISavable
    {
        private readonly string savePath;

        public SaveSystem()
        {
            savePath = Path.Combine(Application.persistentDataPath, "game_progression.json");
        }

        /// <summary>
        /// Saves the game progression data to a JSON file.
        /// </summary>
        /// <param name="progressData">The game progression data to save.</param>
        public void Save(ProgressionData progressData)
        {
            string json = JsonUtility.ToJson(progressData, true);
            File.WriteAllText(savePath, json);
        }

        /// <summary>
        /// Loads the game progression data from a JSON file.
        /// Returns null if the file does not exist.
        /// </summary>
        /// <returns>The loaded game progression data, or null if the file does not exist.</returns>
        public ProgressionData Load()
        {
            if (File.Exists(savePath))
            {
                string json = File.ReadAllText(savePath);
                return JsonUtility.FromJson<ProgressionData>(json);
            }
            return null;
        }
    }
}
