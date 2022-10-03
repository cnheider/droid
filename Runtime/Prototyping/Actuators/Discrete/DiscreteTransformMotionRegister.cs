namespace droid.Runtime.Prototyping.Actuators.Discrete {
  /// <inheritdoc />
  /// <summary>
  ///   A register of functionality to performed on transforms
  /// </summary>
  public class DiscreteTransformMotionRegister : UnityEngine.MonoBehaviour {
    [UnityEngine.SerializeField] float translation_unit = 1;
    [UnityEngine.SerializeField] float rotation_unit = 90;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    protected string _Layer_Mask = "Obstructions";

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    protected bool _No_Collisions = true;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    protected UnityEngine.Space _Relative_To = UnityEngine.Space.Self;

    void CheckCollisionTranslate(UnityEngine.Vector3 vec) {
      var layer_mask = 1 << UnityEngine.LayerMask.NameToLayer(layerName : this._Layer_Mask);
      if (this._No_Collisions) {
        if (!UnityEngine.Physics.Raycast(origin : this.transform.position,
                                         direction :
                                         this.transform.TransformDirection(direction : vec.normalized),
                                         maxDistance : UnityEngine.Mathf.Abs(f : vec.magnitude),
                                         layerMask : layer_mask)) {
          this.transform.Translate(translation : vec, relativeTo : this._Relative_To);
        }
      } else {
        this.transform.Translate(translation : vec, relativeTo : this._Relative_To);
      }
    }

    /// <summary>
    ///   Self explanatory
    /// </summary>
    public void TranslateUnitForward() {
      this.CheckCollisionTranslate(vec : UnityEngine.Vector3.forward * this.translation_unit);
    }

    /// <summary>
    ///   Self explanatory
    /// </summary>
    public void TranslateUnitBackward() {
      this.CheckCollisionTranslate(vec : UnityEngine.Vector3.back * this.translation_unit);
    }

    /// <summary>
    ///   Self explanatory
    /// </summary>
    public void TranslateUnitLeft() {
      this.CheckCollisionTranslate(vec : UnityEngine.Vector3.left * this.translation_unit);
    }

    /// <summary>
    ///   Self explanatory
    /// </summary>
    public void TranslateUnitRight() {
      this.CheckCollisionTranslate(vec : UnityEngine.Vector3.right * this.translation_unit);
    }

    /// <summary>
    ///   Self explanatory
    /// </summary>
    public void TranslateUnitUp() {
      this.CheckCollisionTranslate(vec : UnityEngine.Vector3.up * this.translation_unit);
    }

    /// <summary>
    ///   Self explanatory
    /// </summary>
    public void TranslateUnitDown() {
      this.CheckCollisionTranslate(vec : UnityEngine.Vector3.down * this.translation_unit);
    }

    /// <summary>
    ///   Self explanatory
    /// </summary>
    public void RotateUnitYClockWise() {
      this.transform.Rotate(axis : UnityEngine.Vector3.up, angle : this.rotation_unit);
    }

    /// <summary>
    ///   Self explanatory
    /// </summary>
    public void RotateUnitYAntiClockWise() {
      this.transform.Rotate(axis : UnityEngine.Vector3.up, angle : -this.rotation_unit);
    }

    /// <summary>
    ///   Self explanatory
    /// </summary>
    public void RotateUnitXClockWise() {
      this.transform.Rotate(axis : UnityEngine.Vector3.left, angle : this.rotation_unit);
    }

    /// <summary>
    ///   Self explanatory
    /// </summary>
    public void RotateUnitXAntiClockWise() {
      this.transform.Rotate(axis : UnityEngine.Vector3.left, angle : -this.rotation_unit);
    }

    /// <summary>
    ///   Self explanatory
    /// </summary>
    public void RotateUnitZClockWise() {
      this.transform.Rotate(axis : UnityEngine.Vector3.forward, angle : -this.rotation_unit);
    }

    /// <summary>
    ///   Self explanatory
    /// </summary>
    public void RotateUnitZAntiClockWise() {
      this.transform.Rotate(axis : UnityEngine.Vector3.forward, angle : this.rotation_unit);
    }
  }
}