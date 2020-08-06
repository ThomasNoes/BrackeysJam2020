namespace Assets.Scripts.Input
{
    using UnityEngine;

    public class VacuumControls : MonoBehaviour
    {
        private IVacuumControls _testComponent;
        private InputHandler _inputHandler;
        private PlayerControls _inputActions;

        private void Start()
        {
            _inputHandler = InputHandler.inputHandler;
            if (_inputHandler != null)
                _inputActions = _inputHandler.GetPlayerControls();

            _testComponent = GetComponent<IVacuumControls>();

            _inputActions.Player.Suck.started += ctx => _testComponent?.StartSuck();
            _inputActions.Player.Suck.canceled += ctx => _testComponent?.StopSuck();
            _inputActions.Player.Blow.started += ctx => _testComponent?.StartBlow();
            _inputActions.Player.Blow.canceled += ctx => _testComponent?.StopBlow();
        }
    }
}