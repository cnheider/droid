namespace droid.Runtime.Prototyping.Actuators {
  /// <inheritdoc cref="GameObjects.PrototypingGameObject" />
  /// <summary>
  /// </summary>
  [UnityEngine.ExecuteInEditMode]
  [System.SerializableAttribute]
  public abstract class Actuator : droid.Runtime.GameObjects.PrototypingGameObject,
                                   droid.Runtime.Interfaces.IActuator {
    /// <summary>
    /// </summary>
    public droid.Runtime.Interfaces.IHasRegister<Actuator> Parent {
      get { return this._parent; }
      set { this._parent = value; }
    }

    public abstract string[] InnerMotionNames { get; }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    public void ApplyMotion(droid.Runtime.Interfaces.IMotion motion) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log(message : "Applying " + motion + " To " + this.name);
      }
      #endif

      motion.Strength = this._motion_value_space._space.Reproject(v : motion.Strength);

      this.InnerApplyMotion(motion : motion);
    }

    public droid.Runtime.Structs.Space.Space1 MotionSpace {
      get { return this._motion_value_space._space; }
      set { this._motion_value_space.Space = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public virtual float Sample() { return this._motion_value_space.Sample(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this._overriden = false;
      this.Parent =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : (droid.Runtime.
                Interfaces.IHasRegister<droid.Runtime.Interfaces.IActuator>)this.Parent,
            c : this,
            true);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this._overriden) {
        throw new System.NotImplementedException();
      }

      this._overriden = true;
      this.Parent?.UnRegister(obj : this);
    }

    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    protected abstract void InnerApplyMotion(droid.Runtime.Interfaces.IMotion motion);

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString() { return this.Identifier; }

    #region Fields

    [UnityEngine.HeaderAttribute("References", order = 99)]
    [UnityEngine.SerializeField]
    droid.Runtime.Interfaces.IHasRegister<Actuator> _parent;

    [UnityEngine.HeaderAttribute("General", order = 101)]
    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Sample.SampleSpace1 _motion_value_space =
        new droid.Runtime.Structs.Space.Sample.SampleSpace1 {
                                                                _space = droid.Runtime.Structs.Space.Space1
                                                                    .DiscreteMinusOneOne
                                                            };

    bool _overriden = false;

    #endregion
  }
}