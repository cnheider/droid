namespace droid.Editor.Utilities.ObjectDropdown {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class ObjectDropdownAttribute : UnityEngine.PropertyAttribute { }

  public class ObjectDropdownFilterAttribute : UnityEngine.PropertyAttribute {
    public System.Type _FilterType;
    public ObjectDropdownFilterAttribute(System.Type a_type) { this._FilterType = a_type; }
  }
}