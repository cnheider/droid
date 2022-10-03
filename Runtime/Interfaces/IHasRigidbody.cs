namespace droid.Runtime.Interfaces {
  /// <summary>
  /// </summary>
  public interface IHasRigidbody {
    /// <summary>
    /// </summary>
    UnityEngine.Vector3 Velocity { get; }

    /// <summary>
    /// </summary>
    UnityEngine.Vector3 AngularVelocity { get; }

    /// <summary>
    /// </summary>
    droid.Runtime.Structs.Space.Space3 VelocitySpace { get; }

    /// <summary>
    /// </summary>
    droid.Runtime.Structs.Space.Space3 AngularSpace { get; }
  }
}