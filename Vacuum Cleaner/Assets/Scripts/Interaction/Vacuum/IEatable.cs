namespace Assets.Scripts.Interaction.Vacuum
{
    using UnityEngine;

    public interface IEatable
    {
        GameObject Eat();
        void ThrowUp();
    }
}