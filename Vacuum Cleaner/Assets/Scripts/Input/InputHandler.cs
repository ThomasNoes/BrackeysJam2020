namespace Assets.Scripts.Input
{
    using UnityEngine;

    public class InputHandler : MonoBehaviour
    {
        public PlayerControls playerControls;

        private void Awake()
        {
            playerControls = new PlayerControls();
        }

        public PlayerControls GetPlayerControls()
        {
            return playerControls;
        }

        private void OnEnable()
        {
            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }
    }
}