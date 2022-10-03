namespace droid.Runtime.Prototyping.Configurables.Experimental {
  /// <inheritdoc cref="Configurable" />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : ConfigurableComponentMenuPath._ComponentMenuPath
                                           + "Mesh"
                                           + ConfigurableComponentMenuPath._Postfix)]
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.MeshFilter))]
  public class MeshConfigurable : Configurable {
    [UnityEngine.SerializeField] UnityEngine.Mesh[] _meshes = null;
    [UnityEngine.SerializeField] UnityEngine.MeshFilter _mesh_filter = null;
    [UnityEngine.SerializeField] bool _displace_mesh = false;

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Sample.SampleSpace1 _deformation_space =
        new droid.Runtime.Structs.Space.Sample.SampleSpace1 {
                                                                _space =
                                                                    new droid.Runtime.Structs.Space.Space1 {
                                                                        Min = 1f, Max = 5f
                                                                    }
                                                            };

    UnityEngine.Mesh _deforming_mesh = null;
    string _mesh_str;
    droid.Runtime.Sampling.Perlin _noise = null;
    UnityEngine.Vector3[] _original_vertices = null, _displaced_vertices = null;
    float _speed = 1.0f;

    public droid.Runtime.Interfaces.ISamplable ConfigurableValueSpace {
      get { return this._deformation_space; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
      this._mesh_str = this.Identifier + "Mesh";
      this._mesh_filter = this.GetComponent<UnityEngine.MeshFilter>();
      if (UnityEngine.Application.isPlaying) {
        this._deforming_mesh = this._mesh_filter.mesh;
        this._original_vertices = this._deforming_mesh.vertices;
        this._displaced_vertices = new UnityEngine.Vector3[this._original_vertices.Length];
        for (var i = 0; i < this._original_vertices.Length; i++) {
          this._displaced_vertices[i] = this._original_vertices[i];
        }
      }

      this._noise = new droid.Runtime.Sampling.Perlin();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._mesh_str);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// n
    protected override void UnRegisterComponent() {
      this.ParentEnvironment?.UnRegister(t : this, identifier : this._mesh_str);
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
      #if NEODROID_DEBUG
      droid.Runtime.Utilities.DebugPrinting.ApplyPrint(debugging : this.Debugging,
                                                       configuration : configuration,
                                                       identifier : this.Identifier);
      #endif

      if (configuration.ConfigurableName == this._mesh_str) {
        if (this._displace_mesh) {
          if (this._deforming_mesh) {
            var time_x = UnityEngine.Time.time * this._speed + 0.1365143f;
            var time_y = UnityEngine.Time.time * this._speed + 1.21688f;
            var time_z = UnityEngine.Time.time * this._speed + 2.5564f;

            for (var i = 0; i < this._displaced_vertices.Length; i++) {
              var orig = this._original_vertices[i];
              //orig.y = orig.y * (1+(float)Math.Cos(Time.deltaTime))*(configuration.ConfigurableValue);
              //orig.x = orig.x * (1+(float)Math.Sin(Time.deltaTime))*(configuration.ConfigurableValue);

              orig.x += this._noise.Noise(x : time_x + orig.x, y : time_x + orig.y, z : time_x + orig.z)
                        * configuration.ConfigurableValue;
              orig.y += this._noise.Noise(x : time_y + orig.x, y : time_y + orig.y, z : time_y + orig.z)
                        * configuration.ConfigurableValue;
              orig.z += this._noise.Noise(x : time_z + orig.x, y : time_z + orig.y, z : time_z + orig.z)
                        * configuration.ConfigurableValue;

              this._displaced_vertices[i] = orig;
            }

            this._deforming_mesh.vertices = this._displaced_vertices;

            this._deforming_mesh.RecalculateNormals();
          }
        } else if (this._meshes.Length > 0) {
          var idx = (int)(configuration.ConfigurableValue * this._meshes.Length);
          this._mesh_filter.mesh = this._meshes[idx];
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override droid.Runtime.Messaging.Messages.Configuration[] SampleConfigurations() {
      return new[] {
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._mesh_str,
                         configurable_value : this._deformation_space.Sample())
                   };
    }
  }
}