namespace droid.Runtime.Interfaces {
  /// <summary>
  /// </summary>
  public interface IActor : IRegisterable,
                            IHasRegister<IActuator> {
    /// <summary>
    /// </summary>
    System.Collections.Generic.SortedDictionary<string, IActuator> Actuators { get; }

    /// <summary>
    /// </summary>
    UnityEngine.Transform CachedTransform { get; }

    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    void ApplyMotion(IMotion motion);
  }
}