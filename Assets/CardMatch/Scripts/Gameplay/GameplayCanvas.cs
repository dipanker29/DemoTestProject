using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace CardMatch.Gameplay
{
    public class GameplayCanvas : MonoBehaviour
    {
        [SerializeField] private Text matchesText;
        [SerializeField] private Text turnsText;
        [SerializeField] private Text scoreText;
        [SerializeField] private GameObject gameOverPanel;

        public static System.Action<int> OnScoreChanged;
        public static System.Action<int> OnMatchesChanged;
        public static System.Action<int> OnTurnsChanged;

        private void OnEnable()
        {
            OnScoreChanged += UpdateScore;
            OnMatchesChanged += UpdateMatches;
            OnTurnsChanged += UpdateTurns;
            GameplayController.OnGameStatusEvent += OnGameplayStatusChanged;
        }
        private void OnDisable()
        {
            OnScoreChanged -= UpdateScore;
            OnMatchesChanged -= UpdateMatches;
            OnTurnsChanged -= UpdateTurns;
            GameplayController.OnGameStatusEvent -= OnGameplayStatusChanged;
        }

        private void UpdateScore(int score)
        {
            scoreText.text = $"{score}";
        }

        private void OnGameplayStatusChanged(string status)
        {
            ShowGameOverPanel(status == "GameOver");
        }

        private void UpdateMatches(int matches)
        {
            matchesText.text = $"{matches}";
        }
        private void UpdateTurns(int turns)
        {
            turnsText.text = $"{turns}";
        }

        private void ShowGameOverPanel(bool active)
        {
            gameOverPanel.SetActive(active);
        }

        #region Button Events

        public void OnClickCloseButton()
        {
            ShowGameOverPanel(true);
        }

        public void OnClickMenuButton()
        {
            GameManager.Instance.LoadScene("Menu");
        }

        public void OnClickRestartButton()
        {
            GameplayController.OnRestartGameEvent?.Invoke();
            ShowGameOverPanel(false);
        }

        public void OnClickNextButton()
        {
        }

        #endregion Button Events
    }
}
