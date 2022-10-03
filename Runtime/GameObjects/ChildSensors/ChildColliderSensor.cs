namespace droid.Runtime.GameObjects.ChildSensors {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public abstract class ChildColliderSensor<TCollider, TCollision> : UnityEngine.MonoBehaviour
      where TCollider : UnityEngine.Component {
    /// <summary>
    /// </summary>
    /// <param name="child_sensor_game_object"></param>
    /// <param name="collision"></param>
    public delegate void OnChildCollisionEnterDelegate(UnityEngine.GameObject child_sensor_game_object,
                                                       TCollision collision);

    /// <summary>
    /// </summary>
    /// <param name="child_sensor_game_object"></param>
    /// <param name="collision"></param>
    public delegate void OnChildCollisionExitDelegate(UnityEngine.GameObject child_sensor_game_object,
                                                      TCollision collision);

    /// <summary>
    /// </summary>
    /// <param name="child_sensor_game_object"></param>
    /// <param name="collision"></param>
    public delegate void OnChildCollisionStayDelegate(UnityEngine.GameObject child_sensor_game_object,
                                                      TCollision collision);

    /// <summary>
    /// </summary>
    /// <param name="child_sensor_game_object"></param>
    /// <param name="collider"></param>
    public delegate void OnChildTriggerEnterDelegate(UnityEngine.GameObject child_sensor_game_object,
                                                     TCollider collider);

    /// <summary>
    /// </summary>
    /// <param name="child_sensor_game_object"></param>
    /// <param name="collider"></param>
    public delegate void OnChildTriggerExitDelegate(UnityEngine.GameObject child_sensor_game_object,
                                                    TCollider collider);

    /// <summary>
    /// </summary>
    /// <param name="child_sensor_game_object"></param>
    /// <param name="collider"></param>
    public delegate void OnChildTriggerStayDelegate(UnityEngine.GameObject child_sensor_game_object,
                                                    TCollider collider);

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    protected UnityEngine.Component _caller;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    protected OnChildCollisionEnterDelegate _on_collision_enter_delegate;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    protected OnChildCollisionExitDelegate _on_collision_exit_delegate;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    protected OnChildCollisionStayDelegate _on_collision_stay_delegate;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    protected OnChildTriggerEnterDelegate _on_trigger_enter_delegate;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    protected OnChildTriggerExitDelegate _on_trigger_exit_delegate;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    protected OnChildTriggerStayDelegate _on_trigger_stay_delegate;

    /// <summary>
    /// </summary>
    public OnChildCollisionEnterDelegate OnCollisionEnterDelegate {
      set { this._on_collision_enter_delegate = value; }
    }

    /// <summary>
    /// </summary>
    public OnChildTriggerEnterDelegate OnTriggerEnterDelegate {
      set { this._on_trigger_enter_delegate = value; }
    }

    /// <summary>
    /// </summary>
    public OnChildTriggerStayDelegate OnTriggerStayDelegate {
      set { this._on_trigger_stay_delegate = value; }
    }

    /// <summary>
    /// </summary>
    public OnChildCollisionStayDelegate OnCollisionStayDelegate {
      set { this._on_collision_stay_delegate = value; }
    }

    /// <summary>
    /// </summary>
    public OnChildCollisionExitDelegate OnCollisionExitDelegate {
      set { this._on_collision_exit_delegate = value; }
    }

    /// <summary>
    /// </summary>
    public OnChildTriggerExitDelegate OnTriggerExitDelegate {
      set { this._on_trigger_exit_delegate = value; }
    }

    /// <summary>
    /// </summary>
    public UnityEngine.Component Caller { get { return this._caller; } set { this._caller = value; } }
  }
}