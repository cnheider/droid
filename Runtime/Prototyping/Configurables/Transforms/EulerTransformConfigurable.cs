namespace droid.Runtime.Prototyping.Configurables.Transforms {
  /// <inheritdoc cref="Configurable" />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : ConfigurableComponentMenuPath._ComponentMenuPath
                                           + "EulerTransform"
                                           + ConfigurableComponentMenuPath._Postfix)]
  public class EulerTransformConfigurable : SpatialConfigurable,
                                            droid.Runtime.Interfaces.IHasEulerTransform {
    [UnityEngine.HeaderAttribute("Observation", order = 103)]
    [UnityEngine.SerializeField]
    UnityEngine.Vector3 _position;

    [UnityEngine.SerializeField] UnityEngine.Vector3 _direction;
    [UnityEngine.SerializeField] UnityEngine.Vector3 _rotation;

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Sample.SampleSpace3 pos_space =
        new droid.Runtime.Structs.Space.Sample.SampleSpace3();

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Sample.SampleSpace3 dir_space =
        new droid.Runtime.Structs.Space.Sample.SampleSpace3();

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Sample.SampleSpace3 rot_space =
        new droid.Runtime.Structs.Space.Sample.SampleSpace3();

    string _dir_x;
    string _dir_y;
    string _dir_z;

    string _rot_x;
    string _rot_y;
    string _rot_z;

    string _x;
    string _y;
    string _z;

    /// <summary>
    /// </summary>
    public droid.Runtime.Interfaces.ISamplable ConfigurableValueSpace {
      get {
        return this.pos_space;
        //return DirectionSpace;
        //return RotationSpace;
      }
    }

    /// <summary>
    /// </summary>
    public UnityEngine.Vector3 Position { get { return this._position; } set { this._position = value; } }

    /// <summary>
    /// </summary>
    public UnityEngine.Vector3 Direction { get { return this._direction; } set { this._direction = value; } }

    /// <summary>
    /// </summary>
    public UnityEngine.Vector3 Rotation { get { return this._rotation; } set { this._rotation = value; } }

    /// <summary>
    /// </summary>
    public droid.Runtime.Structs.Space.Space3 PositionSpace {
      get { return (droid.Runtime.Structs.Space.Space3)this.pos_space.Space; }
    }

    /// <summary>
    /// </summary>
    public droid.Runtime.Structs.Space.Space3 DirectionSpace {
      get { return (droid.Runtime.Structs.Space.Space3)this.dir_space.Space; }
    }

    /// <summary>
    /// </summary>
    public droid.Runtime.Structs.Space.Space3 RotationSpace {
      get { return (droid.Runtime.Structs.Space.Space3)this.rot_space.Space; }
    }

    /// <summary>
    /// </summary>
    public override void UpdateCurrentConfiguration() { //TODO: IMPLEMENT LOCAL SPACE
      if (this._coordinate_spaceEnum == droid.Runtime.Enums.CoordinateSpaceEnum.Environment_) {
        this.Position = this.ParentEnvironment.TransformPoint(point : this.transform.position);
        this.Direction = this.ParentEnvironment.TransformDirection(direction : this.transform.forward);
        this.Rotation = this.ParentEnvironment.TransformDirection(direction : this.transform.up);
      } else {
        var transform1 = this.transform;
        this.Position = transform1.position;
        this.Direction = transform1.forward;
        this.Rotation = transform1.up;
      }
    }

    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : this,
            identifier : this._x);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : this,
            identifier : this._y);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : this,
            identifier : this._z);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : this,
            identifier : this._dir_x);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : this,
            identifier : this._dir_y);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : this,
            identifier : this._dir_z);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : this,
            identifier : this._rot_x);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : this,
            identifier : this._rot_y);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : this,
            identifier : this._rot_z);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
      //TODO: use envs bound extent if available for space

      this._x = this.Identifier + "X_";
      this._y = this.Identifier + "Y_";
      this._z = this.Identifier + "Z_";
      this._dir_x = this.Identifier + "DirX_";
      this._dir_y = this.Identifier + "DirY_";
      this._dir_z = this.Identifier + "DirZ_";
      this._rot_x = this.Identifier + "RotX_";
      this._rot_y = this.Identifier + "RotY_";
      this._rot_z = this.Identifier + "RotZ_";
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment == null) {
        return;
      }

      this.ParentEnvironment.UnRegister(t : this, identifier : this._x);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._y);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._z);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._dir_x);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._dir_y);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._dir_z);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._rot_x);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._rot_y);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._rot_z);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="configuration"></param>
    public override void
        ApplyConfiguration(droid.Runtime.Interfaces.IConfigurableConfiguration configuration) {
      //TODO: Denormalize configuration if space is marked as normalised
      var transform1 = this.transform;
      var pos = transform1.position;
      var dir = transform1.forward;
      var rot = transform1.up;
      if (this._coordinate_spaceEnum == droid.Runtime.Enums.CoordinateSpaceEnum.Environment_) {
        pos = this.ParentEnvironment.TransformPoint(point : pos);
        dir = this.ParentEnvironment.TransformDirection(direction : dir);
        rot = this.ParentEnvironment.TransformDirection(direction : rot);
      }

      var v = configuration.ConfigurableValue;
      if (this.PositionSpace.DecimalGranularity >= 0) {
        v = (int)System.Math.Round(value : v, digits : this.PositionSpace.DecimalGranularity);
      }

      if (this.PositionSpace.Min[0].CompareTo(this.PositionSpace.Max[0]) != 0) {
        //TODO NOT IMPLEMENTED CORRECTLY VelocitySpace should not be index but should check all pairwise values, PositionSpace.MinValues == PositionSpace.MaxValues, and use other space aswell!
        if (v < this.PositionSpace.Min[0] || v > this.PositionSpace.Max[0]) {
          UnityEngine.Debug.Log(message :
                                string
                                    .Format("Configurable does not accept input{2}, outside allowed range {0} to {1}",
                                            this.PositionSpace.Min[0],
                                            this.PositionSpace.Max[0],
                                            v));
          return; // Do nothing
        }
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log(message : "Applying " + v + " To " + this.Identifier);
      }
      #endif
      if (this.RelativeToExistingValue) {
        if (configuration.ConfigurableName == this._x) {
          pos.Set(newX : v - pos.x, newY : pos.y, newZ : pos.z);
        } else if (configuration.ConfigurableName == this._y) {
          pos.Set(newX : pos.x, newY : v - pos.y, newZ : pos.z);
        } else if (configuration.ConfigurableName == this._z) {
          pos.Set(newX : pos.x, newY : pos.y, newZ : v - pos.z);
        } else if (configuration.ConfigurableName == this._dir_x) {
          dir.Set(newX : v - dir.x, newY : dir.y, newZ : dir.z);
        } else if (configuration.ConfigurableName == this._dir_y) {
          dir.Set(newX : dir.x, newY : v - dir.y, newZ : dir.z);
        } else if (configuration.ConfigurableName == this._dir_z) {
          dir.Set(newX : dir.x, newY : dir.y, newZ : v - dir.z);
        } else if (configuration.ConfigurableName == this._rot_x) {
          rot.Set(newX : v - rot.x, newY : rot.y, newZ : rot.z);
        } else if (configuration.ConfigurableName == this._rot_y) {
          rot.Set(newX : rot.x, newY : v - rot.y, newZ : rot.z);
        } else if (configuration.ConfigurableName == this._rot_z) {
          rot.Set(newX : rot.x, newY : rot.y, newZ : v - rot.z);
        }
      } else {
        if (configuration.ConfigurableName == this._x) {
          pos.Set(newX : v, newY : pos.y, newZ : pos.z);
        } else if (configuration.ConfigurableName == this._y) {
          pos.Set(newX : pos.x, newY : v, newZ : pos.z);
        } else if (configuration.ConfigurableName == this._z) {
          pos.Set(newX : pos.x, newY : pos.y, newZ : v);
        } else if (configuration.ConfigurableName == this._dir_x) {
          dir.Set(newX : v, newY : dir.y, newZ : dir.z);
        } else if (configuration.ConfigurableName == this._dir_y) {
          dir.Set(newX : dir.x, newY : v, newZ : dir.z);
        } else if (configuration.ConfigurableName == this._dir_z) {
          dir.Set(newX : dir.x, newY : dir.y, newZ : v);
        } else if (configuration.ConfigurableName == this._rot_x) {
          rot.Set(newX : v, newY : rot.y, newZ : rot.z);
        } else if (configuration.ConfigurableName == this._rot_y) {
          rot.Set(newX : rot.x, newY : v, newZ : rot.z);
        } else if (configuration.ConfigurableName == this._rot_z) {
          rot.Set(newX : rot.x, newY : rot.y, newZ : v);
        }
      }

      var inv_pos = pos;
      var inv_dir = dir;
      var inv_rot = rot;
      if (this._coordinate_spaceEnum == droid.Runtime.Enums.CoordinateSpaceEnum.Environment_) {
        inv_pos = this.ParentEnvironment.InverseTransformPoint(point : pos);
        inv_dir = this.ParentEnvironment.InverseTransformDirection(direction : dir);
        inv_rot = this.ParentEnvironment.InverseTransformDirection(direction : rot);
      }

      this.transform.position = inv_pos;
      this.transform.rotation = UnityEngine.Quaternion.identity;
      this.transform.rotation = UnityEngine.Quaternion.LookRotation(forward : inv_dir, upwards : inv_rot);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override droid.Runtime.Messaging.Messages.Configuration[] SampleConfigurations() {
      var sample = this.pos_space.Sample();
      var sample1 = this.rot_space.Sample();
      return new[] {
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._x,
                         configurable_value : sample.x),
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._y,
                         configurable_value : sample.y),
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._z,
                         configurable_value : sample.z),
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._rot_x,
                         configurable_value : sample1.x),
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._rot_y,
                         configurable_value : sample1.y),
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._rot_z,
                         configurable_value : sample1.z)
                   };
    }
  }
}