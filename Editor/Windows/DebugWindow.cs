#if UNITY_EDITOR && NEODROID_DEBUG
namespace droid.Editor.Windows {
  /// <summary>
  /// </summary>
  public class DebugWindow : UnityEditor.EditorWindow {
    droid.Runtime.Prototyping.Actors.Actor[] _actors;

    droid.Runtime.Prototyping.Actuators.Actuator[] _actuators;

    droid.Runtime.Prototyping.Configurables.Configurable[] _configurables;
    bool _debug_all;

    droid.Runtime.Prototyping.Displayers.Displayer[] _displayers;

    droid.Runtime.Environments.NeodroidEnvironment[] _environments;

    UnityEngine.Texture _icon;

    droid.Runtime.Prototyping.EnvironmentListener.EnvironmentListener[] _listeners;

    droid.Runtime.Managers.AbstractNeodroidManager _manager;

    droid.Runtime.Prototyping.ObjectiveFunctions.ObjectiveFunction[] _objective_functions_function;

    droid.Runtime.Utilities.InternalReactions.PlayerReactions _player_reactions;

    UnityEngine.Vector2 _scroll_position;

    droid.Runtime.Prototyping.Sensors.Sensor[] _sensors;

    bool _show_actors_debug;
    bool _show_actuators_debug;
    bool _show_configurables_debug;
    bool _show_displayers_debug;
    bool _show_environments_debug;
    bool _show_listeners_debug;
    bool _show_objective_functions_debug;
    bool _show_player_reactions_debug;
    bool _show_sensors_debug;
    bool _show_simulation_manager_debug;

    void OnEnable() {
      this.FindObjects();
      this._icon =
          (UnityEngine.Texture2D)UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath :
            $"{NeodroidSettings.Current.NeodroidImportLocationProp}Gizmos/Icons/information.png",
            type : typeof(UnityEngine.Texture2D));
      this.titleContent =
          new UnityEngine.GUIContent("Neo:Debug",
                                     image : this._icon,
                                     "Window for controlling debug messages");
    }

    void OnGUI() {
      this.FindObjects();

      var prev_debug_all = this._debug_all;
      this._debug_all = UnityEditor.EditorGUILayout.Toggle("Debug everything", value : this._debug_all);
      if (this._debug_all != prev_debug_all) {
        if (this._debug_all) {
          this.EnableAll();
        } else {
          this.DisableAll();
        }
      }

      UnityEditor.EditorGUILayout.Separator();

      this._scroll_position =
          UnityEditor.EditorGUILayout.BeginScrollView(scrollPosition : this._scroll_position);
      UnityEditor.EditorGUILayout.BeginVertical("Box");

      this._show_simulation_manager_debug =
          UnityEditor.EditorGUILayout.Toggle("Debug simulation manager",
                                             value : this._show_simulation_manager_debug);
      this._show_player_reactions_debug =
          UnityEditor.EditorGUILayout.Toggle("Debug player reactions",
                                             value : this._show_player_reactions_debug);
      this._show_environments_debug =
          UnityEditor.EditorGUILayout.Toggle("Debug all environments", value : this._show_environments_debug);
      this._show_actors_debug =
          UnityEditor.EditorGUILayout.Toggle("Debug all actors", value : this._show_actors_debug);
      this._show_actuators_debug =
          UnityEditor.EditorGUILayout.Toggle("Debug all Actuators", value : this._show_actuators_debug);
      this._show_sensors_debug =
          UnityEditor.EditorGUILayout.Toggle("Debug all sensors", value : this._show_sensors_debug);
      this._show_configurables_debug =
          UnityEditor.EditorGUILayout.Toggle("Debug all configurables",
                                             value : this._show_configurables_debug);
      this._show_objective_functions_debug =
          UnityEditor.EditorGUILayout.Toggle("Debug all objective functions",
                                             value : this._show_objective_functions_debug);
      this._show_displayers_debug =
          UnityEditor.EditorGUILayout.Toggle("Debug all displayers", value : this._show_displayers_debug);
      this._show_listeners_debug =
          UnityEditor.EditorGUILayout.Toggle("Debug all listeners", value : this._show_listeners_debug);

      UnityEditor.EditorGUILayout.EndVertical();

      UnityEditor.EditorGUILayout.EndScrollView();

      this._debug_all = this.AreAllChecked();

      if (UnityEngine.GUILayout.Button("Apply")) {
        if (this._manager != null) {
          this._manager.Debugging = this._show_simulation_manager_debug;
        }

        if (this._player_reactions != null) {
          this._player_reactions.Debugging = this._show_player_reactions_debug;
        }

        foreach (var environment in this._environments) {
          environment.Debugging = this._show_environments_debug;
        }

        foreach (var actor in this._actors) {
          actor.Debugging = this._show_actors_debug;
        }

        foreach (var actuator in this._actuators) {
          actuator.Debugging = this._show_actuators_debug;
        }

        foreach (var observer in this._sensors) {
          observer.Debugging = this._show_sensors_debug;
        }

        foreach (var configurable in this._configurables) {
          configurable.Debugging = this._show_configurables_debug;
        }

        foreach (var objective_functions in this._objective_functions_function) {
          objective_functions.Debugging = this._show_objective_functions_debug;
        }

        foreach (var displayer in this._displayers) {
          displayer.Debugging = this._show_displayers_debug;
        }

        foreach (var listener in this._listeners) {
          listener.Debugging = this._show_listeners_debug;
        }
      }

      if (UnityEngine.GUI.changed && !UnityEngine.Application.isPlaying) {
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene : UnityEngine.SceneManagement
                                                                          .SceneManager.GetActiveScene());
        // Unity not tracking changes to properties of a GameObject made through this window automatically and
        // are not saved unless other changes are made from a working inspector window
      }
    }

    /// <summary>
    /// </summary>
    public void OnInspectorUpdate() { this.Repaint(); }

    /// <summary>
    /// </summary>
    [UnityEditor.MenuItem(itemName : EditorWindowMenuPath._WindowMenuPath + "DebugWindow")]
    [UnityEditor.MenuItem(itemName : EditorWindowMenuPath._ToolMenuPath + "DebugWindow")]
    public static void ShowWindow() {
      GetWindow<DebugWindow>(); //Show existing window instance. If one doesn't exist, make one.
    }

    void FindObjects() {
      this._manager = FindObjectOfType<droid.Runtime.Managers.NeodroidManager>();
      this._environments = FindObjectsOfType<droid.Runtime.Environments.NeodroidEnvironment>();
      this._actors = FindObjectsOfType<droid.Runtime.Prototyping.Actors.Actor>();
      this._actuators = FindObjectsOfType<droid.Runtime.Prototyping.Actuators.Actuator>();
      this._sensors = FindObjectsOfType<droid.Runtime.Prototyping.Sensors.Sensor>();
      this._configurables = FindObjectsOfType<droid.Runtime.Prototyping.Configurables.Configurable>();
      this._objective_functions_function =
          FindObjectsOfType<droid.Runtime.Prototyping.ObjectiveFunctions.ObjectiveFunction>();
      this._displayers = FindObjectsOfType<droid.Runtime.Prototyping.Displayers.Displayer>();
      this._listeners =
          FindObjectsOfType<droid.Runtime.Prototyping.EnvironmentListener.EnvironmentListener>();
      this._player_reactions = FindObjectOfType<droid.Runtime.Utilities.InternalReactions.PlayerReactions>();
    }

    void EnableAll() {
      this._show_simulation_manager_debug = true;
      this._show_player_reactions_debug = true;
      this._show_environments_debug = true;
      this._show_actors_debug = true;
      this._show_actuators_debug = true;
      this._show_sensors_debug = true;
      this._show_configurables_debug = true;
      this._show_objective_functions_debug = true;
      this._show_displayers_debug = true;
      this._show_listeners_debug = true;
    }

    void DisableAll() {
      this._show_simulation_manager_debug = false;
      this._show_player_reactions_debug = false;
      this._show_environments_debug = false;
      this._show_actors_debug = false;
      this._show_actuators_debug = false;
      this._show_sensors_debug = false;
      this._show_configurables_debug = false;
      this._show_objective_functions_debug = false;
      this._show_displayers_debug = false;
      this._show_listeners_debug = false;
    }

    bool AreAllChecked() {
      if (this._show_simulation_manager_debug
          && this._show_player_reactions_debug
          && this._show_environments_debug
          && this._show_actors_debug
          && this._show_actuators_debug
          && this._show_sensors_debug
          && this._show_configurables_debug
          && this._show_objective_functions_debug
          && this._show_displayers_debug
          && this._show_listeners_debug) {
        return true;
      }

      return false;
    }
  }
}
#endif