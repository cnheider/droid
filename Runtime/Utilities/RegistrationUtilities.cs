namespace droid.Runtime.Utilities {
  /// <summary>
  /// </summary>
  public static class NeodroidRegistrationUtilities {
    /// <summary>
    /// </summary>
    /// <param name="caller"></param>
    /// <param name="parent"></param>
    /// <param name="on_collision_enter_child"></param>
    /// <param name="on_trigger_enter_child"></param>
    /// <param name="on_collision_exit_child"></param>
    /// <param name="on_trigger_exit_child"></param>
    /// <param name="on_collision_stay_child"></param>
    /// <param name="on_trigger_stay_child"></param>
    /// <param name="debug"></param>
    /// <typeparam name="TChildColliderSensor"></typeparam>
    /// <typeparam name="TCollider"></typeparam>
    /// <typeparam name="TCollision"></typeparam>
    public static void
        RegisterCollisionTriggerCallbacksOnChildren<TChildColliderSensor, TCollider, TCollision>(
            UnityEngine.Component caller,
            UnityEngine.Transform parent,
            droid.Runtime.GameObjects.ChildSensors.ChildColliderSensor<TCollider, TCollision>.
                OnChildCollisionEnterDelegate on_collision_enter_child = null,
            droid.Runtime.GameObjects.ChildSensors.ChildColliderSensor<TCollider, TCollision>.
                OnChildTriggerEnterDelegate on_trigger_enter_child = null,
            droid.Runtime.GameObjects.ChildSensors.ChildColliderSensor<TCollider, TCollision>.
                OnChildCollisionExitDelegate on_collision_exit_child = null,
            droid.Runtime.GameObjects.ChildSensors.ChildColliderSensor<TCollider, TCollision>.
                OnChildTriggerExitDelegate on_trigger_exit_child = null,
            droid.Runtime.GameObjects.ChildSensors.ChildColliderSensor<TCollider, TCollision>.
                OnChildCollisionStayDelegate on_collision_stay_child = null,
            droid.Runtime.GameObjects.ChildSensors.ChildColliderSensor<TCollider, TCollision>.
                OnChildTriggerStayDelegate on_trigger_stay_child = null,
            bool debug = false)
        where TChildColliderSensor :
        droid.Runtime.GameObjects.ChildSensors.ChildColliderSensor<TCollider, TCollision>
        where TCollider : UnityEngine.Component {
      var children_with_colliders = parent.GetComponentsInChildren<TCollider>();

      //TODO add check and warning for not all callbacks = null

      for (var index = 0; index < children_with_colliders.Length; index++) {
        var child = children_with_colliders[index];
        var child_sensors = child.GetComponents<TChildColliderSensor>();
        droid.Runtime.GameObjects.ChildSensors.ChildColliderSensor<TCollider, TCollision> collider_sensor =
            null;
        for (var i = 0; i < child_sensors.Length; i++) {
          var child_sensor = child_sensors[i];
          if (child_sensor.Caller != null && child_sensor.Caller == caller) {
            collider_sensor = child_sensor;
            break;
          }

          if (child_sensor.Caller == null) {
            child_sensor.Caller = caller;
            collider_sensor = child_sensor;
            break;
          }
        }

        if (collider_sensor == null) {
          collider_sensor = child.gameObject.AddComponent<TChildColliderSensor>();
          collider_sensor.Caller = caller;
        }

        if (on_collision_enter_child != null) {
          collider_sensor.OnCollisionEnterDelegate = on_collision_enter_child;
        }

        if (on_trigger_enter_child != null) {
          collider_sensor.OnTriggerEnterDelegate = on_trigger_enter_child;
        }

        if (on_collision_exit_child != null) {
          collider_sensor.OnCollisionExitDelegate = on_collision_exit_child;
        }

        if (on_trigger_exit_child != null) {
          collider_sensor.OnTriggerExitDelegate = on_trigger_exit_child;
        }

        if (on_trigger_stay_child != null) {
          collider_sensor.OnTriggerStayDelegate = on_trigger_stay_child;
        }

        if (on_collision_stay_child != null) {
          collider_sensor.OnCollisionStayDelegate = on_collision_stay_child;
        }

        #if NEODROID_DEBUG
        if (debug) {
          UnityEngine.Debug.Log(message :
                                $"{caller.name} has created {collider_sensor.name} on {child.name} under parent {parent.name}");
        }
        #endif
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="r"></param>
    /// <param name="c"></param>
    /// <param name="only_parents"></param>
    /// <param name="debug"></param>
    /// <typeparam name="TCaller"></typeparam>
    /// <returns></returns>
    public static droid.Runtime.Interfaces.IHasRegister<droid.Runtime.Interfaces.IActuator>
        RegisterComponent<TCaller>(
            droid.Runtime.Interfaces.IHasRegister<droid.Runtime.Interfaces.IActuator> r,
            TCaller c,
            bool only_parents = false,
            bool debug = false)
        where TCaller : UnityEngine.Component, droid.Runtime.Interfaces.IRegisterable {
      droid.Runtime.Interfaces.IHasRegister<droid.Runtime.Interfaces.IActuator> component = null;
      if (r != null) {
        component = r; //.GetComponent<Recipient>();
      } else {
        if (c.GetComponentInParent<droid.Runtime.Prototyping.Actors.Actor>() != null) {
          component = c.GetComponentInParent<droid.Runtime.Prototyping.Actors.Actor>();
        } else if (!only_parents) {
          component = UnityEngine.Object.FindObjectOfType<droid.Runtime.Prototyping.Actors.Actor>();
        }
      }

      if (component == null) {
        if (c.GetComponentInParent<droid.Runtime.Environments.Prototyping.PrototypingEnvironment>() != null) {
          component = c.GetComponentInParent<droid.Runtime.Environments.Prototyping.PrototypingEnvironment>();
        } else if (!only_parents) {
          component = UnityEngine.Object
                                 .FindObjectOfType<
                                     droid.Runtime.Environments.Prototyping.PrototypingEnvironment>();
        }
      }

      if (component != null) {
        component.Register(obj : (droid.Runtime.Interfaces.IActuator)c);
      } else {
        #if NEODROID_DEBUG
        if (debug) {
          UnityEngine.Debug.Log("Could not find a IHasRegister<IActuator> recipient during registration");
        }
        #endif
      }

      return component;
    }

    /// <summary>
    /// </summary>
    /// <param name="r"></param>
    /// <param name="c"></param>
    /// <param name="identifier"></param>
    /// <param name="only_parents"></param>
    /// <param name="debug"></param>
    /// <typeparam name="TRecipient"></typeparam>
    /// <typeparam name="TCaller"></typeparam>
    /// <returns></returns>
    public static droid.Runtime.Interfaces.IHasRegister<droid.Runtime.Interfaces.IActuator>
        RegisterComponent<TCaller>(
            droid.Runtime.Interfaces.IHasRegister<droid.Runtime.Interfaces.IActuator> r,
            TCaller c,
            string identifier,
            bool only_parents = false,
            bool debug = false)
        where TCaller : UnityEngine.Component, droid.Runtime.Interfaces.IRegisterable {
      droid.Runtime.Interfaces.IHasRegister<droid.Runtime.Interfaces.IActuator> component = null;
      if (r != null) {
        component = r; //.GetComponent<Recipient>();
      } else {
        if (c.GetComponentInParent<droid.Runtime.Prototyping.Actors.Actor>() != null) {
          component = c.GetComponentInParent<droid.Runtime.Prototyping.Actors.Actor>();
        } else if (!only_parents) {
          component = UnityEngine.Object.FindObjectOfType<droid.Runtime.Prototyping.Actors.Actor>();
        }
      }

      if (component == null) {
        if (c.GetComponentInParent<droid.Runtime.Environments.Prototyping.PrototypingEnvironment>() != null) {
          component = c.GetComponentInParent<droid.Runtime.Environments.Prototyping.PrototypingEnvironment>();
        } else if (!only_parents) {
          component = UnityEngine.Object
                                 .FindObjectOfType<
                                     droid.Runtime.Environments.Prototyping.PrototypingEnvironment>();
        }
      }

      if (component != null) {
        component.Register(obj : (droid.Runtime.Interfaces.IActuator)c, identifier : identifier);
      } else {
        #if NEODROID_DEBUG
        if (debug) {
          UnityEngine.Debug.Log("Could not find a IHasRegister<IActuator> recipient during registration");
        }
        #endif
      }

      return component;
    }

    /// <summary>
    /// </summary>
    /// <param name="r"></param>
    /// <param name="c"></param>
    /// <param name="only_parents"></param>
    /// <param name="debug"></param>
    /// <typeparam name="TRecipient"></typeparam>
    /// <typeparam name="TCaller"></typeparam>
    /// <returns></returns>
    public static TRecipient RegisterComponent<TRecipient, TCaller>(
        TRecipient r,
        TCaller c,
        bool only_parents = false,
        bool debug = false)
        where TRecipient : UnityEngine.Object, droid.Runtime.Interfaces.IHasRegister<TCaller>
        where TCaller : UnityEngine.Component, droid.Runtime.Interfaces.IRegisterable {
      TRecipient component = null;
      if (r != null) {
        component = r; //.GetComponent<Recipient>();
      } else if (c.GetComponentInParent<TRecipient>() != null) {
        component = c.GetComponentInParent<TRecipient>();
      } else if (!only_parents) {
        component = UnityEngine.Object.FindObjectOfType<TRecipient>();
      }

      if (component != null) {
        component.Register(obj : c);
      } else {
        #if NEODROID_DEBUG
        if (debug) {
          UnityEngine.Debug.Log(message :
                                $"Could not find a {typeof(TRecipient)} recipient during registration");
        }
        #endif
      }

      return component;
    }

    /// <summary>
    /// </summary>
    /// <param name="r"></param>
    /// <param name="c"></param>
    /// <param name="identifier"></param>
    /// <param name="only_parents"></param>
    /// <param name="debug"></param>
    /// <typeparam name="TRecipient"></typeparam>
    /// <typeparam name="TCaller"></typeparam>
    /// <returns></returns>
    public static TRecipient RegisterComponent<TRecipient, TCaller>(
        TRecipient r,
        TCaller c,
        string identifier,
        bool only_parents = false,
        bool debug = false)
        where TRecipient : UnityEngine.Object, droid.Runtime.Interfaces.IHasRegister<TCaller>
        where TCaller : UnityEngine.Component, droid.Runtime.Interfaces.IRegisterable {
      TRecipient component = null;
      if (r != null) {
        component = r;
      } else if (c.GetComponentInParent<TRecipient>() != null) {
        component = c.GetComponentInParent<TRecipient>();
      } else if (!only_parents) {
        component = UnityEngine.Object.FindObjectOfType<TRecipient>();
      }

      if (component != null) {
        component.Register(obj : c, identifier : identifier);
      } else {
        #if NEODROID_DEBUG
        if (debug) {
          UnityEngine.Debug.Log(message :
                                $"Could not find a {typeof(TRecipient)} recipient during registration");
        }

        #endif
      }

      return component;
    }
  }
}