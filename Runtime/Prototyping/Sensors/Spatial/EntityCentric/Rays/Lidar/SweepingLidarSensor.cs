namespace droid.Runtime.Prototyping.Sensors.Spatial.EntityCentric.Rays.Lidar {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                                           + "SweepingLidar"
                                           + SensorComponentMenuPath._Postfix)]
  public class SweepingLidarSensor : Sensor,
                                     droid.Runtime.Interfaces.IHasSingle {
    [UnityEngine.SerializeField] UnityEngine.Vector3 current_direction = UnityEngine.Vector3.forward;

    [UnityEngine.HeaderAttribute("Observation", order = 103)]
    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Space1 _ray_space =
        new droid.Runtime.Structs.Space.Space1 {Min = 0, Max = 5f, DecimalGranularity = 2};

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Space1 _sweeping_space =
        new droid.Runtime.Structs.Space.Space1 {Min = -180, Max = 180, DecimalGranularity = 5};

    [UnityEngine.SerializeField]
    droid.Runtime.Enums.AxisEnum _sweeping_axisEnum = droid.Runtime.Enums.AxisEnum.Rot_y_;

    [UnityEngine.SerializeField] int tick_i;

    [UnityEngine.RangeAttribute(0.001f, 999)]
    [UnityEngine.SerializeField]
    float speed = 1;

    /// <summary>
    ///   Does not use the defined sweep space
    /// </summary>
    [UnityEngine.SerializeField]
    bool loop = false;

    [UnityEngine.SerializeField] UnityEngine.RaycastHit _hit;

    /// <summary>
    /// </summary>
    public override System.Collections.Generic.IEnumerable<float> FloatEnumerable {
      get { yield return this.ObservationValue; }
    }

    /// <summary>
    /// </summary>
    [field : UnityEngine.SerializeField]
    public float ObservationValue { get; set; }

    public droid.Runtime.Structs.Space.Space1 SingleSpace { get { return this._ray_space; } }

    /// <summary>
    /// </summary>
    public override void PreSetup() { this.PrototypingReset(); }

    public override void PrototypingReset() { this.tick_i = 0; }

    /// <summary>
    /// </summary>
    /// <exception cref="System.ArgumentOutOfRangeException"></exception>
    public override void Tick() {
      var c = UnityEngine.Vector3.forward;
      float a = this._sweeping_space.Precision * this.tick_i * this.speed;
      if (!this.loop) {
        a = this._sweeping_space.Reproject(v : UnityEngine.Mathf.Cos(f : a) * .5f + .5f);
      }

      switch (this._sweeping_axisEnum) {
        case droid.Runtime.Enums.AxisEnum.Rot_x_:
          c = UnityEngine.Quaternion.Euler(x : a, 0, 0) * c;
          break;
        case droid.Runtime.Enums.AxisEnum.Rot_y_:
          c = UnityEngine.Quaternion.Euler(0, y : a, 0) * c;
          break;

        default: throw new System.ArgumentOutOfRangeException();
      }

      this.current_direction = c;

      this.tick_i += 1;
    }

    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      if (UnityEngine.Physics.Raycast(origin : this.transform.position
                                               + this._ray_space.Min * this.current_direction,
                                      direction :
                                      this.transform.TransformDirection(direction : this.current_direction),
                                      hitInfo : out this._hit,
                                      maxDistance : this._ray_space.Max)) {
        this.ObservationValue = this._ray_space.Project(v : this._hit.distance);
      } else {
        this.ObservationValue = this._ray_space.Max;
      }
    }

    #if UNITY_EDITOR
    [UnityEngine.SerializeField] UnityEngine.Color _color = UnityEngine.Color.green;

    void OnDrawGizmosSelected() {
      if (this.enabled) {
        var position = this.transform.position;
        UnityEngine.Debug.DrawLine(start : position,
                                   end : position
                                         + this.transform.TransformDirection(direction : this
                                             .current_direction)
                                         * this._ray_space.Max,
                                   color : this._color);
      }
    }
    #endif
  }
}