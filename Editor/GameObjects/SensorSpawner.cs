#if UNITY_EDITOR

namespace droid.Editor.GameObjects {
  /// <summary>
  /// </summary>
  public class SensorSpawner : UnityEngine.MonoBehaviour {
    [UnityEditor.MenuItem(itemName : EditorGameObjectMenuPath._GameObjectMenuPath + "Sensors/Base",
                          false,
                          10)]
    static void CreateSensorGameObject(UnityEditor.MenuCommand menu_command) {
      var go = new UnityEngine.GameObject("Sensor");
      go.AddComponent<droid.Runtime.Prototyping.Sensors.Sensor>();
      UnityEditor.GameObjectUtility.SetParentAndAlign(child : go,
                                                      parent : menu_command
                                                                       .context as
                                                                   UnityEngine.
                                                                   GameObject); // Ensure it gets reparented if this was a context click (otherwise does nothing)
      UnityEditor.Undo.RegisterCreatedObjectUndo(objectToUndo : go,
                                                 name : $"Create {go.name}"); // Register the creation in the undo system
      UnityEditor.Selection.activeObject = go;
    }

    [UnityEditor.MenuItem(itemName : EditorGameObjectMenuPath._GameObjectMenuPath + "Sensors/EulerTransform",
                          false,
                          10)]
    static void CreateEulerTransformSensorGameObject(UnityEditor.MenuCommand menu_command) {
      var go = new UnityEngine.GameObject("EulerTransformSensor");
      go.AddComponent<droid.Runtime.Prototyping.Sensors.Spatial.Transform.EulerTransformSensor>();
      UnityEditor.GameObjectUtility.SetParentAndAlign(child : go,
                                                      parent : menu_command
                                                                       .context as
                                                                   UnityEngine.
                                                                   GameObject); // Ensure it gets reparented if this was a context click (otherwise does nothing)
      UnityEditor.Undo.RegisterCreatedObjectUndo(objectToUndo : go,
                                                 name : $"Create {go.name}"); // Register the creation in the undo system
      UnityEditor.Selection.activeObject = go;
    }
  }
}
#endif