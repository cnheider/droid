#if UNITY_EDITOR

namespace droid.Editor.Utilities {
  /// <inheritdoc />
  /// <summary>
  ///   Scene preview.
  ///   https://diegogiacomelli.com.br/unity3d-scenepreview-inspector/
  /// </summary>
  [UnityEditor.CustomEditor(inspectedType : typeof(UnityEditor.SceneAsset))]
  [UnityEditor.CanEditMultipleObjects]
  public class ScenePreview : UnityEditor.Editor {
    /// <summary>
    /// </summary>
    [UnityEngine.RuntimeInitializeOnLoadMethodAttribute]
    public static void CaptureScreenShot() {
      if (NeodroidSettings.Current.NeodroidGeneratePreviewsProp) {
        var preview_path =
            GetPreviewPath(scene_name : UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        UnityEngine.Debug.Log(message : $"Saving scene preview at {preview_path}");
        TakeScreenshot(name : preview_path);
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="name"></param>
    public static void TakeScreenshot(string name) {
      var path = System.IO.Path.GetDirectoryName(path : name);
      UnityEngine.Windows.Directory.CreateDirectory(path : path);
      // Take the screenshot
      UnityEngine.ScreenCapture.CaptureScreenshot(filename : name); // TODO: VERY broken, unitys fault

/*
      //Wait for 4 frames
      for (int i = 0; i < 5; i++)
      {
        yield return null;
      }

      // Read the data from the file
      byte[] data = File.ReadAllBytes(Application.persistentDataPath + "/" + name);

      // Create the texture
      Texture2D screenshotTexture = new Texture2D(Screen.width, Screen.height);

      // Load the image
      screenshotTexture.LoadImage(data);

      // Create a sprite
      Sprite screenshotSprite = Sprite.Create(screenshotTexture, new Rect(0, 0, Screen.width, Screen.height), new Vector2(0.5f, 0.5f));

      // Set the sprite to the screenshotPreview
      screenshotPreview.GetComponent<Image>().sprite = screenshotSprite;

      OR

          Texture2D screenImage = new Texture2D(Screen.width, Screen.height);
    //Get Image from screen
    screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
    screenImage.Apply();
    //Convert to png
    byte[] imageBytes = screenImage.EncodeToPNG();

    //Save image to file
    System.IO.File.WriteAllBytes(path, imageBytes);

*/
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void OnInspectorGUI() {
      if (NeodroidSettings.Current.NeodroidGeneratePreviewsProp) {
        //AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        var scene_names =
            System.Linq.Enumerable.ToArray(source :
                                           System.Linq.Enumerable.OrderBy(source : System.Linq.Enumerable
                                                 .Select(source : this.targets,
                                                         t => ((UnityEditor.SceneAsset)t).name),
                                             n => n));

        var previews_count = scene_names.Length;
        var preview_width = UnityEngine.Screen.width;
        var preview_height =
            (UnityEngine.Screen.height
             - NeodroidEditorConstants._Editor_Margin * 2
             - NeodroidEditorConstants._Preview_Margin * previews_count)
            / previews_count;

        for (var i = 0; i < scene_names.Length; i++) {
          DrawPreview(index : i,
                      scene_name : scene_names[i],
                      width : preview_width,
                      height : preview_height);
        }
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="index"></param>
    /// <param name="scene_name"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public static void DrawPreview(int index, string scene_name, float width, float height) {
      var preview_path = GetPreviewPath(scene_name : scene_name);
      //var ob = Resources.Load(scene_name);
      //var preview = ob as RenderTexture;
      var preview = LoadPng(file_path : preview_path);

      if (preview != null) { // TODO: Is broken
        /*
  EditorGUI.DrawPreviewTexture(new Rect(index, NeodroidEditorConstants._Editor_Margin + index * (height +

NeodroidEditorConstants._Preview_Margin), width, height),
                preview
               );
*/
        UnityEngine.GUI.DrawTexture(position : new UnityEngine.Rect(x : index,
                                                                    y : NeodroidEditorConstants._Editor_Margin
                                                                        + index
                                                                        * (height
                                                                              + NeodroidEditorConstants
                                                                                  ._Preview_Margin),
                                                                    width : width,
                                                                    height : height),
                                    image : preview,
                                    scaleMode : UnityEngine.ScaleMode.ScaleToFit);
      } else {
        UnityEditor.EditorGUILayout.HelpBox(message :
                                            $"There is no image preview for scene {scene_name} at {preview_path}. Please play the scene on editor and image preview will be captured automatically or create the missing path: {preview_path}.",
                                            type : UnityEditor.MessageType.Info);
      }
    }

    static string GetPreviewPath(string scene_name) {
      //return $"{NeodroidEditorInfo.ScenePreviewsLocation}{scene_name}.png";
      return
          $"{UnityEngine.Application.dataPath}/{NeodroidSettings.Current.NeodroidPreviewsLocationProp}{scene_name}.png";
    }

    /// <summary>
    /// </summary>
    /// <param name="file_path"></param>
    /// <returns></returns>
    public static UnityEngine.Texture2D LoadPng(string file_path) {
      UnityEngine.Texture2D tex = null;

      if (UnityEngine.Windows.File.Exists(path : file_path)) {
        var file_data = UnityEngine.Windows.File.ReadAllBytes(path : file_path);
        tex = new UnityEngine.Texture2D(2, 2);
        UnityEngine.ImageConversion.LoadImage(tex : tex,
                                              data : file_data); //..this will auto-resize the texture dimensions.
      }

      return tex;
    }
  }
}
#endif