namespace droid.Runtime.Messaging {
  /// <summary>
  /// </summary>
  public static class FbsStateUtilities {
    static FlatBuffers.VectorOffset _null_vector_offset = new FlatBuffers.VectorOffset();

    static FlatBuffers.Offset<droid.Runtime.Messaging.FBS.FUnobservables> _null_unobservables_offset =
        new FlatBuffers.Offset<droid.Runtime.Messaging.FBS.FUnobservables>();

    #region PublicMethods

    /// <summary>
    /// </summary>
    /// <param name="states"></param>
    /// <param name="simulator_configuration"></param>
    /// <param name="serialise_individual_observables"></param>
    /// <param name="do_serialise_unobservables"></param>
    /// <param name="do_serialise_observables"></param>
    /// <param name="api_version"></param>
    /// <returns></returns>
    public static byte[] Serialise(droid.Runtime.Messaging.Messages.EnvironmentSnapshot[] states,
                                   droid.Runtime.Messaging.Messages.SimulatorConfigurationMessage
                                       simulator_configuration = null,
                                   bool do_serialise_unobservables = false,
                                   bool serialise_individual_observables = false,
                                   bool do_serialise_observables = false,
                                   string api_version = "N/A") {
      var b = new FlatBuffers.FlatBufferBuilder(1);
      var state_offsets = new FlatBuffers.Offset<droid.Runtime.Messaging.FBS.FState>[states.Length];
      var i = 0;
      for (var index = 0; index < states.Length; index++) {
        var state = states[index];
        state_offsets[i++] = SerialiseState(b : b,
                                            snapshot : state,
                                            do_serialise_unobservables : do_serialise_unobservables,
                                            do_serialise_aggregated_float_array : do_serialise_observables,
                                            serialise_individual_observables :
                                            serialise_individual_observables);
      }

      var states_vector_offset =
          droid.Runtime.Messaging.FBS.FStates.CreateStatesVector(builder : b, data : state_offsets);

      var api_version_offset = b.CreateString(s : api_version);

      droid.Runtime.Messaging.FBS.FStates.StartFStates(builder : b);
      droid.Runtime.Messaging.FBS.FStates.AddStates(builder : b, statesOffset : states_vector_offset);
      droid.Runtime.Messaging.FBS.FStates.AddApiVersion(builder : b, apiVersionOffset : api_version_offset);
      droid.Runtime.Messaging.FBS.FStates.AddSimulatorConfiguration(builder : b,
                                                                    simulatorConfigurationOffset :
                                                                    Serialise(b : b,
                                                                      configuration :
                                                                      simulator_configuration));
      var states_offset = droid.Runtime.Messaging.FBS.FStates.EndFStates(builder : b);

      droid.Runtime.Messaging.FBS.FStates.FinishFStatesBuffer(builder : b, offset : states_offset);

      return b.SizedByteArray();
    }

    #endregion

    #region PrivateMethods

    /// <summary>
    /// </summary>
    /// <param name="b"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    static FlatBuffers.Offset<droid.Runtime.Messaging.FBS.FSimulatorConfiguration>
        Serialise(FlatBuffers.FlatBufferBuilder b,
                  droid.Runtime.Messaging.Messages.SimulatorConfigurationMessage configuration) {
      return droid.Runtime.Messaging.FBS.FSimulatorConfiguration.CreateFSimulatorConfiguration(builder : b,
           Width : configuration.Width,
           Height : configuration.Height,
           FullScreen : configuration.FullScreen,
           QualityLevel : configuration.QualityLevel,
           TimeScale : configuration.TimeScale,
           TargetFrameRate : configuration.TargetFrameRate,
           SimulationType : (droid.Runtime.Messaging.FBS.FSimulationType)configuration.SimulationType,
           FrameSkips : configuration.FrameSkips,
           0, //TODO: Remove
           NumOfEnvironments : configuration.NumOfEnvironments,
           DoSerialiseIndividualSensors : configuration.DoSerialiseIndividualSensors,
           DoSerialiseUnobservables : configuration.DoSerialiseUnobservables
         //TODO: ,configuration.DoSerialiseAggregatedFloatArray
        );
    }

    /// <summary>
    /// </summary>
    /// <param name="b"></param>
    /// <param name="snapshot"></param>
    /// <param name="serialise_individual_observables"></param>
    /// <param name="do_serialise_unobservables"></param>
    /// <param name="do_serialise_aggregated_float_array"></param>
    /// <returns></returns>
    static FlatBuffers.Offset<droid.Runtime.Messaging.FBS.FState> SerialiseState(
        FlatBuffers.FlatBufferBuilder b,
        droid.Runtime.Messaging.Messages.EnvironmentSnapshot snapshot,
        bool do_serialise_unobservables = false,
        bool do_serialise_aggregated_float_array = false,
        bool serialise_individual_observables = false) {
      var n = b.CreateString(s : snapshot.EnvironmentName);

      var observables_vector = _null_vector_offset;
      if (do_serialise_aggregated_float_array) {
        observables_vector =
            droid.Runtime.Messaging.FBS.FState.CreateObservablesVector(builder : b,
                                                                       data : snapshot.Observables);
      }

      var unobservables = _null_unobservables_offset;
      if (do_serialise_unobservables) {
        var state_unobservables = snapshot.Unobservables;
        if (state_unobservables != null) {
          var bodies = state_unobservables.Bodies;

          droid.Runtime.Messaging.FBS.FUnobservables.StartBodiesVector(builder : b, numElems : bodies.Length);
          for (var index = 0; index < bodies.Length; index++) {
            var rig = bodies[index];
            var vel = rig.Velocity;
            var ang = rig.AngularVelocity;
            droid.Runtime.Messaging.FBS.FBody.CreateFBody(builder : b,
                                                          velocity_X : vel.x,
                                                          velocity_Y : vel.y,
                                                          velocity_Z : vel.z,
                                                          angular_velocity_X : ang.x,
                                                          angular_velocity_Y : ang.y,
                                                          angular_velocity_Z : ang.z);
          }

          var bodies_vector = b.EndVector();

          var poses = state_unobservables.Poses;

          droid.Runtime.Messaging.FBS.FUnobservables.StartPosesVector(builder : b, numElems : poses.Length);
          for (var index = 0; index < poses.Length; index++) {
            var tra = poses[index];
            var pos = tra.position;
            var rot = tra.rotation;
            droid.Runtime.Messaging.FBS.FQuaternionTransform.CreateFQuaternionTransform(builder : b,
              position_X : pos.x,
              position_Y : pos.y,
              position_Z : pos.z,
              rotation_X : rot.x,
              rotation_Y : rot.y,
              rotation_Z : rot.z,
              rotation_W : rot.w);
          }

          var poses_vector = b.EndVector();

          droid.Runtime.Messaging.FBS.FUnobservables.StartFUnobservables(builder : b);
          droid.Runtime.Messaging.FBS.FUnobservables.AddPoses(builder : b, posesOffset : poses_vector);
          droid.Runtime.Messaging.FBS.FUnobservables.AddBodies(builder : b, bodiesOffset : bodies_vector);
          unobservables = droid.Runtime.Messaging.FBS.FUnobservables.EndFUnobservables(builder : b);
        }
      }

      var description_offset = new FlatBuffers.Offset<droid.Runtime.Messaging.FBS.FEnvironmentDescription>();
      if (snapshot.Description != null) {
        description_offset = Serialise(b : b, snapshot : snapshot);
      }

      var d = new FlatBuffers.StringOffset();
      if (snapshot.DebugMessage != "") {
        d = b.CreateString(s : snapshot.DebugMessage);
      }

      var t = b.CreateString(s : snapshot.TerminationReason);

      droid.Runtime.Messaging.FBS.FState.StartFState(builder : b);
      droid.Runtime.Messaging.FBS.FState.AddEnvironmentName(builder : b, environmentNameOffset : n);

      droid.Runtime.Messaging.FBS.FState.AddFrameNumber(builder : b, frameNumber : snapshot.FrameNumber);
      if (do_serialise_aggregated_float_array) {
        droid.Runtime.Messaging.FBS.FState.AddObservables(builder : b,
                                                          observablesOffset : observables_vector);
      }

      if (do_serialise_unobservables) {
        droid.Runtime.Messaging.FBS.FState.AddUnobservables(builder : b, unobservablesOffset : unobservables);
      }

      droid.Runtime.Messaging.FBS.FState.AddSignal(builder : b, signal : snapshot.Signal);

      droid.Runtime.Messaging.FBS.FState.AddTerminated(builder : b, terminated : snapshot.Terminated);
      droid.Runtime.Messaging.FBS.FState.AddTerminationReason(builder : b, terminationReasonOffset : t);

      if (snapshot.Description != null) {
        droid.Runtime.Messaging.FBS.FState.AddEnvironmentDescription(builder : b,
                                                                     environmentDescriptionOffset :
                                                                     description_offset);
      }

      if (snapshot.DebugMessage != "") {
        droid.Runtime.Messaging.FBS.FState.AddExtraSerialisedMessage(builder : b,
                                                                     extraSerialisedMessageOffset : d);
      }

      return droid.Runtime.Messaging.FBS.FState.EndFState(builder : b);
    }

    static FlatBuffers.Offset<droid.Runtime.Messaging.FBS.FActuator> Serialise(
        FlatBuffers.FlatBufferBuilder b,
        droid.Runtime.Interfaces.IActuator actuator,
        string identifier) {
      var n = b.CreateString(s : identifier);
      droid.Runtime.Messaging.FBS.FActuator.StartFActuator(builder : b);
      droid.Runtime.Messaging.FBS.FActuator.AddActuatorName(builder : b, actuatorNameOffset : n);
      droid.Runtime.Messaging.FBS.FActuator.AddActuatorRange(builder : b,
                                                             actuatorRangeOffset :
                                                             droid.Runtime.Messaging.FBS.FRange
                                                                  .CreateFRange(builder : b,
                                                                    DecimalGranularity :
                                                                    actuator.MotionSpace
                                                                        .DecimalGranularity,
                                                                    MaxValue : actuator.MotionSpace.Max,
                                                                    MinValue : actuator.MotionSpace.Min,
                                                                    Normalised : actuator.MotionSpace
                                                                        .NormalisedBool));
      return droid.Runtime.Messaging.FBS.FActuator.EndFActuator(builder : b);
    }

    /// <summary>
    /// </summary>
    /// <param name="b"></param>
    /// <param name="sensor"></param>
    /// <returns></returns>
    static FlatBuffers.Offset<droid.Runtime.Messaging.FBS.FETObs> Serialise(
        FlatBuffers.FlatBufferBuilder b,
        droid.Runtime.Interfaces.IHasEulerTransform sensor) {
      droid.Runtime.Messaging.FBS.FETObs.StartFETObs(builder : b);
      UnityEngine.Vector3 pos = sensor.Position, rot = sensor.Rotation, dir = sensor.Direction;
      droid.Runtime.Messaging.FBS.FETObs.AddTransform(builder : b,
                                                      transformOffset :
                                                      droid.Runtime.Messaging.FBS.FEulerTransform
                                                           .CreateFEulerTransform(builder : b,
                                                             position_X : pos.x,
                                                             position_Y : pos.y,
                                                             position_Z : pos.z,
                                                             rotation_X : rot.x,
                                                             rotation_Y : rot.y,
                                                             rotation_Z : rot.z,
                                                             direction_X : dir.x,
                                                             direction_Y : dir.y,
                                                             direction_Z : dir.z));

      return droid.Runtime.Messaging.FBS.FETObs.EndFETObs(builder : b);
    }

    /// <summary>
    /// </summary>
    /// <param name="b"></param>
    /// <param name="sensor"></param>
    /// <returns></returns>
    static FlatBuffers.Offset<droid.Runtime.Messaging.FBS.FQTObs> Serialise(
        FlatBuffers.FlatBufferBuilder b,
        droid.Runtime.Interfaces.IHasQuaternionTransform sensor) {
      var pos = sensor.Position;
      var rot = sensor.Rotation;
      var pos_range = sensor.PositionSpace;
      var rot_range = sensor.RotationSpace;
      droid.Runtime.Messaging.FBS.FQTObs.StartFQTObs(builder : b);
      droid.Runtime.Messaging.FBS.FQTObs.AddPosRange(builder : b,
                                                     posRangeOffset :
                                                     droid.Runtime.Messaging.FBS.FRange.CreateFRange(builder :
                                                       b,
                                                       DecimalGranularity : pos_range.DecimalGranularity,
                                                       MaxValue : pos_range.Max,
                                                       MinValue : pos_range.Min,
                                                       Normalised : pos_range.NormalisedBool));
      droid.Runtime.Messaging.FBS.FQTObs.AddRotRange(builder : b,
                                                     rotRangeOffset :
                                                     droid.Runtime.Messaging.FBS.FRange.CreateFRange(builder :
                                                       b,
                                                       DecimalGranularity : rot_range.DecimalGranularity,
                                                       MaxValue : rot_range.Max,
                                                       MinValue : rot_range.Min,
                                                       Normalised : rot_range.NormalisedBool));
      droid.Runtime.Messaging.FBS.FQTObs.AddTransform(builder : b,
                                                      transformOffset : droid.Runtime.Messaging.FBS
                                                          .FQuaternionTransform
                                                          .CreateFQuaternionTransform(builder : b,
                                                            position_X : pos.x,
                                                            position_Y : pos.y,
                                                            position_Z : pos.z,
                                                            rotation_X : rot.x,
                                                            rotation_Y : rot.y,
                                                            rotation_Z : rot.z,
                                                            rotation_W : rot.w));

      return droid.Runtime.Messaging.FBS.FQTObs.EndFQTObs(builder : b);
    }

    static FlatBuffers.Offset<droid.Runtime.Messaging.FBS.FByteArray> Serialise(
        FlatBuffers.FlatBufferBuilder b,
        droid.Runtime.Interfaces.IHasByteArray sensor) {
      var v_offset =
          droid.Runtime.Messaging.FBS.FByteArray.CreateBytesVectorBlock(builder : b, data : sensor.Bytes);
      //var v_offset = CustomFlatBufferImplementation.CreateByteVector(b, camera.Bytes);
      droid.Runtime.Messaging.FBS.FByteDataType a;
      switch (sensor.ArrayEncoding) {
        case "UINT8":
          a = droid.Runtime.Messaging.FBS.FByteDataType.UINT8;
          break;
        case "FLOAT16":
          a = droid.Runtime.Messaging.FBS.FByteDataType.FLOAT16;
          break;
        case "FLOAT32":
          a = droid.Runtime.Messaging.FBS.FByteDataType.FLOAT32;
          break;
        case "JPEG":
          a = droid.Runtime.Messaging.FBS.FByteDataType.JPEG;
          break;
        case "PNG":
          a = droid.Runtime.Messaging.FBS.FByteDataType.PNG;
          break;
        default:
          a = droid.Runtime.Messaging.FBS.FByteDataType.Other;
          break;
      }

      var c = droid.Runtime.Messaging.FBS.FByteArray.CreateShapeVector(builder : b, data : sensor.Shape);

      droid.Runtime.Messaging.FBS.FByteArray.StartFByteArray(builder : b);
      droid.Runtime.Messaging.FBS.FByteArray.AddType(builder : b, type : a);
      droid.Runtime.Messaging.FBS.FByteArray.AddShape(builder : b, shapeOffset : c);
      droid.Runtime.Messaging.FBS.FByteArray.AddBytes(builder : b, bytesOffset : v_offset);
      return droid.Runtime.Messaging.FBS.FByteArray.EndFByteArray(builder : b);
    }

    static FlatBuffers.Offset<droid.Runtime.Messaging.FBS.FArray> Serialise(
        FlatBuffers.FlatBufferBuilder b,
        droid.Runtime.Interfaces.IHasFloatArray float_a) {
      var v_offset =
          droid.Runtime.Messaging.FBS.FArray.CreateArrayVectorBlock(builder : b,
                                                                    data : float_a.ObservationArray);
      //var v_offset = CustomFlatBufferImplementation.CreateFloatVector(b, float_a.ObservationArray);

      droid.Runtime.Messaging.FBS.FArray.StartRangesVector(builder : b,
                                                           numElems : float_a.ObservationSpace.Length);
      for (var index = 0; index < float_a.ObservationSpace.Length; index++) {
        var tra = float_a.ObservationSpace[index];
        droid.Runtime.Messaging.FBS.FRange.CreateFRange(builder : b,
                                                        DecimalGranularity : tra.DecimalGranularity,
                                                        MaxValue : tra.Max,
                                                        MinValue : tra.Min,
                                                        Normalised : tra.NormalisedBool);
      }

      var ranges_vector = b.EndVector();

      droid.Runtime.Messaging.FBS.FArray.StartFArray(builder : b);
      droid.Runtime.Messaging.FBS.FArray.AddArray(builder : b, arrayOffset : v_offset);

      droid.Runtime.Messaging.FBS.FArray.AddRanges(builder : b, rangesOffset : ranges_vector);

      return droid.Runtime.Messaging.FBS.FArray.EndFArray(builder : b);
    }

    /// <summary>
    /// </summary>
    /// <param name="b"></param>
    /// <param name="rigidbody"></param>
    /// <returns></returns>
    static FlatBuffers.Offset<droid.Runtime.Messaging.FBS.FRBObs> Serialise(
        FlatBuffers.FlatBufferBuilder b,
        droid.Runtime.Interfaces.IHasRigidbody rigidbody) {
      droid.Runtime.Messaging.FBS.FRBObs.StartFRBObs(builder : b);
      var a = rigidbody.Velocity;
      var c = rigidbody.AngularVelocity;

      droid.Runtime.Messaging.FBS.FRBObs.AddBody(builder : b,
                                                 bodyOffset :
                                                 droid.Runtime.Messaging.FBS.FBody.CreateFBody(builder : b,
                                                   velocity_X : a.x,
                                                   velocity_Y : a.y,
                                                   velocity_Z : a.z,
                                                   angular_velocity_X : c.x,
                                                   angular_velocity_Y : c.y,
                                                   angular_velocity_Z : c.z));
      return droid.Runtime.Messaging.FBS.FRBObs.EndFRBObs(builder : b);
    }

    static FlatBuffers.Offset<droid.Runtime.Messaging.FBS.FSingle> Serialise(
        FlatBuffers.FlatBufferBuilder b,
        droid.Runtime.Interfaces.IHasSingle numeral) {
      droid.Runtime.Messaging.FBS.FSingle.StartFSingle(builder : b);
      droid.Runtime.Messaging.FBS.FSingle.AddValue(builder : b, value : numeral.ObservationValue);

      var range_offset = droid.Runtime.Messaging.FBS.FRange.CreateFRange(builder : b,
                                                                           DecimalGranularity :
                                                                           numeral.SingleSpace
                                                                               .DecimalGranularity,
                                                                           MaxValue : numeral.SingleSpace.Max,
                                                                           MinValue : numeral.SingleSpace.Min,
                                                                           Normalised : numeral.SingleSpace
                                                                               .NormalisedBool);
      droid.Runtime.Messaging.FBS.FSingle.AddRange(builder : b, rangeOffset : range_offset);
      return droid.Runtime.Messaging.FBS.FSingle.EndFSingle(builder : b);
    }

    static FlatBuffers.Offset<droid.Runtime.Messaging.FBS.FDouble> Serialise(
        FlatBuffers.FlatBufferBuilder b,
        droid.Runtime.Interfaces.IHasDouble numeral) {
      droid.Runtime.Messaging.FBS.FDouble.StartFDouble(builder : b);
      var vec2 = numeral.ObservationValue;

      var granularity = numeral.DoubleSpace.DecimalGranularity;
      var xs = numeral.DoubleSpace.Xspace;
      var ys = numeral.DoubleSpace.Yspace;

      droid.Runtime.Messaging.FBS.FDouble.AddXRange(builder : b,
                                                    xRangeOffset : droid.Runtime.Messaging.FBS.FRange
                                                                        .CreateFRange(builder : b,
                                                                          DecimalGranularity : granularity,
                                                                          MaxValue : xs.Max,
                                                                          MinValue : xs.Min,
                                                                          Normalised : xs.NormalisedBool));
      droid.Runtime.Messaging.FBS.FDouble.AddYRange(builder : b,
                                                    yRangeOffset : droid.Runtime.Messaging.FBS.FRange
                                                                        .CreateFRange(builder : b,
                                                                          DecimalGranularity : granularity,
                                                                          MaxValue : ys.Max,
                                                                          MinValue : ys.Min,
                                                                          Normalised : ys.NormalisedBool));
      droid.Runtime.Messaging.FBS.FDouble.AddVec2(builder : b,
                                                  vec2Offset : droid.Runtime.Messaging.FBS.FVector2
                                                                    .CreateFVector2(builder : b,
                                                                      X : vec2.x,
                                                                      Y : vec2.y));

      return droid.Runtime.Messaging.FBS.FDouble.EndFDouble(builder : b);
    }

    static FlatBuffers.Offset<droid.Runtime.Messaging.FBS.FTriple> Serialise(
        FlatBuffers.FlatBufferBuilder b,
        droid.Runtime.Interfaces.IHasTriple numeral) {
      droid.Runtime.Messaging.FBS.FTriple.StartFTriple(builder : b);
      var vec3 = numeral.ObservationValue;

      droid.Runtime.Messaging.FBS.FTriple.AddVec3(builder : b,
                                                  vec3Offset : droid.Runtime.Messaging.FBS.FVector3
                                                                    .CreateFVector3(builder : b,
                                                                      X : vec3.x,
                                                                      Y : vec3.y,
                                                                      Z : vec3.z));
      var granularity = numeral.TripleSpace.DecimalGranularity;
      var xs = numeral.TripleSpace.Xspace;
      var ys = numeral.TripleSpace.Yspace;
      var zs = numeral.TripleSpace.Zspace;
      droid.Runtime.Messaging.FBS.FTriple.AddXRange(builder : b,
                                                    xRangeOffset : droid.Runtime.Messaging.FBS.FRange
                                                                        .CreateFRange(builder : b,
                                                                          DecimalGranularity : granularity,
                                                                          MaxValue : xs.Max,
                                                                          MinValue : xs.Min,
                                                                          Normalised : xs.NormalisedBool));
      droid.Runtime.Messaging.FBS.FTriple.AddYRange(builder : b,
                                                    yRangeOffset : droid.Runtime.Messaging.FBS.FRange
                                                                        .CreateFRange(builder : b,
                                                                          DecimalGranularity : granularity,
                                                                          MaxValue : ys.Max,
                                                                          MinValue : ys.Min,
                                                                          Normalised : ys.NormalisedBool));
      droid.Runtime.Messaging.FBS.FTriple.AddZRange(builder : b,
                                                    zRangeOffset : droid.Runtime.Messaging.FBS.FRange
                                                                        .CreateFRange(builder : b,
                                                                          DecimalGranularity : granularity,
                                                                          MaxValue : zs.Max,
                                                                          MinValue : zs.Min,
                                                                          Normalised : zs.NormalisedBool));
      return droid.Runtime.Messaging.FBS.FTriple.EndFTriple(builder : b);
    }

    static FlatBuffers.Offset<droid.Runtime.Messaging.FBS.FQuadruple> Serialise(
        FlatBuffers.FlatBufferBuilder b,
        droid.Runtime.Interfaces.IHasQuadruple numeral) {
      droid.Runtime.Messaging.FBS.FQuadruple.StartFQuadruple(builder : b);
      var quad = numeral.ObservationValue;
      droid.Runtime.Messaging.FBS.FQuadruple.AddQuat(builder : b,
                                                     quatOffset : droid.Runtime.Messaging.FBS.FQuaternion
                                                                       .CreateFQuaternion(builder : b,
                                                                         X : quad.x,
                                                                         Y : quad.y,
                                                                         Z : quad.z,
                                                                         W : quad.z));
      var granularity = numeral.QuadSpace.DecimalGranularity;
      var xs = numeral.QuadSpace.Xspace;
      var ys = numeral.QuadSpace.Yspace;
      var zs = numeral.QuadSpace.Zspace;
      var ws = numeral.QuadSpace.Wspace;
      droid.Runtime.Messaging.FBS.FQuadruple.AddXRange(builder : b,
                                                       xRangeOffset : droid.Runtime.Messaging.FBS.FRange
                                                           .CreateFRange(builder : b,
                                                                           DecimalGranularity : granularity,
                                                                           MaxValue : xs.Max,
                                                                           MinValue : xs.Min,
                                                                           Normalised : xs.NormalisedBool));
      droid.Runtime.Messaging.FBS.FQuadruple.AddYRange(builder : b,
                                                       yRangeOffset : droid.Runtime.Messaging.FBS.FRange
                                                           .CreateFRange(builder : b,
                                                                           DecimalGranularity : granularity,
                                                                           MaxValue : ys.Max,
                                                                           MinValue : ys.Min,
                                                                           Normalised : ys.NormalisedBool));
      droid.Runtime.Messaging.FBS.FQuadruple.AddZRange(builder : b,
                                                       zRangeOffset : droid.Runtime.Messaging.FBS.FRange
                                                           .CreateFRange(builder : b,
                                                                           DecimalGranularity : granularity,
                                                                           MaxValue : zs.Max,
                                                                           MinValue : zs.Min,
                                                                           Normalised : zs.NormalisedBool));
      droid.Runtime.Messaging.FBS.FQuadruple.AddWRange(builder : b,
                                                       wRangeOffset : droid.Runtime.Messaging.FBS.FRange
                                                           .CreateFRange(builder : b,
                                                                           DecimalGranularity : granularity,
                                                                           MaxValue : ws.Max,
                                                                           MinValue : ws.Min,
                                                                           Normalised : ws.NormalisedBool));
      return droid.Runtime.Messaging.FBS.FQuadruple.EndFQuadruple(builder : b);
    }

    static FlatBuffers.Offset<droid.Runtime.Messaging.FBS.FString> Serialise(
        FlatBuffers.FlatBufferBuilder b,
        droid.Runtime.Interfaces.IHasString numeral) {
      var string_offset = b.CreateString(s : numeral.ObservationValue);
      droid.Runtime.Messaging.FBS.FString.StartFString(builder : b);
      droid.Runtime.Messaging.FBS.FString.AddStr(builder : b, strOffset : string_offset);

      return droid.Runtime.Messaging.FBS.FString.EndFString(builder : b);
    }

    static FlatBuffers.Offset<droid.Runtime.Messaging.FBS.FActor> Serialise(FlatBuffers.FlatBufferBuilder b,
      FlatBuffers.Offset<droid.Runtime.Messaging.FBS.FActuator>[] actuators,
      droid.Runtime.Interfaces.IActor actor,
      string identifier) {
      var n = b.CreateString(s : identifier);
      var actuator_vector =
          droid.Runtime.Messaging.FBS.FActor.CreateActuatorsVector(builder : b, data : actuators);
      droid.Runtime.Messaging.FBS.FActor.StartFActor(builder : b);
      if (actor is droid.Runtime.Prototyping.Actors.KillableActor) {
        droid.Runtime.Messaging.FBS.FActor.AddAlive(builder : b,
                                                    alive : ((droid.Runtime.Prototyping.Actors.KillableActor)
                                                              actor).IsAlive);
      } else {
        droid.Runtime.Messaging.FBS.FActor.AddAlive(builder : b, true);
      }

      droid.Runtime.Messaging.FBS.FActor.AddActorName(builder : b, actorNameOffset : n);
      droid.Runtime.Messaging.FBS.FActor.AddActuators(builder : b, actuatorsOffset : actuator_vector);
      return droid.Runtime.Messaging.FBS.FActor.EndFActor(builder : b);
    }

    static FlatBuffers.Offset<droid.Runtime.Messaging.FBS.FSensor> Serialise(
        FlatBuffers.FlatBufferBuilder b,
        string identifier,
        droid.Runtime.Interfaces.ISensor sensor) {
      var n = b.CreateString(s : identifier);

      int observation_offset;
      droid.Runtime.Messaging.FBS.FObservation observation_type;
      switch (sensor) {
        case droid.Runtime.Interfaces.IHasString numeral:
          observation_offset = Serialise(b : b, numeral : numeral).Value;
          observation_type = droid.Runtime.Messaging.FBS.FObservation.FString;
          break;
        case droid.Runtime.Interfaces.IHasFloatArray a:
          observation_offset = Serialise(b : b, float_a : a).Value;
          observation_type = droid.Runtime.Messaging.FBS.FObservation.FArray;
          break;
        case droid.Runtime.Interfaces.IHasSingle single:
          observation_offset = Serialise(b : b, numeral : single).Value;
          observation_type = droid.Runtime.Messaging.FBS.FObservation.FSingle;
          break;
        case droid.Runtime.Interfaces.IHasDouble has_double:
          observation_offset = Serialise(b : b, numeral : has_double).Value;
          observation_type = droid.Runtime.Messaging.FBS.FObservation.FDouble;
          break;
        case droid.Runtime.Interfaces.IHasTriple triple:
          observation_offset = Serialise(b : b, numeral : triple).Value;
          observation_type = droid.Runtime.Messaging.FBS.FObservation.FTriple;
          break;
        case droid.Runtime.Interfaces.IHasQuadruple quadruple:
          observation_offset = Serialise(b : b, numeral : quadruple).Value;
          observation_type = droid.Runtime.Messaging.FBS.FObservation.FQuadruple;
          break;
        case droid.Runtime.Interfaces.IHasEulerTransform transform:
          observation_offset = Serialise(b : b, sensor : transform).Value;
          observation_type = droid.Runtime.Messaging.FBS.FObservation.FETObs;
          break;
        case droid.Runtime.Interfaces.IHasQuaternionTransform quaternion_transform:
          observation_offset = Serialise(b : b, sensor : quaternion_transform).Value;
          observation_type = droid.Runtime.Messaging.FBS.FObservation.FQTObs;
          break;
        case droid.Runtime.Interfaces.IHasRigidbody rigidbody:
          observation_offset = Serialise(b : b, rigidbody : rigidbody).Value;
          observation_type = droid.Runtime.Messaging.FBS.FObservation.FRBObs;
          break;
        case droid.Runtime.Interfaces.IHasByteArray array:
          observation_offset = Serialise(b : b, sensor : array).Value;
          observation_type = droid.Runtime.Messaging.FBS.FObservation.FByteArray;
          break;
        default:
          return droid.Runtime.Messaging.FBS.FSensor.CreateFSensor(builder : b, sensor_nameOffset : n);
      }

      droid.Runtime.Messaging.FBS.FSensor.StartFSensor(builder : b);
      droid.Runtime.Messaging.FBS.FSensor.AddSensorName(builder : b, sensorNameOffset : n);
      droid.Runtime.Messaging.FBS.FSensor.AddSensorValueType(builder : b, sensorValueType : observation_type);
      droid.Runtime.Messaging.FBS.FSensor.AddSensorValue(builder : b, sensorValueOffset : observation_offset);
      return droid.Runtime.Messaging.FBS.FSensor.EndFSensor(builder : b);
    }

    static FlatBuffers.Offset<droid.Runtime.Messaging.FBS.FEnvironmentDescription> Serialise(
        FlatBuffers.FlatBufferBuilder b,
        droid.Runtime.Messaging.Messages.EnvironmentSnapshot snapshot) {
      var actors_offsets =
          new FlatBuffers.Offset<droid.Runtime.Messaging.FBS.FActor>[snapshot.Description.Actors.Values
              .Count];
      var j = 0;
      foreach (var actor in snapshot.Description.Actors) {
        var actuators_offsets =
            new FlatBuffers.Offset<droid.Runtime.Messaging.FBS.FActuator>[actor.Value.Actuators.Values.Count];
        var i = 0;
        foreach (var actuator in actor.Value.Actuators) {
          actuators_offsets[i++] = Serialise(b : b, actuator : actuator.Value, identifier : actuator.Key);
        }

        actors_offsets[j++] = Serialise(b : b,
                                        actuators : actuators_offsets,
                                        actor : actor.Value,
                                        identifier : actor.Key);
      }

      var actors_vector_offset =
          droid.Runtime.Messaging.FBS.FEnvironmentDescription.CreateActorsVector(builder : b,
            data : actors_offsets);

      var configurables_offsets =
          new FlatBuffers.Offset<droid.Runtime.Messaging.FBS.FConfigurable>[snapshot.Description.Configurables
              .Values.Count];
      var k = 0;
      foreach (var configurable in snapshot.Description.Configurables) {
        configurables_offsets[k++] =
            Serialise(b : b, configurable : configurable.Value, identifier : configurable.Key);
      }

      var configurables_vector_offset =
          droid.Runtime.Messaging.FBS.FEnvironmentDescription.CreateConfigurablesVector(builder : b,
            data : configurables_offsets);

      var objective_offset = Serialise(b : b, description : snapshot.Description);

      var sensors =
          new FlatBuffers.Offset<droid.Runtime.Messaging.FBS.FSensor>[snapshot.Description.Sensors.Values
              .Count];
      var js = 0;
      foreach (var sensor in snapshot.Description.Sensors) {
        sensors[js++] = Serialise(b : b, identifier : sensor.Key, sensor : sensor.Value);
      }

      var sensors_vector =
          droid.Runtime.Messaging.FBS.FEnvironmentDescription
               .CreateSensorsVector(builder : b, data : sensors);

      droid.Runtime.Messaging.FBS.FEnvironmentDescription.StartFEnvironmentDescription(builder : b);

      droid.Runtime.Messaging.FBS.FEnvironmentDescription.AddObjective(builder : b,
                                                                       objectiveOffset : objective_offset);
      droid.Runtime.Messaging.FBS.FEnvironmentDescription.AddActors(builder : b,
                                                                    actorsOffset : actors_vector_offset);
      droid.Runtime.Messaging.FBS.FEnvironmentDescription.AddConfigurables(builder : b,
        configurablesOffset : configurables_vector_offset);
      droid.Runtime.Messaging.FBS.FEnvironmentDescription.AddSensors(builder : b,
                                                                     sensorsOffset : sensors_vector);

      return droid.Runtime.Messaging.FBS.FEnvironmentDescription.EndFEnvironmentDescription(builder : b);
    }

    static FlatBuffers.Offset<droid.Runtime.Messaging.FBS.FObjective> Serialise(
        FlatBuffers.FlatBufferBuilder b,
        droid.Runtime.Messaging.Messages.EnvironmentDescription description) {
      var ob_name = "None";
      var ep_len = -1;
      var a = 0;
      var f = 0f;
      var c = 0f;
      var d = false;
      if (description.ObjectiveFunction != null) {
        ob_name = description.ObjectiveFunction.Identifier;
        ep_len = description.ObjectiveFunction.EpisodeLength;
        f = description.ObjectiveFunction.SignalSpace.Min;
        c = description.ObjectiveFunction.SignalSpace.Max;
        a = description.ObjectiveFunction.SignalSpace.DecimalGranularity;
        d = description.ObjectiveFunction.SignalSpace.NormalisedBool;
      }

      var objective_name_offset = b.CreateString(s : ob_name);
      droid.Runtime.Messaging.FBS.FObjective.StartFObjective(builder : b);
      droid.Runtime.Messaging.FBS.FObjective.AddMaxEpisodeLength(builder : b, maxEpisodeLength : ep_len);
      droid.Runtime.Messaging.FBS.FObjective.AddSignalSpace(builder : b,
                                                            signalSpaceOffset : droid.Runtime.Messaging.FBS
                                                                .FRange.CreateFRange(builder : b,
                                                                  DecimalGranularity : a,
                                                                  MaxValue : f,
                                                                  MinValue : c,
                                                                  Normalised : d));
      droid.Runtime.Messaging.FBS.FObjective.AddObjectiveName(builder : b,
                                                              objectiveNameOffset : objective_name_offset);
      return droid.Runtime.Messaging.FBS.FObjective.EndFObjective(builder : b);
    }

    static FlatBuffers.Offset<droid.Runtime.Messaging.FBS.FTriple> Serialise(
        FlatBuffers.FlatBufferBuilder b,
        droid.Runtime.Prototyping.Configurables.Transforms.PositionConfigurable sensor) {
      var pos = sensor.ObservationValue;
      droid.Runtime.Messaging.FBS.FTriple.StartFTriple(builder : b);
      droid.Runtime.Messaging.FBS.FTriple.AddVec3(builder : b,
                                                  vec3Offset : droid.Runtime.Messaging.FBS.FVector3
                                                                    .CreateFVector3(builder : b,
                                                                      X : pos.x,
                                                                      Y : pos.y,
                                                                      Z : pos.z));
      return droid.Runtime.Messaging.FBS.FTriple.EndFTriple(builder : b);
    }

    static FlatBuffers.Offset<droid.Runtime.Messaging.FBS.FConfigurable> Serialise(
        FlatBuffers.FlatBufferBuilder b,
        droid.Runtime.Interfaces.IConfigurable configurable,
        string identifier) {
      var n = b.CreateString(s : identifier);

      int observation_offset;
      droid.Runtime.Messaging.FBS.FObservation observation_type;

      if (configurable is droid.Runtime.Interfaces.IHasQuaternionTransform) {
        observation_offset =
            Serialise(b : b, sensor : (droid.Runtime.Interfaces.IHasQuaternionTransform)configurable).Value;
        observation_type = droid.Runtime.Messaging.FBS.FObservation.FQTObs;
      } else if (configurable is droid.Runtime.Prototyping.Configurables.Transforms.PositionConfigurable) {
        observation_offset = Serialise(b : b,
                                       sensor : (droid.Runtime.Prototyping.Configurables.Transforms.
                                           PositionConfigurable)configurable).Value;
        observation_type = droid.Runtime.Messaging.FBS.FObservation.FTriple;
      } else if (configurable is droid.Runtime.Interfaces.IHasSingle) {
        observation_offset =
            Serialise(b : b, numeral : (droid.Runtime.Interfaces.IHasSingle)configurable).Value;
        observation_type = droid.Runtime.Messaging.FBS.FObservation.FSingle;
        // ReSharper disable once SuspiciousTypeConversion.Global
      } else if (configurable is droid.Runtime.Interfaces.IHasDouble) {
        // ReSharper disable once SuspiciousTypeConversion.Global
        observation_offset =
            Serialise(b : b, numeral : (droid.Runtime.Interfaces.IHasDouble)configurable).Value;
        observation_type = droid.Runtime.Messaging.FBS.FObservation.FDouble;
      } else if (configurable is droid.Runtime.Prototyping.Configurables.Transforms.EulerTransformConfigurable
      ) {
        observation_offset =
            Serialise(b : b, sensor : (droid.Runtime.Interfaces.IHasEulerTransform)configurable).Value;
        observation_type = droid.Runtime.Messaging.FBS.FObservation.FETObs;
      } else {
        droid.Runtime.Messaging.FBS.FConfigurable.StartFConfigurable(builder : b);
        droid.Runtime.Messaging.FBS.FConfigurable.AddConfigurableName(builder : b,
                                                                      configurableNameOffset : n);
        return droid.Runtime.Messaging.FBS.FConfigurable.EndFConfigurable(builder : b);
      }

      droid.Runtime.Messaging.FBS.FConfigurable.StartFConfigurable(builder : b);
      droid.Runtime.Messaging.FBS.FConfigurable.AddConfigurableName(builder : b, configurableNameOffset : n);
      droid.Runtime.Messaging.FBS.FConfigurable.AddConfigurableValue(builder : b,
                                                                     configurableValueOffset :
                                                                     observation_offset);
      droid.Runtime.Messaging.FBS.FConfigurable.AddConfigurableValueType(builder : b,
                                                                           configurableValueType :
                                                                           observation_type);
      droid.Runtime.Messaging.FBS.FConfigurable.AddConfigurableRange(builder : b,
                                                                     configurableRangeOffset : droid.Runtime
                                                                         .Messaging.FBS.FRange
                                                                         .CreateFRange(builder : b,
                                                                           0,
                                                                           0,
                                                                           0,
                                                                           false));
      return droid.Runtime.Messaging.FBS.FConfigurable.EndFConfigurable(builder : b);
    }

    #endregion
  }
}