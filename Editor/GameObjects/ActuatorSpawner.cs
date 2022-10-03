#if UNITY_EDITOR

namespace droid.Editor.GameObjects {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class ActuatorSpawner : UnityEngine.MonoBehaviour {
    [UnityEditor.MenuItem(itemName : EditorGameObjectMenuPath._GameObjectMenuPath
                                     + "Actuators/TransformActuator",
                          false,
                          10)]
    static void CreateTransformActuatorGameObject(UnityEditor.MenuCommand menu_command) {
      var go = new UnityEngine.GameObject("TransformActuator");
      go.AddComponent<droid.Runtime.Prototyping.Actuators.EulerTransform1DofActuator>();
      UnityEditor.GameObjectUtility.SetParentAndAlign(child : go,
                                                      parent : menu_command
                                                                       .context as
                                                                   UnityEngine.
                                                                   GameObject); // Ensure it gets reparented if this was a context click (otherwise does nothing)
      UnityEditor.Undo.RegisterCreatedObjectUndo(objectToUndo : go,
                                                 name : $"Create {go.name}"); // Register the creation in the undo system
      UnityEditor.Selection.activeObject = go;
    }

    [UnityEditor.MenuItem(itemName : EditorGameObjectMenuPath._GameObjectMenuPath
                                     + "Actuators/RigidbodyActuator",
                          false,
                          10)]
    static void CreateRigidbodyActuatorGameObject(UnityEditor.MenuCommand menu_command) {
      var go = new UnityEngine.GameObject("RigidbodyActuator");
      go.AddComponent<droid.Runtime.Prototyping.Actuators.Rigidbody1DofActuator>();
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