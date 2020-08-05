namespace Assets.Scripts.Input
{
    using UnityEngine;

    public class VacuumControls : MonoBehaviour
    {
        private IVacuumControls _testComponent;
        private PlayerControls _inputActions;

        private void Awake()
        {
            _inputActions = new PlayerControls();
        }

        private void Start()
        {
            _testComponent = GetComponent<IVacuumControls>();
            _inputActions.Player.Suck.started += ctx => _testComponent?.StartSuck();
            _inputActions.Player.Suck.canceled += ctx => _testComponent?.StopSuck();
            _inputActions.Player.Blow.started += ctx => _testComponent?.StartBlow();
            _inputActions.Player.Blow.canceled += ctx => _testComponent?.StopBlow();
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