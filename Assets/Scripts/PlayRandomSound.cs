using UnityEngine;

namespace Paxn.Toolkit {

    /// <summary>
    /// Component to play a Random Sound each time from a defined list.
    /// </summary>
    /// 
    /// * Author: Paxn (jfriffoa@gmail.com)
    /// * Last time modified: 2020-11-17
    public class PlayRandomSound : MonoBehaviour {

        #region Parameters
        /// <summary>
        /// Play a random sound at Start.
        /// </summary>
        [Tooltip("Play a random sound at Start.")]
        public bool playOnStart;

        /// <summary>
        /// AudioSource to play the sound.
        /// </summary>
        [Tooltip("AudioSource to play the sound.")]
        public AudioSource source;

        /// <summary>
        /// List of clips to choose.
        /// </summary>
        [Tooltip("List of clips to choose.")]
        public AudioClip[] clips;

        int _lastPick = -1;
        #endregion

        #region Methods
        void Start() {
            if (playOnStart)
                PlaySound();
        }

        /// <summary>
        /// Play a Random Sound from the list.
        /// </summary>
        public void PlaySound() {
            if (source == null) {
                Debug.LogError("I NEED AN <b>[AUDIOSOURCE]</b>", gameObject);
                return;
            }

            AudioClip clip;
            (clip, _lastPick) = RandomUtil.RandomPick(clips, _lastPick);
            source.PlayOneShot(clip);
        }
        #endregion
    }
}