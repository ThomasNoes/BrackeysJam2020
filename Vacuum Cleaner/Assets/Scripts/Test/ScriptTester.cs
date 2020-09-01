namespace Assets.Scripts.Test
{
    using UnityEngine;

    public class ScriptTester : MonoBehaviour
    {
        public KeyCode startKey, stopKey;
        private ITester _testComponent;
        private PlayerControls _inputActions;

        private void Awake()
        {
            _inputActions = new PlayerControls();
        }

        private void Start()
        {
            _testComponent = GetComponent<ITester>();
            _inputActions.Player.Suck.started += ctx => _testComponent?.StartTest();
            _inputActions.Player.Suck.canceled += ctx => _testComponent?.StopTest();
        }

        private void Update()
        {
            if (Input.GetKeyDown(startKey))
                _testComponent?.StartTest();
            else if (Input.GetKeyDown(stopKey))
                _testComponent?.StopTest();
        }

        private void OnEnable()
        {
            _inputActions.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Disable();
        }
    }
}