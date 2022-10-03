#if UNITY_EDITOR
namespace droid.Editor.Windows.Repl {
  public class PythonReplWindow : UnityEditor.EditorWindow {
    string _cmd = string.Empty;
    string _last_message = string.Empty;
    string _python = "/usr/bin/python";

    void OnEnable() { this.titleContent = new UnityEngine.GUIContent("Python Repl"); }

    void OnGUI() {
      this._python = UnityEngine.GUILayout.TextField(text : this._python);
      this._cmd = UnityEngine.GUILayout.TextField(text : this._cmd);
      if (UnityEngine.GUILayout.Button("Run")) {
        this._last_message =
            droid.Editor.Utilities.Commands.Commands.PythonCommand(input : this._cmd,
                                                                   python_path : this._python);
      }

      UnityEngine.GUILayout.Label(text : this._last_message);
    }

    [UnityEditor.MenuItem(itemName : EditorWindowMenuPath._WindowMenuPath + "/REPLs/" + "Python Repl")]
    static void ShowWindow() { GetWindow<PythonReplWindow>(); }
  }
}
#endif