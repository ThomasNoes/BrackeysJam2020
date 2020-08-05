namespace Assets.Scripts.Interaction.Vacuum
{
#if UNITY_EDITOR
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(RailVacuumable))]
    public class RailVacuumable_Inspector : UnityEditor.Editor
    {
        private GUIStyle headerStyle;

        public override void OnInspectorGUI()
        {
            var script = target as RailVacuumable;

            DrawDefaultInspector();

            if (script == null)
                return;

            if (GUILayout.Button("Reset from/to transforms"))
            {
                script.fromPos = script.gameObject.transform.position;
                script.toPos = script.gameObject.transform.position;
            }
        }
    }
#endif
}