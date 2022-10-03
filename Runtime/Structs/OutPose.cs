namespace droid.Runtime.Structs {
  public class OutPose {
    /// Encapsulates a rotation and a translation.  This is a convenience class that allows
    /// construction and value access either by Matrix4x4 or Quaternion + Vector3 types.
    /// Right-handed to left-handed matrix converter (and vice versa).
    protected static readonly UnityEngine.Matrix4x4 _FlipZ =
        UnityEngine.Matrix4x4.Scale(vector : new UnityEngine.Vector3(1, 1, -1));

    /// Default constructor.
    /// Initializes position to the origin and orientation to the identity rotation.
    public OutPose() {
      this.Position = UnityEngine.Vector3.zero;
      this.Orientation = UnityEngine.Quaternion.identity;
      this.Matrix = UnityEngine.Matrix4x4.identity;
    }

    /// Constructor that takes a Vector3 and a Quaternion.
    public OutPose(UnityEngine.Vector3 position, UnityEngine.Quaternion orientation) {
      this.Set(position : position, orientation : orientation);
    }

    /// Constructor that takes a Matrix4x4.
    public OutPose(UnityEngine.Matrix4x4 matrix) { this.Set(matrix : matrix); }

    /// The translation component of the pose.
    public UnityEngine.Vector3 Position { get; protected set; }

    /// The rotation component of the pose.
    public UnityEngine.Quaternion Orientation { get; protected set; }

    /// The pose as a matrix in Unity gameobject convention (left-handed).
    public UnityEngine.Matrix4x4 Matrix { get; protected set; }

    /// The pose as a matrix in right-handed coordinates.
    public UnityEngine.Matrix4x4 RightHandedMatrix { get { return _FlipZ * this.Matrix * _FlipZ; } }

    /// <summary>
    /// </summary>
    /// <param name="position"></param>
    /// <param name="orientation"></param>
    protected void Set(UnityEngine.Vector3 position, UnityEngine.Quaternion orientation) {
      this.Position = position;
      this.Orientation = orientation;
      this.Matrix = UnityEngine.Matrix4x4.TRS(pos : position, q : orientation, s : UnityEngine.Vector3.one);
    }

    /// <summary>
    /// </summary>
    /// <param name="matrix"></param>
    protected void Set(UnityEngine.Matrix4x4 matrix) {
      this.Matrix = matrix;
      this.Position = matrix.GetColumn(3);
      this.Orientation =
          UnityEngine.Quaternion.LookRotation(forward : matrix.GetColumn(2), upwards : matrix.GetColumn(1));
    }
  }

  /// <summary>
  /// </summary>
  public class MutableOutPose : OutPose {
    /// Sets the position and orientation from a Vector3 + Quaternion.
    public new void Set(UnityEngine.Vector3 position, UnityEngine.Quaternion orientation) {
      base.Set(position : position, orientation : orientation);
    }

    /// Sets the position and orientation from a Matrix4x4.
    public new void Set(UnityEngine.Matrix4x4 matrix) { base.Set(matrix : matrix); }

    /// Sets the position and orientation from a right-handed Matrix4x4.
    public void SetRightHanded(UnityEngine.Matrix4x4 matrix) { this.Set(matrix : _FlipZ * matrix * _FlipZ); }
  }
}