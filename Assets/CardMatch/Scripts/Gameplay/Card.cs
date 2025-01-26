using System.Collections;
using System.Collections.Generic;
using CardMatch.Progress;
using UnityEngine;
using UnityEngine.UI;

namespace CardMatch.Gameplay
{
    public class Card : MonoBehaviour
    {
        [SerializeField] private Text cardText;
        [SerializeField] private Image cardImage;
        [SerializeField] private CardFlip cardFlip;

        public int CardId => cardID;

        private int cardID;
        private bool isFlipped = false;

        void Awake()
        {
            if (TryGetComponent<CardFlip>(out var cardFlip))
            {
                this.cardFlip = cardFlip;
            }
        }

        /// <summary>
        /// Initializes a Card with a given CardState.
        /// Sets the card's ID and image, and flips the card if it is matched.
        /// </summary>
        /// <param name="cardID">The unique ID of the card.</param>
        /// <param name="cardState">The current state of the card.</param>
        public void InitCard(int cardID, CardState cardState)
        {
            this.cardID = cardID;
            if(cardState.isMatched)
            {
                FlipCard();
                SetMatched();
            }
            // cardText.enabled = false;
            // cardText.text = GetAlphaNumericString(cardID);
        }

        /// <summary>
        /// Sets the sprite of the card.
        /// </summary>
        /// <param name="sprite">The sprite to set the card to.</param>
        public void SetSprite(Sprite sprite)
        {
            cardImage.sprite = sprite;
        }

        /// <summary>
        /// Flips the card.
        /// If the card is already flipped, it unflips the card.
        /// If the card is not flipped, it flips the card and invokes the OnCardFlippedEvent.
        /// </summary>
        public void FlipCard()
        {
            if (!isFlipped)
            {
                isFlipped = true;
                // cardText.enabled = true;
                cardFlip.StartFlip();
                GameplayController.OnCardFlippedEvent?.Invoke(this);
            }
            else
            {
                isFlipped = false;
                // cardText.enabled = false;
                cardFlip.StartFlip();
            }
        }

        /// <summary>
        /// Sets the button on the card to not be interactable, when the card as matched.
        /// </summary>
        public void SetMatched()
        {
            if (TryGetComponent<Button>(out var button))
            {
                button.interactable = false;
            }
        }

        public void OnClickCardSelect()
        {
            FlipCard();
        }

        /// <summary>
        /// Converts an integer to a string of alphanumeric characters.
        /// 
        /// If the number is below the length of the alphabet, it returns the corresponding letter (A-Z).
        /// If the number is above, it subtracts the length of the alphabet and returns the remaining number as a string.
        /// </summary>
        /// <param name="input">The number to convert.</param>
        /// <returns>A string of alphanumeric characters.</returns>
        public static string GetAlphaNumericString(int input)
        {
            const int alphabetLength = 26;
            if (input < alphabetLength)
            {
                char alphabet = (char)('A' + input);
                return alphabet.ToString();
            }
            else
            {
                int remainingNumber = input - alphabetLength;
                return remainingNumber.ToString();
            }
        }
    }
}
