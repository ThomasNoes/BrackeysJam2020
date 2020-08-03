namespace Assets.Scripts.Test
{
    using UnityEngine;

    public class ScriptTester : MonoBehaviour
    {
        public KeyCode startKey, stopKey;
        private ITester _testComponent;

        private void Start()
        {
            _testComponent = GetComponent<ITester>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(startKey))
                _testComponent?.StartTest();
            else if (Input.GetKeyDown(stopKey))
                _testComponent?.StopTest();
        }
    }
}