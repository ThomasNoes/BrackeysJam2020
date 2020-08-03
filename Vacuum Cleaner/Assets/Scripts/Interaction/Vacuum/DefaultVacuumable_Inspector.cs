namespace Assets.Scripts.Interaction.Vacuum
{
#if UNITY_EDITOR
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(DefaultVacuumable))]
    public class DefaultVacuumable_Inspector : UnityEditor.Editor
    {
        private GUIStyle headerStyle;

        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("Set the weight on the Rigidbody component as it affects how suckable the object is ;)", MessageType.None);

            var script = target as DefaultVacuumable;

            DrawDefaultInspector();
        }
    }
#endif
}