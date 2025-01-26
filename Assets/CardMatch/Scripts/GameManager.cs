using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardMatch.Progress;

namespace CardMatch
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        [SerializeField] private GameProgressManager progressManager;

        public GameProgressManager ProgressManager => progressManager;
        public Difficulty currentDifficulty = Difficulty.FourXFour;
        public bool IsInitilized { get; private set; }


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            progressManager = new GameProgressManager(new SaveSystem());
            progressManager.LoadProgress();
            IsInitilized = true;
        }

        /// <summary>
        /// Sets the current difficulty and outputs the number of rows and columns required for it.
        /// </summary>
        /// <param name="difficulty">The difficulty to set.</param>
        /// <param name="rows">The number of rows required for the given difficulty.</param>
        /// <param name="columns">The number of columns required for the given difficulty.</param>
        public void SetDifficulty(Difficulty difficulty, out int rows, out int columns)
        {
            rows = difficulty switch
            {
                Difficulty.TwoXTwo => 2,
                Difficulty.ThreeXThree => 3,
                Difficulty.FourXFour => 4,
                Difficulty.FourXFive => 4,
                Difficulty.FiveXSix => 5,
                _ => 2,
            };
            columns = difficulty switch
            {
                Difficulty.TwoXTwo => 2,
                Difficulty.ThreeXThree => 2,
                Difficulty.FourXFour => 4,
                Difficulty.FourXFive => 5,
                Difficulty.FiveXSix => 6,
                _ => 2,
            };
        } 

        /// <summary>
        /// Loads a scene with the given name.
        /// </summary>
        /// <param name="sceneName">The name of the scene to load.</param>
        public void LoadScene(string sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        } 
    }
}
