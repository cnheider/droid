namespace droid.Runtime.Prototyping.Sensors.Spatial.Transform {
  public class SphericalCoordinateSensor : Sensor,
                                           droid.Runtime.Interfaces.IHasDouble {
    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Space2 _spherical_space = new droid.Runtime.Structs.Space.Space2 {
                                                              Min = UnityEngine.Vector2.zero,
                                                              Max =
                                                                  new UnityEngine.Vector2(x : UnityEngine
                                                                          .Mathf.PI
                                                                      * 2f,
                                                                    y : UnityEngine.Mathf.PI * 2f),
                                                              DecimalGranularity = 4
                                                          };

    [UnityEngine.SerializeField] droid.Runtime.Structs.Space.SphericalSpace sc;

    public override System.Collections.Generic.IEnumerable<float> FloatEnumerable {
      get {
        yield return this.ObservationValue.x;
        yield return this.ObservationValue.y;
      }
    }

    public UnityEngine.Vector2 ObservationValue { get { return this.sc.ToVector2; } }
    public droid.Runtime.Structs.Space.Space2 DoubleSpace { get { return this._spherical_space; } }

    public override void PrototypingReset() {
      this.sc =
          droid.Runtime.Structs.Space.SphericalSpace.FromCartesian(cartesian_coordinate :
                                                                   this.transform.position,
                                                                   3f,
                                                                   10f,
                                                                   0f,
                                                                   max_polar : UnityEngine.Mathf.PI * 2f,
                                                                   0f,
                                                                   max_elevation : UnityEngine.Mathf.PI * 2f);
    }

    public override void UpdateObservation() {
      this.sc.UpdateFromCartesian(cartesian_coordinate : this.transform.position);
    } //TODO: IMPLEMENT LOCAL SPACE
  }
}