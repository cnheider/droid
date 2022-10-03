namespace droid.Runtime.Interfaces {
  /// <summary>
  /// </summary>
  public interface IHasEulerTransform {
    /// <summary>
    /// </summary>
    UnityEngine.Vector3 Position { get; }

    /// <summary>
    /// </summary>
    UnityEngine.Vector3 Direction { get; }

    /// <summary>
    /// </summary>
    UnityEngine.Vector3 Rotation { get; }

    /// <summary>
    /// </summary>
    droid.Runtime.Structs.Space.Space3 PositionSpace { get; }

    /// <summary>
    /// </summary>
    droid.Runtime.Structs.Space.Space3 DirectionSpace { get; }

    /// <summary>
    /// </summary>
    droid.Runtime.Structs.Space.Space3 RotationSpace { get; }
  }
}