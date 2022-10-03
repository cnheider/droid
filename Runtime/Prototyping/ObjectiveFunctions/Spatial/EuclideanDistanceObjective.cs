namespace droid.Runtime.Prototyping.ObjectiveFunctions.Spatial {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : EvaluationComponentMenuPath._ComponentMenuPath
                                           + "EuclideanDistance"
                                           + EvaluationComponentMenuPath._Postfix)]
  class EuclideanDistanceObjective : SpatialObjective {
    [UnityEngine.SerializeField] UnityEngine.Transform _g1;
    [UnityEngine.SerializeField] UnityEngine.Transform _g2;
    [UnityEngine.SerializeField] float margin = 0.01f;
    [UnityEngine.SerializeField] bool inverse = false;

    void OnDrawGizmosSelected() {
      UnityEngine.Debug.DrawLine(start : this._g1.position, end : this._g2.position);
    }

    public override void InternalReset() { }

    public override float InternalEvaluate() {
      var signal = 0.0f;
      var distance = UnityEngine.Vector3.Distance(a : this._g1.position, b : this._g2.position);

      if (distance <= this.margin) {
        this.ParentEnvironment.Terminate("Within margin");
        signal += this.SolvedSignal;
      } else {
        if (this.inverse) {
          signal += distance;
        } else {
          signal -= distance;
        }
      }

      return signal;
    }

    public override void RemotePostSetup() {
      if (this._g1 == null) {
        this._g1 = FindObjectOfType<droid.Runtime.Prototyping.Actors.Actor>().transform;
      }

      if (this._g2 == null) {
        this._g2 = this.transform;
      }
    }
  }
}