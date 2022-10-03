namespace droid.Runtime.Prototyping.Sensors.Spatial.EntityCentric.Rays {
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                                           + "Raycast"
                                           + SensorComponentMenuPath._Postfix)]
  [UnityEngine.ExecuteInEditMode]
  public class RaycastSensor : Sensor,
                               droid.Runtime.Interfaces.IHasSingle {
    [UnityEngine.SerializeField] UnityEngine.Vector3 _direction = UnityEngine.Vector3.forward;

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Space1 _observation_space =
        new droid.Runtime.Structs.Space.Space1 {DecimalGranularity = 3, Min = 0f, Max = 100.0f};

    [UnityEngine.HeaderAttribute("Observation", order = 103)]
    [UnityEngine.SerializeField]
    float _observation_value = 0;

    [UnityEngine.SerializeField] UnityEngine.RaycastHit _hit = new UnityEngine.RaycastHit();

    /// <summary>
    /// </summary>
    public override string PrototypingTypeName {
      get { return "Raycast" + $"{this._direction.x}{this._direction.y}{this._direction.z}"; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override System.Collections.Generic.IEnumerable<float> FloatEnumerable {
      get { yield return this.ObservationValue; }
    }

    /// <summary>
    /// </summary>
    public droid.Runtime.Structs.Space.Space1 SingleSpace { get { return this._observation_space; } }

    /// <summary>
    /// </summary>
    public float ObservationValue {
      get { return this._observation_value; }
      private set { this._observation_value = this.SingleSpace.Project(v : value); }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      if (UnityEngine.Physics.Raycast(origin : this.transform.position,
                                      direction :
                                      this.transform.TransformDirection(direction : this._direction),
                                      hitInfo : out this._hit,
                                      maxDistance : this._observation_space.Max)) {
        this.ObservationValue = this._hit.distance;
      } else {
        this.ObservationValue = this._observation_space.Max;
      }
      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log(message : $"Raycast hit at distance {this._hit.distance}");
      }
      #endif
    }

    #if UNITY_EDITOR
    [UnityEngine.SerializeField] UnityEngine.Color _color = UnityEngine.Color.green;

    void OnDrawGizmosSelected() {
      if (this.enabled) {
        var position = this.transform.position;
        UnityEngine.Debug.DrawLine(start : position,
                                   end : position
                                         + this.transform.TransformDirection(direction : this._direction)
                                         * this._observation_space.Max,
                                   color : this._color);
      }
    }
    #endif
  }
}