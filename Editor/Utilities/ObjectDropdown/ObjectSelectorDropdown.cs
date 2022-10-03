#if UNITY_EDITOR

namespace droid.Editor.Utilities.ObjectDropdown {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEditor.CustomPropertyDrawer(type : typeof(ObjectDropdownAttribute))]
  public class ObjectSelectorDropdown : UnityEditor.PropertyDrawer {
    System.Collections.Generic.List<UnityEngine.Object> _m_list =
        new System.Collections.Generic.List<UnityEngine.Object>();

    /// <summary>
    /// </summary>
    /// <param name="position"></param>
    /// <param name="property"></param>
    /// <param name="label"></param>
    public override void OnGUI(UnityEngine.Rect position,
                               UnityEditor.SerializedProperty property,
                               UnityEngine.GUIContent label) {
      var e = UnityEngine.Event.current;
      if (property.propertyType == UnityEditor.SerializedPropertyType.ObjectReference) {
        if ((e.type == UnityEngine.EventType.DragPerform
             || e.type == UnityEngine.EventType.DragExited
             || e.type == UnityEngine.EventType.DragUpdated
             || e.type == UnityEngine.EventType.Repaint)
            && position.Contains(point : e.mousePosition)
            && e.shift) {
          if (UnityEditor.DragAndDrop.objectReferences != null) {
            this._m_list.Clear();
            foreach (var o in UnityEditor.DragAndDrop.objectReferences) {
              this._m_list.Add(item : o);
              var go = o as UnityEngine.GameObject;
              if (go == null && o is UnityEngine.Component) {
                go = ((UnityEngine.Component)o).gameObject;
                this._m_list.Add(item : go);
              }

              if (go != null) {
                foreach (var c in go.GetComponents<UnityEngine.Component>()) {
                  if (c != o) {
                    this._m_list.Add(item : c);
                  }
                }
              }
            }

            var field_info = property.GetPropertyReferenceType();
            if (field_info != null) {
              var type = field_info.FieldType;
              for (var i = this._m_list.Count - 1; i >= 0; i--) {
                if (this._m_list[index : i] == null
                    || !type.IsAssignableFrom(c : this._m_list[index : i].GetType())) {
                  this._m_list.RemoveAt(index : i);
                }
              }
            }

            if (this.attribute is ObjectDropdownFilterAttribute att) {
              var type = att._FilterType;
              for (var i = this._m_list.Count - 1; i >= 0; i--) {
                if (!type.IsAssignableFrom(c : this._m_list[index : i].GetType())) {
                  this._m_list.RemoveAt(index : i);
                }
              }
            }

            if (this._m_list.Count == 0) {
              UnityEditor.DragAndDrop.visualMode = UnityEditor.DragAndDropVisualMode.Rejected;
            } else {
              UnityEditor.DragAndDrop.visualMode = UnityEditor.DragAndDropVisualMode.Link;
              if (e.type == UnityEngine.EventType.DragPerform) {
                var gm = new UnityEditor.GenericMenu();
                UnityEditor.GenericMenu.MenuFunction2 func = o => {
                                                               property.objectReferenceValue =
                                                                   (UnityEngine.Object)o;
                                                               property.serializedObject
                                                                       .ApplyModifiedProperties();
                                                             };
                foreach (var item in this._m_list) {
                  gm.AddItem(content : new UnityEngine.GUIContent(text :
                                                                  $"{item.name}({item.GetType().Name})"),
                             false,
                             func : func,
                             userData : item);
                }

                gm.ShowAsContext();
                e.Use();
              }
            }

            this._m_list.Clear();
          }
        }

        UnityEditor.EditorGUI.ObjectField(position : position, property : property, label : label);
      } else {
        UnityEditor.EditorGUI.PropertyField(position : position, property : property, label : label);
      }
    }
  }

  /// <summary>
  /// </summary>
  public static class SerializedPropertyExt {
    /// <summary>
    /// </summary>
    /// <param name="a_property"></param>
    /// <returns></returns>
    public static System.Reflection.FieldInfo GetPropertyReferenceType(
        this UnityEditor.SerializedProperty a_property) {
      var current_type = a_property.serializedObject.targetObject.GetType();
      System.Reflection.FieldInfo fi = null;
      var parts = a_property.propertyPath.Split('.');
      for (var index = 0; index < parts.Length; index++) {
        var field_name = parts[index];
        fi = current_type.GetField(name : field_name,
                                   bindingAttr : System.Reflection.BindingFlags.Instance
                                                 | System.Reflection.BindingFlags.Public
                                                 | System.Reflection.BindingFlags.NonPublic);
        if (fi == null) {
          return null;
        }

        current_type = fi.FieldType;
      }

      return fi;
    }
  }
}
#endif