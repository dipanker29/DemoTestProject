using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CardMatch.Progress;

namespace CardMatch.Menu
{
    public class MenuController : MonoBehaviour
    {
        public static System.Action<Difficulty> OnDifficultySelectedEvent;

        [SerializeField] private DifficultySelect[] difficultySelects;

        [SerializeField] private Button startButton;

        private void Awake()
        {
        }

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => GameManager.Instance.IsInitilized);
            InitDifficulty();
        }

        void OnEnable()
        {
            OnDifficultySelectedEvent += OnDifficultySelected;
            startButton.onClick.AddListener(OnClickStartButton);
            // startButton.interactable = false;
        }

        void OnDisable()
        {
            OnDifficultySelectedEvent -= OnDifficultySelected;
            startButton.onClick.RemoveListener(OnClickStartButton);
        }

        private void InitDifficulty()
        {
            foreach (var difficultySelect in difficultySelects)
            {
                var progressData = GameManager.Instance.ProgressManager.GetGameplayData(difficultySelect.Difficulty);
                difficultySelect.InitScore(progressData.score, progressData.highestScore);
            }
        }

        private void OnDifficultySelected(Difficulty difficulty)
        {
            Debug.Log($"Difficulty selected: {DifficultySelect.ConvertDifficultyToString(difficulty)}");
            GameManager.Instance.currentDifficulty = difficulty;
            startButton.interactable = true;
        }

        private void OnClickStartButton()
        {
            Debug.Log("Start button clicked");
            GameManager.Instance.LoadScene("Gameplay");
        }
    }
}
