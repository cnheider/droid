namespace droid.Runtime.Messaging {
  /// <summary>
  /// </summary>
  public static class FbsReactionUtilities {
    static System.Collections.Generic.List<UnityEngine.Vector3> _vector_out =
        new System.Collections.Generic.List<UnityEngine.Vector3>();

    static System.Collections.Generic.List<float> _float_out = new System.Collections.Generic.List<float>();

    static System.Collections.Generic.List<droid.Runtime.Structs.Points.ValuePoint> _output =
        new System.Collections.Generic.List<droid.Runtime.Structs.Points.ValuePoint>();

    #region PublicMethods

    static droid.Runtime.Messaging.Messages.Reaction _null_reaction =
        new droid.Runtime.Messaging.Messages.Reaction(null,
                                                      null,
                                                      null,
                                                      null,
                                                      null,
                                                      "");

    static System.Collections.Generic.List<droid.Runtime.Messaging.Messages.Reaction> _out_reactions =
        new System.Collections.Generic.List<droid.Runtime.Messaging.Messages.Reaction>();

    /// <summary>
    /// </summary>
    /// <param name="reactions"></param>
    /// <returns></returns>
    public static
        System.Tuple<droid.Runtime.Messaging.Messages.Reaction[], bool, string,
            droid.Runtime.Messaging.Messages.SimulatorConfigurationMessage> deserialise_reactions(
            droid.Runtime.Messaging.FBS.FReactions? reactions) {
      _out_reactions.Clear();

      var close = false;
      var api_version = "";
      var simulator_configuration = new droid.Runtime.Messaging.Messages.SimulatorConfigurationMessage();

      if (reactions.HasValue) {
        var rs = reactions.Value;
        for (var i = 0; i < rs.ReactionsLength; i++) {
          _out_reactions.Add(item : deserialise_reaction(reaction : rs.Reactions(j : i)));
        }

        close = rs.Close;
        api_version = rs.ApiVersion;
        if (rs.SimulatorConfiguration.HasValue) {
          simulator_configuration.FbsParse(flat_simulator_configuration : rs.SimulatorConfiguration.Value);
        }
      }

      if (_out_reactions.Count == 0) {
        UnityEngine.Debug.LogWarning("Empty reactions received");
      }

      return new System.Tuple<droid.Runtime.Messaging.Messages.Reaction[], bool, string,
          droid.Runtime.Messaging.Messages.SimulatorConfigurationMessage>(item1 : _out_reactions.ToArray(),
        item2 : close,
        item3 : api_version,
        item4 : simulator_configuration);
    }

    /// <summary>
    /// </summary>
    /// <param name="reaction"></param>
    /// <returns></returns>
    public static droid.Runtime.Messaging.Messages.Reaction deserialise_reaction(
        droid.Runtime.Messaging.FBS.FReaction? reaction) {
      if (reaction.HasValue) {
        var r = reaction.Value;
        var motions = deserialise_motions(reaction : r);
        var configurations = deserialise_configurations(reaction : r);
        var displayables = deserialise_displayables(reaction : r);
        var unobservables = deserialise_unobservables(reaction : r);
        var parameters = deserialise_parameters(reaction : r);
        var serialised_message = deserialise_serialised_message(reaction_value : r);

        return new droid.Runtime.Messaging.Messages.Reaction(parameters : parameters,
                                                             motions : motions,
                                                             configurations : configurations,
                                                             unobservables : unobservables,
                                                             displayables : displayables,
                                                             serialised_message : serialised_message,
                                                             recipient_environment : r.EnvironmentName);
      }

      UnityEngine.Debug.LogWarning("Empty reaction received");
      return _null_reaction;
    }

    #endregion

    #region PrivateMethods

    static string deserialise_simulator_configuration(droid.Runtime.Messaging.FBS.FReaction reaction_value) {
      return reaction_value.SerialisedMessage;
    }

    static string deserialise_serialised_message(droid.Runtime.Messaging.FBS.FReaction reaction_value) {
      return reaction_value.SerialisedMessage;
    }

    static droid.Runtime.Messaging.Messages.Unobservables deserialise_unobservables(
        droid.Runtime.Messaging.FBS.FReaction reaction) {
      if (reaction.Unobservables.HasValue) {
        var bodies = deserialise_bodies(unobservables : reaction.Unobservables.Value);

        var poses = deserialise_poses(unobservables : reaction.Unobservables.Value);

        return new droid.Runtime.Messaging.Messages.Unobservables(bodies : ref bodies, poses : ref poses);
      }

      return new droid.Runtime.Messaging.Messages.Unobservables();
    }

    static droid.Runtime.Messaging.Messages.ReactionParameters deserialise_parameters(
        droid.Runtime.Messaging.FBS.FReaction reaction) {
      if (reaction.Parameters.HasValue) {
        var s = droid.Runtime.Messaging.Messages.ReactionTypeEnum.Observe_;
        if (reaction.Parameters.Value.Reset) {
          s = droid.Runtime.Messaging.Messages.ReactionTypeEnum.Reset_;
        } else if (reaction.Parameters.Value.Step) {
          s = droid.Runtime.Messaging.Messages.ReactionTypeEnum.Step_;
        }

        return new droid.Runtime.Messaging.Messages.ReactionParameters(reaction_type : s,
                                                                       terminable :
                                                                       reaction.Parameters.Value.Terminable,
                                                                       configure :
                                                                       reaction.Parameters.Value.Configure,
                                                                       episode_count : reaction.Parameters
                                                                           .Value.EpisodeCount,
                                                                       describe : reaction.Parameters.Value
                                                                           .Describe);
      }
      #if  NEODROID_DEBUG
        UnityEngine.Debug.LogError("NULL PARAMETERS");
      #endif

      return null;
    }

    static droid.Runtime.Messaging.Messages.Configuration[] deserialise_configurations(
        droid.Runtime.Messaging.FBS.FReaction reaction) {
      var l = reaction.ConfigurationsLength;
      var configurations = new droid.Runtime.Messaging.Messages.Configuration[l];
      for (var i = 0; i < l; i++) {
        configurations[i] = deserialise_configuration(configuration : reaction.Configurations(j : i));
      }

      return configurations;
    }

    static droid.Runtime.Messaging.Messages.Displayables.Displayable[] deserialise_displayables(
        droid.Runtime.Messaging.FBS.FReaction reaction) {
      var l = reaction.DisplayablesLength;
      var configurations = new droid.Runtime.Messaging.Messages.Displayables.Displayable[l];
      for (var i = 0; i < l; i++) {
        configurations[i] = deserialise_displayable(displayable : reaction.Displayables(j : i));
      }

      return configurations;
    }

    static droid.Runtime.Messaging.Messages.Displayables.Displayable deserialise_displayable(
        droid.Runtime.Messaging.FBS.FDisplayable? displayable) {
      if (displayable.HasValue) {
        var d = displayable.Value;

        switch (d.DisplayableValueType) {
          case droid.Runtime.Messaging.FBS.FDisplayableValue.NONE: break;

          case droid.Runtime.Messaging.FBS.FDisplayableValue.FValue:
            return new droid.Runtime.Messaging.Messages.Displayables.DisplayableFloat(displayable_name :
              d.DisplayableName,
              displayable_value : d.DisplayableValue<droid.Runtime.Messaging.FBS.FValue>()?.Val);

          case droid.Runtime.Messaging.FBS.FDisplayableValue.FValues:
            var v3 = d.DisplayableValue<droid.Runtime.Messaging.FBS.FValues>().GetValueOrDefault();
            _float_out.Clear();
            for (var i = 0; i < v3.ValsLength; i++) {
              _float_out.Add(item : (float)v3.Vals(j : i));
            }

            return new droid.Runtime.Messaging.Messages.Displayables.DisplayableValues(displayable_name :
              d.DisplayableName,
              displayable_value : _float_out.ToArray());

          case droid.Runtime.Messaging.FBS.FDisplayableValue.FVector3s:
            var v2 = d.DisplayableValue<droid.Runtime.Messaging.FBS.FVector3s>().GetValueOrDefault();
            _vector_out.Clear();
            for (var i = 0; i < v2.PointsLength; i++) {
              var p = v2.Points(j : i).GetValueOrDefault();
              var v = new UnityEngine.Vector3(x : (float)p.X, y : (float)p.Y, z : (float)p.Z);
              _vector_out.Add(item : v);
            }

            return new droid.Runtime.Messaging.Messages.Displayables.DisplayableVector3S(displayable_name :
              d.DisplayableName,
              displayable_value : _vector_out.ToArray());

          case droid.Runtime.Messaging.FBS.FDisplayableValue.FValuedVector3s:
            var flat_fvec3 = d.DisplayableValue<droid.Runtime.Messaging.FBS.FValuedVector3s>()
                              .GetValueOrDefault();
            _output.Clear();

            for (var i = 0; i < flat_fvec3.PointsLength; i++) {
              var val = (float)flat_fvec3.Vals(j : i);
              var p = flat_fvec3.Points(j : i).GetValueOrDefault();
              var v =
                  new droid.Runtime.Structs.Points.ValuePoint(pos : new UnityEngine.Vector3(x : (float)p.X,
                                                                y : (float)p.Y,
                                                                z : (float)p.Z),
                                                              val : val,
                                                              1);
              _output.Add(item : v);
            }

            return new
                droid.Runtime.Messaging.Messages.Displayables.DisplayableValuedVector3S(displayable_name :
                  d.DisplayableName,
                  displayable_value : _output.ToArray());

          case droid.Runtime.Messaging.FBS.FDisplayableValue.FString:
            return new droid.Runtime.Messaging.Messages.Displayables.DisplayableString(displayable_name :
              d.DisplayableName,
              displayable_value : d.DisplayableValue<droid.Runtime.Messaging.FBS.FString>()?.Str);

          case droid.Runtime.Messaging.FBS.FDisplayableValue.FByteArray: break;
          default: throw new System.ArgumentOutOfRangeException();
        }
      }

      return null;
    }

    static droid.Runtime.Interfaces.IMotion[] deserialise_motions(
        droid.Runtime.Messaging.FBS.FReaction reaction) {
      var l = reaction.MotionsLength;
      var motions = new droid.Runtime.Interfaces.IMotion[l];
      for (var i = 0; i < l; i++) {
        motions[i] = deserialise_motion(motion : reaction.Motions(j : i));
      }

      return motions;
    }

    static droid.Runtime.Messaging.Messages.Configuration deserialise_configuration(
        droid.Runtime.Messaging.FBS.FConfiguration? configuration) {
      if (configuration.HasValue) {
        var c = configuration.Value;
        var sample_random = false; //TODO: c.SampleRandom;
        return new droid.Runtime.Messaging.Messages.Configuration(configurable_name : c.ConfigurableName,
                                                                  configurable_value : (float)c
                                                                      .ConfigurableValue,
                                                                  sample_random : sample_random);
      }

      return null;
    }

    static droid.Runtime.Messaging.Messages.ActuatorMotion deserialise_motion(
        droid.Runtime.Messaging.FBS.FMotion? motion) {
      if (motion.HasValue) {
        return new droid.Runtime.Messaging.Messages.ActuatorMotion(actor_name : motion.Value.ActorName,
                                                                   actuator_name : motion.Value.ActuatorName,
                                                                   strength : (float)motion.Value.Strength);
      }

      return null;
    }

    static UnityEngine.Pose[] deserialise_poses(droid.Runtime.Messaging.FBS.FUnobservables unobservables) {
      var l = unobservables.PosesLength;
      var poses = new UnityEngine.Pose[l];
      for (var i = 0; i < l; i++) {
        poses[i] = deserialise_pose(trans : unobservables.Poses(j : i));
      }

      return poses;
    }

    static droid.Runtime.Messaging.Messages.Body[] deserialise_bodies(
        droid.Runtime.Messaging.FBS.FUnobservables unobservables) {
      var l = unobservables.BodiesLength;
      var bodies = new droid.Runtime.Messaging.Messages.Body[l];
      for (var i = 0; i < l; i++) {
        bodies[i] = deserialise_body(body : unobservables.Bodies(j : i));
      }

      return bodies;
    }

    static UnityEngine.Pose deserialise_pose(droid.Runtime.Messaging.FBS.FQuaternionTransform? trans) {
      if (trans.HasValue) {
        var position = trans.Value.Position;
        var rotation = trans.Value.Rotation;
        var vec3_pos =
            new UnityEngine.Vector3(x : (float)position.X, y : (float)position.Y, z : (float)position.Z);
        var quat_rot = new UnityEngine.Quaternion(x : (float)rotation.X,
                                                  y : (float)rotation.Y,
                                                  z : (float)rotation.Z,
                                                  w : (float)rotation.W);
        return new UnityEngine.Pose(position : vec3_pos, rotation : quat_rot);
      }

      return new UnityEngine.Pose();
    }

    static droid.Runtime.Messaging.Messages.Body deserialise_body(droid.Runtime.Messaging.FBS.FBody? body) {
      if (body.HasValue) {
        var vel = body.Value.Velocity;
        var ang = body.Value.AngularVelocity;
        var vec3_vel = new UnityEngine.Vector3(x : (float)vel.X, y : (float)vel.Y, z : (float)vel.Z);
        var vec3_ang = new UnityEngine.Vector3(x : (float)ang.X, y : (float)ang.Y, z : (float)ang.Z);
        return new droid.Runtime.Messaging.Messages.Body(vel : vec3_vel, ang : vec3_ang);
      }

      return null;
    }

    #endregion
  }
}