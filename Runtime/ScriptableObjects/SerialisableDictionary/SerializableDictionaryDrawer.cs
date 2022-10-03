#if UNITY_EDITOR

namespace droid.Runtime.ScriptableObjects.SerialisableDictionary {
  public abstract class SerializableKeyValueTemplate<TK, TV> : UnityEngine.ScriptableObject {
    public TK _Key;
    public TV _Value;
  }

  public abstract class SerializableDictionaryDrawer<TK, TV> : UnityEditor.PropertyDrawer {
    System.Collections.Generic.Dictionary<int,
        System.Collections.Generic.Dictionary<int, UnityEditor.SerializedProperty>> _indexed_property_dicts =
        new System.Collections.Generic.Dictionary<int,
            System.Collections.Generic.Dictionary<int, UnityEditor.SerializedProperty>>();

    System.Collections.Generic.Dictionary<int, UnityEditor.SerializedProperty> _keys_props =
        new System.Collections.Generic.Dictionary<int, UnityEditor.SerializedProperty>();

    System.Collections.Generic.Dictionary<int, UnityEditor.SerializedProperty> _template_key_prop =
        new System.Collections.Generic.Dictionary<int, UnityEditor.SerializedProperty>();

    System.Collections.Generic.Dictionary<int, UnityEditor.SerializedProperty> _template_value_prop =
        new System.Collections.Generic.Dictionary<int, UnityEditor.SerializedProperty>();

    System.Collections.Generic.Dictionary<int, UnityEditor.SerializedProperty> _values_props =
        new System.Collections.Generic.Dictionary<int, UnityEditor.SerializedProperty>();

    protected abstract SerializableKeyValueTemplate<TK, TV> GetTemplate();

    protected T GetGenericTemplate<T>() where T : SerializableKeyValueTemplate<TK, TV> {
      return UnityEngine.ScriptableObject.CreateInstance<T>();
    }

    public override void OnGUI(UnityEngine.Rect position,
                               UnityEditor.SerializedProperty property,
                               UnityEngine.GUIContent label) {
      UnityEditor.EditorGUI.BeginProperty(totalPosition : position, label : label, property : property);

      var first_line = position;
      first_line.height = UnityEditor.EditorGUIUtility.singleLineHeight;
      UnityEditor.EditorGUI.PropertyField(position : first_line, property : property);

      if (property.isExpanded) {
        var second_line = first_line;
        second_line.y += UnityEditor.EditorGUIUtility.singleLineHeight;

        UnityEditor.EditorGUIUtility.labelWidth = 50f;

        second_line.x += 15f; // indentation
        second_line.width -= 15f;

        var second_line_key = second_line;

        var button_width = 60f;
        second_line_key.width -= button_width; // assign button
        second_line_key.width /= 2f;

        var second_line_value = second_line_key;
        second_line_value.x += second_line_value.width;
        if (this.GetTemplateValueProp(main_prop : property).hasVisibleChildren) {
          // if the value has children, indent to make room for fold arrow
          second_line_value.x += 15;
          second_line_value.width -= 15;
        }

        var second_line_button = second_line_value;
        second_line_button.x += second_line_value.width;
        second_line_button.width = button_width;

        var k_height =
            UnityEditor.EditorGUI.GetPropertyHeight(property : this.GetTemplateKeyProp(main_prop : property));
        var v_height =
            UnityEditor.EditorGUI.GetPropertyHeight(property :
                                                    this.GetTemplateValueProp(main_prop : property));
        var extra_height = UnityEngine.Mathf.Max(a : k_height, b : v_height);

        second_line_key.height = k_height;
        second_line_value.height = v_height;

        UnityEditor.EditorGUI.PropertyField(position : second_line_key,
                                            property : this.GetTemplateKeyProp(main_prop : property),
                                            true);
        UnityEditor.EditorGUI.PropertyField(position : second_line_value,
                                            property : this.GetTemplateValueProp(main_prop : property),
                                            true);

        var keys_prop = this.GetKeysProp(main_prop : property);
        var values_prop = this.GetValuesProp(main_prop : property);

        var num_lines = keys_prop.arraySize;

        if (UnityEngine.GUI.Button(position : second_line_button, "Assign")) {
          var assignment = false;
          for (var i = 0; i < num_lines; i++)
              // Try to replace existing value
          {
            if (SerializedPropertyExtension.EqualBasics(left : this.GetIndexedItemProp(array_prop : keys_prop,
                                                          index : i),
                                                        right : this.GetTemplateKeyProp(main_prop : property))
            ) {
              SerializedPropertyExtension.CopyBasics(source : this.GetTemplateValueProp(main_prop : property),
                                                     target : this.GetIndexedItemProp(array_prop :
                                                       values_prop,
                                                       index : i));
              assignment = true;
              break;
            }
          }

          if (!assignment) {
            // Create a new value
            keys_prop.arraySize += 1;
            values_prop.arraySize += 1;
            SerializedPropertyExtension.CopyBasics(source : this.GetTemplateKeyProp(main_prop : property),
                                                   target : this.GetIndexedItemProp(array_prop : keys_prop,
                                                     index : num_lines));
            SerializedPropertyExtension.CopyBasics(source : this.GetTemplateValueProp(main_prop : property),
                                                   target : this.GetIndexedItemProp(array_prop : values_prop,
                                                     index : num_lines));
          }
        }

        for (var i = 0; i < num_lines; i++) {
          second_line_key.y += extra_height;
          second_line_value.y += extra_height;
          second_line_button.y += extra_height;

          k_height =
              UnityEditor.EditorGUI.GetPropertyHeight(property :
                                                      this.GetIndexedItemProp(array_prop : keys_prop,
                                                        index : i));
          v_height =
              UnityEditor.EditorGUI.GetPropertyHeight(property :
                                                      this.GetIndexedItemProp(array_prop : values_prop,
                                                        index : i));
          extra_height = UnityEngine.Mathf.Max(a : k_height, b : v_height);

          second_line_key.height = k_height;
          second_line_value.height = v_height;

          UnityEditor.EditorGUI.PropertyField(position : second_line_key,
                                              property : this.GetIndexedItemProp(array_prop : keys_prop,
                                                index : i),
                                              true);
          UnityEditor.EditorGUI.PropertyField(position : second_line_value,
                                              property : this.GetIndexedItemProp(array_prop : values_prop,
                                                index : i),
                                              true);

          if (UnityEngine.GUI.Button(position : second_line_button, "Remove")) {
            keys_prop.DeleteArrayElementAtIndex(index : i);
            values_prop.DeleteArrayElementAtIndex(index : i);
          }
        }
      }

      UnityEditor.EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(UnityEditor.SerializedProperty property,
                                            UnityEngine.GUIContent label) {
      if (!property.isExpanded) {
        return UnityEditor.EditorGUIUtility.singleLineHeight;
      }

      var total = UnityEditor.EditorGUIUtility.singleLineHeight;

      var k_height =
          UnityEditor.EditorGUI.GetPropertyHeight(property : this.GetTemplateKeyProp(main_prop : property));
      var v_height =
          UnityEditor.EditorGUI.GetPropertyHeight(property : this.GetTemplateValueProp(main_prop : property));
      total += UnityEngine.Mathf.Max(a : k_height, b : v_height);

      var keys_prop = this.GetKeysProp(main_prop : property);
      var values_prop = this.GetValuesProp(main_prop : property);
      var num_lines = keys_prop.arraySize;
      for (var i = 0; i < num_lines; i++) {
        k_height =
            UnityEditor.EditorGUI.GetPropertyHeight(property : this.GetIndexedItemProp(array_prop : keys_prop,
                                                      index : i));
        v_height =
            UnityEditor.EditorGUI.GetPropertyHeight(property :
                                                    this.GetIndexedItemProp(array_prop : values_prop,
                                                      index : i));
        total += UnityEngine.Mathf.Max(a : k_height, b : v_height);
      }

      return total;
    }

    UnityEditor.SerializedProperty GetTemplateKeyProp(UnityEditor.SerializedProperty main_prop) {
      return this.GetTemplateProp(source : this._template_key_prop, main_prop : main_prop);
    }

    UnityEditor.SerializedProperty GetTemplateValueProp(UnityEditor.SerializedProperty main_prop) {
      return this.GetTemplateProp(source : this._template_value_prop, main_prop : main_prop);
    }

    UnityEditor.SerializedProperty GetTemplateProp(
        System.Collections.Generic.Dictionary<int, UnityEditor.SerializedProperty> source,
        UnityEditor.SerializedProperty main_prop) {
      if (!source.TryGetValue(key : main_prop.GetObjectCode(), value : out var p)) {
        var template_object = this.GetTemplate();
        var template_serialized_object = new UnityEditor.SerializedObject(obj : template_object);
        var k_prop = template_serialized_object.FindProperty("key");
        var v_prop = template_serialized_object.FindProperty("value");
        this._template_key_prop[key : main_prop.GetObjectCode()] = k_prop;
        this._template_value_prop[key : main_prop.GetObjectCode()] = v_prop;
        p = source == this._template_key_prop ? k_prop : v_prop;
      }

      return p;
    }

    UnityEditor.SerializedProperty GetKeysProp(UnityEditor.SerializedProperty main_prop) {
      return this.GetCachedProp(main_prop : main_prop, "keys", source : this._keys_props);
    }

    UnityEditor.SerializedProperty GetValuesProp(UnityEditor.SerializedProperty main_prop) {
      return this.GetCachedProp(main_prop : main_prop, "values", source : this._values_props);
    }

    UnityEditor.SerializedProperty GetCachedProp(UnityEditor.SerializedProperty main_prop,
                                                 string relative_property_name,
                                                 System.Collections.Generic.Dictionary<int,
                                                     UnityEditor.SerializedProperty> source) {
      var object_code = main_prop.GetObjectCode();
      if (!source.TryGetValue(key : object_code, value : out var p)) {
        source[key : object_code] =
            p = main_prop.FindPropertyRelative(relativePropertyPath : relative_property_name);
      }

      return p;
    }

    UnityEditor.SerializedProperty GetIndexedItemProp(UnityEditor.SerializedProperty array_prop, int index) {
      if (!this._indexed_property_dicts.TryGetValue(key : array_prop.GetObjectCode(), value : out var d)) {
        this._indexed_property_dicts[key : array_prop.GetObjectCode()] =
            d = new System.Collections.Generic.Dictionary<int, UnityEditor.SerializedProperty>();
      }

      if (!d.TryGetValue(key : index, value : out var result)) {
        d[key : index] =
            result = array_prop.FindPropertyRelative(relativePropertyPath : $"Array.data[{index}]");
      }

      return result;
    }
  }
}
#endif