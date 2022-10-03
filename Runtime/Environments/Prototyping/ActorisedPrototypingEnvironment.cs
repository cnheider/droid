namespace droid.Runtime.Environments.Prototyping {
  /// <inheritdoc cref="NeodroidEnvironment" />
  /// <summary>
  ///   Environment to be used with the prototyping components.
  /// </summary>
  [UnityEngine.AddComponentMenu("Neodroid/Environments/ActorisedPrototypingEnvironment")]
  public class ActorisedPrototypingEnvironment : AbstractPrototypingEnvironment,
                                                 droid.Runtime.Interfaces.IActorisedPrototypingEnvironment {
    #region NeodroidCallbacks

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void Clear() {
      this.Displayers.Clear();
      this.Configurables.Clear();
      this.Actors.Clear();
      this.Sensors.Clear();
      this.Listeners.Clear();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override droid.Runtime.Messaging.Messages.Reaction SampleReaction() {
      if (this.Terminated) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          UnityEngine.Debug.Log("SampleReaction resetting environment");
        }
        #endif

        var reset_reaction =
            new droid.Runtime.Messaging.Messages.ReactionParameters(reaction_type : droid.Runtime.Messaging
                                                                        .Messages.ReactionTypeEnum.Reset_,
                                                                    false,
                                                                    true);
        return new droid.Runtime.Messaging.Messages.Reaction(reaction_parameters : reset_reaction,
                                                             recipient_environment : this.Identifier);
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log(message : $"Sampling a reaction for environment {this.Identifier}");
      }
      #endif

      var sample_motions = new System.Collections.Generic.List<droid.Runtime.Interfaces.IMotion>();

      foreach (var actor in this.Actors) {
        var actor_value = actor.Value;
        if (actor_value?.Actuators != null) {
          foreach (var actuator in actor_value.Actuators) {
            var actuator_value = actuator.Value;
            if (actuator_value != null) {
              sample_motions.Add(item : new droid.Runtime.Messaging.Messages.ActuatorMotion(actor_name :
                                   actor.Key,
                                   actuator_name : actuator.Key,
                                   strength : actuator_value.Sample()));
            }
          }
        }
      }

      var rp =
          new droid.Runtime.Messaging.Messages.ReactionParameters(reaction_type : droid.Runtime.Messaging
                                                                      .Messages.ReactionTypeEnum.Step_,
                                                                  true,
                                                                  episode_count : true);
      return new droid.Runtime.Messaging.Messages.Reaction(parameters : rp,
                                                           motions : sample_motions.ToArray(),
                                                           null,
                                                           null,
                                                           null,
                                                           "",
                                                           recipient_environment : this.Identifier);
    }

    #endregion

    #region PublicMethods

    #region Getters

    /// <summary>
    /// </summary>
    public System.Collections.Generic.SortedDictionary<string, droid.Runtime.Interfaces.IActor>
        Actors { get; } =
      new System.Collections.Generic.SortedDictionary<string, droid.Runtime.Interfaces.IActor>();

    #endregion

    #region Registration

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="actor"></param>
    public void Register(droid.Runtime.Interfaces.IActor actor) {
      this.Register(actor : actor, identifier : actor.Identifier);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="actor"></param>
    /// <param name="identifier"></param>
    public void Register(droid.Runtime.Interfaces.IActor actor, string identifier) {
      if (!this.Actors.ContainsKey(key : identifier)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          UnityEngine.Debug.Log(message : $"Environment {this.name} has registered actor {identifier}");
        }
        #endif

        this.Actors.Add(key : identifier, value : actor);
      } else {
        UnityEngine.Debug.LogWarning(message :
                                     $"WARNING! Please check for duplicates, Environment {this.name} already has actor {identifier} registered");
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="actor"></param>
    public void UnRegister(droid.Runtime.Interfaces.IActor actor) {
      this.UnRegister(t : actor, obj : actor.Identifier);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="t"></param>
    /// <param name="obj"></param>
    public void UnRegister(droid.Runtime.Interfaces.IActor t, string obj) {
      if (this.Actors.ContainsKey(key : obj)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          UnityEngine.Debug.Log(message : $"Environment {this.name} unregistered actor {obj}");
        }
        #endif
        this.Actors.Remove(key : obj);
      }
    }

    #endregion

    #endregion

    #region PrivateMethods

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override droid.Runtime.Messaging.Messages.EnvironmentSnapshot Snapshot() {
      var signal = 0f;

      if (this.ObjectiveFunction != null) {
        signal = this.ObjectiveFunction.Evaluate();
      }

      droid.Runtime.Messaging.Messages.EnvironmentDescription description = null;
      if (this.ProvideFullDescription
          || this.SimulationManager.SimulatorConfiguration.SerialiseIndividualObservables) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          UnityEngine.Debug.Log("Describing Environment");
        }
        #endif

        description =
            new droid.Runtime.Messaging.Messages.EnvironmentDescription(objective_function_function :
                                                                        this.ObjectiveFunction,
                                                                        actors : this.Actors,
                                                                        configurables : this.Configurables,
                                                                        sensors : this.Sensors,
                                                                        displayers : this.Displayers);
      }

      var obs = new float[] { };
      if (this.ProvideFullDescription
          || this.SimulationManager.SimulatorConfiguration.SerialiseAggregatedFloatArray) {
        this._Observables.Clear();

        foreach (var item in this.Sensors) {
          if (item.Value != null) {
            if (item.Value.FloatEnumerable != null) {
              this._Observables.AddRange(collection : item.Value.FloatEnumerable);
            } else {
              #if NEODROID_DEBUG
              if (this.Debugging) {
                UnityEngine.Debug.Log(message :
                                      $"Sensor with key {item.Key} has a null FloatEnumerable value");
              }
              #endif
            }
          } else {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              UnityEngine.Debug.Log(message : $"Sensor with key {item.Key} has a null value");
            }
            #endif
          }
        }

        obs = this._Observables.ToArray();
      }

      var time = UnityEngine.Time.realtimeSinceStartup - this.LastResetTime;

      var state = new droid.Runtime.Messaging.Messages.EnvironmentSnapshot(environment_name : this.Identifier,
        frame_number : this.StepI,
        time : time,
        signal : signal,
        terminated : this.Terminated,
        observables : ref obs,
        termination_reason : this.LastTerminationReason,
        description : description);

      if (this.SimulationManager.SimulatorConfiguration.SerialiseUnobservables
          || this.ProvideFullDescription) {
        state.Unobservables =
            new droid.Runtime.Messaging.Messages.Unobservables(rigidbodies : ref this._Tracked_Rigid_Bodies,
                                                               transforms : ref this._Poses);
      }

      this.ProvideFullDescription = false;

      return state;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="recipient"></param>
    public override void
        ObservationsString(droid.Runtime.GameObjects.StatusDisplayer.EventRecipients.DataPoller recipient) {
      recipient.PollData(data : string.Join("\n\n",
                                            values : System.Linq.Enumerable.Select(source : this.Sensors
                                                  .Values,
                                              e => $"{e.Identifier}:\n{e}")));
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void RemotePostSetup() {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log("PostSetup");
      }
      #endif

      foreach (var configurable in this.Configurables.Values) {
        configurable?.RemotePostSetup();
      }

      foreach (var actor in this.Actors.Values) {
        actor?.RemotePostSetup();
      }

      foreach (var sensor in this.Sensors.Values) {
        sensor?.RemotePostSetup();
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="reaction"></param>
    void SendToDisplayers(droid.Runtime.Messaging.Messages.Reaction reaction) {
      if (reaction.Displayables != null && reaction.Displayables.Length > 0) {
        foreach (var displayable in reaction.Displayables) {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            UnityEngine.Debug.Log(message : "Applying " + displayable + " To " + this.name + "'s displayers");
          }
          #endif
          var displayable_name = displayable.DisplayableName;
          if (this.Displayers.ContainsKey(key : displayable_name)
              && this.Displayers[key : displayable_name] != null) {
            var v = displayable.DisplayableValue;
            this.Displayers[key : displayable_name].Display(v);
          } else {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              UnityEngine.Debug.Log(message : "Could find not displayer with the specified name: "
                                              + displayable_name);
            }
            #endif
          }
        }
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="reaction"></param>
    protected override void SendToActors(droid.Runtime.Messaging.Messages.Reaction reaction) {
      if (reaction.Motions != null && reaction.Motions.Length > 0) {
        foreach (var motion in reaction.Motions) {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            UnityEngine.Debug.Log(message : "Applying " + motion + " To " + this.name + "'s actors");
          }
          #endif
          var motion_actor_name = motion.ActorName;
          if (this.Actors.ContainsKey(key : motion_actor_name)
              && this.Actors[key : motion_actor_name] != null) {
            this.Actors[key : motion_actor_name].ApplyMotion(motion : motion);
          } else {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              UnityEngine.Debug.Log(message : "Could find not actor with the specified name: "
                                              + motion_actor_name);
            }
            #endif
          }
        }
      }
    }

    /// <summary>
    /// </summary>
    protected override void InnerResetRegisteredObjects() {
      foreach (var actor in this.Actors.Values) {
        actor?.PrototypingReset();
      }
    }

    #endregion
  }
}