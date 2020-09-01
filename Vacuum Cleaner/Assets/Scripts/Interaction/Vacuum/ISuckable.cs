namespace Assets.Scripts.Interaction.Vacuum
{
    using UnityEngine;

    public interface ISuckable
    {
        void Suck(Vector3 suckTo, float suckForce);
    }
}