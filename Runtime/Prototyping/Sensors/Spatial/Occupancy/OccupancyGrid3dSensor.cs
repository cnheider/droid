namespace droid.Runtime.Prototyping.Sensors.Spatial.Occupancy {


  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// TODO: SHOULD MAINTAIN SPANNING GRID VOLUME of occupancy! 
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                                           + "Occupancy3d"
                                           + SensorComponentMenuPath._Postfix)]
  [UnityEngine.ExecuteInEditMode]
  public class OccupancyGrid3dSensor : Sensor,
                                   droid.Runtime.Interfaces.IHasByteArray {
    [UnityEngine.HeaderAttribute("Observation", order = 103)]
    [UnityEngine.SerializeField]
    UnityEngine.Vector3[] _observation_value;

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Space1 _observation_value_space = droid.Runtime.Structs.Space.Space1.ZeroOne;
    
    System.Collections.Generic.IEnumerable<UnityEngine.Transform> _transforms;

    public System.Byte[] Bytes { get; }
    public System.Int32[] Shape { get; }
    public System.String ArrayEncoding { get; }
    
    /// <summary>
    /// </summary>
    public droid.Runtime.Structs.Space.Space1 SingleSpace { get { return this._observation_value_space; } }

    public override System.Collections.Generic.IEnumerable<float> FloatEnumerable {
      get {
        var a = new float[this.ObservationArray.Length * 3];
        for (var i = 0; i < this.ObservationArray.Length * 3; i += 3) {
          a[i] = this.ObservationArray[i].x;
          a[i + 1] = this.ObservationArray[i].y;
          a[i + 2] = this.ObservationArray[i].z;
        }

        return a;
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public UnityEngine.Vector3[] ObservationArray {
      get { return this._observation_value; }
      set { this._observation_value = this.SingleSpace.Project(v : value); }
    }
    

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
//TODO: 
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      //TODO: 
    }
  }
}