namespace droid.Runtime.Utilities {
  /// <summary>
  /// </summary>
  public static class NeodroidSceneUtilities {
    /// <summary>
    ///   Find UnityEngine.Object assignables from Generic Type T, this allows for FindObjectOfType with interfaces.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="System.ArgumentException"></exception>
    public static T[] FindObjectsOfType<T>() {
      if (FindAllObjectsOfTypeInScene<T>() is T[] obj) {
        return obj;
      }

      throw new System.ArgumentException(message :
                                         $"Found no UnityEngine.Object assignables from type {typeof(T).Name}");
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T FindObjectOfType<T>() { return FindObjectsOfType<T>()[0]; }

    /// <summary>
    /// </summary>
    /// <param name="layer"></param>
    /// <returns></returns>
    public static UnityEngine.GameObject[] FindAllGameObjectsExceptLayer(int layer) {
      var goa = UnityEngine.Object.FindObjectsOfType<UnityEngine.GameObject>();
      var game_objects = new System.Collections.Generic.List<UnityEngine.GameObject>();
      foreach (var go in goa) {
        if (go.layer != layer) {
          game_objects.Add(item : go);
        }
      }

      return game_objects.ToArray();
    }

    /// <summary>
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="layer"></param>
    /// <param name="child"></param>
    /// <returns></returns>
    public static T RecursiveFirstSelfSiblingParentGetComponent<T>(UnityEngine.Transform child)
        where T : UnityEngine.Component {
      var a = child.GetComponent<T>();
      if (a != null) {
        return a;
      }

      if (child.parent) {
        foreach (UnityEngine.Transform go in child.parent) {
          a = go.GetComponent<T>();
          if (a != null) {
            return a;
          }
        }

        a = child.parent.GetComponent<T>();
        if (a != null) {
          return a;
        }

        return RecursiveFirstSelfSiblingParentGetComponent<T>(child : child.parent);
      }

      return null;
    }

    /// <summary>
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="layer"></param>
    /// <returns></returns>
    public static UnityEngine.GameObject[] RecursiveChildGameObjectsExceptLayer(
        UnityEngine.Transform parent,
        int layer) {
      var game_objects = new System.Collections.Generic.List<UnityEngine.GameObject>();
      foreach (UnityEngine.Transform go in parent) {
        if (go) {
          if (go.gameObject.layer != layer) {
            game_objects.Add(item : go.gameObject);
            var children = RecursiveChildGameObjectsExceptLayer(parent : go, layer : layer);
            if (children != null && children.Length > 0) {
              game_objects.AddRange(collection : children);
            }
          }
        }
      }

      return game_objects.ToArray();
    }

    /// Use this method to get all loaded objects of some type, including inactive objects.
    /// This is an alternative to Resources.FindObjectsOfTypeAll (returns project assets, including prefabs), and GameObject.FindObjectsOfTypeAll (deprecated).
    public static T[] FindAllObjectsOfTypeInScene<T>() {
      //(Scene scene) {
      var results = new System.Collections.Generic.List<T>();
      for (var i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++) {
        var s = UnityEngine.SceneManagement.SceneManager.GetSceneAt(index : i); // maybe EditorSceneManager
        if (!s.isLoaded) {
          continue;
        }

        var all_game_objects = s.GetRootGameObjects();
        foreach (var go in all_game_objects) {
          results.AddRange(collection : go.GetComponentsInChildren<T>(true));
        }
      }

      return results.ToArray();
    }
  }
}