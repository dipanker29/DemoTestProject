using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardMatch.Gameplay
{
    public class CardFlip : MonoBehaviour
    {
        [SerializeField] private GameObject cardBack;
        [SerializeField] private float flipSpeed = 2f; // Speed of the flip

        private bool cardBackIsActive;

        /// <summary>
        /// Starts a coroutine to flip the card.
        /// </summary>
        public void StartFlip()
        {
            StartCoroutine(CalculateFlip());
        }

        /// <summary>
        /// Flips the card.
        /// 
        /// If the card back is active, this method will disable the card back.
        /// If the card back is inactive, this method will enable the card back.
        /// </summary>
        private void Flip()
        {
            if (cardBackIsActive)
            {
                cardBack.SetActive(false);
                cardBackIsActive = false;
            }
            else
            {
                cardBack.SetActive(true);
                cardBackIsActive = true;
            }
        }

        private IEnumerator CalculateFlip()
        {
            float elapsedTime = 0f;
            bool isFlipped = false;
            Quaternion startRotation = transform.rotation;
            Quaternion endRotation = startRotation * Quaternion.Euler(0, 180, 0); // Rotate 180 degrees on Y-axis

            while (elapsedTime < 1f / flipSpeed)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime * flipSpeed); // Normalize elapsed time
                transform.rotation = Quaternion.Slerp(startRotation, endRotation, t); // Smooth rotation

                if (!isFlipped && transform.eulerAngles.y >= 90)
                {
                    isFlipped = true;
                    Flip();
                }
                else if (!isFlipped && transform.eulerAngles.y <= -90)
                {
                    isFlipped = true;
                    Flip();
                }
                yield return null;
            }

            transform.rotation = endRotation; // Ensure exact alignment
        }
    }
}
