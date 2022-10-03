namespace droid.Runtime.Prototyping.Actors {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : ActorComponentMenuPath._ComponentMenuPath
                                           + "Killable"
                                           + ActorComponentMenuPath._Postfix)]
  public class KillableActor : Actor {
    [UnityEngine.SerializeField] bool _is_alive = true;

    /// <summary>
    /// </summary>
    public bool IsAlive { get { return this._is_alive; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "KillableActor"; } }

    /// <summary>
    /// </summary>
    public void Kill() { this._is_alive = false; }

    public override void ApplyMotion(droid.Runtime.Interfaces.IMotion motion) {
      if (this._is_alive) {
        base.ApplyMotion(motion : motion);
      } else {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          UnityEngine.Debug.Log("Actor is dead, cannot apply motion");
        }
        #endif
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PrototypingReset() {
      base.PrototypingReset();

      this._is_alive = true;
    }
  }
}