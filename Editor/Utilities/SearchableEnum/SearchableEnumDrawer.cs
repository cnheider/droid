#if UNITY_EDITOR

namespace droid.Editor.Utilities.SearchableEnum {
  /// <inheritdoc />
  /// <summary>
  ///   Draws the custom enum selector popup for enum fileds using the
  ///   SearchableEnumAttribute.
  /// </summary>
  [UnityEditor.CustomPropertyDrawer(type : typeof(droid.Runtime.Utilities.SearchableEnumAttribute))]
  public class SearchableEnumDrawer : UnityEditor.PropertyDrawer {
    const string _type_error = "SearchableEnum can only be used on enum fields.";

    /// <summary>
    ///   Cache of the hash to use to resolve the ID for the drawer.
    /// </summary>
    int _id_hash;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="position"></param>
    /// <param name="property"></param>
    /// <param name="label"></param>
    public override void OnGUI(UnityEngine.Rect position,
                               UnityEditor.SerializedProperty property,
                               UnityEngine.GUIContent label) {
      // If this is not used on an enum, show an error
      if (property.type != "Enum") {
        UnityEngine.GUIStyle error_style = "CN EntryErrorIconSmall";
        var r = new UnityEngine.Rect(source : position) {width = error_style.fixedWidth};
        position.xMin = r.xMax;
        UnityEngine.GUI.Label(position : r, "", style : error_style);
        UnityEngine.GUI.Label(position : position, text : _type_error);
        return;
      }

      // By manually creating the control ID, we can keep the ID for the
      // label and button the same. This lets them be selected together
      // with the keyboard in the inspector, much like a normal popup.
      if (this._id_hash == 0) {
        this._id_hash = "SearchableEnumDrawer".GetHashCode();
      }

      var id = UnityEngine.GUIUtility.GetControlID(hint : this._id_hash,
                                                   focusType : UnityEngine.FocusType.Keyboard,
                                                   rect : position);

      label = UnityEditor.EditorGUI.BeginProperty(totalPosition : position,
                                                  label : label,
                                                  property : property);
      position = UnityEditor.EditorGUI.PrefixLabel(totalPosition : position, id : id, label : label);

      var button_text = new UnityEngine.GUIContent(text : property.enumDisplayNames[property.enumValueIndex]);
      if (DropdownButton(id : id, position : position, content : button_text)) {
        void OnSelect(int i) {
          property.enumValueIndex = i;
          property.serializedObject.ApplyModifiedProperties();
        }

        SearchablePopup.Show(activator_rect : position,
                             options : property.enumDisplayNames,
                             current : property.enumValueIndex,
                             on_selection_made : OnSelect);
      }

      UnityEditor.EditorGUI.EndProperty();
    }

    /// <summary>
    ///   A custom button drawer that allows for a controlID so that we can
    ///   sync the button ID and the label ID to allow for keyboard
    ///   navigation like the built-in enum drawers.
    /// </summary>
    static bool DropdownButton(int id, UnityEngine.Rect position, UnityEngine.GUIContent content) {
      var current = UnityEngine.Event.current;
      switch (current.type) {
        case UnityEngine.EventType.MouseDown:
          if (position.Contains(point : current.mousePosition) && current.button == 0) {
            UnityEngine.Event.current.Use();
            return true;
          }

          break;
        case UnityEngine.EventType.KeyDown:
          if (UnityEngine.GUIUtility.keyboardControl == id && current.character == '\n') {
            UnityEngine.Event.current.Use();
            return true;
          }

          break;
        case UnityEngine.EventType.Repaint:
          UnityEditor.EditorStyles.popup.Draw(position : position,
                                              content : content,
                                              controlID : id,
                                              false);
          break;
      }

      return false;
    }
  }
}
#endif