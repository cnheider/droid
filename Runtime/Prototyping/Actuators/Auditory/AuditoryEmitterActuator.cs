namespace droid.Runtime.Prototyping.Actuators.Auditory {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.AudioSource))]
  public class AuditoryEmitterActuator : Actuator {
    UnityEngine.AudioSource _audio_source;

    public override string[] InnerMotionNames { get; }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
      base.PreSetup();
      this._audio_source = this.GetComponent<UnityEngine.AudioSource>();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    protected override void InnerApplyMotion(droid.Runtime.Interfaces.IMotion motion) {
      if (this._audio_source && this._audio_source.clip && !this._audio_source.mute) {
        this._audio_source.Play();
      }
    }
  }
}