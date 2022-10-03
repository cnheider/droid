#if UNITY_EDITOR

namespace droid.Editor.GameObjects {
  public class GeneralSpawner : UnityEngine.MonoBehaviour {
    [UnityEditor.MenuItem(itemName : EditorGameObjectMenuPath._GameObjectMenuPath + "SimulationManager",
                          false,
                          10)]
    static void CreateSimulationManagerGameObject(UnityEditor.MenuCommand menu_command) {
      var go = new UnityEngine.GameObject("SimulationManager");
      go.AddComponent<droid.Runtime.Managers.NeodroidManager>();
      UnityEditor.GameObjectUtility.SetParentAndAlign(child : go,
                                                      parent : menu_command
                                                                       .context as
                                                                   UnityEngine.
                                                                   GameObject); // Ensure it gets reparented if this was a context click (otherwise does nothing)
      UnityEditor.Undo.RegisterCreatedObjectUndo(objectToUndo : go,
                                                 name : $"Create {go.name}"); // Register the creation in the undo system
      UnityEditor.Selection.activeObject = go;
    }

    [UnityEditor.MenuItem(itemName : EditorGameObjectMenuPath._GameObjectMenuPath + "Environment", false, 10)]
    static void CreateEnvironmentGameObject(UnityEditor.MenuCommand menu_command) {
      var go = new UnityEngine.GameObject("Environment");
      var plane = UnityEngine.GameObject.CreatePrimitive(type : UnityEngine.PrimitiveType.Plane);
      plane.transform.parent = go.transform;
      go.AddComponent<droid.Runtime.Environments.Prototyping.PrototypingEnvironment>();
      UnityEditor.GameObjectUtility.SetParentAndAlign(child : go,
                                                      parent : menu_command
                                                                       .context as
                                                                   UnityEngine.
                                                                   GameObject); // Ensure it gets reparented if this was a context click (otherwise does nothing)
      UnityEditor.Undo.RegisterCreatedObjectUndo(objectToUndo : go,
                                                 name : $"Create {go.name}"); // Register the creation in the undo system
      UnityEditor.Selection.activeObject = go;
    }

    [UnityEditor.MenuItem(itemName : EditorGameObjectMenuPath._GameObjectMenuPath + "Actor", false, 10)]
    static void CreateActorGameObject(UnityEditor.MenuCommand menu_command) {
      var go = new UnityEngine.GameObject("Actor");
      var capsule = UnityEngine.GameObject.CreatePrimitive(type : UnityEngine.PrimitiveType.Capsule);
      capsule.transform.parent = go.transform;
      go.AddComponent<droid.Runtime.Prototyping.Actors.Actor>();
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