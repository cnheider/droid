namespace droid.Runtime.Prototyping.ObjectiveFunctions.Spatial {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : EvaluationComponentMenuPath._ComponentMenuPath
                                           + "PoseDeviance"
                                           + EvaluationComponentMenuPath._Postfix)]
  public class PoseDevianceObjective : SpatialObjective {
    void OnDrawGizmosSelected() {
      var goal_position = this._target_transform.position;
      var actor_position = this._actor_transform.position;
      UnityEngine.Debug.DrawLine(start : actor_position, end : goal_position);

      var off_up = goal_position
                   + UnityEngine.Vector3.up
                   * UnityEngine.Vector3.SignedAngle(@from : this._target_transform.forward,
                                                     to : this._actor_transform.forward,
                                                     axis : UnityEngine.Vector3.up)
                   / 180;
      UnityEngine.Debug.DrawLine(start : goal_position, end : off_up);
      UnityEngine.Debug.DrawLine(start : actor_position, end : off_up);

      var up = this._actor_transform.up;
      var up1 = this._target_transform.up;
      var off_forward = goal_position
                        + UnityEngine.Vector3.forward
                        * UnityEngine.Vector3.SignedAngle(@from : up1,
                                                          to : up,
                                                          axis : UnityEngine.Vector3.forward)
                        / 180;
      UnityEngine.Debug.DrawLine(start : goal_position, end : off_forward);
      UnityEngine.Debug.DrawLine(start : actor_position, end : off_forward);

      var off_left = goal_position
                     + UnityEngine.Vector3.left
                     * UnityEngine.Vector3.SignedAngle(@from : up1, to : up, axis : UnityEngine.Vector3.left)
                     / 180;
      UnityEngine.Debug.DrawLine(start : goal_position, end : off_left);
      UnityEngine.Debug.DrawLine(start : actor_position, end : off_left);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override float InternalEvaluate() {
      var signal = this.DefaultSignal;

      /*if (this._playable_area != null && !this._playable_area.Bounds.Intersects(this._actor_transform.ActorBounds)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log("Outside playable area");
        }
        #endif
        this.ParentEnvironment.Terminate("Outside playable area");
      }*/

      var distance =
          UnityEngine.Mathf.Abs(f : UnityEngine.Vector3.Distance(a : this._target_transform.position,
                                                                 b : this._actor_transform.position));
      var angle = UnityEngine.Quaternion.Angle(a : this._target_transform.rotation,
                                               b : this._actor_transform.rotation);
      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log(message : $"Distance: {distance}");
        UnityEngine.Debug.Log(message : $"Angle: {angle}");
      }
      #endif

      if (!this._sparse) {
        if (this._inverse) {
          signal -= distance;
          signal -= angle;
        } else {
          signal += 1 / (distance + 1);
          signal += 1 / (angle + 1);

          if (this._state_full) {
            if (signal <= this._peak_reward) {
              signal = 0.0f;
            } else {
              this._peak_reward = signal;
            }
          }
        }
      }

      if (distance < this._goal_reached_radius) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          UnityEngine.Debug.Log("Within range of goal");
        }
        #endif

        signal += this.SolvedSignal;
        if (this._terminate_on_goal_reached) {
          this.ParentEnvironment?.Terminate("Within range of goal");
        }
      }

      if (this._has_collided) {
        this.ParentEnvironment?.Terminate("Actor has collided");
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log(message : $"Frame Number: {this.ParentEnvironment?.StepI}, "
                                        + $"Terminated: {this.ParentEnvironment?.Terminated}, "
                                        + $"Last Reason: {this.ParentEnvironment?.LastTerminationReason}, "
                                        + $"Internal Feedback Signal: {signal}, "
                                        + $"Distance: {distance}");
      }
      #endif

      return signal;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void InternalReset() {
      this._peak_reward = 0.0f;
      this._has_collided = false;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void RemotePostSetup() {
      if (!this._target_transform) {
        this._target_transform = FindObjectOfType<UnityEngine.Transform>();
      }

      if (!this._actor_transform) {
        this._actor_transform = FindObjectOfType<UnityEngine.Transform>();

        var remote_sensor = this._actor_transform
                                .GetComponentInChildren<droid.Runtime.GameObjects.ChildSensors.
                                    ChildColliderSensor<UnityEngine.Collider, UnityEngine.Collision>>();
        if (!remote_sensor) {
          var col = this._actor_transform.GetComponentInChildren<UnityEngine.Collider>();
          if (col) {
            remote_sensor = col.gameObject
                               .AddComponent<droid.Runtime.GameObjects.ChildSensors.ChildColliderSensor<
                                   UnityEngine.Collider, UnityEngine.Collision>>();
          }
        }

        if (remote_sensor) {
          remote_sensor.Caller = this;
          remote_sensor.OnTriggerEnterDelegate = this.OnChildTriggerEnter;
          remote_sensor.OnCollisionEnterDelegate = this.OnChildTriggerEnter;
        }
      }

      if (this._obstructions == null || this._obstructions.Length <= 0) {
        this._obstructions = FindObjectsOfType<droid.Runtime.Utilities.Extensions.Obstruction>();
      }

      if (!this._playable_area) {
        this._playable_area = FindObjectOfType<droid.Runtime.GameObjects.BoundingBoxes.NeodroidBoundingBox>();
      }
    }

    void OnChildTriggerEnter(UnityEngine.GameObject child_sensor_game_object,
                             UnityEngine.Collision collision) {
      if (collision.collider.CompareTag("Obstruction")) {
        if (this._terminate_on_obstruction_collision) {
          this.ParentEnvironment.Terminate("Collided with obstruction");
        }
      }

      this._has_collided = true;
    }

    void OnChildTriggerEnter(UnityEngine.GameObject child_sensor_game_object,
                             UnityEngine.Collider collider1) {
      if (collider1.CompareTag("Obstruction")) {
        if (this._terminate_on_obstruction_collision) {
          this.ParentEnvironment.Terminate("Collided with obstruction");
        }
      }

      this._has_collided = true;
    }

    #region Fields

    [UnityEngine.HeaderAttribute("Specific", order = 102)]
    [UnityEngine.SerializeField]
    float _peak_reward = 0;

    [UnityEngine.SerializeField]
    [UnityEngine.RangeAttribute(0.1f, 10f)]
    float _distance_base = 2f;

    [UnityEngine.SerializeField]
    [UnityEngine.RangeAttribute(0.1f, 10f)]
    float _distance_nominator = 5f;

    [UnityEngine.SerializeField]
    [UnityEngine.RangeAttribute(0.1f, 10f)]
    float _angle_base = 6f;

    [UnityEngine.SerializeField]
    [UnityEngine.RangeAttribute(0.1f, 10f)]
    float _angle_nominator = 3f;

    [UnityEngine.SerializeField] bool _sparse = true;
    [UnityEngine.SerializeField] bool _inverse = false;
    [UnityEngine.SerializeField] UnityEngine.Transform _target_transform = null;
    [UnityEngine.SerializeField] UnityEngine.Transform _actor_transform = null;

    [UnityEngine.SerializeField]
    droid.Runtime.GameObjects.BoundingBoxes.NeodroidBoundingBox _playable_area = null;

    [UnityEngine.SerializeField] droid.Runtime.Utilities.Extensions.Obstruction[] _obstructions = null;
    [UnityEngine.SerializeField] bool _state_full = false;
    [UnityEngine.SerializeField] float _goal_reached_radius = 0.01f; // Equivalent to 1 cm.
    [UnityEngine.SerializeField] bool _terminate_on_obstruction_collision = true; //TODO: implement
    [UnityEngine.SerializeField] bool _has_collided = false;
    [UnityEngine.SerializeField] bool _terminate_on_goal_reached = true;

    #endregion
  }
}