using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace CardMatch.Gameplay
{
    public class GameplayController : MonoBehaviour
    {
        [SerializeField] private GameplayCanvas gameplayCanvas;
        [SerializeField] private Card cardPrefab;
        [SerializeField] private Transform gridParent;
        [SerializeField] private int rows = 2;
        [SerializeField] private int columns = 2;

        public static bool CanSelectCard { get; private set; } = true;
        public static System.Action<Card> OnCardFlippedEvent;
        private List<Card> flippedCards = new List<Card>();
        private int turns = 0;
        private int matches = 0;
        private int comboCount = 0;
        private int score = 0;

        // Start is called before the first frame update
        private void Start()
        {
            GenerateGrid();
        }

        private void OnEnable()
        {
            OnCardFlippedEvent += OnCardFlipped;
        }

        private void OnDisable()
        {
            OnCardFlippedEvent -= OnCardFlipped;
        }

        #region Card Generation

        // Generate the card grid dynamically
        private void GenerateGrid()
        {
            int totalCards = rows * columns;
            int totalPairs = totalCards - totalCards % 4;
            int[] cardIDs = GenerateCardIDs(totalCards);

            ShuffleCardIDs(ref cardIDs);// Shuffle the card IDs

            for (int i = 0; i < cardIDs.Length; i++)
            {
                Card card = Instantiate(cardPrefab, gridParent);
                card.InitCard(cardIDs[i]);
            }

            if (gridParent.TryGetComponent(out GridLayoutGroup gridLayoutGroup))
            {
                gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
                int rowCount = rows > columns ? rows : columns;
                gridLayoutGroup.constraintCount = rowCount > 4 ? 4 : rowCount;
            }
            else
            {
                Debug.LogError("Grid Layout Group not found on the grid parent");
            }
        }

        // Shuffle the card IDs
        private void ShuffleCardIDs(ref int[] cardIDs)
        {
            System.Random random = new System.Random();
            cardIDs = cardIDs.OrderBy(x => random.Next()).ToArray();
        }

        // Generate card IDs for the grid
        private int[] GenerateCardIDs(int totalIds)
        {
            int[] cardIDs = new int[totalIds];
            for (int i = 0; i < totalIds / 2; i++)
            {
                cardIDs[i * 2] = i;
                cardIDs[i * 2 + 1] = i;
            }
            return cardIDs;
        }

        #endregion Card Generation

        #region Card Selection

        private void OnCardFlipped(Card card)
        {
            flippedCards.Add(card);

            if (flippedCards.Count == 2)
            {
                turns++;
                CanSelectCard = false;
                CheckMatch();
            }

            gameplayCanvas.UpdateUI(matches: matches, turns: turns, score: score);
        }

        void CheckMatch()
        {
            if (flippedCards[0].CardId == flippedCards[1].CardId)
            {
                matches++;
                flippedCards[0].SetMatched();
                flippedCards[1].SetMatched();
                flippedCards.Clear();
                CanSelectCard = true;

                comboCount++;
                score += 10 * comboCount; // Combo bonus

                if (matches == (rows * columns) / 4)
                {
                    Debug.Log("Game Over");
                }
            }
            else
            {
                comboCount = 0;
                score -= 5; // Penalty for mismatch
                StartCoroutine(UnflipCards());
            }

            // UpdateScore(score);
        }

        IEnumerator UnflipCards()
        {
            yield return new WaitForSeconds(0.5f);
            flippedCards[0].FlipCard();
            flippedCards[1].FlipCard();
            flippedCards.Clear();
            CanSelectCard = true;
        }

        #endregion Card Selection
    }
}
