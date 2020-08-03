namespace Assets.Scripts.Audio 
{
    using UnityEngine;

    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour, IAudio
    {
        // Public:
        public bool playSoundOnStart = false;
        [HideInInspector] public float startDelay = 0.0f;

        // Private:
        private AudioSource _thisSource;

        private void Awake()
        {
            _thisSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            if (playSoundOnStart)
                if (!_thisSource.playOnAwake) // If playOnAwake is true, the source plays before start 
                    if (startDelay <= 0.1f)
                        Play();
                    else
                        PlayWithDelay(startDelay);
        }

        public void Play()
        {
            _thisSource.Play();
        }

        public void PlayWithDelay(float delayTime)
        {
            Invoke("Play", delayTime);
        }

        public void Stop()
        {
            _thisSource.Stop();
        }

        public void StopWithDelay(float delayTime)
        {
            Invoke("Stop", delayTime);
        }
    }
}