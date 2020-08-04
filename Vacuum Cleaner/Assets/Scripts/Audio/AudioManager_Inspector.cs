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
            EditorGUILayout.HelpBox("For audio events you can call Play(), Stop(), PlayWithDelay(time), and StopWithDelay(time)", MessageType.None);

            var script = target as AudioManager;

            if (script == null)
                return;

            DrawDefaultInspector();

            if (script.audioSources == null)
                return;

            DrawUILine(true);

            script.inspectorIndex = EditorGUILayout.IntField("Sound at Index: ", script.inspectorIndex);

            if (GUILayout.Button("Play Sound"))
            {
                if (Application.isPlaying)
                {
                    if (script.inspectorIndex >= 0 && script.inspectorIndex < script.audioSources.Length)
                        script.Play(script.inspectorIndex);
                }
                else
                    Debug.Log("You must run the game to play sounds");
            }
            if (GUILayout.Button("Stop Sound"))
            {
                if (Application.isPlaying)
                {
                    if (script.inspectorIndex >= 0 && script.inspectorIndex < script.audioSources.Length)
                        script.Stop(script.inspectorIndex);
                }
                else
                    Debug.Log("You must run the game to stop sounds");
            }

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