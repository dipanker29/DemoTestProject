using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CardMatch.Gameplay
{
    public class Card : MonoBehaviour
    {
        [SerializeField] private Text cardText;
        [SerializeField] private Image cardImage;
        [SerializeField] private CardFlip cardFlip;

        void Awake()
        {
            if (TryGetComponent<CardFlip>(out var cardFlip))
            {
                this.cardFlip = cardFlip;
            }
        }

        public int CardId => cardID;
        
        private int cardID;
        private bool isFlipped = false;

        // Start is called before the first frame update
        void Start()
        {

        }

        public void InitCard(int cardID)
        {
            this.cardID = cardID;
            // cardText.enabled = false;
            // cardText.text = GetAlphaNumericString(cardID);
        }

        public void SetSprite(Sprite sprite)
        {
            cardImage.sprite = sprite;
        }

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
