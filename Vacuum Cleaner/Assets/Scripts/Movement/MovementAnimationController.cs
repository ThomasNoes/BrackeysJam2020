namespace Assets.Scripts.Movement
{
    using UnityEngine;

    public class MovementAnimationController : MonoBehaviour, IMove
    {
        private Animator _animator;

        void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void StartMoving()
        {
            _animator?.SetTrigger("Walk");
        }

        public void StopMoving()
        {
            _animator?.SetTrigger("Standing");
        }
    }
}