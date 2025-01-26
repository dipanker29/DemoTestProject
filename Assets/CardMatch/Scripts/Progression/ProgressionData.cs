using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardMatch.Progress
{
    /// <summary>
    /// The ProgressionData class is a serializable class that stores a list of GameplayData objects, each representing a specific difficulty level.
    /// </summary>
    [System.Serializable]
    public class ProgressionData
    {
        public List<GameplayData> gameplayData;

        public GameplayData GetGameplayData(Difficulty difficulty)
        {
            gameplayData ??= new List<GameplayData>();

            GameplayData data = gameplayData.Find(x => x.difficulty == difficulty);
            if (data == null)
            {
                data = new GameplayData
                {
                    difficulty = difficulty,
                    cards = new List<CardState>()
                };
                gameplayData.Add(data);
            }

            return data;
        }
    }

    /// <summary>
    /// The GameplayData class represents the state of a game, including player progress and game statistics.
    /// </summary>
    [System.Serializable]
    public class GameplayData
    {
        public int turns;
        public int score;
        public int matches;
        public int highestScore;
        public int comboCount;
        public Difficulty difficulty;
        public List<CardState> cards;

        /// <summary>
        /// Retrieves the card with the given ID from the list of cards.
        /// 
        /// If the card does not exist, a new card is created with the given ID and state of unmatched.
        /// </summary>
        /// <param name="id">The unique ID of the card to retrieve.</param>
        /// <returns>The card with the given ID.</returns>
        public CardState GetCard(int id)
        {
            var card = cards.Find(x => x.id == id);
            if (card == null)
            {
                card = new CardState
                {
                    id = id,
                    isMatched = false
                };
                cards.Add(card);
            }
            return card;
        }

        /// <summary>
        /// Resets all the game data, including the score, matches, and turns, as well as the state of all cards.
        /// </summary>
        public void Reset()
        {
            turns = 0;
            score = 0;
            matches = 0;
            comboCount = 0;
            cards = new List<CardState>();
        }

        /// <summary>
        /// Checks if all cards are already matched.
        /// 
        /// This method returns true if all cards have been matched, false otherwise.
        /// </summary>
        /// <returns>True if all cards are matched, false otherwise.</returns>
        public bool IsAlreadyMatchedAll()
        {
            return cards.TrueForAll(x => x.isMatched);
        }

        /// <summary>
        /// Updates the state of a card with the given ID.
        /// 
        /// If the card with the given ID exists, its state is updated to the given value.
        /// If the card does not exist, a new card is created with the given ID and state.
        /// </summary>
        /// <param name="id">The unique ID of the card to update.</param>
        /// <param name="isMatched">The new state of the card.</param>
        public void UpdateCardState(int id, bool isMatched)
        {
            var card = GetCard(id);
            if (card != null)
            {
                card.isMatched = isMatched;
            }
            else
            {
                card = new CardState
                {
                    id = id,
                    isMatched = isMatched
                };
                cards.Add(card);
            }
        }

        /// <summary>
        /// Updates the highest score if the current score is higher.
        /// </summary>
        public void UpdateHighestScore()
        {
            if (highestScore > 0 && highestScore > score)
                return;

            highestScore = score;
        }
    }

    /// <summary>
    /// This class definition represents the state of a card in the game.
    /// </summary>
    [System.Serializable]
    public class CardState
    {
        public int id;
        public bool isMatched;
    }
}
