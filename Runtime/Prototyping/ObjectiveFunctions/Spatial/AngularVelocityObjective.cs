namespace droid.Runtime.Prototyping.ObjectiveFunctions.Spatial {
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : EvaluationComponentMenuPath._ComponentMenuPath
                                           + "AngularVelocity"
                                           + EvaluationComponentMenuPath._Postfix)]
  public class AngularVelocityObjective : SpatialObjective {
    [UnityEngine.SerializeField] bool _penalty = false;
    [UnityEngine.SerializeField] UnityEngine.Rigidbody _rigidbody = null;

    void OnDrawGizmosSelected() {
      var rb_pos = this._rigidbody.position;
      UnityEngine.Debug.DrawLine(start : rb_pos, end : rb_pos + this._rigidbody.angularVelocity);
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override float InternalEvaluate() {
      if (this._penalty) {
        if (this._rigidbody) {
          return -this._rigidbody.angularVelocity.magnitude;
        }
      }

      if (this._rigidbody) {
        return 1 / (this._rigidbody.angularVelocity.magnitude + 1);
      }

      return 0;
    }

    /// <summary>
    /// </summary>
    public override void InternalReset() { }

    /// <summary>
    /// </summary>
    public override void RemotePostSetup() {
      if (this._rigidbody == null) {
        this._rigidbody = FindObjectOfType<UnityEngine.Rigidbody>();
      }
    }
  }
}