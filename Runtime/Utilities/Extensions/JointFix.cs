namespace droid.Runtime.Utilities.Extensions {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.Joint))]
  public class JointFix : UnityEngine.MonoBehaviour {
    [UnityEngine.SerializeField] bool counting = false;
    [UnityEngine.SerializeField] int resetAfterFrames = 500;
    UnityEngine.JointDrive[] _angular_x_drive;
    UnityEngine.Rigidbody[] _connected_bodies;
    bool[] _enable_processings;
    float[] _force_break_limits;
    int _frames_counted;
    UnityEngine.Vector3 _initial_local_position;
    UnityEngine.Quaternion _initial_local_rotation;
    System.Type[] _joint_types;

    UnityEngine.Joint[] _joints;
    UnityEngine.JointLimits[] _limits;
    UnityEngine.SoftJointLimitSpring[] _linear_limit_springs;
    UnityEngine.SoftJointLimit[] _linear_limits;
    UnityEngine.Vector3 _local_position_on_disable;

    UnityEngine.Quaternion _local_rotation_on_disable;
    UnityEngine.JointDrive[] _slerp_drives;
    UnityEngine.Vector3[] _target_angulars;
    UnityEngine.Vector3[] _target_positions;
    UnityEngine.Quaternion[] _target_rotations;
    UnityEngine.Vector3[] _target_velocities;
    float[] _torque_break_limits;

    bool _was_disabled;
    UnityEngine.SoftJointLimit[] _x_ang_high_limits;
    UnityEngine.SoftJointLimit[] _x_ang_low_limits;
    UnityEngine.ConfigurableJointMotion[] _x_ang_motion;
    UnityEngine.JointDrive[] _x_drives;
    UnityEngine.ConfigurableJointMotion[] _x_motion;
    UnityEngine.ConfigurableJointMotion[] _y_ang_motion;
    UnityEngine.JointDrive[] _y_drives;
    UnityEngine.ConfigurableJointMotion[] _y_motion;
    UnityEngine.ConfigurableJointMotion[] _z_ang_motion;
    UnityEngine.JointDrive[] _z_drives;
    UnityEngine.ConfigurableJointMotion[] _z_motion;

    void Awake() { this.Setup(); }

    void Update() {
      if (this._frames_counted >= this.resetAfterFrames) {
        this.JointReset();
        this._frames_counted = 0;
      }

      if (this.counting) {
        this._frames_counted++;
      }

      if (this._was_disabled) {
        this._was_disabled = false;
        var transform1 = this.transform;
        transform1.localRotation = this._local_rotation_on_disable;
        transform1.localPosition = this._local_position_on_disable;
      }
    }

    void OnDisable() {
      var transform1 = this.transform;
      this._local_rotation_on_disable = transform1.localRotation;
      transform1.localRotation = this._initial_local_rotation;

      this._local_position_on_disable = transform1.localPosition;
      transform1.localPosition = this._initial_local_position;

      this._was_disabled = true;
    }

    void Setup() {
      var transform1 = this.transform;
      this._initial_local_rotation = transform1.localRotation;
      this._initial_local_position = transform1.localPosition;
      this._joints = this.GetComponents<UnityEngine.Joint>();
      var length = this._joints.Length;
      this._connected_bodies = new UnityEngine.Rigidbody[length];
      this._joint_types = new System.Type[length];
      this._x_ang_low_limits = new UnityEngine.SoftJointLimit[length];
      this._x_ang_high_limits = new UnityEngine.SoftJointLimit[length];
      this._limits = new UnityEngine.JointLimits[length];
      this._force_break_limits = new float[length];
      this._torque_break_limits = new float[length];
      this._x_motion = new UnityEngine.ConfigurableJointMotion[length];
      this._y_motion = new UnityEngine.ConfigurableJointMotion[length];
      this._z_motion = new UnityEngine.ConfigurableJointMotion[length];
      this._x_ang_motion = new UnityEngine.ConfigurableJointMotion[length];
      this._y_ang_motion = new UnityEngine.ConfigurableJointMotion[length];
      this._z_ang_motion = new UnityEngine.ConfigurableJointMotion[length];
      this._angular_x_drive = new UnityEngine.JointDrive[length];
      this._target_rotations = new UnityEngine.Quaternion[length];
      this._target_angulars = new UnityEngine.Vector3[length];
      this._target_positions = new UnityEngine.Vector3[length];
      this._target_velocities = new UnityEngine.Vector3[length];
      this._x_drives = new UnityEngine.JointDrive[length];
      this._y_drives = new UnityEngine.JointDrive[length];
      this._z_drives = new UnityEngine.JointDrive[length];
      this._linear_limits = new UnityEngine.SoftJointLimit[length];
      this._linear_limit_springs = new UnityEngine.SoftJointLimitSpring[length];
      this._slerp_drives = new UnityEngine.JointDrive[length];
      this._enable_processings = new bool[length];

      for (var i = 0; i < length; i++) {
        this._connected_bodies[i] = this._joints[i].connectedBody;
        this._joint_types[i] = this._joints[i].GetType();
        this._force_break_limits[i] = this._joints[i].breakForce;
        this._torque_break_limits[i] = this._joints[i].breakTorque;
        this._enable_processings[i] = this._joints[i].enablePreprocessing;
        if (this._joints[i] is UnityEngine.HingeJoint) {
          this._limits[i] = ((UnityEngine.HingeJoint)this._joints[i]).limits;
        } else if (this._joints[i] is UnityEngine.ConfigurableJoint) {
          this._x_ang_low_limits[i] = ((UnityEngine.ConfigurableJoint)this._joints[i]).lowAngularXLimit;
          this._x_ang_high_limits[i] = ((UnityEngine.ConfigurableJoint)this._joints[i]).highAngularXLimit;
          this._x_motion[i] = ((UnityEngine.ConfigurableJoint)this._joints[i]).xMotion;
          this._y_motion[i] = ((UnityEngine.ConfigurableJoint)this._joints[i]).yMotion;
          this._z_motion[i] = ((UnityEngine.ConfigurableJoint)this._joints[i]).zMotion;
          this._x_ang_motion[i] = ((UnityEngine.ConfigurableJoint)this._joints[i]).angularXMotion;
          this._y_ang_motion[i] = ((UnityEngine.ConfigurableJoint)this._joints[i]).angularYMotion;
          this._z_ang_motion[i] = ((UnityEngine.ConfigurableJoint)this._joints[i]).angularZMotion;
          this._angular_x_drive[i] = ((UnityEngine.ConfigurableJoint)this._joints[i]).angularXDrive;
          this._target_rotations[i] = ((UnityEngine.ConfigurableJoint)this._joints[i]).targetRotation;
          this._linear_limits[i] = ((UnityEngine.ConfigurableJoint)this._joints[i]).linearLimit;
          this._slerp_drives[i] = ((UnityEngine.ConfigurableJoint)this._joints[i]).slerpDrive;
          this._linear_limit_springs[i] = ((UnityEngine.ConfigurableJoint)this._joints[i]).linearLimitSpring;
          this._x_drives[i] = ((UnityEngine.ConfigurableJoint)this._joints[i]).xDrive;
          this._y_drives[i] = ((UnityEngine.ConfigurableJoint)this._joints[i]).yDrive;
          this._z_drives[i] = ((UnityEngine.ConfigurableJoint)this._joints[i]).zDrive;
          this._target_positions[i] = ((UnityEngine.ConfigurableJoint)this._joints[i]).targetPosition;
          this._target_velocities[i] = ((UnityEngine.ConfigurableJoint)this._joints[i]).targetVelocity;
          this._target_angulars[i] = ((UnityEngine.ConfigurableJoint)this._joints[i]).targetAngularVelocity;
        }
      }
    }

    /// <summary>
    /// </summary>
    public void JointReset() {
      if (this._joints == null) {
        this.Setup();
      }

      var transform1 = this.transform;
      transform1.localRotation = this._initial_local_rotation;
      transform1.localPosition = this._initial_local_position;
      for (var i = 0; i < this._joints.Length; i++) {
        if (this._joints[i] == null) {
          this._joints[i] =
              (UnityEngine.Joint)this.gameObject.AddComponent(componentType : this._joint_types[i]);
          this._joints[i].connectedBody = this._connected_bodies[i];
        }

        this._joints[i].breakForce = this._force_break_limits[i];
        this._joints[i].breakTorque = this._torque_break_limits[i];
        this._joints[i].enablePreprocessing = this._enable_processings[i];

        if (this._joints[i] is UnityEngine.HingeJoint) {
          ((UnityEngine.HingeJoint)this._joints[i]).limits = this._limits[i];
        } else if (this._joints[i] is UnityEngine.SpringJoint) {
          //((SpringJoint)this._joints[i]).anchor = this.anchor[i];
        } else if (this._joints[i] is UnityEngine.ConfigurableJoint) {
          ((UnityEngine.ConfigurableJoint)this._joints[i]).lowAngularXLimit = this._x_ang_low_limits[i];
          ((UnityEngine.ConfigurableJoint)this._joints[i]).highAngularXLimit = this._x_ang_high_limits[i];
          ((UnityEngine.ConfigurableJoint)this._joints[i]).xMotion = this._x_motion[i];
          ((UnityEngine.ConfigurableJoint)this._joints[i]).yMotion = this._y_motion[i];
          ((UnityEngine.ConfigurableJoint)this._joints[i]).zMotion = this._z_motion[i];
          ((UnityEngine.ConfigurableJoint)this._joints[i]).angularXMotion = this._x_ang_motion[i];
          ((UnityEngine.ConfigurableJoint)this._joints[i]).angularYMotion = this._y_ang_motion[i];
          ((UnityEngine.ConfigurableJoint)this._joints[i]).angularZMotion = this._z_ang_motion[i];
          ((UnityEngine.ConfigurableJoint)this._joints[i]).angularXDrive = this._angular_x_drive[i];
          ((UnityEngine.ConfigurableJoint)this._joints[i]).targetRotation = this._target_rotations[i];
          ((UnityEngine.ConfigurableJoint)this._joints[i]).linearLimit = this._linear_limits[i];
          ((UnityEngine.ConfigurableJoint)this._joints[i]).linearLimitSpring = this._linear_limit_springs[i];
          ((UnityEngine.ConfigurableJoint)this._joints[i]).xDrive = this._x_drives[i];
          ((UnityEngine.ConfigurableJoint)this._joints[i]).yDrive = this._y_drives[i];
          ((UnityEngine.ConfigurableJoint)this._joints[i]).zDrive = this._z_drives[i];
          ((UnityEngine.ConfigurableJoint)this._joints[i]).targetPosition = this._target_positions[i];
          ((UnityEngine.ConfigurableJoint)this._joints[i]).targetVelocity = this._target_velocities[i];
          ((UnityEngine.ConfigurableJoint)this._joints[i]).targetAngularVelocity = this._target_angulars[i];
          ((UnityEngine.ConfigurableJoint)this._joints[i]).slerpDrive = this._slerp_drives[i];
        }
      }
    }
  }
}