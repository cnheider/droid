

namespace droid.Runtime.Utilities.InternalReactions {

  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.ExecuteInEditMode]
  public class PlayerReactions : ScriptedReactions {
    
    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    bool _auto_reset = true;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    droid.Runtime.ScriptableObjects.PlayerMotions _player_motions = null;

    [UnityEngine.SerializeField] bool terminated;

    #if !INPUT_SYSTEM_EXISTS
    /// <summary>
    /// </summary>
    void Start() {
      this._Manager = FindObjectOfType<droid.Runtime.Managers.AbstractNeodroidManager>();
      if (UnityEngine.Application.isPlaying) {
        var reset_reaction =
            new droid.Runtime.Messaging.Messages.ReactionParameters(reaction_type : droid.Runtime.Messaging
                                                                        .Messages.ReactionTypeEnum.Reset_);
        this._Manager.DelegateReactions(reactions : new[] {
                                                              new droid.Runtime.Messaging.Messages.
                                                                  Reaction(reaction_parameters :
                                                                    reset_reaction,
                                                                    "all")
                                                          });
      }
    }

    void Update() {
      if (UnityEngine.Application.isPlaying) {
        if (this._player_motions != null) {
          var motions = new System.Collections.Generic.List<droid.Runtime.Interfaces.IMotion>();
          if (this._player_motions._Motions != null) {
            for (var index = 0; index < this._player_motions._Motions.Length; index++) {
              var player_motion = this._player_motions._Motions[index];
              if (UnityEngine.Input.GetKey(key : player_motion._Key)) {
                #if NEODROID_DEBUG
                if (this.Debugging) {
                  UnityEngine.Debug.Log(message :
                                        $"{player_motion._Actor} {player_motion._Actuator} {player_motion._Strength}");
                }
                #endif

                if (player_motion._Actuator == "Reset") {
                  this.terminated = true;
                  break;
                }

                var motion =
                    new droid.Runtime.Messaging.Messages.ActuatorMotion(actor_name : player_motion._Actor,
                                                                        actuator_name : player_motion
                                                                            ._Actuator,
                                                                        strength : player_motion._Strength);
                motions.Add(item : motion);
              }
            }
          }

          if (this.terminated && this._auto_reset) {
            var reset_reaction_parameters =
                new droid.Runtime.Messaging.Messages.ReactionParameters(reaction_type : droid.Runtime
                                                                            .Messaging.Messages
                                                                            .ReactionTypeEnum.Reset_);
            this._Manager.DelegateReactions(reactions : new[] {
                                                                  new droid.Runtime.Messaging.Messages.
                                                                      Reaction(reaction_parameters :
                                                                        reset_reaction_parameters,
                                                                        "all")
                                                              });
            var any = false;
            var es = this._Manager.GatherSnapshots();
            for (var index = 0; index < es.Length; index++) {
              var e = es[index];
              if (e.Terminated) {
                any = true;
                break;
              }
            }

            this.terminated = any;
          } else if (motions.Count > 0) {
            var parameters =
                new droid.Runtime.Messaging.Messages.ReactionParameters(reaction_type : droid.Runtime
                                                                            .Messaging.Messages
                                                                            .ReactionTypeEnum.Step_,
                                                                        true,
                                                                        episode_count : true);
            var reaction = new droid.Runtime.Messaging.Messages.Reaction(parameters : parameters,
                                                                           motions : motions.ToArray(),
                                                                           null,
                                                                           null,
                                                                           null,
                                                                           "",
                                                                           reaction_source :
                                                                           "PlayerReactions");
            this._Manager.DelegateReactions(reactions : new[] {reaction});
            var any = false;
            var es = this._Manager.GatherSnapshots();
            for (var index = 0; index < es.Length; index++) {
              var e = es[index];
              if (e.Terminated) {
                any = true;
                break;
              }
            }

            this.terminated = any;
            motions.Clear();
          }
        }
      } else {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          UnityEngine.Debug.Log("No PlayerMotions ScriptableObject assigned");
        }
        #endif
      }
    }
    #endif
  }
  
}