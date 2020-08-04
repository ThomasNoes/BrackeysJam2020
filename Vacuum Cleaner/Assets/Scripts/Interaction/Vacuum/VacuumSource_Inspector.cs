namespace Assets.Scripts.Interaction.Vacuum
{
#if UNITY_EDITOR
    using UnityEngine;
    using UnityEditor;
    using Assets.Scripts.Audio;

    [CustomEditor(typeof(VacuumSource))]
    public class VacuumSource_Inspector : UnityEditor.Editor
    {
        private GUIStyle headerStyle;

        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("", MessageType.None);

            var script = target as VacuumSource;

            DrawDefaultInspector();
        }
    }
#endif
}