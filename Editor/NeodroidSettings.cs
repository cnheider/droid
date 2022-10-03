namespace droid.Editor {
  /// <inheritdoc />
  /// <summary>
  ///   Create a new type of Settings Asset.
  /// </summary>
  [System.SerializableAttribute]
  class NeodroidSettings : UnityEngine.ScriptableObject {
    static bool _force = false;

    internal static NeodroidSettings Current {
      get {
        var settings =
            UnityEditor.AssetDatabase.LoadAssetAtPath<NeodroidSettings>(assetPath : NeodroidEditorConstants
                                                                            ._NeodroidSettingsPath);
        if (settings == null) {
          settings = Defaults();

          var path = System.IO.Path.GetDirectoryName(path : NeodroidEditorConstants._NeodroidSettingsPath);
          System.IO.Directory.CreateDirectory(path : path);
          UnityEditor.AssetDatabase.CreateAsset(asset : settings,
                                                path : NeodroidEditorConstants._NeodroidSettingsPath);
          UnityEditor.AssetDatabase.SaveAssets();
        }

        return settings;
      }
    }

    void OnValidate() {
      //ReapplyProperties();
    }

    internal static NeodroidSettings Defaults() {
      var settings = CreateInstance<NeodroidSettings>();
      settings.NeodroidEnableDebugProp = false;
      settings.NeodroidGithubExtensionProp = false;
      settings.NeodroidIsPackageProp = false;
      settings.NeodroidImportLocationProp = NeodroidEditorConstants._Default_Import_Location;

      settings.NeodroidGeneratePreviewsProp = false;
      settings.NeodroidPreviewsLocationProp = NeodroidEditorConstants._Default_Scene_Previews_Location;
      settings.NeodroidGenerateDescriptionsProp = false;
      settings.NeodroidDescriptionLocationProp = NeodroidEditorConstants._Default_Scene_Description_Location;

      return settings;
    }

    internal static void ReapplyProperties(bool force = false) {
      _force = force;
      Current.NeodroidEnableDebugProp = Current.NeodroidEnableDebug;
      Current.NeodroidGithubExtensionProp = Current.NeodroidGithubExtension;
      Current.NeodroidIsPackageProp = Current.NeodroidIsPackage;
      Current.NeodroidImportLocationProp = Current.NeodroidImportLocation;
      Current.NeodroidGeneratePreviewsProp = Current.NeodroidGeneratePreviews;
      Current.NeodroidPreviewsLocationProp = Current.NeodroidPreviewsLocation;
      Current.NeodroidGenerateDescriptionsProp = Current.NeodroidGenerateDescriptions;
      Current.NeodroidDescriptionLocationProp = Current.NeodroidDescriptionLocation;
      _force = false;
    }

    internal static UnityEditor.SerializedObject GetSerializedSettings() {
      var serialized_object = new UnityEditor.SerializedObject(obj : Current);
      return serialized_object;
    }

    #region Fields

    [UnityEngine.SerializeField] bool NeodroidEnableDebug = false;

    [UnityEngine.SerializeField] bool NeodroidGithubExtension = false;

    [UnityEngine.SerializeField] bool NeodroidIsPackage = false;

    [UnityEngine.SerializeField]
    string NeodroidImportLocation = NeodroidEditorConstants._Default_Import_Location;

    [UnityEngine.SerializeField] bool NeodroidGeneratePreviews = false;

    [UnityEngine.SerializeField]
    string NeodroidPreviewsLocation = NeodroidEditorConstants._Default_Scene_Previews_Location;

    [UnityEngine.SerializeField] bool NeodroidGenerateDescriptions = false;

    [UnityEngine.SerializeField]
    string NeodroidDescriptionLocation = NeodroidEditorConstants._Default_Scene_Description_Location;

    #endregion

    #region Properties

    public bool NeodroidEnableDebugProp {
      get { return this.NeodroidEnableDebug; }
      set {
        if (value != this.NeodroidEnableDebug || _force) {
          ApplyDebug(value : value);
          this.NeodroidEnableDebug = value;
        }
      }
    }

    public static void ApplyDebug(bool value) {
      if (value) {
        UnityEngine.Debug.Log("Neodroid Debugging enabled");
        droid.Editor.Utilities.DefineSymbolsFunctionality.AddDebugDefineSymbol();
      } else {
        UnityEngine.Debug.Log("Neodroid Debugging disabled");
        droid.Editor.Utilities.DefineSymbolsFunctionality.RemoveDebugDefineSymbols();
      }
    }

    public bool NeodroidGithubExtensionProp {
      get { return this.NeodroidGithubExtension; }
      set {
        if (value != this.NeodroidGithubExtension || _force) {
          ApplyGithubExt(value : value);

          this.NeodroidGithubExtension = value;
        }
      }
    }

    public static void ApplyGithubExt(bool value) {
      if (value) {
        droid.Editor.Utilities.DefineSymbolsFunctionality.AddGithubDefineSymbols();
        UnityEngine.Debug.Log("Neodroid GitHub Extension enabled");
      } else {
        droid.Editor.Utilities.DefineSymbolsFunctionality.RemoveGithubDefineSymbols();
        UnityEngine.Debug.Log("Neodroid GitHub Extension disabled");
      }
    }

    public bool NeodroidIsPackageProp {
      get { return this.NeodroidIsPackage; }
      set {
        if (value != this.NeodroidIsPackage || _force) {
          ApplyIsPackage(value : value);
          this.NeodroidIsPackage = value;
        }
      }
    }

    public static void ApplyIsPackage(bool value) {
      if (value) {
        droid.Editor.Utilities.DefineSymbolsFunctionality.AddIsPackageDefineSymbols();
        UnityEngine.Debug.Log("Neodroid is set as an imported asset");
      } else {
        droid.Editor.Utilities.DefineSymbolsFunctionality.RemoveIsPackageDefineSymbols();
        UnityEngine.Debug.Log("Neodroid is set as an package asset");
      }
    }

    public string NeodroidImportLocationProp {
      get { return this.NeodroidImportLocation; }
      set {
        if (value != this.NeodroidImportLocation || _force) {
          var new_path = PathTrim(value : value);
          UnityEngine.Debug.Log(message : $"Setting Neodroid import location to: {new_path}");

          this.NeodroidImportLocation = new_path;
        }
      }
    }

    public bool NeodroidGeneratePreviewsProp {
      get { return this.NeodroidGeneratePreviews; }
      set { this.NeodroidGeneratePreviews = value; }
    }

    public string NeodroidPreviewsLocationProp {
      get { return this.NeodroidPreviewsLocation; }
      set {
        if (value != this.NeodroidPreviewsLocation || _force) {
          var new_path = PathTrim(value : value);
          UnityEngine.Debug.Log(message : $"Setting Neodroid ScenePreview location to: {new_path}");

          this.NeodroidPreviewsLocation = new_path;
        }
      }
    }

    public bool NeodroidGenerateDescriptionsProp {
      get { return this.NeodroidGenerateDescriptions; }
      set { this.NeodroidGenerateDescriptions = value; }
    }

    public string NeodroidDescriptionLocationProp {
      get { return this.NeodroidDescriptionLocation; }
      set {
        if (value != this.NeodroidDescriptionLocation || _force) {
          var new_path = PathTrim(value : value);
          UnityEngine.Debug.Log(message : $"Setting Neodroid SceneDescription location to: {new_path}");

          this.NeodroidDescriptionLocation = new_path;
        }
      }
    }

    public static string PathTrim(string value) {
      var new_path = $"{value.TrimEnd('/')}/";
      return new_path;
    }

    #endregion
  }

  /// <inheritdoc />
  /// <summary>
  ///   SettingsProvider for Neodroid
  /// </summary>
  class NeodroidSettingsProvider : UnityEditor.SettingsProvider {
    UnityEditor.SerializedObject _neodroid_settings;

    public NeodroidSettingsProvider(string path,
                                    UnityEditor.SettingsScope scope = UnityEditor.SettingsScope.User) :
        base(path : path, scopes : scope) { }

    public static bool IsSettingsAvailable() {
      return System.IO.File.Exists(path : NeodroidEditorConstants._NeodroidSettingsPath);
    }

    /// <summary>
    ///   This function is called when the user clicks on the MyCustom element in the Settings window.
    /// </summary>
    public override void
        OnActivate(string search_context, UnityEngine.UIElements.VisualElement root_element) {
      this._neodroid_settings = NeodroidSettings.GetSerializedSettings();
    }

    public override void OnGUI(string search_context) {
      UnityEditor.EditorGUILayout.HelpBox(message : $"Version {droid.Runtime.NeodroidRuntimeInfo._Version}",
                                          type : UnityEditor.MessageType.Info);

      var is_package =
          this._neodroid_settings.FindProperty(propertyPath : NeodroidEditorConstants._IsPackage_Pref_Key);
      UnityEditor.EditorGUILayout.PropertyField(property : is_package, label : Styles._IsPackage);
      if (!is_package.boolValue) {
        UnityEditor.EditorGUILayout.HelpBox("Enter import path of Neodroid",
                                            type : UnityEditor.MessageType.Info);
        UnityEditor.EditorGUILayout.PropertyField(property :
                                                  this._neodroid_settings.FindProperty(propertyPath :
                                                    NeodroidEditorConstants._Import_Location_Pref_Key),
                                                  label : Styles._ImportLocation);
      }

      UnityEditor.EditorGUILayout.HelpBox("Functionality", type : UnityEditor.MessageType.Info);

      UnityEditor.EditorGUILayout.PropertyField(property :
                                                this._neodroid_settings.FindProperty(propertyPath :
                                                  NeodroidEditorConstants._Debug_Pref_Key),
                                                label : Styles._EnableNeodroidDebug);
      UnityEditor.EditorGUILayout.PropertyField(property :
                                                this._neodroid_settings.FindProperty(propertyPath :
                                                  NeodroidEditorConstants._Github_Extension_Pref_Key),
                                                label : Styles._EnableGithubExtension);

      var generate_scene_preview =
          this._neodroid_settings.FindProperty(propertyPath : NeodroidEditorConstants
                                                   ._Generate_Previews_Pref_Key);
      UnityEditor.EditorGUILayout.PropertyField(property : generate_scene_preview,
                                                label : Styles._GenerateScenePreview);
      if (generate_scene_preview.boolValue) {
        UnityEditor.EditorGUILayout.HelpBox("Enter path for scene preview storage",
                                            type : UnityEditor.MessageType.Info);
        UnityEditor.EditorGUILayout.PropertyField(property :
                                                  this._neodroid_settings.FindProperty(propertyPath :
                                                    NeodroidEditorConstants
                                                        ._Generate_Previews_Loc_Pref_Key),
                                                  label : Styles._ScenePreviewLocation);
      }

      var generate_scene_descriptions =
          this._neodroid_settings.FindProperty(propertyPath : NeodroidEditorConstants
                                                   ._Generate_Descriptions_Pref_Key);
      UnityEditor.EditorGUILayout.PropertyField(property : generate_scene_descriptions,
                                                label : Styles._GenerateSceneDescription);
      if (generate_scene_descriptions.boolValue) {
        UnityEditor.EditorGUILayout.HelpBox("Enter path for scene description storage",
                                            type : UnityEditor.MessageType.Info);
        UnityEditor.EditorGUILayout.PropertyField(property :
                                                  this._neodroid_settings.FindProperty(propertyPath :
                                                    NeodroidEditorConstants
                                                        ._Generate_Descriptions_Loc_Pref_Key),
                                                  label : Styles._SceneDescriptionLocation);
      }

      this._neodroid_settings.ApplyModifiedProperties();

      if (UnityEditor.EditorGUILayout.Toggle("Apply", false)) {
        NeodroidSettings.ReapplyProperties(true);
        UnityEditor.EditorUtility.SetDirty(target : NeodroidSettings.Current);
      }
    }

    // Register the SettingsProvider
    [UnityEditor.SettingsProviderAttribute]
    public static UnityEditor.SettingsProvider CreateNeodroidSettingsProvider() {
      if (IsSettingsAvailable()) {
        var provider =
            new NeodroidSettingsProvider(path : NeodroidEditorConstants._Neodroid_Project_Settings_Menu_Path,
                                         scope : UnityEditor.SettingsScope.Project) {
                keywords = GetSearchKeywordsFromGUIContentProperties<Styles>()
            };

        //provider.keywords = GetSearchKeywordsFromPath(NeodroidEditorConstants._Neodroid_Project_Settings_Menu_Path);

        // Automatically extract all keywords from the Styles.
        return provider;
      }

      // Settings Asset doesn't exist yet; no need to display anything in the Settings window.
      return null;
    }

    #region Nested type: Styles

    /// <summary>
    /// </summary>
    class Styles {
      public static UnityEngine.GUIContent _EnableNeodroidDebug =
          new UnityEngine.GUIContent(text : NeodroidEditorConstants._Debug_Pref_Key);

      public static UnityEngine.GUIContent _EnableGithubExtension =
          new UnityEngine.GUIContent(text : NeodroidEditorConstants._Github_Extension_Pref_Key);

      public static UnityEngine.GUIContent _IsPackage =
          new UnityEngine.GUIContent(text : NeodroidEditorConstants._IsPackage_Pref_Key);

      public static UnityEngine.GUIContent _ImportLocation =
          new UnityEngine.GUIContent(text : NeodroidEditorConstants._Import_Location_Pref_Key);

      public static UnityEngine.GUIContent _GenerateScenePreview =
          new UnityEngine.GUIContent(text : NeodroidEditorConstants._Generate_Previews_Pref_Key);

      public static UnityEngine.GUIContent _ScenePreviewLocation =
          new UnityEngine.GUIContent(text : NeodroidEditorConstants._Generate_Previews_Loc_Pref_Key);

      public static UnityEngine.GUIContent _GenerateSceneDescription =
          new UnityEngine.GUIContent(text : NeodroidEditorConstants._Generate_Descriptions_Pref_Key);

      public static UnityEngine.GUIContent _SceneDescriptionLocation =
          new UnityEngine.GUIContent(text : NeodroidEditorConstants._Generate_Descriptions_Loc_Pref_Key);
    }

    #endregion
  }
}