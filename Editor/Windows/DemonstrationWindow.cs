#if UNITY_EDITOR
namespace droid.Editor.Windows {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class DemonstrationWindow : UnityEditor.EditorWindow {
    int _captured_frame;

    string _file_name = "Demonstration/frame";
    UnityEngine.Texture _icon;
    float _last_frame_time;
    string _record_button = "Record";
    bool _recording;

    string _status = "Idle";

    void Update() {
      if (this._recording) {
        if (UnityEditor.EditorApplication.isPlaying && !UnityEditor.EditorApplication.isPaused) {
          this.RecordImages();
          this.Repaint();
        } else {
          this._status = "Waiting for Editor to Play";
        }
      }
    }

    void OnEnable() {
      this._icon =
          (UnityEngine.Texture2D)UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath :
            $"{NeodroidSettings.Current.NeodroidImportLocationProp}Gizmos/Icons/bullet_red.png",
            type : typeof(UnityEngine.Texture2D));
      this.titleContent =
          new UnityEngine.GUIContent("Neo:Rec", image : this._icon, "Window for recording demonstrations");
    }

    void OnGUI() {
      this._file_name = UnityEditor.EditorGUILayout.TextField("File Name:", text : this._file_name);

      if (UnityEngine.GUILayout.Button(text : this._record_button)) {
        if (this._recording) {
          //recording
          this._status = "Idle...";
          this._record_button = "Record";
          this._recording = false;
        } else {
          // idle
          this._captured_frame = 0;
          this._record_button = "Stop";
          this._recording = true;
        }
      }

      UnityEditor.EditorGUILayout.LabelField("Status: ", label2 : this._status);
    }

    /// <summary>
    /// </summary>
    public void OnInspectorUpdate() { this.Repaint(); }

    /// <summary>
    /// </summary>
    [UnityEditor.MenuItem(itemName : EditorWindowMenuPath._WindowMenuPath + "DemonstrationWindow")]
    [UnityEditor.MenuItem(itemName : EditorWindowMenuPath._ToolMenuPath + "DemonstrationWindow")]
    public static void ShowWindow() {
      GetWindow(t : typeof(DemonstrationWindow)); //Show existing window instance. If one doesn't exist, make one.
    }

    void RecordImages() {
      if (this._last_frame_time < UnityEngine.Time.time + 1 / 24f) {
        // 24fps
        this._status = $"Captured frame{this._captured_frame}";
        UnityEngine.ScreenCapture.CaptureScreenshot(filename : $"{this._file_name}{this._captured_frame}.png");
        this._captured_frame++;
        this._last_frame_time = UnityEngine.Time.time;
      }
    }
  }
}
#endif