#if UNITY_EDITOR
namespace droid.Editor.Windows.Repl {
  public class SystemReplWindow : UnityEditor.EditorWindow {
    string _args = string.Empty;
    string _cmd = string.Empty;

    string _last_message = string.Empty;

    void OnEnable() { this.titleContent = new UnityEngine.GUIContent("System Repl"); }

    void OnGUI() {
      this._cmd = UnityEngine.GUILayout.TextField(text : this._cmd);
      this._args = UnityEngine.GUILayout.TextField(text : this._args);
      if (UnityEngine.GUILayout.Button("Run")) {
        this._last_message =
            droid.Editor.Utilities.Commands.Commands.SystemCommand(input : this._cmd, arguments : this._args);
      }

      UnityEngine.GUILayout.Label(text : this._last_message);
    }

    [UnityEditor.MenuItem(itemName : EditorWindowMenuPath._WindowMenuPath + "/REPLs/" + "System Repl")]
    static void ShowWindow() { GetWindow<SystemReplWindow>(); }
  }
}
#endif