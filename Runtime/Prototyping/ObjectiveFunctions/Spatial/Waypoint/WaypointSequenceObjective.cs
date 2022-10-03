namespace droid.Runtime.Prototyping.ObjectiveFunctions.Spatial.Waypoint {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : EvaluationComponentMenuPath._ComponentMenuPath
                                           + "WaypointSequence"
                                           + EvaluationComponentMenuPath._Postfix)]
  class WaypointSequenceObjective : SpatialObjective {
    [UnityEngine.SerializeField] Waypoint[] waypoints;
    [UnityEngine.SerializeField] Waypoint last_waypoint;
    [UnityEngine.SerializeField] Waypoint current_waypoint;
    [UnityEngine.SerializeField] Waypoint next_waypoint;
    [UnityEngine.SerializeField] UnityEngine.Transform tracking_point;

    [UnityEngine.SerializeField] float margin = 0.01f;
    [UnityEngine.SerializeField] bool inverse = false;
    [UnityEngine.SerializeField] bool reward_for_accounting_for_next;
    [UnityEngine.SerializeField] bool reward_for_smoothing_with_last;
    [UnityEngine.SerializeField] System.Collections.IEnumerator waypoint_enumerator;

    public override void InternalReset() {
      if (this.waypoint_enumerator == null) {
        this.waypoint_enumerator = this.waypoints.GetEnumerator();
      }

      this.waypoint_enumerator.Reset();

      this.last_waypoint = null;
      this.current_waypoint = this.waypoint_enumerator.Current as Waypoint;

      this.waypoint_enumerator.MoveNext();
      this.next_waypoint = this.waypoint_enumerator.Current as Waypoint;
    }

    public void MoveToNext() {
      this.waypoint_enumerator.MoveNext();
      var nex = this.waypoint_enumerator.Current as Waypoint;

      if (!nex) {
        this.ParentEnvironment.Terminate("Last waypoint reached");
      } else {
        this.last_waypoint = this.current_waypoint;
        this.current_waypoint = this.next_waypoint;
        this.next_waypoint = nex;
      }
    }

    public override float InternalEvaluate() {
      var signal = 0.0f;
      var distance = UnityEngine.Vector3.Distance(a : this.tracking_point.position,
                                                  b : this.current_waypoint.transform.position);

      if (distance <= this.current_waypoint.Radius + this.margin) {
        this.MoveToNext();
        signal += this.SolvedSignal;
      } else {
        if (this.inverse) {
          signal += distance;
        } else {
          signal -= distance;
        }
      }

      if (this.reward_for_accounting_for_next) {
        signal += 1
                  / UnityEngine.Vector3.Distance(a : this.tracking_point.position,
                                                 b : this.current_waypoint.transform.position);
        signal += UnityEngine.Vector3.Dot(lhs : this.tracking_point.forward,
                                          rhs : this.next_waypoint.transform.forward);
      }

      if (this.reward_for_smoothing_with_last) {
        signal += UnityEngine.Vector3.Dot(lhs : this.tracking_point.forward,
                                          rhs : this.next_waypoint.transform.forward);
      }

      return signal;
    }

    public override void RemotePostSetup() { }
  }
}