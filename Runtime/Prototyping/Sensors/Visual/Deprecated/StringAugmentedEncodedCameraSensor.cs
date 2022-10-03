namespace droid.Runtime.Prototyping.Sensors.Visual.Deprecated {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                                           + "StringAugmentedCamera"
                                           + SensorComponentMenuPath._Postfix)]
  [UnityEngine.ExecuteInEditMode]
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.Camera))]
  public class StringAugmentedEncodedCameraSensor : EncodedCameraSensor,
                                                    droid.Runtime.Interfaces.IHasString {
    const string _color_identifier = "Colors";

    /// <summary>
    /// </summary>
    [UnityEngine.HeaderAttribute("Observation", order = 103)]
    [UnityEngine.SerializeField]
    protected string serialised_string;

    string _colors;

    /// <summary>
    /// </summary>
    public string ObservationValue { get { return this.serialised_string; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
      base.PreSetup();
      this._colors = this.Identifier + _color_identifier;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : this,
            identifier : this.Identifier);

      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : this,
            identifier : this._colors);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      this.ParentEnvironment?.UnRegister(t : this, identifier : this.Identifier);
      this.ParentEnvironment?.UnRegister(t : this, identifier : this._colors);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      base.UpdateObservation();
      this.serialised_string = "";
    }
  }
}