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
        private Vector3 _suckTo, _blowFrom;
        private float _suckForce, _blowForce;
        private bool _isSucked, _isBlown;

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
            } else if (_isBlown)
            {
                _thisRb.AddForce((transform.position - _blowFrom).normalized * _blowForce * Time.smoothDeltaTime);
                _isBlown = false;
            }
        }

        public GameObject Eat()
        {
            if (!isEatable)
                return null;

            Debug.Log(gameObject + " was eaten");
            return gameObject;
        }

        public void ThrowUp()
        {
            Debug.Log(gameObject + " was thrown up");
            // TODO: Needs implementation
        }

        public void Blow(Vector3 blowFrom, float blowForce)
        {
            if (!isBlowable)
                return;

            _blowFrom = blowFrom;
            _blowForce = blowForce;
            _isBlown = true;
        }
    }
}