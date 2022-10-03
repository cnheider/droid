namespace droid.Runtime.Prototyping.Configurables.Experimental {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : ConfigurableComponentMenuPath._ComponentMenuPath
                                           + "ScreenSpaceOcclusion"
                                           + ConfigurableComponentMenuPath._Postfix)]
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.Renderer))]
  public class ScreenSpaceOcclusionConfigurable : Configurable {
    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Sample.SampleSpace4 rot_space =
        new droid.Runtime.Structs.Space.Sample.SampleSpace4 {
                                                                _space = droid.Runtime.Structs.Space.Space4
                                                                    .ZeroOne
                                                            };

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Sample.SampleSpace2 xy_space2 =
        new droid.Runtime.Structs.Space.Sample.SampleSpace2 {
                                                                _space = droid.Runtime.Structs.Space.Space2
                                                                    .ZeroOne
                                                            };

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Sample.SampleSpace1 depth_space1 =
        new droid.Runtime.Structs.Space.Sample.SampleSpace1 {
                                                                _space = droid.Runtime.Structs.Space.Space1
                                                                    .ZeroOne
                                                            };

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Sample.SampleSpace3 size_space =
        new droid.Runtime.Structs.Space.Sample.SampleSpace3 {
                                                                _space = droid.Runtime.Structs.Space.Space3
                                                                    .ZeroOne
                                                            };

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    UnityEngine.Camera _camera = null;

    [UnityEngine.SerializeField] UnityEngine.GameObject[] _prefabs = null;
    [UnityEngine.SerializeField] int num_occlusions = 10;

    /// <summary>
    ///   Alpha
    /// </summary>
    string _a;

    /// <summary>
    ///   Blue
    /// </summary>
    string _b;

    /// <summary>
    ///   Green
    /// </summary>
    string _g;

    bool _once_pre_setup = true;

    /// <summary>
    ///   Red
    /// </summary>
    string _r;

    System.Collections.Generic.List<UnityEngine.GameObject> _spawned =
        new System.Collections.Generic.List<UnityEngine.GameObject>();

    public droid.Runtime.Interfaces.ISamplable ConfigurableValueSpace {
      get { return new droid.Runtime.Structs.Space.Sample.SampleSpace1(); }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
      this._r = this.Identifier + "R";
      this._g = this.Identifier + "G";
      this._b = this.Identifier + "B";
      this._a = this.Identifier + "A";

      var s = new droid.Runtime.Structs.Space.Sample.SampleSpace1 {
                                                                      _space = droid.Runtime.Structs.Space
                                                                          .Space1.ZeroOne
                                                                  };

      if (UnityEngine.Application.isPlaying && this._once_pre_setup) {
        if (this._prefabs != null && this._prefabs.Length > 0 && this._camera) {
          for (var i = 0; i < this.num_occlusions; i++) {
            var prefab = this._prefabs[(int)(s.Sample() * this._prefabs.Length)];

            var xy = this.xy_space2.Sample();
            var z = this._camera.nearClipPlane + this.depth_space1.Sample() * this._camera.farClipPlane;

            var a = new UnityEngine.Vector3(x : xy.x, y : xy.y, z : z);

            var c = this._camera.ViewportToWorldPoint(position : a);

            var rot = this.rot_space.Sample();
            var b = new UnityEngine.Quaternion(x : rot.x,
                                               y : rot.y,
                                               z : rot.z,
                                               w : rot.w);

            var d = Instantiate(original : prefab,
                                position : c,
                                rotation : b,
                                parent : this.transform);
            d.transform.localScale = this.size_space.Sample();

            this._spawned.Add(item : d);
          }
        }

        this._once_pre_setup = false;
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._r);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._g);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._b);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._a);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment == null) {
        return;
      }

      this.ParentEnvironment.UnRegister(t : this, identifier : this._r);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._g);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._b);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._a);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateCurrentConfiguration() { }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="configuration"></param>
    public override void
        ApplyConfiguration(droid.Runtime.Interfaces.IConfigurableConfiguration configuration) {
      var aa = UnityEngine.Random.Range(0, maxExclusive : this._spawned.Count);
      for (var index = 0; index < this._spawned.Count; index++) {
        var bb = this._spawned[index : index];
        var xy = this.xy_space2.Sample();
        var z = this._camera.nearClipPlane + this.depth_space1.Sample() * this._camera.farClipPlane;

        var a = new UnityEngine.Vector3(x : xy.x, y : xy.y, z : z);

        var c = this._camera.ViewportToWorldPoint(position : a);

        var rot = this.rot_space.Sample();
        var b = new UnityEngine.Quaternion(x : rot.x,
                                           y : rot.y,
                                           z : rot.z,
                                           w : rot.w);

        bb.transform.localScale = this.size_space.Sample();

        bb.transform.position = c;

        bb.transform.rotation = b;
      }
      #if NEODROID_DEBUG
      if (this.Debugging) {
        droid.Runtime.Utilities.DebugPrinting.ApplyPrint(debugging : this.Debugging,
                                                         configuration : configuration,
                                                         identifier : this.Identifier);
      }
      #endif
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override droid.Runtime.Messaging.Messages.Configuration[] SampleConfigurations() {
      var s = this.size_space.Sample();

      return new[] {
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._r,
                         configurable_value : s.x),
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._b,
                         configurable_value : s.y),
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._g,
                         configurable_value : s.z)
                   };
    }
  }
}