using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CardMatch.Gameplay
{
    public class GameplayCanvas : MonoBehaviour
    {
        [SerializeField] private Text matchesText;
        [SerializeField] private Text turnsText;
        [SerializeField] private Text scoreText;

        // Start is called before the first frame update
        void Start()
        {

        }

        public void UpdateUI(int matches, int turns, int score)
        {
            matchesText.text = $"{matches}";
            turnsText.text = $"{turns}";
            scoreText.text = $"{score}";
        }
    }
}
