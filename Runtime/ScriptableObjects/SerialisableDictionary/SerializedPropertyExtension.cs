#if UNITY_EDITOR

namespace droid.Runtime.ScriptableObjects.SerialisableDictionary {
  /// <summary>
  /// </summary>
  public static class SerializedPropertyExtension {
    public static int GetObjectCode(this UnityEditor.SerializedProperty p) {
      // Unique code per serialized object and property path
      return p.propertyPath.GetHashCode() ^ p.serializedObject.GetHashCode();
    }

    public static bool EqualBasics(UnityEditor.SerializedProperty left,
                                   UnityEditor.SerializedProperty right) {
      if (left.propertyType != right.propertyType) {
        return false;
      }

      if (left.propertyType == UnityEditor.SerializedPropertyType.Integer) {
        if (left.type == right.type) {
          if (left.type == "int") {
            return left.intValue == right.intValue;
          }

          return left.longValue == right.longValue;
        }

        return false;
      }

      if (left.propertyType == UnityEditor.SerializedPropertyType.String) {
        return left.stringValue == right.stringValue;
      }

      if (left.propertyType == UnityEditor.SerializedPropertyType.ObjectReference) {
        return left.objectReferenceValue == right.objectReferenceValue;
      }

      if (left.propertyType == UnityEditor.SerializedPropertyType.Enum) {
        return left.enumValueIndex == right.enumValueIndex;
      }

      if (left.propertyType == UnityEditor.SerializedPropertyType.Boolean) {
        return left.boolValue == right.boolValue;
      }

      if (left.propertyType == UnityEditor.SerializedPropertyType.Float) {
        if (left.type == right.type) {
          if (left.type == "float") {
            return System.Math.Abs(value : left.floatValue - right.floatValue) < double.Epsilon;
          }

          return System.Math.Abs(value : left.doubleValue - right.doubleValue) < double.Epsilon;
        }

        return false;
      }

      if (left.propertyType == UnityEditor.SerializedPropertyType.Color) {
        return left.colorValue == right.colorValue;
      }

      if (left.propertyType == UnityEditor.SerializedPropertyType.LayerMask) {
        return left.intValue == right.intValue;
      }

      if (left.propertyType == UnityEditor.SerializedPropertyType.Vector2) {
        return left.vector2Value == right.vector2Value;
      }

      if (left.propertyType == UnityEditor.SerializedPropertyType.Vector3) {
        return left.vector3Value == right.vector3Value;
      }

      if (left.propertyType == UnityEditor.SerializedPropertyType.Vector4) {
        return left.vector4Value == right.vector4Value;
      }

      if (left.propertyType == UnityEditor.SerializedPropertyType.Rect) {
        return left.rectValue == right.rectValue;
      }

      if (left.propertyType == UnityEditor.SerializedPropertyType.ArraySize) {
        return left.arraySize == right.arraySize;
      }

      if (left.propertyType == UnityEditor.SerializedPropertyType.Character) {
        return left.intValue == right.intValue;
      }

      if (left.propertyType == UnityEditor.SerializedPropertyType.AnimationCurve) {
        return false;
      }

      if (left.propertyType == UnityEditor.SerializedPropertyType.Bounds) {
        return left.boundsValue == right.boundsValue;
      }

      if (left.propertyType == UnityEditor.SerializedPropertyType.Gradient) {
        return false;
      }

      if (left.propertyType == UnityEditor.SerializedPropertyType.Quaternion) {
        return left.quaternionValue == right.quaternionValue;
      }

      return false;
    }

    public static void CopyBasics(UnityEditor.SerializedProperty source,
                                  UnityEditor.SerializedProperty target) {
      if (source.propertyType != target.propertyType) {
        return;
      }

      if (source.propertyType == UnityEditor.SerializedPropertyType.Integer) {
        if (source.type == target.type) {
          if (source.type == "int") {
            target.intValue = source.intValue;
          } else {
            target.longValue = source.longValue;
          }
        }
      } else if (source.propertyType == UnityEditor.SerializedPropertyType.String) {
        target.stringValue = source.stringValue;
      } else if (source.propertyType == UnityEditor.SerializedPropertyType.ObjectReference) {
        target.objectReferenceValue = source.objectReferenceValue;
      } else if (source.propertyType == UnityEditor.SerializedPropertyType.Enum) {
        target.enumValueIndex = source.enumValueIndex;
      } else if (source.propertyType == UnityEditor.SerializedPropertyType.Boolean) {
        target.boolValue = source.boolValue;
      } else if (source.propertyType == UnityEditor.SerializedPropertyType.Float) {
        if (source.type == target.type) {
          if (source.type == "float") {
            target.floatValue = source.floatValue;
          } else {
            target.doubleValue = source.doubleValue;
          }
        }
      } else if (source.propertyType == UnityEditor.SerializedPropertyType.Color) {
        target.colorValue = source.colorValue;
      } else if (source.propertyType == UnityEditor.SerializedPropertyType.LayerMask) {
        target.intValue = source.intValue;
      } else if (source.propertyType == UnityEditor.SerializedPropertyType.Vector2) {
        target.vector2Value = source.vector2Value;
      } else if (source.propertyType == UnityEditor.SerializedPropertyType.Vector3) {
        target.vector3Value = source.vector3Value;
      } else if (source.propertyType == UnityEditor.SerializedPropertyType.Vector4) {
        target.vector4Value = source.vector4Value;
      } else if (source.propertyType == UnityEditor.SerializedPropertyType.Rect) {
        target.rectValue = source.rectValue;
      } else if (source.propertyType == UnityEditor.SerializedPropertyType.ArraySize) {
        target.arraySize = source.arraySize;
      } else if (source.propertyType == UnityEditor.SerializedPropertyType.Character) {
        target.intValue = source.intValue;
      } else if (source.propertyType == UnityEditor.SerializedPropertyType.AnimationCurve) {
        target.animationCurveValue = source.animationCurveValue;
      } else if (source.propertyType == UnityEditor.SerializedPropertyType.Bounds) {
        target.boundsValue = source.boundsValue;
      } else if (source.propertyType == UnityEditor.SerializedPropertyType.Gradient) {
        // TODO?
      } else if (source.propertyType == UnityEditor.SerializedPropertyType.Quaternion) {
        target.quaternionValue = source.quaternionValue;
      } else {
        if (source.hasChildren && target.hasChildren) {
          var source_iterator = source.Copy();
          var target_iterator = target.Copy();
          while (true) {
            if (source_iterator.propertyType == UnityEditor.SerializedPropertyType.Generic) {
              if (!source_iterator.Next(true) || !target_iterator.Next(true)) {
                break;
              }
            } else if (!source_iterator.Next(false) || !target_iterator.Next(false)) {
              break;
            }

            CopyBasics(source : source_iterator, target : target_iterator);
          }
        }
      }
    }
  }
}
#endif