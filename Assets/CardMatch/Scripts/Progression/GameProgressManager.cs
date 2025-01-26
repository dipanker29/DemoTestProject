using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardMatch.Progress
{
    /// <summary>
    /// The GameProgressManager class is responsible for managing the game's progress data, including saving, loading, and retrieving gameplay data.
    /// </summary>
    public class GameProgressManager
    {
        private readonly ISavable saveSystem;
        [SerializeField] private ProgressionData currentProgress;

        public GameProgressManager(ISavable saveSystem)
        {
            this.saveSystem = saveSystem;
            currentProgress = new();
        }

        /// <summary>
        /// Saves the current progress data to the save system.
        /// </summary>
        public void SaveProgress()
        {
            saveSystem.Save(currentProgress);
        }

        /// <summary>
        /// Retrieves the gameplay data for the given difficulty level.
        /// </summary>
        /// <param name="difficulty">The difficulty level to retrieve the gameplay data for.</param>
        /// <returns>The gameplay data for the given difficulty level.</returns>
        public GameplayData GetGameplayData(Difficulty difficulty)
        {
            return currentProgress.GetGameplayData(difficulty);
        }

        /// <summary>
        /// Loads the game progress data from the save system.
        /// If the load is successful, the current progress is set to the loaded state.
        /// </summary>
        public void LoadProgress()
        {
            ProgressionData loadedState = saveSystem.Load();
            if (loadedState != null)
            {
                currentProgress = loadedState;
            }
        }
    }
}
