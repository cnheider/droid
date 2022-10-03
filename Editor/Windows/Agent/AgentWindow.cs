#if UNITY_EDITOR
namespace droid.Editor.Windows.Agent {
  public class AgentWindow : UnityEditor.EditorWindow {
    //string _last_message = String.Empty;

    void OnEnable() { this.titleContent = new UnityEngine.GUIContent("Agents"); }

    void OnGUI() {
      if (UnityEngine.GUILayout.Button("DQN-Agent")) {
        droid.Editor.Utilities.Commands.Commands.RunDqnAgent();
      }

      if (UnityEngine.GUILayout.Button("PG-Agent")) {
        droid.Editor.Utilities.Commands.Commands.RunPgAgent();
      }

      //GUILayout.Label(text : this._last_message);
    }

    [UnityEditor.MenuItem(itemName : EditorWindowMenuPath._WindowMenuPath + "/Agents")]
    static void ShowWindow() { GetWindow<AgentWindow>(); }
  }
}
#endif