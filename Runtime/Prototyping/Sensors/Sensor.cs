namespace droid.Runtime.Prototyping.Sensors {
  /// <inheritdoc cref="GameObjects.PrototypingGameObject" />
  /// <summary>
  /// </summary>
  [UnityEngine.ExecuteInEditMode]
  [System.SerializableAttribute]
  public abstract class Sensor : droid.Runtime.GameObjects.PrototypingGameObject,
                                 droid.Runtime.Interfaces.ISensor {
    #region Fields

    [UnityEngine.HeaderAttribute("References", order = 99)]
    [UnityEngine.SerializeField]
    droid.Runtime.Environments.Prototyping.AbstractPrototypingEnvironment _environment;

    #endregion

    /// <summary>
    /// </summary>
    public droid.Runtime.Environments.Prototyping.AbstractPrototypingEnvironment ParentEnvironment {
      get { return this._environment; }
      set { this._environment = value; }
    }

    /// <summary>
    /// </summary>
    protected virtual void Update() {
      if (UnityEngine.Application.isPlaying) {
        if (this.FloatEnumerable == null) {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            UnityEngine.Debug.LogWarning(message :
                                         $"FloatEnumerable of {this.Identifier} is empty! Maybe you forget an assignment to it when updating observations");
          }
          #endif
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public abstract System.Collections.Generic.IEnumerable<float> FloatEnumerable { get; }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public abstract void UpdateObservation();

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : this);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() { this.ParentEnvironment?.UnRegister(sensor : this); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
      var any = false;
      if (this.FloatEnumerable != null) {
        foreach (var f in this.FloatEnumerable) {
          any = true;
          break;
        }
      }

      return any ? string.Join(",", values : this.FloatEnumerable) : "Empty FloatEnumerable";
    }
  }
}