namespace Assets.Scripts.Audio
{
#if UNITY_EDITOR
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(AudioManager))]
    public class AudioManager_Inspector : UnityEditor.Editor
    {
        private GUIStyle headerStyle;

        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("For audio events call Play()", MessageType.None);

            var script = target as AudioManager;

            DrawDefaultInspector();

            DrawUILine(true);

            serializedObject.ApplyModifiedProperties();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(script);
            }
        }

        #region DrawUILine function
        public static void DrawUILine(bool start)
        {
            Color color = new Color(1, 1, 1, 0.3f);
            int thickness = 1;
            if (start)
                thickness = 7;
            int padding = 8;

            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y += padding / 2;
            r.x -= 2;
            r.width += 6;
            EditorGUI.DrawRect(r, color);
        }
        #endregion
    }
#endif
}