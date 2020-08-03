namespace Assets.Scripts.Interaction.Vacuum
{
    using UnityEngine;

    [RequireComponent(typeof(Rigidbody))]
    public class DefaultVacuumable : MonoBehaviour, ISuckable, IEatable, IBlowable
    {
        // Public:
        public bool isSuckable = true, isEatable = true, isBlowable = true;

        // Private: 
        private Rigidbody _thisRb;
        private Vector3 _suckTo;
        private float _suckForce;
        private bool _isSucked;

        private void Start()
        {
            _thisRb = GetComponent<Rigidbody>();
        }

        public void Suck(Vector3 suckTo, float suckForce)
        {
            if (!isSuckable)
                return;

            _suckTo = suckTo;
            _suckForce = suckForce;
            _isSucked = true;
        }

        private void FixedUpdate()
        {
            if (_isSucked)
            {
                _thisRb.AddForce((_suckTo - transform.position).normalized * _suckForce * Time.smoothDeltaTime);
                _isSucked = false;
            }
        }

        public void Eat()
        {
            if (!isEatable)
                return;

            Debug.Log(gameObject + " was eaten");
            // TODO: Needs implementation
        }

        public void ThrowUp()
        {
            Debug.Log(gameObject + " was thrown up");
            // TODO: Needs implementation
        }

        public void Blow() // Does nothing yet
        {
            if (!isBlowable)
                return;
        }
    }
}