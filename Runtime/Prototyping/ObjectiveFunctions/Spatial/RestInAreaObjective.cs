namespace droid.Runtime.Prototyping.ObjectiveFunctions.Spatial {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : EvaluationComponentMenuPath._ComponentMenuPath
                                           + "RestInArea"
                                           + EvaluationComponentMenuPath._Postfix)]
  public class RestInAreaObjective : SpatialObjective {
    [UnityEngine.SerializeField] droid.Runtime.Prototyping.Actors.Actor _actor = null;

    [UnityEngine.SerializeField] UnityEngine.Collider _area = null;
    [UnityEngine.SerializeField] bool _is_resting = false;

    [UnityEngine.SerializeField] droid.Runtime.Utilities.Extensions.Obstruction[] _obstructions = null;

    //Used for.. if outside playable area then reset
    [UnityEngine.SerializeField] ActorOverlapping _overlapping = ActorOverlapping.Outside_area_;

    [UnityEngine.SerializeField]
    droid.Runtime.GameObjects.BoundingBoxes.NeodroidBoundingBox _playable_area = null;

    [UnityEngine.SerializeField] float _resting_time = 3f;
    [UnityEngine.SerializeField] bool _sparse = false;
    [UnityEngine.SerializeField] UnityEngine.Coroutine _wait_for_resting = null;
    UnityEngine.WaitForSeconds _wait_for_seconds = new UnityEngine.WaitForSeconds(3f);

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override float InternalEvaluate() {
      var signal = 0f;

      if (this._overlapping == ActorOverlapping.Inside_area_ && this._is_resting) {
        if (this._actor is droid.Runtime.Prototyping.Actors.KillableActor) {
          if (((droid.Runtime.Prototyping.Actors.KillableActor)this._actor).IsAlive) {
            this.ParentEnvironment.Terminate("Inside goal area");
            return 1f;
          }
        } else {
          this.ParentEnvironment.Terminate("Inside goal area");
          return 1f;
        }
      }

      if (!this._sparse) {
        signal += 1
                  / UnityEngine.Vector3.Distance(a : this._actor.transform.position,
                                                 b : this._area.transform.position);
      }

      if (this._playable_area && this._actor) {
        if (!this._playable_area.Bounds.Intersects(bounds : this._actor.GetComponent<UnityEngine.Collider>()
                                                                .bounds)) {
          this.ParentEnvironment.Terminate("Actor is outside playable area");
        }
      }

      return signal;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void InternalReset() {
      if (this._wait_for_resting != null) {
        this.StopCoroutine(routine : this._wait_for_resting);
      }

      this._is_resting = false;
    }

    System.Collections.IEnumerator WaitForResting() {
      yield return this._wait_for_seconds;

      this._is_resting = true;
    }

    /// <summary>
    /// </summary>
    public override void RemotePostSetup() {
      if (!this._area) {
        this._area = FindObjectOfType<droid.Runtime.Prototyping.Sensors.Sensor>().gameObject
            .GetComponent<UnityEngine.Collider>();
      }

      if (!this._actor) {
        this._actor = FindObjectOfType<droid.Runtime.Prototyping.Actors.Actor>();
      }

      if (this._obstructions.Length <= 0) {
        this._obstructions = FindObjectsOfType<droid.Runtime.Utilities.Extensions.Obstruction>();
      }

      if (!this._playable_area) {
        this._playable_area = FindObjectOfType<droid.Runtime.GameObjects.BoundingBoxes.NeodroidBoundingBox>();
      }

      droid.Runtime.Utilities.NeodroidRegistrationUtilities
           .RegisterCollisionTriggerCallbacksOnChildren<
               droid.Runtime.GameObjects.ChildSensors.ChildCollider3DSensor, UnityEngine.Collider,
               UnityEngine.Collision>(caller : this,
                                      parent : this._area.transform,
                                      null,
                                      on_trigger_enter_child : this.OnTriggerEnterChild,
                                      null,
                                      on_trigger_exit_child : this.OnTriggerExitChild,
                                      null,
                                      on_trigger_stay_child : this.OnTriggerStayChild);

      droid.Runtime.Utilities.NeodroidRegistrationUtilities
           .RegisterCollisionTriggerCallbacksOnChildren<
               droid.Runtime.GameObjects.ChildSensors.ChildCollider3DSensor, UnityEngine.Collider,
               UnityEngine.Collision>(caller : this,
                                      parent : this._actor.transform,
                                      null,
                                      on_trigger_enter_child : this.OnTriggerEnterChild,
                                      null,
                                      on_trigger_exit_child : this.OnTriggerExitChild,
                                      null,
                                      on_trigger_stay_child : this.OnTriggerStayChild);
      this._wait_for_seconds = new UnityEngine.WaitForSeconds(seconds : this._resting_time);
    }

    void OnTriggerEnterChild(UnityEngine.GameObject child_game_object,
                             UnityEngine.Collider other_game_object) {
      if (this._actor) {
        if (child_game_object == this._area.gameObject
            && other_game_object.gameObject == this._actor.gameObject) {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            UnityEngine.Debug.Log("Actor is inside area");
          }
          #endif

          this._overlapping = ActorOverlapping.Inside_area_;
          if (this._wait_for_resting != null) {
            this.StopCoroutine(routine : this._wait_for_resting);
          }

          this._wait_for_resting = this.StartCoroutine(routine : this.WaitForResting());
        }
      }
    }

    void OnTriggerStayChild(UnityEngine.GameObject child_game_object,
                            UnityEngine.Collider other_game_object) {
      if (this._actor) {
        if (child_game_object == this._area.gameObject
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

    void OnTriggerExitChild(UnityEngine.GameObject child_game_object,
                            UnityEngine.Collider other_game_object) {
      if (this._actor) {
        if (child_game_object == this._area.gameObject
            && other_game_object.gameObject == this._actor.gameObject) {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            UnityEngine.Debug.Log("Actor is outside area");
          }
          #endif

          this._overlapping = ActorOverlapping.Outside_area_;
          if (this._wait_for_resting != null) {
            this.StopCoroutine(routine : this._wait_for_resting);
          }
        }
      }
    }
  }
}