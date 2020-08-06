using UnityEngine;

public class IndicatorLight : MonoBehaviour, IIndicator
{
    public Color offColor = Color.red, onColor = Color.green;
    public bool startOn, playAudioSource;
    public Light lightSource;

    private AudioSource _audioSource;

    private void Start()
    {
        if (lightSource == null)
            lightSource = GetComponent<Light>();

        if (lightSource != null)
            lightSource.color = startOn ? onColor : offColor;

        if (playAudioSource)
            _audioSource = GetComponent<AudioSource>();
    }

    public void IndicatorOn()
    {
        if (lightSource != null)
            lightSource.color = onColor;

        if (playAudioSource)
            _audioSource?.Play();
    }

    public void IndicatorOff()
    {
        if (lightSource != null)
            lightSource.color = offColor;

        if (playAudioSource)
            _audioSource?.Stop();
    }
}