using System.Collections;
using System.Collections.Generic;
using CardMatch;
using UnityEngine;

namespace CardMatch.Sound
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip flipSound;
        [SerializeField] private AudioClip matchSound;
        [SerializeField] private AudioClip mismatchSound;
        [SerializeField] private AudioClip gameOverSound;

        public static System.Action<SoundFx> OnPlaySoundEvent;

        private void Awake()
        {
            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();
        }

        void OnEnable()
        {
            OnPlaySoundEvent += PlaySound;
        }

        void OnDisable()
        {
            OnPlaySoundEvent -= PlaySound;
        }

        private void PlaySound(SoundFx audioFx)
        {
            switch (audioFx)
            {
                case SoundFx.Flip:
                    PlaySound(flipSound);
                    break;
                case SoundFx.Match:
                    PlaySound(matchSound);
                    break;
                case SoundFx.Mismatch:
                    PlaySound(mismatchSound);
                    break;
                case SoundFx.GameOver:
                    PlaySound(gameOverSound);
                    break;
            }
        }

        public void PlaySound(AudioClip clip)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
