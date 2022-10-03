namespace droid.Editor.Utilities.ScriptableObjects.Editor {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEditor.CustomEditor(inspectedType : typeof(ReadMe))]
  [UnityEditor.InitializeOnLoadAttribute]
  public class ReadMeEditor : UnityEditor.Editor {
    const string _showed_readme_session_state_name = "ReadMeEditor.showedReadMe";

    const float _space = 16f;
    public const string _NewAssetPath = "Assets/";

    const string _scriptable_object_menu_path = "Tools/ReadMe/";

    [UnityEngine.SerializeField]
    UnityEngine.GUIStyle bodyStyle =
        new UnityEngine.GUIStyle(other : UnityEditor.EditorStyles.label) {wordWrap = true, fontSize = 14};

    [UnityEngine.SerializeField] UnityEngine.GUIStyle headingStyle;
    [UnityEngine.SerializeField] UnityEngine.GUIStyle linkStyle;
    [UnityEngine.SerializeField] UnityEngine.GUIStyle titleStyle;

    bool _m_initialized;

    UnityEngine.GUIStyle LinkStyle { get { return this.linkStyle; } }

    UnityEngine.GUIStyle TitleStyle { get { return this.titleStyle; } }

    UnityEngine.GUIStyle HeadingStyle { get { return this.headingStyle; } }

    UnityEngine.GUIStyle BodyStyle { get { return this.bodyStyle; } }

    static void SelectReadmeAutomatically() {
      if (!UnityEditor.SessionState.GetBool(key : _showed_readme_session_state_name, false)) {
        var readme = SelectReadme();
        UnityEditor.SessionState.SetBool(key : _showed_readme_session_state_name, true);

        if (readme && !readme.loadedLayout) {
          LoadLayout();
          readme.loadedLayout = true;
        }
      }
    }

    static void LoadLayout() {
      var assembly = typeof(UnityEditor.EditorApplication).Assembly;
      var window_layout_type = assembly.GetType("UnityEditor.WindowLayout", true);
      var method = window_layout_type.GetMethod("LoadWindowLayout",
                                                bindingAttr : System.Reflection.BindingFlags.Public
                                                              | System.Reflection.BindingFlags.Static);
      method?.Invoke(null,
                     parameters : new object[] {
                                                   System.IO.Path.Combine(path1 : UnityEngine.Application
                                                         .dataPath,
                                                     "Excluded/Common/ReadMe/Layout.wlt"),
                                                   false
                                               });
    }

    [UnityEditor.MenuItem(itemName : _scriptable_object_menu_path + "Show ReadMe")]
    static ReadMe SelectReadme() {
      var ids = UnityEditor.AssetDatabase.FindAssets("ReadMe t:ReadMe");
      if (ids.Length == 1) {
        var readme_object =
            UnityEditor.AssetDatabase.LoadMainAssetAtPath(assetPath : UnityEditor.AssetDatabase
                                                              .GUIDToAssetPath(guid : ids[0]));

        UnityEditor.Selection.objects = new[] {readme_object};

        return (ReadMe)readme_object;
      }

      UnityEngine.Debug.Log("Couldn't find a readme");
      return null;
    }

    /// <summary>
    /// </summary>
    [UnityEditor.MenuItem(itemName : _scriptable_object_menu_path + "Create new ReadMe")]
    public static void CreateReadMeAsset() {
      var asset = CreateInstance<ReadMe>();

      UnityEditor.AssetDatabase.CreateAsset(asset : asset, path : $"{_NewAssetPath}NewReadMe.asset");
      UnityEditor.AssetDatabase.SaveAssets();

      UnityEditor.EditorUtility.FocusProjectWindow();

      UnityEditor.Selection.activeObject = asset;
    }

    /// <summary>
    /// </summary>
    protected override void OnHeaderGUI() {
      var readme = (ReadMe)this.target;
      this.Init();

      var icon_width =
          UnityEngine.Mathf.Min(a : UnityEditor.EditorGUIUtility.currentViewWidth / 3f - 20f, 128f);

      UnityEngine.GUILayout.BeginHorizontal("In BigTitle");
      {
        UnityEngine.GUILayout.Label(image : readme.icon,
                                    UnityEngine.GUILayout.Width(width : icon_width),
                                    UnityEngine.GUILayout.Height(height : icon_width));
        UnityEngine.GUILayout.Label(text : readme.title, style : this.TitleStyle);
      }
      UnityEngine.GUILayout.EndHorizontal();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void OnInspectorGUI() {
      var readme = (ReadMe)this.target;
      this.Init();

      if (readme.sections != null) {
        foreach (var section in readme.sections) {
          if (!string.IsNullOrEmpty(value : section.heading)) {
            UnityEngine.GUILayout.Label(text : section.heading, style : this.HeadingStyle);
          }

          if (!string.IsNullOrEmpty(value : section.text)) {
            UnityEngine.GUILayout.Label(text : section.text, style : this.BodyStyle);
          }

          if (!string.IsNullOrEmpty(value : section.linkText)) {
            if (NeodroidEditorUtilities.LinkLabel(label : new UnityEngine.GUIContent(text : section.linkText),
                                                  link_style : this.LinkStyle)) {
              UnityEngine.Application.OpenURL(url : section.url);
            }
          }

          UnityEngine.GUILayout.Space(pixels : _space);
        }
      }
    }

    void Init() {
      if (this._m_initialized) {
        return;
      }

      this.titleStyle = new UnityEngine.GUIStyle(other : this.bodyStyle) {fontSize = 26};

      this.headingStyle = new UnityEngine.GUIStyle(other : this.bodyStyle) {fontSize = 18};

      this.linkStyle = new UnityEngine.GUIStyle(other : this.bodyStyle) {
                                                                            wordWrap = false,
                                                                            normal = {
                                                                                textColor =
                                                                                    new UnityEngine.
                                                                                        Color(r : 0x00
                                                                                            / 255f,
                                                                                          g : 0x78
                                                                                            / 255f,
                                                                                          b : 0xDA
                                                                                            / 255f,
                                                                                          1f)
                                                                            },
                                                                            stretchWidth = false
                                                                        };
      // Match selection color which works nicely for both light and dark skins

      this._m_initialized = true;
    }
  }
}