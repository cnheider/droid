namespace droid.Runtime.Interfaces {
  /// <summary>
  /// </summary>
  public interface IActuator : IRegisterable {
    /// <summary>
    /// </summary>
    droid.Runtime.Structs.Space.Space1 MotionSpace { get; set; }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    float Sample();

    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    void ApplyMotion(IMotion motion);
  }
}