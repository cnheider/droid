namespace droid.Runtime.Prototyping.Actors {
  public class VirtualActor : droid.Runtime.Interfaces.IActor {
    public VirtualActor(
        System.Collections.Generic.SortedDictionary<string, droid.Runtime.Interfaces.IActuator> actuators) {
      this.Actuators = actuators;
    }

    #region IActor Members

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public string Identifier { get; }

    /// <summary>
    /// </summary>
    public System.Collections.Generic.SortedDictionary<string, droid.Runtime.Interfaces.IActuator> Actuators {
      get;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public UnityEngine.Transform CachedTransform { get; }

    public void Tick() { throw new System.NotImplementedException(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="obj"></param>
    /// <exception cref="T:System.NotImplementedException"></exception>
    public void Register(droid.Runtime.Interfaces.IActuator obj) {
      throw new System.NotImplementedException();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="identifier"></param>
    /// <exception cref="T:System.NotImplementedException"></exception>
    public void Register(droid.Runtime.Interfaces.IActuator obj, string identifier) {
      throw new System.NotImplementedException();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="obj"></param>
    /// <exception cref="T:System.NotImplementedException"></exception>
    public void UnRegister(droid.Runtime.Interfaces.IActuator obj) {
      throw new System.NotImplementedException();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="t"></param>
    /// <param name="obj"></param>
    /// <exception cref="T:System.NotImplementedException"></exception>
    public void UnRegister(droid.Runtime.Interfaces.IActuator t, string obj) {
      throw new System.NotImplementedException();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    /// <exception cref="T:System.NotImplementedException"></exception>
    public void ApplyMotion(droid.Runtime.Interfaces.IMotion motion) {
      throw new System.NotImplementedException();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <exception cref="T:System.NotImplementedException"></exception>
    public void PrototypingReset() { throw new System.NotImplementedException(); }

    public void PreSetup() { throw new System.NotImplementedException(); }
    public void Setup() { throw new System.NotImplementedException(); }

    public void RemotePostSetup() { throw new System.NotImplementedException(); }

    #endregion
  }
}