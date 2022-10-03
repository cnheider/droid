﻿#if UNITY_EDITOR
namespace droid.Editor.Windows {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class WindowManager : UnityEditor.EditorWindow {
    static System.Type[] _desired_dock_next_toos = {
                                                       typeof(RenderTextureConfiguratorWindow),
                                                       typeof(CameraSynchronisationWindow),
                                                       #if NEODROID_DEBUG
                                                       typeof(DebugWindow),
                                                       #endif
                                                       typeof(SegmentationWindow),
                                                       typeof(PrototypingWindow),
                                                       typeof(TaskWindow),
                                                       typeof(DemonstrationWindow),
                                                       typeof(SimulationWindow)
                                                   };

    /// <summary>
    /// </summary>
    [UnityEditor.MenuItem(itemName : EditorWindowMenuPath._WindowMenuPath + "ShowAllWindows")]
    [UnityEditor.MenuItem(itemName : EditorWindowMenuPath._ToolMenuPath + "ShowAllWindows")]
    public static void ShowWindow() {
      //Show existing window instance. If one doesn't exist, make one.
      GetWindow<RenderTextureConfiguratorWindow>(desiredDockNextTo : _desired_dock_next_toos);
      GetWindow<CameraSynchronisationWindow>(desiredDockNextTo : _desired_dock_next_toos);
      #if NEODROID_DEBUG
      GetWindow<DebugWindow>(desiredDockNextTo : _desired_dock_next_toos);
      #endif
      GetWindow<SegmentationWindow>(desiredDockNextTo : _desired_dock_next_toos);
      GetWindow<PrototypingWindow>(desiredDockNextTo : _desired_dock_next_toos);
      GetWindow<TaskWindow>(desiredDockNextTo : _desired_dock_next_toos);
      GetWindow<DemonstrationWindow>(desiredDockNextTo : _desired_dock_next_toos);
      GetWindow<SimulationWindow>(desiredDockNextTo : _desired_dock_next_toos);
    }
  }
}
#endif