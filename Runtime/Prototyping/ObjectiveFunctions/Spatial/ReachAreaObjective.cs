﻿namespace droid.Runtime.Prototyping.ObjectiveFunctions.Spatial {
  /// <summary>
  /// </summary>
  enum ActorOverlapping {
    Inside_area_,
    Outside_area_
  }

  /// <summary>
  /// </summary>
  enum ActorColliding {
    Not_colliding_,
    Colliding_
  }

  //[RequireComponent (typeof(BoundingBox))]
  //[RequireComponent (typeof(BoxCollider))]
  [UnityEngine.AddComponentMenu(menuName : EvaluationComponentMenuPath._ComponentMenuPath
                                           + "ReachArea"
                                           + EvaluationComponentMenuPath._Postfix)]
  public class ReachAreaObjective : SpatialObjective {
    [UnityEngine.SerializeField] UnityEngine.Collider _actor = null;

    [UnityEngine.SerializeField] UnityEngine.Collider _area = null;

    [UnityEngine.SerializeField] bool _based_on_tags = false;
    [UnityEngine.SerializeField] ActorColliding _colliding = ActorColliding.Not_colliding_;

    [UnityEngine.SerializeField] droid.Runtime.Utilities.Extensions.Obstruction[] _obstructions;

    //Used for.. if outside playable area then reset
    [UnityEngine.SerializeField] ActorOverlapping _overlapping = ActorOverlapping.Outside_area_;

    [UnityEngine.SerializeField] droid.Runtime.GameObjects.BoundingBoxes.NeodroidBoundingBox _playable_area;

    public override void InternalReset() { }

    public override float InternalEvaluate() {
      /*var regularising_term = 0f;

            foreach (var ob in _obstructions) {
              RaycastHit ray_hit;
              Physics.Raycast (_actor.transform.position, (ob.transform.position - _actor.transform.position).normalized, out ray_hit, LayerMask.NameToLayer ("Obstruction"));
              regularising_term += -Mathf.Abs (Vector3.Distance (ray_hit.point, _actor.transform.position));
              //regularising_term += -Mathf.Abs (Vector3.Distance (ob.transform.position, _actor.transform.position));
            }

            reward += 0.2 * regularising_term;*/

      //reward += 1 / Mathf.Abs (Vector3.Distance (_area.transform.position, _actor.transform.position)); // Inversely porpotional to the absolute distance, closer higher reward

      if (this._overlapping == ActorOverlapping.Inside_area_) {
        this.ParentEnvironment.Terminate("Inside goal area");
        return 1f;
      }

      if (this._colliding == ActorColliding.Colliding_) {
        this.ParentEnvironment.Terminate("Actor colliding with obstruction");
      }

      if (this._playable_area && this._actor) {
        if (!this._playable_area.Bounds.Intersects(bounds : this._actor.GetComponent<UnityEngine.Collider>()
                                                                .bounds)) {
          this.ParentEnvironment.Terminate("Actor is outside playable area");
        }
      }

      return 0f;
    }

    public override void RemotePostSetup() {
      if (!this._area) {
        this._area = FindObjectOfType<droid.Runtime.Prototyping.Sensors.Sensor>().gameObject
            .GetComponent<UnityEngine.Collider>();
      }

      if (!this._actor) {
        this._actor = FindObjectOfType<droid.Runtime.Prototyping.Actors.Actor>().gameObject
            .GetComponent<UnityEngine.Collider>();
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
                                      on_collision_enter_child : this.OnCollisionEnterChild,
                                      on_trigger_enter_child : this.OnTriggerEnterChild,
                                      on_collision_exit_child : this.OnCollisionExitChild,
                                      on_trigger_exit_child : this.OnTriggerExitChild,
                                      on_collision_stay_child : this.OnCollisionStayChild,
                                      on_trigger_stay_child : this.OnTriggerStayChild);

      droid.Runtime.Utilities.NeodroidRegistrationUtilities
           .RegisterCollisionTriggerCallbacksOnChildren<
               droid.Runtime.GameObjects.ChildSensors.ChildCollider3DSensor, UnityEngine.Collider,
               UnityEngine.Collision>(caller : this,
                                      parent : this._actor.transform,
                                      on_collision_enter_child : this.OnCollisionEnterChild,
                                      on_trigger_enter_child : this.OnTriggerEnterChild,
                                      on_collision_exit_child : this.OnCollisionExitChild,
                                      on_trigger_exit_child : this.OnTriggerExitChild,
                                      on_collision_stay_child : this.OnCollisionStayChild,
                                      on_trigger_stay_child : this.OnTriggerStayChild);
    }

    void OnTriggerEnterChild(UnityEngine.GameObject child_game_object,
                             UnityEngine.Collider other_game_object) {
      if (this._actor) {
        if (this._based_on_tags) {
          if (child_game_object.CompareTag(tag : this._area.tag)
              && other_game_object.CompareTag(tag : this._actor.tag)) {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              UnityEngine.Debug.Log("Actor is inside area");
            }
            #endif

            this._overlapping = ActorOverlapping.Inside_area_;
          }

          if (child_game_object.CompareTag(tag : this._actor.tag)
              && other_game_object.CompareTag("Obstruction")) {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              UnityEngine.Debug.Log("Actor is colliding");
            }
            #endif

            this._colliding = ActorColliding.Colliding_;
          }
        } else {
          if (child_game_object == this._area.gameObject
              && other_game_object.gameObject == this._actor.gameObject) {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              UnityEngine.Debug.Log("Actor is inside area");
            }
            #endif

            this._overlapping = ActorOverlapping.Inside_area_;
          }

          if (child_game_object == this._actor.gameObject && other_game_object.CompareTag("Obstruction")) {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              UnityEngine.Debug.Log("Actor is colliding");
            }
            #endif

            this._colliding = ActorColliding.Colliding_;
          }
        }
      }
    }

    void OnTriggerStayChild(UnityEngine.GameObject child_game_object,
                            UnityEngine.Collider other_game_object) {
      if (this._actor) {
        if (this._based_on_tags) {
          if (child_game_object.CompareTag(tag : this._area.tag)
              && other_game_object.CompareTag(tag : this._actor.tag)) {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              UnityEngine.Debug.Log("Actor is inside area");
            }
            #endif

            this._overlapping = ActorOverlapping.Inside_area_;
          }

          if (child_game_object.CompareTag(tag : this._actor.tag)
              && other_game_object.CompareTag("Obstruction")) {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              UnityEngine.Debug.Log("Actor is colliding");
            }
            #endif

            this._colliding = ActorColliding.Colliding_;
          }
        } else {
          if (child_game_object == this._area.gameObject
              && other_game_object.gameObject == this._actor.gameObject) {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              UnityEngine.Debug.Log("Actor is inside area");
            }
            #endif

            this._overlapping = ActorOverlapping.Inside_area_;
          }

          if (child_game_object == this._actor.gameObject && other_game_object.CompareTag("Obstruction")) {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              UnityEngine.Debug.Log("Actor is colliding");
            }
            #endif

            this._colliding = ActorColliding.Colliding_;
          }
        }
      }
    }

    void OnTriggerExitChild(UnityEngine.GameObject child_game_object,
                            UnityEngine.Collider other_game_object) {
      if (this._actor) {
        if (this._based_on_tags) {
          if (child_game_object.CompareTag(tag : this._area.tag)
              && other_game_object.CompareTag(tag : this._actor.tag)) {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              UnityEngine.Debug.Log("Actor is outside area");
            }
            #endif

            this._overlapping = ActorOverlapping.Outside_area_;
          }

          if (child_game_object.CompareTag(tag : this._actor.tag)
              && other_game_object.CompareTag("Obstruction")) {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              UnityEngine.Debug.Log("Actor is not colliding");
            }
            #endif

            this._colliding = ActorColliding.Not_colliding_;
          }
        } else {
          if (child_game_object == this._area.gameObject
              && other_game_object.gameObject == this._actor.gameObject) {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              UnityEngine.Debug.Log("Actor is outside area");
            }
            #endif

            this._overlapping = ActorOverlapping.Outside_area_;
          }

          if (child_game_object == this._actor.gameObject && other_game_object.CompareTag("Obstruction")) {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              UnityEngine.Debug.Log("Actor is not colliding");
            }
            #endif

            this._colliding = ActorColliding.Not_colliding_;
          }
        }
      }
    }

    void OnCollisionEnterChild(UnityEngine.GameObject child_game_object, UnityEngine.Collision collision) { }

    void OnCollisionStayChild(UnityEngine.GameObject child_game_object, UnityEngine.Collision collision) { }

    void OnCollisionExitChild(UnityEngine.GameObject child_game_object, UnityEngine.Collision collision) { }
  }
}