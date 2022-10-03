#if UNITY_EDITOR

namespace droid.Editor.Windows {
  public class TaskWindow : UnityEditor.EditorWindow {
    UnityEngine.Texture _icon;
    UnityEngine.Vector2 _scroll_position;

    droid.Runtime.Prototyping.ObjectiveFunctions.Tasks.TaskSequence _task_sequence;

    void OnEnable() {
      this._icon =
          (UnityEngine.Texture2D)UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath :
            $"{NeodroidSettings.Current.NeodroidImportLocationProp}Gizmos/Icons/script.png",
            type : typeof(UnityEngine.Texture2D));
      this.titleContent =
          new UnityEngine.GUIContent("Neo:Task", image : this._icon, "Window for task descriptions");
      if (!this._task_sequence) {
        this._task_sequence =
            FindObjectOfType<droid.Runtime.Prototyping.ObjectiveFunctions.Tasks.TaskSequence>();
      }
    }

    void OnGUI() {
      UnityEngine.GUILayout.Label("Task list", style : UnityEditor.EditorStyles.boldLabel);
      this._task_sequence =
          FindObjectOfType<droid.Runtime.Prototyping.ObjectiveFunctions.Tasks.TaskSequence>();
      if (this._task_sequence != null) {
        this._scroll_position =
            UnityEditor.EditorGUILayout.BeginScrollView(scrollPosition : this._scroll_position);
        UnityEditor.EditorGUILayout.BeginVertical("Box");

        var seq = this._task_sequence.GetSequence();
        if (seq != null) {
          foreach (var g in seq) {
            if (g != null) {
              if (this._task_sequence.CurrentGoalCell != null
                  && this._task_sequence.CurrentGoalCell.name == g.name) {
                UnityEngine.GUILayout.Label(text : g.name, style : UnityEditor.EditorStyles.whiteLabel);
              } else {
                UnityEngine.GUILayout.Label(text : g.name);
              }
            }
          }
        }

        UnityEditor.EditorGUILayout.EndVertical();
        UnityEditor.EditorGUILayout.EndScrollView();
      }
    }

    /// <summary>
    /// </summary>
    public void OnInspectorUpdate() { this.Repaint(); }

    [UnityEditor.MenuItem(itemName : EditorWindowMenuPath._WindowMenuPath + "TaskWindow")]
    [UnityEditor.MenuItem(itemName : EditorWindowMenuPath._ToolMenuPath + "TaskWindow")]
    public static void ShowWindow() {
      GetWindow(t : typeof(TaskWindow)); //Show existing window instance. If one doesn't exist, make one.
    }
  }
}
#endif