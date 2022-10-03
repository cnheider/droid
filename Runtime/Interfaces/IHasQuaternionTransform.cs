namespace droid.Runtime.Interfaces {
  /// <summary>
  /// </summary>
  public interface IHasQuaternionTransform {
    /// <summary>
    /// </summary>
    UnityEngine.Vector3 Position { get; }

    /// <summary>
    /// </summary>
    UnityEngine.Quaternion Rotation { get; }

    /// <summary>
    /// </summary>
    droid.Runtime.Structs.Space.Space3 PositionSpace { get; }

    /// <summary>
    /// </summary>
    droid.Runtime.Structs.Space.Space4 RotationSpace { get; }
  }
}