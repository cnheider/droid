namespace droid.Runtime.Prototyping.ObjectiveFunctions.Tasks {
  //[ExecuteInEditMode]
  /// <summary>
  /// </summary>
  public class TaskSequence : droid.Runtime.ScriptableObjects.Deprecated.NeodroidTask {
    [UnityEngine.SerializeField]
    droid.Runtime.Prototyping.Sensors.Spatial.Grid.GoalCellSensor _current_goal_cell;

    [UnityEngine.SerializeField]
    System.Collections.Generic.Stack<droid.Runtime.Prototyping.Sensors.Spatial.Grid.GoalCellSensor>
        _goal_stack;

    [UnityEngine.SerializeField] droid.Runtime.Prototyping.Sensors.Spatial.Grid.GoalCellSensor[] _sequence;

    public droid.Runtime.Prototyping.Sensors.Spatial.Grid.GoalCellSensor CurrentGoalCell {
      get { return this._current_goal_cell; }
      private set { this._current_goal_cell = value; }
    }

    void Start() {
      if (this._sequence == null || this._sequence.Length == 0) {
        this._sequence = FindObjectsOfType<droid.Runtime.Prototyping.Sensors.Spatial.Grid.GoalCellSensor>();
        System.Array.Sort(array : this._sequence, (g1, g2) => g1.OrderIndex.CompareTo(value : g2.OrderIndex));
      }

      System.Array.Reverse(array : this._sequence);
      this._goal_stack =
          new System.Collections.Generic.Stack<droid.Runtime.Prototyping.Sensors.Spatial.Grid.GoalCellSensor
          >(collection : this._sequence);
      this.CurrentGoalCell = this.PopGoal();
    }

    public droid.Runtime.Prototyping.Sensors.Spatial.Grid.GoalCellSensor[] GetSequence() {
      return this._sequence;
    }

    public droid.Runtime.Prototyping.Sensors.Spatial.Grid.GoalCellSensor PopGoal() {
      this.CurrentGoalCell = this._goal_stack.Pop();
      return this.CurrentGoalCell;
    }
  }
}