namespace droid.Runtime.Environments {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu("Neodroid/Environments/ScriptedEnvironment")]
  public class ScriptedEnvironment : NeodroidEnvironment {
    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    UnityEngine.Renderer _actor_renderer = null;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    int _actor_x = 0;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    int _actor_y = 0;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    UnityEngine.Renderer _goal_renderer = null;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    int _goal_x = 0;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    int _goal_y = 0;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    int _height = 0;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    int _width = 0;

    /// <summary>
    /// </summary>
    int[,] _grid = null;

    System.Collections.Generic.List<droid.Runtime.Interfaces.IMotion> _motions =
        new System.Collections.Generic.List<droid.Runtime.Interfaces.IMotion>();

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    droid.Runtime.Interfaces.IManager _time_simulation_manager = null;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "ScriptedEnvironment"; } }

    /// <summary>
    /// </summary>
    public int ActorX {
      get { return this._actor_x; }
      set {
        this._actor_x = UnityEngine.Mathf.Max(0, b : UnityEngine.Mathf.Min(a : this._width - 1, b : value));
      }
    }

    /// <summary>
    /// </summary>
    public int ActorY {
      get { return this._actor_y; }
      set {
        this._actor_y = UnityEngine.Mathf.Max(0, b : UnityEngine.Mathf.Min(a : this._height - 1, b : value));
      }
    }

    /// <summary>
    /// </summary>
    public int GoalX {
      get { return this._goal_x; }
      set {
        this._goal_x = UnityEngine.Mathf.Max(0, b : UnityEngine.Mathf.Min(a : this._width - 1, b : value));
      }
    }

    /// <summary>
    /// </summary>
    public int GoalY {
      get { return this._goal_y; }
      set {
        this._goal_y = UnityEngine.Mathf.Max(0, b : UnityEngine.Mathf.Min(a : this._height - 1, b : value));
      }
    }

    public override void Reset() { throw new System.NotImplementedException(); }

    public override void RemotePostSetup() {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log("PostSetup");
      }
      #endif
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Setup() {
      this._grid = new int[this._width, this._height];

      var k = 0;
      for (var i = 0; i < this._width; i++) {
        for (var j = 0; j < this._height; j++) {
          this._grid[i, j] = k++;
        }
      }

      this._time_simulation_manager =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities
               .RegisterComponent(r : (droid.Runtime.Managers.AbstractNeodroidManager)this
                                      ._time_simulation_manager,
                                  c : this);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PostStep() {
      if (this._goal_renderer) {
        this._goal_renderer.transform.position = new UnityEngine.Vector3(x : this.GoalX, 0, z : this.GoalY);
      }

      if (this._actor_renderer) {
        this._actor_renderer.transform.position =
            new UnityEngine.Vector3(x : this.ActorX, 0, z : this.ActorY);
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override droid.Runtime.Messaging.Messages.Reaction SampleReaction() {
      this._motions.Clear();

      var strength = UnityEngine.Random.Range(0, 4);
      this._motions.Add(item : new droid.Runtime.Messaging.Messages.ActuatorMotion("",
                          "",
                          strength : strength));

      var rp =
          new droid.Runtime.Messaging.Messages.ReactionParameters(reaction_type : droid.Runtime.Messaging
                                                                      .Messages.ReactionTypeEnum.Step_,
                                                                  true,
                                                                  episode_count : true);
      return new droid.Runtime.Messaging.Messages.Reaction(parameters : rp,
                                                           motions : this._motions.ToArray(),
                                                           null,
                                                           null,
                                                           null,
                                                           "");
    }

    public override void Step(droid.Runtime.Messaging.Messages.Reaction reaction) {
      foreach (var motion in reaction.Motions) {
        switch ((int)motion.Strength) {
          case 0:
            this.ActorY += 1;
            break;
          case 1:
            this.ActorX += 1;
            break;
          case 2:
            this.ActorY -= 1;
            break;
          case 3:
            this.ActorX -= 1;
            break;
          default:
            throw new System.ArgumentOutOfRangeException();
        }
      }
    }

    public override void Configure(droid.Runtime.Messaging.Messages.Reaction reaction) {
      throw new System.NotImplementedException();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Tick() { }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override droid.Runtime.Messaging.Messages.EnvironmentSnapshot Snapshot() {
      var actor_idx = this._grid[this.ActorX, this.ActorY];
      var goal_idx = this._grid[this.GoalX, this.GoalY];

      var terminated = actor_idx == goal_idx;
      var signal = terminated ? 1 : 0;

      var time = UnityEngine.Time.realtimeSinceStartup - this.LastResetTime;

      var observables = new float[] {actor_idx};

      return new droid.Runtime.Messaging.Messages.EnvironmentSnapshot(environment_name : this.Identifier,
                                                                      0,
                                                                      time : time,
                                                                      signal : signal,
                                                                      terminated : terminated,
                                                                      observables : ref observables);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="recipient"></param>
    public override void ObservationsString(
        droid.Runtime.GameObjects.StatusDisplayer.EventRecipients.DataPoller recipient) {
      recipient.PollData(data : this.Snapshot().ToString());
    }

    public override void PrototypingReset() { }
  }
}