namespace droid.Runtime.Prototyping.Sensors.Spatial.EntityCentric {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                                           + "Compass"
                                           + SensorComponentMenuPath._Postfix)]
  [UnityEngine.ExecuteInEditMode]
  [System.SerializableAttribute]
  public class TargetDirectionSensor : Sensor,
                                       droid.Runtime.Interfaces.IHasDouble {
    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    UnityEngine.Vector2 _2_d_position = UnityEngine.Vector2.zero;

    /// <summary>
    /// </summary>
    [UnityEngine.HeaderAttribute("Observation", order = 103)]
    [UnityEngine.SerializeField]
    UnityEngine.Vector3 _position = UnityEngine.Vector3.zero;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Space3 _position_space =
        new droid.Runtime.Structs.Space.Space3 {
                                                   DecimalGranularity = 1,
                                                   Max = UnityEngine.Vector3.one,
                                                   Min = -UnityEngine.Vector3.one
                                               };

    /// <summary>
    /// </summary>
    [UnityEngine.HeaderAttribute("Specific", order = 102)]
    [UnityEngine.SerializeField]
    UnityEngine.Transform _target = null;

    /// <summary>
    /// </summary>
    public UnityEngine.Vector3 Position {
      get { return this._position; }
      set {
        this._position = this._position_space.Project(v : value);
        this._2_d_position = new UnityEngine.Vector2(x : this._position.x, y : this._position.z);
      }
    }

    public override System.Collections.Generic.IEnumerable<float> FloatEnumerable {
      get {
        yield return this.Position.x;
        yield return this.Position.z;
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public droid.Runtime.Structs.Space.Space2 DoubleSpace {
      get {
        return new droid.Runtime.Structs.Space.Space2 {
                                                          Max =
                                                              new UnityEngine.Vector2(x : this._position_space
                                                                    .Max.x,
                                                                y : this._position_space.Max.y),
                                                          Min =
                                                              new UnityEngine.Vector2(x : this._position_space
                                                                    .Min.x,
                                                                y : this._position_space.Min.y),
                                                          DecimalGranularity = this._position_space
                                                              .DecimalGranularity
                                                      };
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public UnityEngine.Vector2 ObservationValue {
      get { return this._2_d_position; }
      set { this._2_d_position = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      this.Position = this.transform
                          .InverseTransformVector(vector : this.transform.position - this._target.position)
                          .normalized;
    }
  }
}