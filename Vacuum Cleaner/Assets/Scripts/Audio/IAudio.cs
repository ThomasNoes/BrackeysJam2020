namespace Assets.Scripts.Audio
{
    public interface IAudio
    {
        void Play(int atIndex);
        void PlayWithDelay(int atIndex, float delayTime);
        void Stop(int atIndex);
        void StopWithDelay(int atIndex, float delayTime);
    }
}