namespace Assets.Scripts.Interaction.Vacuum
{
    using UnityEngine;

    public interface IBlowable
    {
        void Blow(Vector3 blowFrom, float blowForce);
    }
}