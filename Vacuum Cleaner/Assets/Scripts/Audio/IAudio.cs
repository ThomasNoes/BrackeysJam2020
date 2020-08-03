namespace Assets.Scripts.Audio
{
    public interface IAudio
    {
        void Play();
        void PlayWithDelay(float delayTime);
        void Stop();
        void StopWithDelay(float delayTime);
    }
}