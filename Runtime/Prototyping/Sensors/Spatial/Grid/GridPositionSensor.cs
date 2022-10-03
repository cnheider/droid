namespace droid.Runtime.Prototyping.Sensors.Spatial.Grid {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                                           + "GridPosition"
                                           + SensorComponentMenuPath._Postfix)]
  public class GridPositionSensor : Sensor,
                                    droid.Runtime.Interfaces.IHasSingle {
    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    int _height = 0;

    [UnityEngine.HeaderAttribute("Observation", order = 103)]
    [UnityEngine.SerializeField]
    float _observation_value;

    [UnityEngine.SerializeField] droid.Runtime.Structs.Space.Space1 _observation_value_space;
    [UnityEngine.SerializeField] int _width = 0;

    /// <summary>
    /// </summary>
    int[,] _grid = null;

    public override System.Collections.Generic.IEnumerable<float> FloatEnumerable {
      get { yield return this.ObservationValue; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public float ObservationValue {
      get { return this._observation_value; }
      set { this._observation_value = this.SingleSpace.Project(v : value); }
    }

    public droid.Runtime.Structs.Space.Space1 SingleSpace { get { return this._observation_value_space; } }

    public override void PreSetup() {
      this._grid = new int[this._width, this._height];

      var k = 0;
      for (var i = 0; i < this._width; i++) {
        for (var j = 0; j < this._height; j++) {
          this._grid[i, j] = k++;
        }
      }
    }

    public override void UpdateObservation() {
      var position = this.transform.position;
      var x = position.x + this._width;
      var z = position.z + this._height;

      this.ObservationValue = this._grid[(int)x, (int)z];
    }
  }
}