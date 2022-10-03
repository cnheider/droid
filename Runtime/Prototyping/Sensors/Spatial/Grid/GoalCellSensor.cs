namespace droid.Runtime.Prototyping.Sensors.Spatial.Grid {
  [UnityEngine.AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                                           + "GoalCell"
                                           + SensorComponentMenuPath._Postfix)]
  public class GoalCellSensor : Sensor,
                                droid.Runtime.Interfaces.IHasTriple {
    [UnityEngine.SerializeField] droid.Runtime.Utilities.Grid.EmptyCell _current_goal;
    [UnityEngine.SerializeField] UnityEngine.Vector3 _current_goal_position;

    [UnityEngine.SerializeField] bool _draw_names = true;

    [UnityEngine.SerializeField] int _order_index;

    /// <summary>
    /// </summary>
    public int OrderIndex { get { return this._order_index; } set { this._order_index = value; } }

    /// <summary>
    /// </summary>
    public bool DrawNames { get { return this._draw_names; } set { this._draw_names = value; } }

    /// <summary>
    /// </summary>
    public droid.Runtime.Utilities.Grid.EmptyCell CurrentGoal {
      get {
        this.UpdateObservation();
        return this._current_goal;
      }
      set { this._current_goal = value; }
    }

    public override System.Collections.Generic.IEnumerable<float> FloatEnumerable {
      get {
        yield return this._current_goal_position.x;
        yield return this._current_goal_position.y;
        yield return this._current_goal_position.z;
      }
    }

    #if UNITY_EDITOR
    void OnDrawGizmosSelected() {
      if (this.DrawNames) {
        if (this._current_goal) {
          droid.Runtime.Utilities.Drawing.NeodroidUtilities.DrawString(text : this._current_goal.name,
                                                                       world_pos : this._current_goal
                                                                           .transform.position,
                                                                       color : UnityEngine.Color.green);
        }
      }
    }
    #endif

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public UnityEngine.Vector3 ObservationValue {
      get { return this._current_goal_position; }
      private set { this._current_goal_position = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public droid.Runtime.Structs.Space.Space3 TripleSpace { get; } = new droid.Runtime.Structs.Space.Space3();

    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      if (this._current_goal) {
        this._current_goal_position = this._current_goal.transform.position;
      }
    }
  }
}