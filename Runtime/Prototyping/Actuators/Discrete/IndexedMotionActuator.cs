namespace droid.Runtime.Prototyping.Actuators.Discrete {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : ActuatorComponentMenuPath._ComponentMenuPath
                                           + "IndexedMotion"
                                           + ActuatorComponentMenuPath._Postfix)]
  public class IndexedMotionActuator : Actuator {
    [UnityEngine.SerializeField] UnityEngine.Events.UnityEvent[] _events = { };

    public override string[] InnerMotionNames { get; }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    /// <exception cref="T:System.ArgumentOutOfRangeException"></exception>
    protected override void InnerApplyMotion(droid.Runtime.Interfaces.IMotion motion) {
      var ind = (int)motion.Strength;
      if (ind >= this._events.Length) {
        return;
      }

      this._events[ind].Invoke();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Setup() {
      this.MotionSpace =
          new droid.Runtime.Structs.Space.Space1 {
                                                     Min = 0,
                                                     Max = this._events.Length - 1,
                                                     DecimalGranularity = 0
                                                 };
    }
  }
}