using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using CardMatch.Progress;
using CardMatch.Sound;

namespace CardMatch.Gameplay
{
    public class GameplayController : MonoBehaviour
    {
        // [SerializeField] private GameplayCanvas gameplayCanvas;
        [SerializeField] private Card cardPrefab;
        [SerializeField] private Transform gridParent;
        [SerializeField] private int rows = 2;
        [SerializeField] private int columns = 2;
        [SerializeField] private SpriteAtlas spriteAtlas;

        public static System.Action<Card> OnCardFlippedEvent;
        public static System.Action OnRestartGameEvent;
        public static System.Action<string> OnGameStatusEvent;
        public static bool CanSelectCard { get; private set; } = true;

        private List<Card> flippedCards = new List<Card>();
        // private int turns = 0;
        // private int matches = 0;
        // private int comboCount = 0;
        // private int score = 0;
        private GameplayData gameplayData;

        /// <summary>
        /// Waits until the game manager is initialized, then resets the game data
        /// if all matches have been found, sets the current difficulty and starts
        /// the game.
        /// </summary>
        private IEnumerator Start()
        {
            yield return new WaitUntil(() => GameManager.Instance.IsInitilized);
            gameplayData = GameManager.Instance.ProgressManager.GetGameplayData(GameManager.Instance.currentDifficulty);
            if (gameplayData.IsAlreadyMatchedAll())
            {
                gameplayData.Reset();
            }
            GameManager.Instance.SetDifficulty(GameManager.Instance.currentDifficulty, out rows, out columns);
            StartGame();
        }

        private void OnEnable()
        {
            OnCardFlippedEvent += OnCardFlipped;
            OnRestartGameEvent += RestartGame;
        }

        private void OnDisable()
        {
            OnCardFlippedEvent -= OnCardFlipped;
            OnRestartGameEvent -= RestartGame;
        }

        /// <summary>
        /// Resets the game state and starts a new game by generating a new card grid.
        /// </summary>
        public void StartGame()
        {
            flippedCards.Clear();
            GameplayCanvas.OnScoreChanged?.Invoke(gameplayData.score);
            GameplayCanvas.OnMatchesChanged?.Invoke(gameplayData.matches);
            GameplayCanvas.OnTurnsChanged?.Invoke(gameplayData.turns);
            GenerateGrid();
        }

        /// <summary>
        /// Resets the gameplay data and restarts the game by clearing the current state and initializing a new game grid.
        /// </summary>
        public void RestartGame()
        {
            gameplayData.Reset();
            StartGame();
        }

        #region Card Generation

        /// <summary>
        /// Generates a new grid of cards by destroying all existing cards and instantiating new ones
        /// with unique IDs and sprites. The grid is then rearranged to fit the desired layout.
        /// </summary>
        private void GenerateGrid()
        {
            // Destroy all existing cards
            foreach (Transform child in gridParent)
            {
                Destroy(child.gameObject);
            }

            int totalCards = rows * columns;
            int totalPairs = totalCards - (totalCards % 2);
            int[] cardIDs = GenerateCardIDs(totalPairs);

            ShuffleCardIDs(ref cardIDs);// Shuffle the card IDs

            for (int i = 0; i < cardIDs.Length; i++)
            {
                Card card = Instantiate(cardPrefab, gridParent);
                card.InitCard(cardIDs[i], gameplayData.GetCard(cardIDs[i]));
                Sprite sprite = spriteAtlas.GetSprite(cardIDs[i].ToString());
                if (sprite != null)
                {
                    card.SetSprite(sprite);
                }
            }

            if (gridParent.TryGetComponent(out GridLayoutGroup gridLayoutGroup))
            {
                gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
                int rowCount = rows > columns ? columns : rows;
                gridLayoutGroup.constraintCount = rowCount > 4 ? 4 : rowCount;
            }
            else
            {
                Debug.LogError("Grid Layout Group not found on the grid parent");
            }
        }

        /// <summary>
        /// Shuffles the given array of card IDs using a random number generator.
        /// </summary>
        /// <param name="cardIDs">The array of card IDs to shuffle</param>
        private void ShuffleCardIDs(ref int[] cardIDs)
        {
            System.Random random = new System.Random();
            cardIDs = cardIDs.OrderBy(x => random.Next()).ToArray();
        }

        /// <summary>
        /// Generates a new array of unique card IDs where each ID has a corresponding pair.
        /// </summary>
        /// <param name="totalIds">The total number of card IDs to generate</param>
        /// <returns>An array of unique card IDs with pairs</returns>
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

        /// <summary>
        /// Handles a card being flipped, either by the player or as a result of a match or mismatch.
        /// </summary>
        /// <param name="card">The card that was flipped</param>
        /// <remarks>
        private void OnCardFlipped(Card card)
        {
            AudioManager.OnPlaySoundEvent?.Invoke(SoundFx.Flip);
            flippedCards.Add(card);

            if (flippedCards.Count == 2)
            {
                gameplayData.turns++;
                CanSelectCard = false;
                CheckMatch();
            }

            GameplayCanvas.OnScoreChanged?.Invoke(gameplayData.score);
            GameplayCanvas.OnMatchesChanged?.Invoke(gameplayData.matches);
            GameplayCanvas.OnTurnsChanged?.Invoke(gameplayData.turns);
        }

        /// <summary>
        /// Checks if two cards have been flipped and if they match.
        /// If they match, the card is marked as matched and the score is incremented.
        /// If they don't match, the card is unflipped and the score is decremented.
        /// </summary>
        private void CheckMatch()
        {
            if (flippedCards[0].CardId == flippedCards[1].CardId)
            {
                AudioManager.OnPlaySoundEvent?.Invoke(SoundFx.Match);
                gameplayData.matches++;
                flippedCards[0].SetMatched();
                flippedCards[1].SetMatched();
                gameplayData.UpdateCardState(flippedCards[0].CardId, true);
                flippedCards.Clear();
                CanSelectCard = true;

                gameplayData.comboCount++;
                gameplayData.score += 10 * gameplayData.comboCount; // Combo bonus

                if (gameplayData.matches == rows * columns / 2)
                {
                    Debug.Log("Game Over");
                    OnGameStatusEvent?.Invoke("GameOver");
                    AudioManager.OnPlaySoundEvent?.Invoke(SoundFx.GameOver);
                    // gameplayCanvas.ShowGameOverPanel(true);
                    gameplayData.UpdateHighestScore();
                    GameManager.Instance.ProgressManager.SaveProgress();
                }
            }
            else
            {
                AudioManager.OnPlaySoundEvent?.Invoke(SoundFx.Mismatch);
                gameplayData.comboCount = 0;
                gameplayData.score -= 5; // Penalty for mismatch
                StartCoroutine(UnflipCards());
            }
        }

        /// <summary>
        /// Unflips the two cards that were flipped.
        /// </summary>
        /// <remarks>
        /// Waits for 0.8 seconds before unflipping the cards to create a delay.
        /// </remarks>
        IEnumerator UnflipCards()
        {
            yield return new WaitForSeconds(0.8f);
            flippedCards[0].FlipCard();
            flippedCards[1].FlipCard();
            flippedCards.Clear();
            CanSelectCard = true;
        }

        #endregion Card Selection
    }
}
