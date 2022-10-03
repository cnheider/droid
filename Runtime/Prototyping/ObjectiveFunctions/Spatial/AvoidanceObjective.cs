namespace droid.Runtime.Prototyping.ObjectiveFunctions.Spatial {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : EvaluationComponentMenuPath._ComponentMenuPath
                                           + "PunishmentFunction"
                                           + EvaluationComponentMenuPath._Postfix)]
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.Rigidbody))]
  public class AvoidanceObjective : SpatialObjective {
    [UnityEngine.SerializeField] string _avoid_tag = "balls";
    [UnityEngine.SerializeField] int _hits = 0;

    //[SerializeField] LayerMask _layer_mask;

    [UnityEngine.SerializeField] UnityEngine.GameObject _player = null;
    [UnityEngine.SerializeField] UnityEngine.GameObject[] tagged_gos;

    void OnDrawGizmosSelected() {
      var player_pos = this._player.transform.position;
      foreach (var o in this.tagged_gos) {
        UnityEngine.Debug.DrawLine(start : player_pos, end : o.transform.position);
      }
    }

    // Use this for initialization
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void RemotePostSetup() {
      this.ResetHits();

      this.tagged_gos = UnityEngine.GameObject.FindGameObjectsWithTag(tag : this._avoid_tag);

      foreach (var ball in this.tagged_gos) {
        if (ball) {
          var publisher = ball.GetComponent<droid.Runtime.GameObjects.ChildSensors.ChildCollider3DSensor>();
          if (!publisher || publisher.Caller != this) {
            publisher = ball.AddComponent<droid.Runtime.GameObjects.ChildSensors.ChildCollider3DSensor>();
          }

          publisher.Caller = this;
          publisher.OnCollisionEnterDelegate = this.OnChildCollision;
        }
      }
    }

    void OnChildCollision(UnityEngine.GameObject child_sensor_game_object, UnityEngine.Collision collision) {
      if (collision.collider.name == this._player.name) {
        this._hits += 1;
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log(message : this._hits);
      }
      #endif
    }

    void ResetHits() { this._hits = 0; }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void InternalReset() { this.ResetHits(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override float InternalEvaluate() { return this._hits * -1f; }
  }
}