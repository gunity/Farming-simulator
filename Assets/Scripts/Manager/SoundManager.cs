using System.Collections.Generic;
using Extension;
using UnityEngine;

namespace Manager
{
    public class SoundManager : Singleton<SoundManager>
    {
        [SerializeField] private List<AudioClip> mainThemes = new List<AudioClip>();
        [SerializeField] private AudioClip clickAudioClip = null;
        [SerializeField] private AudioClip plantAudioClip = null;

        private AudioSource _audioSource;
        private int _randomIndex = -1;
        
        private void Start()
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            NextTheme();
        }

        private void NextTheme()
        {
            int newRandomIndex;
            do
            {
                newRandomIndex = Random.Range(0, mainThemes.Count);
            } while (mainThemes.Count > 1 && newRandomIndex == _randomIndex);
            _randomIndex = newRandomIndex;
            
            _audioSource.clip = mainThemes[_randomIndex];
            _audioSource.Play();
            Invoke(nameof(NextTheme), mainThemes[_randomIndex].length);
        }

        public void PlayClick()
        {
            _audioSource.PlayOneShot(clickAudioClip);
        }
        
        public void PlayPlant()
        {
            _audioSource.PlayOneShot(plantAudioClip);
        }
    }
}
