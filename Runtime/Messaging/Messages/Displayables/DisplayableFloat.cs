﻿namespace droid.Runtime.Messaging.Messages.Displayables {
  /// <summary>
  /// </summary>
  public class DisplayableFloat : Displayable {
    public DisplayableFloat(string displayable_name, double displayable_value) {
      this.DisplayableName = displayable_name;
      this.DisplayableValue = displayable_value;
    }

    public DisplayableFloat(string displayable_name, double? displayable_value) {
      this.DisplayableName = displayable_name;
      this.DisplayableValue = displayable_value.GetValueOrDefault();
    }

    public DisplayableFloat(string displayable_name, float displayable_value) {
      this.DisplayableName = displayable_name;
      this.DisplayableValue = displayable_value;
    }
  }
}