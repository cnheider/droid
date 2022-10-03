namespace droid.Runtime.Messaging.Messages {
  /// <summary>
  /// </summary>
  public class Body {
    public Body(UnityEngine.Vector3 vel, UnityEngine.Vector3 ang) {
      this.Velocity = vel;
      this.AngularVelocity = ang;
    }

    /// <summary>
    /// </summary>
    public UnityEngine.Vector3 Velocity { get; }

    /// <summary>
    /// </summary>
    public UnityEngine.Vector3 AngularVelocity { get; }
  }
}