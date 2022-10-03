namespace droid.Runtime.Utilities {
  /// <summary>
  /// </summary>
  public static class NeodroidUtilities {
    /// <summary>
    /// </summary>
    /// <param name="folders"></param>
    /// <param name="file_name"></param>
    /// <returns></returns>
    public static string GetPersistentDataPath(string[] folders, string file_name = null) {
      var data_path = System.IO.Path.Combine(paths : folders);
      data_path =
          System.IO.Path.Combine(path1 : UnityEngine.Application.persistentDataPath, path2 : data_path);

      if (!System.IO.Directory.Exists(path : data_path)) {
        System.IO.Directory.CreateDirectory(path : data_path);
      }

      if (file_name != null) {
        data_path = System.IO.Path.Combine(path1 : data_path, path2 : file_name);
      }

      return data_path;
    }
  }
}