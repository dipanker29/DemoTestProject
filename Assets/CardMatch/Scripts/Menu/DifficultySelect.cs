using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CardMatch.Menu
{
    public class DifficultySelect : MonoBehaviour
    {
        [SerializeField] private Toggle toggle;
        [SerializeField] private Text difficultyText;
        [SerializeField] private Text scoreText;
        [SerializeField] private Text highestScoreText;
        [SerializeField] private Difficulty difficulty;

        public Difficulty Difficulty => difficulty;

        void Awake()
        {
            if (transform.parent.TryGetComponent<ToggleGroup>(out var toggleGroup))
            {
                toggle.group = toggleGroup;
            }
            else
            {
                Debug.LogError("ToggleGroup not found in parent");
            }

            toggle.onValueChanged.AddListener(OnToggleValueChanged);
            difficultyText.text = ConvertDifficultyToString(difficulty);
        }

        /// <summary>
        /// Initializes the score and highest score of the difficulty select.
        /// If the score is higher than the highest score, it sets the highest score to the score.
        /// </summary>
        /// <param name="score">The score of the difficulty select.</param>
        /// <param name="highestScore">The highest score of the difficulty select.</param>
        public void InitScore(int score, int highestScore)
        {
            if (score > highestScore)
            {
                highestScore = score;
            }

            scoreText.text = $"{score}";
            highestScoreText.text = $"{highestScore}";
        }

        private void OnToggleValueChanged(bool isOn)
        {
            if (isOn)
            {
                Debug.Log($"Difficulty selected: {toggle.name}");
                MenuController.OnDifficultySelectedEvent?.Invoke(difficulty);
            }
        }

        /// <summary>
        /// Converts a difficulty to a string.
        /// </summary>
        /// <param name="difficulty">The difficulty to convert.</param>
        /// <returns>The string representation of the difficulty.</returns>
        public static string ConvertDifficultyToString(Difficulty difficulty)
        {
            return difficulty switch
            {
                Difficulty.TwoXTwo => "2X2",
                Difficulty.ThreeXThree => "2X3",
                Difficulty.FourXFour => "4X4",
                Difficulty.FourXFive => "4X5",
                Difficulty.FiveXSix => "5X6",
                _ => "2X2",
            };
        }
    }
}
