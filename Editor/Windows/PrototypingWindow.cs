#if UNITY_EDITOR
namespace droid.Editor.Windows {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class PrototypingWindow : UnityEditor.EditorWindow {
    const int _logo_image_size = 100;

    const string _neodroid_url_text = "Documentation";
    const string _neodroid_url = "https://neodroid.ml/documentation";

    /// <summary>
    /// </summary>
    const bool _refresh_enabled = false;

    droid.Runtime.Environments.Prototyping.PrototypingEnvironment[] _environments;
    UnityEngine.Texture _icon;
    UnityEngine.Texture _neodroid_icon;

    droid.Runtime.Utilities.InternalReactions.PlayerReactions _player_reactions;
    UnityEngine.Vector2 _scroll_position;
    bool _show_detailed_descriptions;
    bool[] _show_environment_properties = new bool[1];

    /// <summary>
    /// </summary>
    droid.Runtime.Managers.AbstractNeodroidManager _simulation_manager;

    /// <summary>
    /// </summary>
    void OnEnable() {
      this._icon =
          (UnityEngine.Texture2D)UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath :
            $"{NeodroidSettings.Current.NeodroidImportLocationProp}Gizmos/Icons/world.png",
            type : typeof(UnityEngine.Texture2D));
      this._neodroid_icon =
          (UnityEngine.Texture)UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath :
                                                                           $"{NeodroidSettings.Current.NeodroidImportLocationProp}Gizmos/Icons/neodroid_favicon_cut.png",
                                                                           type : typeof(UnityEngine.Texture
                                                                           ));
      this.titleContent =
          new UnityEngine.GUIContent("Neo:Env", image : this._icon, "Window for configuring environments");
      this.Setup();
    }

    /// <summary>
    /// </summary>
    void OnGUI() {
      var serialised_object = new UnityEditor.SerializedObject(obj : this);
      this._simulation_manager = FindObjectOfType<droid.Runtime.Managers.NeodroidManager>();
      if (this._simulation_manager) {
        UnityEditor.EditorGUILayout.BeginHorizontal();

        UnityEditor.EditorGUILayout.BeginVertical();
        UnityEngine.GUILayout.Label(image : this._neodroid_icon,
                                    UnityEngine.GUILayout.Width(width : _logo_image_size),
                                    UnityEngine.GUILayout.Height(height : _logo_image_size));

        if (droid.Editor.Utilities.NeodroidEditorUtilities.LinkLabel(label : new
                                                                         UnityEngine.GUIContent(text :
                                                                           _neodroid_url_text))) {
          UnityEngine.Application.OpenURL(url : _neodroid_url);
        }

        UnityEditor.EditorGUILayout.EndVertical();

        UnityEditor.EditorGUILayout.BeginVertical();
        UnityEditor.EditorGUILayout.ObjectField(obj : this._simulation_manager,
                                                objType : typeof(droid.Runtime.Managers.
                                                    AbstractNeodroidManager),
                                                true);

        this._simulation_manager.Configuration =
            (droid.Runtime.Structs.SimulatorConfiguration)
            UnityEditor.EditorGUILayout.ObjectField(obj : (droid.Runtime.Structs.SimulatorConfiguration)this
                                                        ._simulation_manager.Configuration,
                                                    objType : typeof(droid.Runtime.Structs.
                                                        SimulatorConfiguration),
                                                    true);

        this._simulation_manager.Configuration.FrameSkips =
            UnityEditor.EditorGUILayout.IntField("Frame Skips",
                                                 value : this._simulation_manager.Configuration.FrameSkips);

        this._simulation_manager.Configuration.SimulationType =
            (droid.Runtime.Enums.SimulationTypeEnum)UnityEditor.EditorGUILayout.EnumPopup("Simulation Type",
              selected : this._simulation_manager.Configuration.SimulationType);

        this._player_reactions =
            FindObjectOfType<droid.Runtime.Utilities.InternalReactions.PlayerReactions>();
        UnityEditor.EditorGUILayout.ObjectField(obj : this._player_reactions,
                                                objType : typeof(droid.Runtime.Utilities.InternalReactions.
                                                    PlayerReactions),
                                                true);

        this._show_detailed_descriptions =
            UnityEditor.EditorGUILayout.Toggle("Show Details", value : this._show_detailed_descriptions);

        UnityEditor.EditorGUILayout.EndVertical();

        UnityEditor.EditorGUILayout.EndHorizontal();

        this._environments = droid.Runtime.Utilities.NeodroidSceneUtilities
                                  .FindAllObjectsOfTypeInScene<droid.Runtime.Environments.Prototyping.
                                      PrototypingEnvironment>();
        if (this._show_environment_properties.Length != this._environments.Length) {
          this.Setup();
        }

        this._scroll_position =
            UnityEditor.EditorGUILayout.BeginScrollView(scrollPosition : this._scroll_position);

        UnityEditor.EditorGUILayout.BeginVertical("Box");
        var num_active_environments = this._environments.Length;
        var num_inactive_environments = this._environments.Length - num_active_environments;

        //EditorGUILayout.BeginHorizontal();

        UnityEngine.GUILayout.Label(text :
                                    $"Environments - Active({num_active_environments}), Inactive({num_inactive_environments}), Total({this._environments.Length})");

        //EditorGUILayout.EndHorizontal();

        if (this._show_environment_properties != null) {
          for (var i = 0; i < this._show_environment_properties.Length; i++) {
            if (this._environments[i].isActiveAndEnabled) {
              this._show_environment_properties[i] =
                  UnityEditor.EditorGUILayout.Foldout(foldout : this._show_environment_properties[i],
                                                      content : $"{this._environments[i].Identifier}");
              if (this._show_environment_properties[i]) {
                var sensors = this._environments[i].Sensors;
                var configurables = this._environments[i].Configurables;
                var listeners = this._environments[i].Listeners;
                var displayers = this._environments[i].Displayers;

                UnityEditor.EditorGUILayout.BeginVertical("Box");
                this._environments[i].enabled =
                    UnityEditor.EditorGUILayout.BeginToggleGroup(label : this._environments[i].Identifier,
                                                                 toggle : this._environments[i].enabled
                                                                   && this._environments[i].gameObject
                                                                       .activeSelf);

                UnityEditor.EditorGUILayout.ObjectField(obj : this._environments[i],
                                                        objType : typeof(droid.Runtime.Environments.
                                                            Prototyping.PrototypingEnvironment),
                                                        true);

                if (this._show_detailed_descriptions) {
                  this._environments[i].CoordinateSpaceEnum =
                      (droid.Runtime.Enums.CoordinateSpaceEnum)
                      UnityEditor.EditorGUILayout.EnumPopup("Coordinate system",
                                                            selected : this._environments[i]
                                                                .CoordinateSpaceEnum);
                  UnityEditor.EditorGUI.BeginDisabledGroup(disabled : this._environments[i]
                                                                          .CoordinateSpaceEnum
                                                                      != droid.Runtime.Enums
                                                                          .CoordinateSpaceEnum.Environment_);
                  this._environments[i].CoordinateReferencePoint =
                      (UnityEngine.Transform)UnityEditor.EditorGUILayout.ObjectField("Reference point",
                        obj : this._environments[i].CoordinateReferencePoint,
                        objType : typeof(UnityEngine.Transform),
                        true);
                  UnityEditor.EditorGUI.EndDisabledGroup();
                  if (this._environments[i].ObjectiveFunction != null) {
                    this._environments[i].ObjectiveFunction =
                        (droid.Runtime.Prototyping.ObjectiveFunctions.EpisodicObjective)
                        UnityEditor.EditorGUILayout.ObjectField("Objective function",
                                                                obj : this._environments[i].ObjectiveFunction,
                                                                objType : typeof(droid.Runtime.Prototyping.
                                                                    ObjectiveFunctions.EpisodicObjective),
                                                                true);
                    UnityEditor.EditorGUILayout.LabelField(label :
                                                           $"Signal: {this._environments[i].ObjectiveFunction.LastSignal}");
                    this._environments[i].ObjectiveFunction.SignalSpace
                        .FromVector3(vector3_field :
                                     UnityEditor.EditorGUILayout.Vector3Field(label : droid.Runtime.Structs
                                           .Space.Space1.Vector3Description(),
                                       value : this._environments[i].ObjectiveFunction.SignalSpace
                                                   .ToVector3()));
                    this._environments[i].ObjectiveFunction.EpisodeLength =
                        UnityEditor.EditorGUILayout.IntField("Episode Length",
                                                             value : this._environments[i].ObjectiveFunction
                                                                 .EpisodeLength);
                  }
                  //EditorGUILayout.BeginHorizontal("Box");
                  #if NEODROID_DEBUG
                  this._environments[i].Debugging =
                      UnityEditor.EditorGUILayout.Toggle("Debugging",
                                                         value : this._environments[i].Debugging);
                  #endif
                  //EditorGUILayout.EndHorizontal();

                  UnityEditor.EditorGUI.BeginDisabledGroup(true);
                  UnityEditor.EditorGUILayout.LabelField("Info:");
                  UnityEditor.EditorGUILayout.Toggle("Terminated", value : this._environments[i].Terminated);
                  UnityEditor.EditorGUI.EndDisabledGroup();
                }

                var s =
                    this._environments[i] as
                            droid.Runtime.Environments.Prototyping.AbstractPrototypingEnvironment as
                        droid.Runtime.Environments.Prototyping.ActorisedPrototypingEnvironment;
                if (s) {
                  var actors = s.Actors;
                  this.DrawActors(actors : actors);
                } else {
                  var c = this._environments[i];
                  if (c) {
                    this.DrawActuators(actuators : c.Actuators);
                  }
                }

                this.DrawSensors(sensors : sensors);

                this.DrawConfigurables(configurables : configurables);

                this.DrawDisplayers(displayers : displayers);

                this.DrawListeners(listeners : listeners);

                UnityEditor.EditorGUILayout.EndToggleGroup();
                UnityEditor.EditorGUILayout.EndVertical();
              }
            }
          }

          UnityEditor.EditorGUILayout.EndVertical();

          UnityEditor.EditorGUILayout.BeginVertical("Box");
          UnityEngine.GUILayout.Label("Disabled environments");
          for (var i = 0; i < this._show_environment_properties.Length; i++) {
            if (!this._environments[i].isActiveAndEnabled) {
              UnityEditor.EditorGUILayout.ObjectField(obj : this._environments[i],
                                                      objType : typeof(droid.Runtime.Environments.
                                                          NeodroidEnvironment),
                                                      true);
            }
          }

          UnityEditor.EditorGUILayout.EndVertical();

          UnityEditor.EditorGUILayout.EndScrollView();
          serialised_object.ApplyModifiedProperties();
        }
      }
    }

    /// <summary>
    /// </summary>
    void OnHierarchyChange() { this.Refresh(); }

    /// <summary>
    /// </summary>
    public void OnInspectorUpdate() {
      this.Repaint();
      if (UnityEngine.GUI.changed) {
        this.Refresh();
      }
    }

    /// <summary>
    /// </summary>
    void OnValidate() {
      if (UnityEditor.EditorApplication.isPlaying || !_refresh_enabled) {
        return;
      }

      this.Refresh();
    }

    /// <summary>
    /// </summary>
    [UnityEditor.MenuItem(itemName : EditorWindowMenuPath._WindowMenuPath + "PrototypingWindow")]
    [UnityEditor.MenuItem(itemName : EditorWindowMenuPath._ToolMenuPath + "PrototypingWindow")]
    public static void ShowWindow() {
      GetWindow(t : typeof(PrototypingWindow)); //Show existing window instance. If one doesn't exist, make one.
      //window.Show();
    }

    /// <summary>
    /// </summary>
    void Setup() {
      if (this._environments != null) {
        this._show_environment_properties = new bool[this._environments.Length];
      }
    }

    /// <summary>
    /// </summary>
    void Refresh() {
      if (this._simulation_manager) {
        this._simulation_manager.Clear();
      }

      var prototyping_game_objects = FindObjectsOfType<droid.Runtime.GameObjects.PrototypingGameObject>();
      foreach (var obj in prototyping_game_objects) {
        obj.RefreshAwake();
      }

      foreach (var obj in prototyping_game_objects) {
        obj.RefreshStart();
      }
    }

    #region GUIDRAWS

    void DrawListeners(
        System.Collections.Generic.SortedDictionary<string, droid.Runtime.Interfaces.IUnobservable>
            listeners) {
      UnityEditor.EditorGUILayout.BeginVertical("Box");

      UnityEngine.GUILayout.Label("Listeners");
      foreach (var resetable in listeners) {
        var resetable_value =
            (droid.Runtime.Prototyping.EnvironmentListener.EnvironmentListener)resetable.Value;
        if (resetable_value != null) {
          UnityEditor.EditorGUILayout.BeginVertical("Box");
          resetable_value.enabled = UnityEditor.EditorGUILayout.BeginToggleGroup(label : resetable.Key,
            toggle : resetable_value.enabled && resetable_value.gameObject.activeSelf);
          UnityEditor.EditorGUILayout.ObjectField(obj : resetable_value,
                                                  objType : typeof(droid.Runtime.Prototyping.
                                                      EnvironmentListener.EnvironmentListener),
                                                  true);
          if (this._show_detailed_descriptions) {
            //EditorGUILayout.BeginHorizontal("Box");
            #if NEODROID_DEBUG
            resetable_value.Debugging =
                UnityEditor.EditorGUILayout.Toggle("Debugging", value : resetable_value.Debugging);
            #endif
            //EditorGUILayout.EndHorizontal();
          }

          UnityEditor.EditorGUILayout.EndToggleGroup();
          UnityEditor.EditorGUILayout.EndVertical();
        }
      }

      UnityEditor.EditorGUILayout.EndVertical();
    }

    void DrawActors(
        System.Collections.Generic.SortedDictionary<string, droid.Runtime.Interfaces.IActor> actors) {
      UnityEditor.EditorGUILayout.BeginVertical("Box");
      UnityEngine.GUILayout.Label("Actors");
      foreach (var actor in actors) {
        var actor_value = (droid.Runtime.Prototyping.Actors.Actor)actor.Value;
        if (actor_value != null) {
          var actuators = actor_value.Actuators;

          UnityEditor.EditorGUILayout.BeginVertical("Box");

          actor_value.enabled = UnityEditor.EditorGUILayout.BeginToggleGroup(label : actor.Key,
            toggle : actor_value.enabled && actor_value.gameObject.activeSelf);
          UnityEditor.EditorGUILayout.ObjectField(obj : actor_value,
                                                  objType : typeof(droid.Runtime.Prototyping.Actors.Actor),
                                                  true);
          if (this._show_detailed_descriptions) {
            //EditorGUILayout.BeginHorizontal("Box");
            #if NEODROID_DEBUG
            actor_value.Debugging =
                UnityEditor.EditorGUILayout.Toggle("Debugging", value : actor_value.Debugging);
            #endif
            //EditorGUILayout.EndHorizontal();
          }

          this.DrawActuators(actuators : actuators);

          UnityEditor.EditorGUILayout.EndToggleGroup();

          UnityEditor.EditorGUILayout.EndVertical();
        }
      }

      UnityEditor.EditorGUILayout.EndVertical();
    }

    void DrawSensors(
        System.Collections.Generic.SortedDictionary<string, droid.Runtime.Interfaces.ISensor> sensors) {
      UnityEditor.EditorGUILayout.BeginVertical("Box");
      UnityEngine.GUILayout.Label("Sensors");
      foreach (var sensor in sensors) {
        var sensor_value = (droid.Runtime.Prototyping.Sensors.Sensor)sensor.Value;
        if (sensor_value != null) {
          UnityEditor.EditorGUILayout.BeginVertical("Box");
          sensor_value.enabled = UnityEditor.EditorGUILayout.BeginToggleGroup(label : sensor.Key,
            toggle : sensor_value.enabled && sensor_value.gameObject.activeSelf);
          UnityEditor.EditorGUILayout.ObjectField(obj : sensor_value,
                                                  objType : typeof(droid.Runtime.Prototyping.Sensors.Sensor),
                                                  true);
          if (this._show_detailed_descriptions) {
            if (sensor_value is droid.Runtime.Interfaces.IHasSingle as_single) {
              as_single.SingleSpace.FromVector3(vector3_field :
                                                UnityEditor.EditorGUILayout.Vector3Field(label : droid.Runtime
                                                      .Structs.Space.Space1.Vector3Description(),
                                                  value : as_single.SingleSpace
                                                                   .ToVector3())); //MIN;MAX;DECIMALS

              UnityEditor.EditorGUILayout.LabelField(label :
                                                     $"Projection: [{as_single.SingleSpace.Normalised}]");
              UnityEditor.EditorGUILayout.LabelField(label : $"Clipped: [{as_single.SingleSpace.Clipped}]");
            }

            //EditorGUILayout.BeginHorizontal("Box");
            #if NEODROID_DEBUG
            sensor_value.Debugging =
                UnityEditor.EditorGUILayout.Toggle("Debugging", value : sensor_value.Debugging);
            UnityEditor.EditorGUILayout.LabelField(label : $"Observables: [{sensor_value}]");
            #endif
            //EditorGUILayout.EndHorizontal();
          }

          UnityEditor.EditorGUILayout.EndToggleGroup();
          UnityEditor.EditorGUILayout.EndVertical();
        }
      }

      UnityEditor.EditorGUILayout.EndVertical();
    }

    void DrawDisplayers(
        System.Collections.Generic.SortedDictionary<string, droid.Runtime.Interfaces.IDisplayer> displayers) {
      UnityEditor.EditorGUILayout.BeginVertical("Box");
      UnityEngine.GUILayout.Label("Displayers");
      foreach (var displayer in displayers) {
        var displayer_value = (droid.Runtime.Prototyping.Displayers.Displayer)displayer.Value;
        if (displayer_value != null) {
          UnityEditor.EditorGUILayout.BeginVertical("Box");
          displayer_value.enabled = UnityEditor.EditorGUILayout.BeginToggleGroup(label : displayer.Key,
            toggle : displayer_value.enabled && displayer_value.gameObject.activeSelf);
          UnityEditor.EditorGUILayout.ObjectField(obj : displayer_value,
                                                  objType : typeof(droid.Runtime.Prototyping.Displayers.
                                                      Displayer),
                                                  true);
          if (this._show_detailed_descriptions) {
            //EditorGUILayout.BeginHorizontal("Box");
            #if NEODROID_DEBUG
            displayer_value.Debugging =
                UnityEditor.EditorGUILayout.Toggle("Debugging", value : displayer_value.Debugging);
            #endif
            //EditorGUILayout.EndHorizontal();
          }

          UnityEditor.EditorGUILayout.EndToggleGroup();
          UnityEditor.EditorGUILayout.EndVertical();
        }
      }

      UnityEditor.EditorGUILayout.EndVertical();
    }

    void DrawConfigurables(
        System.Collections.Generic.SortedDictionary<string, droid.Runtime.Interfaces.IConfigurable>
            configurables) {
      UnityEditor.EditorGUILayout.BeginVertical("Box");
      UnityEngine.GUILayout.Label("Configurables");
      foreach (var configurable in configurables) {
        var configurable_value = (droid.Runtime.Prototyping.Configurables.Configurable)configurable.Value;
        if (configurable_value != null) {
          UnityEditor.EditorGUILayout.BeginVertical("Box");
          configurable_value.enabled = UnityEditor.EditorGUILayout.BeginToggleGroup(label : configurable.Key,
            toggle : configurable_value.enabled && configurable_value.gameObject.activeSelf);
          UnityEditor.EditorGUILayout.ObjectField(obj : configurable_value,
                                                  objType : typeof(droid.Runtime.Prototyping.Configurables.
                                                      Configurable),
                                                  true);
          if (this._show_detailed_descriptions) {
            //EditorGUILayout.BeginHorizontal("Box");
            #if NEODROID_DEBUG
            configurable_value.Debugging =
                UnityEditor.EditorGUILayout.Toggle("Debugging", value : configurable_value.Debugging);
            #endif
            //EditorGUILayout.EndHorizontal();
          }

          UnityEditor.EditorGUILayout.EndToggleGroup();
          UnityEditor.EditorGUILayout.EndVertical();
        }
      }

      UnityEditor.EditorGUILayout.EndVertical();
    }

    void DrawActuators(
        System.Collections.Generic.SortedDictionary<string, droid.Runtime.Interfaces.IActuator> actuators) {
      UnityEditor.EditorGUILayout.BeginVertical("Box");
      UnityEngine.GUILayout.Label("Actuators");
      foreach (var actuator in actuators) {
        var actuator_value = (droid.Runtime.Prototyping.Actuators.Actuator)actuator.Value;
        if (actuator_value != null) {
          UnityEditor.EditorGUILayout.BeginVertical("Box");
          actuator_value.enabled = UnityEditor.EditorGUILayout.BeginToggleGroup(label : actuator.Key,
            toggle : actuator_value.enabled && actuator_value.gameObject.activeSelf);
          UnityEditor.EditorGUILayout.ObjectField(obj : actuator_value,
                                                  objType : typeof(droid.Runtime.Prototyping.Actuators.
                                                      Actuator),
                                                  true);

          if (this._show_detailed_descriptions) {
            actuator_value.MotionSpace.FromVector3(vector3_field :
                                                   UnityEditor.EditorGUILayout.Vector3Field(label : droid
                                                         .Runtime.Structs.Space.Space1
                                                         .Vector3Description(),
                                                     value : actuator_value.MotionSpace
                                                         .ToVector3())); //MIN;MAX;DECIMALS
            //EditorGUILayout.BeginHorizontal("Box");
            #if NEODROID_DEBUG
            actuator_value.Debugging =
                UnityEditor.EditorGUILayout.Toggle("Debugging", value : actuator_value.Debugging);
            #endif
            //EditorGUILayout.EndHorizontal();
          }

          UnityEditor.EditorGUILayout.EndToggleGroup();

          UnityEditor.EditorGUILayout.EndVertical();
        }
      }

      UnityEditor.EditorGUILayout.EndVertical();
    }

    #endregion
  }
}
#endif