namespace droid.Runtime.Prototyping.Configurables.Transforms {
  [UnityEngine.AddComponentMenu(menuName : ConfigurableComponentMenuPath._ComponentMenuPath
                                           + "QuaternionTransform"
                                           + ConfigurableComponentMenuPath._Postfix)]
  public class QuaternionTransformConfigurable : Configurable,
                                                 droid.Runtime.Interfaces.IHasQuaternionTransform {
    [UnityEngine.HeaderAttribute("Specific", order = 102)]
    [UnityEngine.SerializeField]
    UnityEngine.Vector3 _position;

    [UnityEngine.SerializeField] UnityEngine.Quaternion _rotation;

    string _pos_x = "pos_x";
    string _pos_y = "pos_y";
    string _pos_z = "pos_z";

    string _rot_w = "row_w";
    string _rot_x = "rot_x";
    string _rot_y = "rot_y";
    string _rot_z = "rot_z";

    public droid.Runtime.Interfaces.ISamplable ConfigurableValueSpace { get; }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public UnityEngine.Quaternion Rotation { get { return this._rotation; } }

    public droid.Runtime.Structs.Space.Space3 PositionSpace { get; } //TODO: Implement
    public droid.Runtime.Structs.Space.Space4 RotationSpace { get; } //TODO: Implement

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public UnityEngine.Vector3 Position { get { return this._position; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
      //TODO: use envs bound extent if available for space

      this._pos_x = this.Identifier + "pos_x";
      this._pos_y = this.Identifier + "pos_y";
      this._pos_z = this.Identifier + "pos_z";

      this._rot_x = this.Identifier + "rot_x";
      this._rot_y = this.Identifier + "rot_y";
      this._rot_z = this.Identifier + "rot_z";
      this._rot_w = this.Identifier + "row_w";
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() { throw new System.NotImplementedException(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() { throw new System.NotImplementedException(); }

    public override void PrototypingReset() {
      base.PrototypingReset();
      var transform1 = this.transform;
      transform1.position = this._position;
      transform1.rotation = this._rotation;
    }

    public override void UpdateCurrentConfiguration() { }

    public override droid.Runtime.Messaging.Messages.Configuration[] SampleConfigurations() {
      return new[] {
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this.Identifier,
                         configurable_value : this.ConfigurableValueSpace.Sample())
                   };
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="obj"></param>
    public override void ApplyConfiguration(droid.Runtime.Interfaces.IConfigurableConfiguration obj) {
      //TODO: Denormalize configuration if space is marked as normalised

      if (obj.ConfigurableName == this._pos_x) {
        this._position.x = obj.ConfigurableValue;
      } else if (obj.ConfigurableName == this._pos_y) {
        this._position.y = obj.ConfigurableValue;
      } else if (obj.ConfigurableName == this._pos_z) {
        this._position.z = obj.ConfigurableValue;
      } else if (obj.ConfigurableName == this._rot_x) {
        this._rotation.x = obj.ConfigurableValue;
      } else if (obj.ConfigurableName == this._rot_y) {
        this._rotation.y = obj.ConfigurableValue;
      } else if (obj.ConfigurableName == this._rot_z) {
        this._rotation.z = obj.ConfigurableValue;
      } else if (obj.ConfigurableName == this._rot_w) {
        this._rotation.w = obj.ConfigurableValue;
      }

      this.PrototypingReset();
    }
  }
}