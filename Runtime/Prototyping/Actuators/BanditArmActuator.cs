namespace droid.Runtime.Prototyping.Actuators {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : ActuatorComponentMenuPath._ComponentMenuPath
                                           + "BanditArm"
                                           + ActuatorComponentMenuPath._Postfix)]
  public class BanditArmActuator : Actuator {
    [UnityEngine.SerializeField] UnityEngine.Material _material;

    public override string[] InnerMotionNames {
      get {
        return new[] {
                         "1",
                         "2",
                         "3",
                         "4"
                     };
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Setup() {
      var renderr = this.GetComponent<UnityEngine.Renderer>();
      if (renderr) {
        this._material = renderr.sharedMaterial;
      } else {
        var rendr = this.GetComponent<UnityEngine.CanvasRenderer>();
        if (rendr) {
          this._material = rendr.GetMaterial();
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    /// <exception cref="T:System.ArgumentOutOfRangeException"></exception>
    protected override void InnerApplyMotion(droid.Runtime.Interfaces.IMotion motion) {
      if (this._material) {
        switch ((int)motion.Strength) {
          case 1:
            this._material.color = UnityEngine.Color.blue;
            break;
          case 2:
            this._material.color = UnityEngine.Color.black;
            break;
          case 3:
            this._material.color = UnityEngine.Color.red;
            break;
          case 4:
            this._material.color = UnityEngine.Color.green;
            break;
          default:
            throw new System.ArgumentOutOfRangeException();
        }
      }
    }
  }
}