namespace droid.Runtime.Prototyping.Actuators.Discrete {
  /// <inheritdoc />
  /// <summary>
  ///   A register of functionality to performed on Rigidbody
  ///   es
  /// </summary>
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.Rigidbody))]
  public class DiscreteRigidbodyMotionRegister : UnityEngine.MonoBehaviour {
    [UnityEngine.SerializeField] float force_unit = 1;
    [UnityEngine.SerializeField] float torque_unit = 90;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    protected UnityEngine.Space _Relative_To = UnityEngine.Space.Self;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    protected UnityEngine.ForceMode _force_mode = UnityEngine.ForceMode.Acceleration;

    UnityEngine.Rigidbody _rb;

    void Awake() { this._rb = this.GetComponent<UnityEngine.Rigidbody>(); }

    void AddForce(UnityEngine.Vector3 vec) {
      if (this._rb) {
        switch (this._Relative_To) {
          case UnityEngine.Space.World:
            this._rb.AddForce(force : vec, mode : this._force_mode);
            break;
          case UnityEngine.Space.Self:
            this._rb.AddRelativeForce(force : vec, mode : this._force_mode);
            break;
          default: throw new System.ArgumentOutOfRangeException();
        }
      }
    }

    void AddTorque(UnityEngine.Vector3 vec) {
      if (this._rb) {
        switch (this._Relative_To) {
          case UnityEngine.Space.World:
            this._rb.AddTorque(torque : vec, mode : this._force_mode);
            break;
          case UnityEngine.Space.Self:
            this._rb.AddRelativeTorque(torque : vec, mode : this._force_mode);
            break;
          default: throw new System.ArgumentOutOfRangeException();
        }
      }
    }

    /// <summary>
    ///   Self explanatory
    /// </summary>
    public void AddUnitForceForward() { this.AddForce(vec : UnityEngine.Vector3.forward * this.force_unit); }

    /// <summary>
    ///   Self explanatory
    /// </summary>
    public void AddUnitForceBackward() { this.AddForce(vec : UnityEngine.Vector3.back * this.force_unit); }

    /// <summary>
    ///   Self explanatory
    /// </summary>
    public void AddUnitForceLeft() { this.AddForce(vec : UnityEngine.Vector3.left * this.force_unit); }

    /// <summary>
    ///   Self explanatory
    /// </summary>
    public void AddUnitForceRight() { this.AddForce(vec : UnityEngine.Vector3.right * this.force_unit); }

    /// <summary>
    ///   Self explanatory
    /// </summary>
    public void AddUnitForceUp() { this.AddForce(vec : UnityEngine.Vector3.up * this.force_unit); }

    /// <summary>
    ///   Self explanatory
    /// </summary>
    public void AddUnitForceDown() { this.AddForce(vec : UnityEngine.Vector3.down * this.force_unit); }

    /// <summary>
    ///   Self explanatory
    /// </summary>
    public void AddUnitTorqueYClockWise() { this.AddTorque(vec : UnityEngine.Vector3.up * this.torque_unit); }

    /// <summary>
    ///   Self explanatory
    /// </summary>
    public void AddUnitTorqueYAntiClockWise() {
      this.AddTorque(vec : UnityEngine.Vector3.up * -this.torque_unit);
    }

    /// <summary>
    ///   Self explanatory
    /// </summary>
    public void AddUnitTorqueXClockWise() {
      this.AddTorque(vec : UnityEngine.Vector3.left * this.torque_unit);
    }

    /// <summary>
    ///   Self explanatory
    /// </summary>
    public void AddUnitTorqueXAntiClockWise() {
      this.AddTorque(vec : UnityEngine.Vector3.left * -this.torque_unit);
    }

    /// <summary>
    ///   Self explanatory
    /// </summary>
    public void AddUnitTorqueZClockWise() {
      this.AddTorque(vec : UnityEngine.Vector3.forward * -this.torque_unit);
    }

    /// <summary>
    ///   Self explanatory
    /// </summary>
    public void AddUnitTorqueZAntiClockWise() {
      this.AddTorque(vec : UnityEngine.Vector3.forward * this.torque_unit);
    }
  }
}