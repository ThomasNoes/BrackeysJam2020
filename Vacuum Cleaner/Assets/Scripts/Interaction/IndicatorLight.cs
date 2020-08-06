using UnityEngine;

public class IndicatorLight : MonoBehaviour, IIndicator
{
    public Color offColor = Color.red, onColor = Color.green;
    public bool startOn;
    public Light lightSource;

    private void Start()
    {
        if (lightSource == null)
            lightSource = GetComponent<Light>();

        if (lightSource != null)
            lightSource.color = startOn ? onColor : offColor;

    }

    public void IndicatorOn()
    {
        if (lightSource != null)
            lightSource.color = onColor;
    }

    public void IndicatorOff()
    {
        if (lightSource != null)
            lightSource.color = offColor;
    }
}