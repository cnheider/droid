namespace droid.Runtime.Messaging.Messages {
  /// <summary>
  /// </summary>
  public class EnvironmentDescription {
    public EnvironmentDescription(
        droid.Runtime.Interfaces.IEpisodicObjectiveFunction objective_function_function,
        System.Collections.Generic.SortedDictionary<string, droid.Runtime.Interfaces.IActor> actors,
        System.Collections.Generic.SortedDictionary<string, droid.Runtime.Interfaces.IConfigurable>
            configurables,
        System.Collections.Generic.SortedDictionary<string, droid.Runtime.Interfaces.ISensor> sensors,
        System.Collections.Generic.SortedDictionary<string, droid.Runtime.Interfaces.IDisplayer> displayers) {
      this.Configurables = configurables;
      this.Actors = actors;
      this.Sensors = sensors;

      this.Displayers = displayers;

      this.ObjectiveFunction = objective_function_function;
    }

    /// <summary>
    /// </summary>
    public System.Collections.Generic.SortedDictionary<string, droid.Runtime.Interfaces.IDisplayer>
        Displayers { get; }

    /// <summary>
    /// </summary>
    public droid.Runtime.Interfaces.IEpisodicObjectiveFunction ObjectiveFunction { get; }

    /// <summary>
    /// </summary>
    public System.Collections.Generic.SortedDictionary<string, droid.Runtime.Interfaces.IActor> Actors {
      get;
    }

    /// <summary>
    /// </summary>
    public System.Collections.Generic.SortedDictionary<string, droid.Runtime.Interfaces.IConfigurable>
        Configurables { get; }

    /// <summary>
    /// </summary>
    public System.Collections.Generic.SortedDictionary<string, droid.Runtime.Interfaces.ISensor> Sensors {
      get;
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString() { return $"{this.Sensors.Count},{this.Actors.Count}"; }
  }
}