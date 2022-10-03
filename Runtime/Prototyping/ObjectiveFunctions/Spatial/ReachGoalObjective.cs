namespace droid.Runtime.Prototyping.ObjectiveFunctions.Spatial {
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : EvaluationComponentMenuPath._ComponentMenuPath
                                           + "ReachGoal"
                                           + EvaluationComponentMenuPath._Postfix)]
  public class ReachGoalObjective : SpatialObjective {
    [UnityEngine.SerializeField] droid.Runtime.Prototyping.Actors.Actor _actor = null;

    [UnityEngine.SerializeField] bool _based_on_tags = false;

    [UnityEngine.SerializeField] droid.Runtime.Utilities.Grid.EmptyCell _goal = null;

    //Used for.. if outside playable area then reset
    [UnityEngine.SerializeField] ActorOverlapping _overlapping = ActorOverlapping.Outside_area_;

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override float InternalEvaluate() {
      var distance =
          UnityEngine.Mathf.Abs(f : UnityEngine.Vector3.Distance(a : this._goal.transform.position,
                                                                 b : this._actor.transform.position));

      if (this._overlapping == ActorOverlapping.Inside_area_ || distance < 0.5f) {
        this.ParentEnvironment.Terminate("Inside goal area");
        return 1f;
      }

      return 0f;
    }

    /// <summary>
    /// </summary>
    public override void InternalReset() {
      this.Setup();
      this._overlapping = ActorOverlapping.Outside_area_;
    }

    /// <summary>
    /// </summary>
    /// <param name="goal"></param>
    public void SetGoal(droid.Runtime.Utilities.Grid.EmptyCell goal) {
      this._goal = goal;
      this.InternalReset();
    }

    /// <summary>
    /// </summary>
    public override void RemotePostSetup() {
      if (!this._goal) {
        this._goal = FindObjectOfType<droid.Runtime.Utilities.Grid.EmptyCell>();
      }

      if (!this._actor) {
        this._actor = FindObjectOfType<droid.Runtime.Prototyping.Actors.Actor>();
      }

      if (this._goal) {
        droid.Runtime.Utilities.NeodroidRegistrationUtilities
             .RegisterCollisionTriggerCallbacksOnChildren<
                 droid.Runtime.GameObjects.ChildSensors.ChildCollider3DSensor, UnityEngine.Collider,
                 UnityEngine.Collision>(caller : this,
                                        parent : this._goal.transform,
                                        null,
                                        on_trigger_enter_child : this.OnTriggerEnterChild);
      }

      if (this._actor) {
        droid.Runtime.Utilities.NeodroidRegistrationUtilities
             .RegisterCollisionTriggerCallbacksOnChildren<
                 droid.Runtime.GameObjects.ChildSensors.ChildCollider3DSensor, UnityEngine.Collider,
                 UnityEngine.Collision>(caller : this,
                                        parent : this._actor.transform,
                                        null,
                                        on_trigger_enter_child : this.OnTriggerEnterChild);
      }
    }

    void OnTriggerEnterChild(UnityEngine.GameObject child_game_object,
                             UnityEngine.Collider other_game_object) {
      UnityEngine.Debug.Log("triggered");
      if (this._actor) {
        if (this._based_on_tags) {
          if (other_game_object.CompareTag(tag : this._actor.tag)) {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              UnityEngine.Debug.Log("Actor is inside area");
            }
            #endif

            this._overlapping = ActorOverlapping.Inside_area_;
          }
        } else {
          if (child_game_object == this._goal.gameObject
              && other_game_object.gameObject == this._actor.gameObject) {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              UnityEngine.Debug.Log("Actor is inside area");
            }
            #endif
            this._overlapping = ActorOverlapping.Inside_area_;
          }
        }
      }
    }

    /// <summary>
    /// </summary>
    enum ActorOverlapping {
      /// <summary>
      /// </summary>
      Inside_area_,

      /// <summary>
      /// </summary>
      Outside_area_
    }
  }
}