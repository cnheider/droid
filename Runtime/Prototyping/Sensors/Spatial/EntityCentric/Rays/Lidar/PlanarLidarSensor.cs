namespace droid.Runtime.Prototyping.Sensors.Spatial.EntityCentric.Rays.Lidar {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class PlanarLidarSensor : Sensor,
                                   droid.Runtime.Interfaces.IHasFloatArray {
    [UnityEngine.SerializeField]
    [UnityEngine.RangeAttribute(1, 90)]
    int RaysToShoot = 30;

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Space1 _observation_space = new droid.Runtime.Structs.Space.Space1 {Max = 30};

    [UnityEngine.SerializeField] droid.Runtime.Enums.AxisEnum _axisEnum = droid.Runtime.Enums.AxisEnum.Y_;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override System.Collections.Generic.IEnumerable<float> FloatEnumerable {
      get { return this.ObservationArray; }
    }

    void OnDrawGizmosSelected() {
      float angle = 0;
      for (var i = 0; i < this.RaysToShoot; i++) {
        var x = UnityEngine.Mathf.Sin(f : angle) * this._observation_space.Max;
        var y = UnityEngine.Mathf.Cos(f : angle) * this._observation_space.Max;

        angle += 2 * UnityEngine.Mathf.PI / this.RaysToShoot;

        var position = this.transform.position;
        var dir = new UnityEngine.Vector3(x : position.x + x, y : position.y + y, z : 0);
        if (this._axisEnum == droid.Runtime.Enums.AxisEnum.Y_) {
          dir = new UnityEngine.Vector3(x : position.x + x, y : 0, z : position.z + y);
        } else if (this._axisEnum == droid.Runtime.Enums.AxisEnum.X_) {
          dir = new UnityEngine.Vector3(0, y : position.y + y, z : position.z + x);
        }

        UnityEngine.Debug.DrawLine(start : position, end : dir, color : UnityEngine.Color.red);
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [field : UnityEngine.SerializeField]
    public float[] ObservationArray { get; private set; }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public droid.Runtime.Structs.Space.Space1[] ObservationSpace {
      get { return new[] {this._observation_space}; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      var res = new float[this.RaysToShoot];
      float angle = 0;
      for (var i = 0; i < this.RaysToShoot; i++) {
        var x = UnityEngine.Mathf.Sin(f : angle);
        var y = UnityEngine.Mathf.Cos(f : angle);
        angle += 2 * UnityEngine.Mathf.PI / this.RaysToShoot;

        var position = this.transform.position;

        var dir = new UnityEngine.Vector3(x : position.x + x, y : position.y + y, 0);
        if (this._axisEnum == droid.Runtime.Enums.AxisEnum.Y_) {
          dir = new UnityEngine.Vector3(x : position.x + x, 0, z : position.z + y);
        } else if (this._axisEnum == droid.Runtime.Enums.AxisEnum.X_) {
          dir = new UnityEngine.Vector3(0, y : position.y + y, z : position.z + x);
        }

        UnityEngine.Debug.DrawLine(start : position, end : dir, color : UnityEngine.Color.red);
        if (UnityEngine.Physics.Raycast(origin : this.transform.position,
                                        direction : dir,
                                        hitInfo : out var a,
                                        maxDistance : (float)this._observation_space.Max)) {
          res[i] = this._observation_space.Project(v : a.distance);
        } else {
          res[i] = this._observation_space.Max;
        }
      }

      this.ObservationArray = res;
    }
  }
}