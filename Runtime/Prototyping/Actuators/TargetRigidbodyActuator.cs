namespace droid.Runtime.Prototyping.Actuators {
  /// <inheritdoc cref="Actuator" />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : ActuatorComponentMenuPath._ComponentMenuPath
                                           + "TargetRigidbody"
                                           + ActuatorComponentMenuPath._Postfix)]
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.Rigidbody))]
  public class TargetRigidbodyActuator : Actuator {
    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    protected UnityEngine.Rigidbody _Rigidbody;

    string _movement;
    droid.Runtime.Environments.Prototyping.AbstractPrototypingEnvironment _parent_environment;

    string _turn;

    /// <summary>
    /// </summary>
    public float MovementSpeed { get; set; }

    /// <summary>
    /// </summary>
    public float RotationSpeed { get; set; }

    public override string[] InnerMotionNames { get { return new[] {this._movement, this._turn}; } }

    /// <summary>
    /// </summary>
    public override void Tick() { this.OnStep(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Setup() {
      this._Rigidbody = this.GetComponent<UnityEngine.Rigidbody>();

      this._movement = this.Identifier + "Movement_";
      this._turn = this.Identifier + "Turn_";
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.Parent =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : (droid.Runtime.
                Interfaces.IHasRegister<droid.Runtime.Interfaces.IActuator>)this.Parent,
            c : (Actuator)this,
            identifier : this._movement);
      this.Parent =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : (droid.Runtime.
                Interfaces.IHasRegister<droid.Runtime.Interfaces.IActuator>)this.Parent,
            c : (Actuator)this,
            identifier : this._turn);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    protected override void InnerApplyMotion(droid.Runtime.Interfaces.IMotion motion) {
      if (motion.ActuatorName == this._movement) {
        this.ApplyMovement(movement_change : motion.Strength);
      } else if (motion.ActuatorName == this._turn) {
        this.ApplyRotation(rotation_change : motion.Strength);
      }
    }

    void ApplyRotation(float rotation_change = 0f) { this.RotationSpeed = rotation_change; }

    void ApplyMovement(float movement_change = 0f) { this.MovementSpeed = movement_change; }

    void OnStep() {
      this._Rigidbody.velocity = UnityEngine.Vector3.zero;
      this._Rigidbody.angularVelocity = UnityEngine.Vector3.zero;

      // Move
      var movement = this.MovementSpeed * UnityEngine.Time.deltaTime * this.transform.forward;
      this._Rigidbody.MovePosition(position : this._Rigidbody.position + movement);

      // Turn
      var turn = this.RotationSpeed * UnityEngine.Time.deltaTime;
      var turn_rotation = UnityEngine.Quaternion.Euler(0f, y : turn, 0f);
      this._Rigidbody.MoveRotation(rot : this._Rigidbody.rotation * turn_rotation);
    }
  }
}