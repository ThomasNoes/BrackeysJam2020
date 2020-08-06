using UnityEngine;

public class IndicatorLight : MonoBehaviour, IIndicator
{
    public Color offColor = Color.red, onColor = Color.green;
    public Light lightSource;

    private void Start()
    {
        if (lightSource == null)
            lightSource = GetComponent<Light>();
    }

    public void IndicatorOn()
    {
        if (lightSource != null)
            lightSource.enabled = true;
    }

    public void IndicatorOff()
    {
        if (lightSource != null)
            lightSource.enabled = false;
    }
}