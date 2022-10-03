namespace droid.Runtime.Messaging.Messages.Displayables {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class DisplayableValuedVector3S : Displayable {
    public DisplayableValuedVector3S(string displayable_name,
                                     droid.Runtime.Structs.Points.ValuePoint[] displayable_value) {
      this.DisplayableName = displayable_name;
      this.DisplayableValue = displayable_value;
    }
  }
}