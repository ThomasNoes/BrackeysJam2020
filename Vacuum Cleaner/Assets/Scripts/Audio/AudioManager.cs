namespace Assets.Scripts.Audio 
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class AudioManager : MonoBehaviour, IAudio
    {
        // Public:
        public AudioSource[] audioSources;
        [HideInInspector] public int inspectorIndex;

        // Private:
        private int _tempIndex;

        private void Awake()
        {
            audioSources = GetComponents<AudioSource>();
        }

        public void Play(int index)
        {
            if (audioSources != null)
                if (audioSources.Length >= 0 && index < audioSources.Length)
                    audioSources[index].Play();
        }

        public void PlayWithDelay(int index, float delayTime)
        {
            StartCoroutine(DelayPlay(index, new WaitForSeconds(delayTime)));
        }

        public void Stop(int index)
        {
            if (audioSources != null)
                if (audioSources.Length >= 0 && index < audioSources.Length)
                    audioSources[index].Stop();
        }

        public void StopWithDelay(int index, float delayTime)
        {
            StartCoroutine(DelayStop(index, new WaitForSeconds(delayTime)));
        }

        private IEnumerator DelayPlay(int index, WaitForSeconds delay)
        {
            yield return delay;
            Play(index);
        }

        private IEnumerator DelayStop(int index, WaitForSeconds delay)
        {
            yield return delay;
            Stop(index);
        }
    }
}