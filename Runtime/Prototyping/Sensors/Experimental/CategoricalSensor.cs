namespace droid.Runtime.Prototyping.Sensors.Experimental {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                                           + "Categorical"
                                           + SensorComponentMenuPath._Postfix)]
  public class CategoricalSensor : Sensor,
                                   droid.Runtime.Interfaces.IHasSingle {
    [UnityEngine.SerializeField] droid.Runtime.GameObjects.PrototypingGameObject _categoryProvider = null;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override System.Collections.Generic.IEnumerable<float> FloatEnumerable {
      get { yield return this.ObservationValue; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [field : UnityEngine.SerializeField]
    public float ObservationValue { get; private set; }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [field : UnityEngine.SerializeField]
    public droid.Runtime.Structs.Space.Space1 SingleSpace { get; set; }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Tick() {
      base.Tick();
      this.UpdateObservation();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      if (this._categoryProvider is droid.Runtime.Interfaces.ICategoryProvider provider) {
        this.ObservationValue = provider.CurrentCategoryValue;
      } else {
        UnityEngine.Debug.LogWarning(message :
                                     $"{this._categoryProvider} does not implement ICategoryProvider, and will not provide at categorical value");
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void RemotePostSetup() {
      base.RemotePostSetup();
      if (this._categoryProvider is droid.Runtime.Interfaces.ICategoryProvider provider) {
        this.SingleSpace = provider.Space1;
      }
    }
  }
}