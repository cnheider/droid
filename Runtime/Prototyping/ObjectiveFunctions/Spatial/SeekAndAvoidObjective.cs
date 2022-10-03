namespace droid.Runtime.Prototyping.ObjectiveFunctions.Spatial {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class SeekAndAvoidObjective : ObjectiveFunction {
    [UnityEngine.SerializeField] UnityEngine.Transform _actor = null;
    [UnityEngine.SerializeField] UnityEngine.GameObject _avoidable = null;
    [UnityEngine.SerializeField] UnityEngine.GameObject _collectible = null;
    [UnityEngine.SerializeField] int _end_game_radius = 10;
    [UnityEngine.SerializeField] int _num_avoidables = 50;
    [UnityEngine.SerializeField] int _num_collectibles = 50;
    [UnityEngine.SerializeField] int _penalty = -1;
    [UnityEngine.SerializeField] int _reward = 1;
    [UnityEngine.SerializeField] int _spawn_radius = 10;
    System.Collections.Generic.List<UnityEngine.GameObject> _avoidables = null;
    System.Collections.Generic.List<UnityEngine.GameObject> _collectibles = null;
    UnityEngine.Vector3 _initial_actor_position = UnityEngine.Vector3.zero;
    float _score;
    System.Collections.Generic.List<UnityEngine.Vector3> _spawned_locations = null;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "SeekAndAvoidListener"; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override float InternalEvaluate() {
      // The game ends if the number of good balls is 0, or if the robot is too far from start
      var actor = this._actor;
      if (actor != null) {
        var dist = UnityEngine.Vector3.Distance(a : this._initial_actor_position, b : actor.position);
        var game_objects = this._collectibles;
        var is_over = game_objects != null && (game_objects.Count == 0 || dist > this._end_game_radius);

        if (is_over) {
          this.ParentEnvironment.Terminate(reason : $"Ending Game: Dist {dist} radius {this._spawn_radius}");
        }
      }

      return this._score;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void InternalReset() {
      if (!UnityEngine.Application.isPlaying) {
        return;
      }

      var actor = this._actor;
      if (actor != null) {
        this._initial_actor_position = actor.position;
        var remote_sensor = this._actor
                                .GetComponentInChildren<droid.Runtime.GameObjects.ChildSensors.
                                    ChildColliderSensor<UnityEngine.Collider, UnityEngine.Collision>>();
        if (!remote_sensor) {
          var col = this._actor.GetComponentInChildren<UnityEngine.Collider>();
          if (col) {
            remote_sensor = col.gameObject
                               .AddComponent<droid.Runtime.GameObjects.ChildSensors.ChildColliderSensor<
                                   UnityEngine.Collider, UnityEngine.Collision>>();
          }
        }

        if (remote_sensor) {
          remote_sensor.Caller = this;
          remote_sensor.OnTriggerEnterDelegate = this.OnChildTriggerEnter;
          remote_sensor.OnCollisionEnterDelegate = this.OnChildTriggerEnter;
        }
      }

      this.ClearEnvironment();
      this.SpawnCollectibles();
      this.SpawnAvoidables();
    }

    void OnChildTriggerEnter(UnityEngine.GameObject child_game_object, UnityEngine.Collision collision) {
      this.OnChildTriggerEnter(child_game_object : child_game_object, collider1 : collision.collider);
    }

    void OnChildTriggerEnter(UnityEngine.GameObject child_game_object, UnityEngine.Collider collider1) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        print(message : $"{child_game_object} is colliding with {collider1}");
      }
      #endif

      var collectible = this._collectible;
      if (collectible != null && collider1.gameObject.name.Contains(value : collectible.name)) {
        this._collectibles.Remove(item : collider1.gameObject);
        Destroy(obj : collider1.gameObject);
        this._score += this._reward;
      }

      var game_object = this._avoidable;
      if (game_object != null && collider1.gameObject.name.Contains(value : game_object.name)) {
        this._avoidables.Remove(item : collider1.gameObject);
        this._score += this._penalty;
        Destroy(obj : collider1.gameObject);
      }
    }

    void SpawnCollectibles() {
      for (var i = 0; i < this._num_collectibles; i++) {
        var game_object = this._collectible;
        if (game_object != null) {
          var collectible =
              this.RandomSpawn(prefab : this._collectible, position : this._initial_actor_position);
          this._collectibles.Add(item : collectible);
        }
      }
    }

    void SpawnAvoidables() {
      for (var i = 0; i < this._num_avoidables; i++) {
        var game_object = this._avoidable;
        if (game_object != null) {
          var avoidable = this.RandomSpawn(prefab : this._avoidable, position : this._initial_actor_position);
          this._avoidables.Add(item : avoidable);
        }
      }
    }

    UnityEngine.GameObject RandomSpawn(UnityEngine.GameObject prefab, UnityEngine.Vector3 position) {
      this._spawned_locations.Add(item : position);

      if (prefab != null) {
        UnityEngine.Vector3 location;
        do {
          location = this._actor.transform.position;
          location.x += UnityEngine.Random.Range(minInclusive : -this._spawn_radius, maxExclusive : this._spawn_radius);
          location.y = position.y + 1f;
          location.z += UnityEngine.Random.Range(minInclusive : -this._spawn_radius, maxExclusive : this._spawn_radius);
        } while (this._spawned_locations.Contains(item : location));

        this._spawned_locations.Add(item : location);

        return Instantiate(original : prefab,
                           position : location,
                           rotation : UnityEngine.Quaternion.identity,
                           parent : this.ParentEnvironment.Transform);
      }

      return null;
    }

    void ClearEnvironment() {
      if (this._spawned_locations != null) {
        this._spawned_locations.Clear();
      } else {
        this._spawned_locations = new System.Collections.Generic.List<UnityEngine.Vector3>();
      }

      if (this._collectibles != null) {
        foreach (var obj in this._collectibles) {
          Destroy(obj : obj);
        }

        this._collectibles.Clear();
      } else {
        this._collectibles = new System.Collections.Generic.List<UnityEngine.GameObject>();
      }

      if (this._avoidables != null) {
        foreach (var obj in this._avoidables) {
          Destroy(obj : obj);
        }

        this._avoidables.Clear();
      } else {
        this._avoidables = new System.Collections.Generic.List<UnityEngine.GameObject>();
      }
    }
  }
}