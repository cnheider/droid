using System;
using droid.Editor.Utilities;
using UnityEngine;

namespace droid.Editor.ScriptableObjects {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [Serializable]
  [ExecuteInEditMode]
  // ReSharper disable once RequiredBaseTypesIsNotInherited
  public class NeodroidSettings : ScriptableObject {
    static NeodroidSettings _instance;

    static string _setting_file_name = "neodroid_settings";
    static string _version = "1.3.0";

    /// <summary>
    ///   Returns the release version of the product.
    /// </summary>
    public static string Version { get { return _version; } }

    /// <summary>
    ///   Get a singleton instance of the settings class.
    /// </summary>
    public static NeodroidSettings Instance {
      get {
        if (_instance == null) {
          _instance = Resources.Load<NeodroidSettings>(_setting_file_name);

          #if UNITY_EDITOR
          // Make sure UPM(Unity Package Manager) packages resources have been added to the user project
          if (_instance == null) {
            // Open Resources Importer
            NeodroidPackageImporterWindow.ShowPackageImporterWindow();
          }
          #endif
        }

        return _instance;
      }
    }

    /// <summary>
    ///   Static Function to load the Settings file.
    /// </summary>
    /// <returns></returns>
    public static NeodroidSettings LoadDefaultSettings() {
      if (_instance == null) {
        // Load settings from Settings file
        var settings = Resources.Load<NeodroidSettings>(_setting_file_name);
        if (settings != null) {
          _instance = settings;
        }
      }

      return _instance;
    }

    /// <summary>
    ///   Returns the Sprite Asset defined in the Settings file.
    /// </summary>
    /// <returns></returns>
    public static NeodroidSettings GetSettings() {
      if (Instance == null) {
        return null;
      }

      return Instance;
    }
  }
}
