namespace droid.Runtime.Prototyping.Configurables.Selection {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : ConfigurableComponentMenuPath._ComponentMenuPath
                                           + "ObjectSpawner"
                                           + ConfigurableComponentMenuPath._Postfix)]
  public class ObjectSpawnerConfigurable : Configurable {
    [UnityEngine.SerializeField] int _amount = 0;

    [UnityEngine.SerializeField] droid.Runtime.Enums.AxisEnum _axisEnum = droid.Runtime.Enums.AxisEnum.X_;

    [UnityEngine.SerializeField] UnityEngine.GameObject _object_to_spawn = null;

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Sample.SampleSpace1 _configurable_value_space =
        new droid.Runtime.Structs.Space.Sample.SampleSpace1 {
                                                                _space = droid.Runtime.Structs.Space.Space1
                                                                    .TwentyEighty
                                                            };

    System.Collections.Generic.List<UnityEngine.GameObject> _spawned_objects = null;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "ObjectSpawnerConfigurable"; } }

    /// <summary>
    /// </summary>
    public droid.Runtime.Interfaces.ISamplable ConfigurableValueSpace {
      get { return this._configurable_value_space; }
    }

    void OnApplicationQuit() { this.DestroyObjects(); }

    public override void PreSetup() {
      this.DestroyObjects();
      this._spawned_objects = new System.Collections.Generic.List<UnityEngine.GameObject>();
      this.SpawnObjects();
    }

    void DestroyObjects() {
      if (this._spawned_objects != null) {
        foreach (var o in this._spawned_objects) {
          Destroy(obj : o);
        }
      }

      foreach (UnityEngine.Transform c in this.transform) {
        Destroy(obj : c.gameObject);
      }
    }

    void SpawnObjects() {
      if (this._object_to_spawn) {
        var dir = UnityEngine.Vector3.up;
        if (this._axisEnum == droid.Runtime.Enums.AxisEnum.X_) {
          dir = UnityEngine.Vector3.right;
        } else if (this._axisEnum == droid.Runtime.Enums.AxisEnum.Z_) {
          dir = UnityEngine.Vector3.forward;
        }

        var transform1 = this.transform;
        for (var i = 0; i < this._amount; i++) {
          this._spawned_objects.Add(item : Instantiate(original : this._object_to_spawn,
                                                       position : transform1.position + dir * i,
                                                       rotation : UnityEngine.Random.rotation,
                                                       parent : transform1));
        }
      }
    }

    public override droid.Runtime.Messaging.Messages.Configuration[] SampleConfigurations() {
      return new droid.Runtime.Messaging.Messages.Configuration[] {this.ConfigurableValueSpace.Sample()};
    }

    public override void UpdateCurrentConfiguration() { }

    public override void ApplyConfiguration(droid.Runtime.Interfaces.IConfigurableConfiguration obj) {
      if (this._spawned_objects.Count < obj.ConfigurableValue) {
        var go = Instantiate(original : this._object_to_spawn, parent : this.transform);
        this._spawned_objects.Add(item : go);
      } else if (this._spawned_objects.Count > obj.ConfigurableValue) {
        if (this._spawned_objects.Count > 0) {
          this._spawned_objects.RemoveAt(index : this._spawned_objects.Count - 1);
        }
      }
    }
  }
}