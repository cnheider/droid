namespace droid.Runtime.Prototyping.EnvironmentListener.StateValidation {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class EnsureValidState : EnvironmentListener {
    [UnityEngine.SerializeField] droid.Runtime.Prototyping.Actors.Actor _actor;

    [UnityEngine.SerializeField]
    droid.Runtime.Environments.Prototyping.AbstractPrototypingEnvironment _environment;

    [UnityEngine.SerializeField] UnityEngine.Transform _goal;

    [UnityEngine.SerializeField] droid.Runtime.Utilities.Extensions.Obstruction[] _obstructions;
    [UnityEngine.SerializeField] bool _only_initial_state = true;

    [UnityEngine.SerializeField] droid.Runtime.GameObjects.BoundingBoxes.NeodroidBoundingBox _playable_area;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void Clear() {
      if (!this._goal) {
        this._goal = FindObjectOfType<UnityEngine.Transform>();
      }

      if (!this._actor) {
        this._actor = FindObjectOfType<droid.Runtime.Prototyping.Actors.Actor>();
      }

      if (!this._environment) {
        this._environment =
            FindObjectOfType<droid.Runtime.Environments.Prototyping.AbstractPrototypingEnvironment>();
      }

      if (this._obstructions.Length <= 0) {
        this._obstructions = FindObjectsOfType<droid.Runtime.Utilities.Extensions.Obstruction>();
      }

      if (!this._playable_area) {
        this._playable_area = FindObjectOfType<droid.Runtime.GameObjects.BoundingBoxes.NeodroidBoundingBox>();
      }
    }

    /// <summary>
    /// </summary>
    void ValidateState() {
      if (this._only_initial_state && this._environment.StepI != 0) {
        return;
      }

      if (this._playable_area != null
          && !this._playable_area.Bounds.Intersects(bounds : this._actor.ActorBounds)) {
        this._environment.Terminate("Actor outside playable area");
      }

      if (this._playable_area != null
          && !this._playable_area.Bounds.Intersects(bounds : this._goal.GetComponent<UnityEngine.Collider>()
                                                                 .bounds)) {
        this._environment.Terminate("Goal outside playable area");
      }

      for (var index = 0; index < this._obstructions.Length; index++) {
        var obstruction = this._obstructions[index];
        if (obstruction != null
            && obstruction.GetComponent<UnityEngine.Collider>().bounds
                          .Intersects(bounds : this._actor.ActorBounds)) {
          this._environment.Terminate("Actor overlapping obstruction");
        }

        if (obstruction != null
            && obstruction.GetComponent<UnityEngine.Collider>().bounds
                          .Intersects(bounds : this._goal.GetComponent<UnityEngine.Collider>().bounds)) {
          this._environment.Terminate("Goal overlapping obstruction");
        }
      }
    }

    public override void PrototypingReset() { }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreStep() { this.ValidateState(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Step() { this.ValidateState(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PostStep() { this.ValidateState(); }
  }
}