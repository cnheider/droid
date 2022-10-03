namespace droid.Runtime.Prototyping.Sensors.Spatial.EntityCentric.Rays {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                                           + "LineOfSight"
                                           + SensorComponentMenuPath._Postfix)]
  [UnityEngine.ExecuteInEditMode]
  [System.SerializableAttribute]
  public class LineOfSightSensor : Sensor,
                                   droid.Runtime.Interfaces.IHasSingle {
    [UnityEngine.SerializeField] float _obs_value = 0;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Space1 _observation_value_space = droid.Runtime.Structs.Space.Space1.ZeroOne;

    [UnityEngine.HeaderAttribute("Specific", order = 102)]
    [UnityEngine.SerializeField]
    UnityEngine.Transform _target = null;

    UnityEngine.RaycastHit _hit = new UnityEngine.RaycastHit();

    public override System.Collections.Generic.IEnumerable<float> FloatEnumerable {
      get { yield return this.ObservationValue; }
    }

    public float ObservationValue { get { return this._obs_value; } private set { this._obs_value = value; } }

    public droid.Runtime.Structs.Space.Space1 SingleSpace { get { return this._observation_value_space; } }

    public override void UpdateObservation() {
      var distance = UnityEngine.Vector3.Distance(a : this.transform.position, b : this._target.position);
      if (UnityEngine.Physics.Raycast(origin : this.transform.position,
                                      direction : this._target.position - this.transform.position,
                                      hitInfo : out this._hit,
                                      maxDistance : distance)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          UnityEngine.Debug.Log(message : this._hit.distance);
        }
        #endif

        if (this._hit.collider.gameObject != this._target.gameObject) {
          this.ObservationValue = 0;
        } else {
          this.ObservationValue = 1;
        }
      } else {
        this.ObservationValue = 1;
      }
    }
  }
}