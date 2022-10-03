﻿#if UNITY_EDITOR
namespace droid.Runtime.Utilities.Drawing {
  public static partial class NeodroidUtilities {
    /// <summary>
    /// </summary>
    /// <param name="text"></param>
    /// <param name="world_pos"></param>
    /// <param name="color"></param>
    /// <param name="o_x"></param>
    /// <param name="o_y"></param>
    public static void DrawString(string text,
                                  UnityEngine.Vector3 world_pos,
                                  UnityEngine.Color? color = null,
                                  float o_x = 0,
                                  float o_y = 0) {
      UnityEditor.Handles.BeginGUI();

      var restore_color = UnityEngine.GUI.color;

      if (color.HasValue) {
        UnityEngine.GUI.color = color.Value;
      }

      var view = UnityEditor.SceneView.currentDrawingSceneView;
      var screen_pos = view.camera.WorldToScreenPoint(position : world_pos);

      if (screen_pos.y < 0
          || screen_pos.y > UnityEngine.Screen.height
          || screen_pos.x < 0
          || screen_pos.x > UnityEngine.Screen.width
          || screen_pos.z < 0) {
        UnityEngine.GUI.color = restore_color;
        UnityEditor.Handles.EndGUI();
        return;
      }

      UnityEditor.Handles.Label(position : TransformByPixel(position : world_pos, x : o_x, y : o_y),
                                text : text);

      UnityEngine.GUI.color = restore_color;
      UnityEditor.Handles.EndGUI();
    }

    /// <summary>
    /// </summary>
    /// <param name="position"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static UnityEngine.Vector3 TransformByPixel(UnityEngine.Vector3 position, float x, float y) {
      return TransformByPixel(position : position, translate_by : new UnityEngine.Vector3(x : x, y : y));
    }

    /// <summary>
    /// </summary>
    /// <param name="position"></param>
    /// <param name="translate_by"></param>
    /// <returns></returns>
    public static UnityEngine.Vector3 TransformByPixel(UnityEngine.Vector3 position,
                                                       UnityEngine.Vector3 translate_by) {
      var cam = UnityEditor.SceneView.currentDrawingSceneView.camera;
      return cam
                 ? cam.ScreenToWorldPoint(position : cam.WorldToScreenPoint(position : position)
                                                     + translate_by)
                 : position;
    }
  }
}
#endif