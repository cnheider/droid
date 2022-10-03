namespace droid.Runtime.Prototyping.Sensors.Spatial.EntityCentric {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                                           + "Angle"
                                           + SensorComponentMenuPath._Postfix)]
  [UnityEngine.ExecuteInEditMode]
  public class AngleSensor : droid.Runtime.Prototyping.Sensors.Experimental.SingleValueSensor {
    [UnityEngine.SerializeField] UnityEngine.Vector3 reference = -UnityEngine.Vector3.forward;
    [UnityEngine.SerializeField] UnityEngine.Vector3 axis = UnityEngine.Vector3.up;

    void Reset() {
      this._observation_value_space = new droid.Runtime.Structs.Space.Space1 {
                                          DecimalGranularity = 4,
                                          Max = 180.0f,
                                          Min = -180.0f,
                                          Normalised = droid.Runtime.Enums.ProjectionEnum.None_,
                                          NormalisedBool = false
                                      };
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <exception cref="T:System.ArgumentOutOfRangeException"></exception>
    public override void UpdateObservation() {
      float val;
      if (false) {
        val = UnityEngine.Vector3.Dot(lhs : this.transform.TransformVector(vector : this.reference),
                                      rhs : this.reference);
      }

      var t = this.transform.TransformDirection(direction : this.reference);
      //var axis = Vector3.Cross(t, this.reference);
      val = UnityEngine.Vector3.SignedAngle(from : t, to : this.reference, axis : this.axis);

      this.ObservationValue = val;
    }
  }
}