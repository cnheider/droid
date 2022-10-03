namespace droid.Editor.Utilities {
  /// <summary>
  /// </summary>
  public static class NeodroidEditorUtilities {
    static UnityEngine.Color _link_color = new UnityEngine.Color(r : 0x00 / 255f,
                                                                 g : 0x78 / 255f,
                                                                 b : 0xDA / 255f,
                                                                 1f);

    static UnityEngine.GUIStyle _default_link_style =
        new UnityEngine.GUIStyle(other : UnityEditor.EditorStyles.label) {
            fontSize = 14, wordWrap = false, normal = {textColor = _link_color}, stretchWidth = false
        };

    /// <summary>
    /// </summary>
    /// <param name="label"></param>
    /// <param name="link_style"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static bool LinkLabel(UnityEngine.GUIContent label,
                                 UnityEngine.GUIStyle link_style = null,
                                 params UnityEngine.GUILayoutOption[] options) {
      if (link_style == null) {
        link_style = _default_link_style;
      }

      var position =
          UnityEngine.GUILayoutUtility.GetRect(content : label, style : link_style, options : options);

      UnityEditor.Handles.BeginGUI();

      UnityEditor.Handles.color = link_style.normal.textColor;
      UnityEditor.Handles.DrawLine(p1 : new UnityEngine.Vector3(x : position.xMin, y : position.yMax),
                                   p2 : new UnityEngine.Vector3(x : position.xMax, y : position.yMax));
      UnityEditor.Handles.color = UnityEngine.Color.white;
      UnityEditor.Handles.EndGUI();

      UnityEditor.EditorGUIUtility.AddCursorRect(position : position, mouse : UnityEditor.MouseCursor.Link);

      return UnityEngine.GUI.Button(position : position, content : label, style : link_style);
    }
  }
}