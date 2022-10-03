#if UNITY_EDITOR
namespace droid.Editor.Utilities.NoteUtilities {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEditor.CustomEditor(inspectedType : typeof(droid.Runtime.Utilities.Note))]
  public class NoteEditor : UnityEditor.Editor {
    NoteType _note_type = NoteType.Box_info_;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void OnInspectorGUI() {
      var note = (droid.Runtime.Utilities.Note)this.target;

      if (note._Editing) {
        //DrawDefaultInspector();// Unity function
        note._Text = UnityEditor.EditorGUILayout.TextArea(text : note._Text);

        UnityEditor.EditorGUILayout.Separator();

        this._note_type = (NoteType)UnityEditor.EditorGUILayout.EnumPopup(selected : this._note_type);

        if (UnityEngine.GUILayout.Button("Done")) {
          note.EditToggle();
        }
      } else {
        switch (this._note_type) {
          case NoteType.Text_area_:
            UnityEditor.EditorGUILayout.TextArea(text : note._Text);
            break;
          case NoteType.Text_field_:
            UnityEditor.EditorGUILayout.TextField(text : note._Text);
            break;
          case NoteType.Label_:
            UnityEditor.EditorGUILayout.LabelField(label : note._Text);
            break;
          case NoteType.Box_text_:
            UnityEditor.EditorGUILayout.HelpBox(message : note._Text, type : UnityEditor.MessageType.None);
            break;
          case NoteType.Box_info_:
            UnityEditor.EditorGUILayout.HelpBox(message : note._Text, type : UnityEditor.MessageType.Info);
            break;
          case NoteType.Box_warning_:
            UnityEditor.EditorGUILayout.HelpBox(message : note._Text, type : UnityEditor.MessageType.Warning);
            break;
          case NoteType.Box_error_:
            UnityEditor.EditorGUILayout.HelpBox(message : note._Text, type : UnityEditor.MessageType.Error);
            break;
          case NoteType.Delayed_text_field_:
            break;
          default:
            UnityEditor.EditorGUILayout.HelpBox(message : note._Text, type : UnityEditor.MessageType.Info);
            break;
        }

        UnityEditor.EditorGUILayout.Separator();

        if (UnityEngine.GUILayout.Button("Edit")) {
          note.EditToggle();
        }
      }
    }
  }
}
#endif