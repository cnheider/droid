namespace droid.Editor.Windows {
  /// <summary>
  /// </summary>
  public class SimulationWindow : UnityEditor.EditorWindow {
    UnityEngine.Texture _icon;
    droid.Runtime.Managers.NeodroidManager _simulation_manager;

    /// <summary>
    /// </summary>
    void OnEnable() {
      this._icon =
          (UnityEngine.Texture2D)UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath :
            $"{NeodroidSettings.Current.NeodroidImportLocationProp}Gizmos/Icons/clock.png",
            type : typeof(UnityEngine.Texture2D));
      this.titleContent =
          new UnityEngine.GUIContent("Neo:Sim", image : this._icon, "Window for controlling simulation");
    }

    void OnGUI() {
      UnityEditor.EditorGUILayout.ObjectField(obj : this._simulation_manager,
                                              objType :
                                              typeof(droid.Runtime.Managers.AbstractNeodroidManager),
                                              true);
      UnityEditor.EditorGUI.BeginDisabledGroup(disabled : !UnityEngine.Application.isPlaying);

      if (UnityEngine.GUILayout.Button("Step")) {
        this._simulation_manager?.DelegateReactions(reactions : new[] {
                                                                          new droid.Runtime.Messaging.Messages
                                                                              .Reaction(parameters :
                                                                                new droid.Runtime.Messaging
                                                                                    .Messages.
                                                                                    ReactionParameters(reaction_type
                                                                                      : droid.Runtime
                                                                                          .Messaging
                                                                                          .Messages
                                                                                          .ReactionTypeEnum
                                                                                          .Step_,
                                                                                      true,
                                                                                      true),
                                                                                null,
                                                                                null,
                                                                                null,
                                                                                null,
                                                                                "")
                                                                      });
      }

      if (UnityEngine.GUILayout.Button("Reset")) {
        this._simulation_manager?.ResetAllEnvironments();
      }

      if (this._simulation_manager) {
        this._simulation_manager.TestActuators =
            UnityEditor.EditorGUILayout.Toggle("Test Actuators",
                                               value : this._simulation_manager.TestActuators);
      }

      UnityEditor.EditorGUI.EndDisabledGroup();
    }

    void OnFocus() { this.Setup(); }

    /// <summary>
    /// </summary>
    [UnityEditor.MenuItem(itemName : EditorWindowMenuPath._WindowMenuPath + "SimulationWindow")]
    [UnityEditor.MenuItem(itemName : EditorWindowMenuPath._ToolMenuPath + "SimulationWindow")]
    public static void ShowWindow() {
      GetWindow(t : typeof(SimulationWindow)); //Show existing window instance. If one doesn't exist, make one.
      //window.Show();
    }

    void Setup() {
      var serialised_object = new UnityEditor.SerializedObject(obj : this);
      if (this._simulation_manager == null) {
        this._simulation_manager = FindObjectOfType<droid.Runtime.Managers.NeodroidManager>();
      }

      serialised_object.ApplyModifiedProperties();
    }
  }
}