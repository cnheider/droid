namespace droid.Runtime.Messaging.Messages {
  /// <summary>
  ///   Has a possible direction given by the sign of the float in strength
  /// </summary>
  [System.SerializableAttribute]
  public class ActuatorMotion : droid.Runtime.Interfaces.IMotion {
    public ActuatorMotion(string actor_name, string actuator_name, float strength) {
      this.ActorName = actor_name;
      this.ActuatorName = actuator_name;
      this.Strength = strength;
    }

    /// <summary>
    /// </summary>
    public float Strength { get; set; }

    /// <summary>
    /// </summary>
    public string ActorName { get; }

    /// <summary>
    /// </summary>
    public string ActuatorName { get; }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
      return "<ActuatorMotion> "
             + this.ActorName
             + ", "
             + this.ActuatorName
             + ", "
             + this.Strength
             + " </ActuatorMotion>";
    }
  }
}