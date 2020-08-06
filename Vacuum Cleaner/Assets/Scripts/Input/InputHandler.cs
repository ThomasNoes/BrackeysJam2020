namespace Assets.Scripts.Input
{
    using UnityEngine;

    public class InputHandler : MonoBehaviour
    {
        public PlayerControls playerControls;
        public static InputHandler inputHandler;

        private void Awake()
        {
            if( inputHandler == null)
            {
                inputHandler = this;
            }
            else
            {
                Destroy(this);
            }
            playerControls = new PlayerControls();
            DontDestroyOnLoad(this);
        }

        public PlayerControls GetPlayerControls()
        {
            return playerControls;
        }

        private void OnEnable()
        {
            playerControls?.Enable();
        }

        private void OnDisable()
        {
            playerControls?.Disable();
        }
    }
}