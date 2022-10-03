namespace droid.Runtime.Managers.Experimental {
  [UnityEngine.AddComponentMenu("Neodroid/Managers/NotUsed/Curriculum")]
  public class CurriculumManager : AbstractNeodroidManager {
    [UnityEngine.SerializeField] droid.Runtime.ScriptableObjects.Deprecated.Curriculum _curriculum = null;

    [UnityEngine.SerializeField] bool _draw_levels = false;

    public droid.Runtime.ScriptableObjects.Deprecated.Curriculum Curriculum1 {
      get { return this._curriculum; }
      set { this._curriculum = value; }
    }

    public bool DrawLevels { get { return this._draw_levels; } set { this._draw_levels = value; } }

    #if UNITY_EDITOR
    void OnDrawGizmosSelected() {
      if (this._draw_levels) {
        var i = 0;
        var len = this._curriculum._Levels.Length;
        foreach (var level in this._curriculum._Levels) {
          if (level._Configurable_Entries != null && level._Configurable_Entries.Length > 0) {
            var frac = i++ / (float)len;
            foreach (var entry in level._Configurable_Entries) {
              var configurable = UnityEngine.GameObject.Find(name : entry._Configurable_Name);
              if (configurable != null) {
                UnityEngine.Gizmos.color = new UnityEngine.Color(r : frac,
                                                                 0,
                                                                 b : 1 - frac,
                                                                 0.1F);
                var position = configurable.transform.position;
                UnityEngine.Gizmos.DrawSphere(center : position, radius : entry._Max_Value);
                UnityEngine.Gizmos.color = new UnityEngine.Color(1,
                                                                 1,
                                                                 1,
                                                                 0.4F);
                UnityEngine.Gizmos.DrawWireSphere(center : position, radius : entry._Max_Value);
                var pos_up = position;
                pos_up.y += entry._Max_Value;
                droid.Runtime.Utilities.Drawing.NeodroidUtilities.DrawString(text : i.ToString(),
                  world_pos : pos_up,
                  color : new UnityEngine.Color(1,
                                                1,
                                                1,
                                                1));
                var pos_left = position;
                pos_left.x += entry._Max_Value;
                droid.Runtime.Utilities.Drawing.NeodroidUtilities.DrawString(text : i.ToString(),
                  world_pos : pos_left,
                  color : new UnityEngine.Color(1,
                                                1,
                                                1,
                                                1));
                var pos_forward = position;
                pos_forward.z += entry._Max_Value;
                droid.Runtime.Utilities.Drawing.NeodroidUtilities.DrawString(text : i.ToString(),
                  world_pos : pos_forward,
                  color : new UnityEngine.Color(1,
                                                1,
                                                1,
                                                1));
                var pos_down = position;
                pos_down.y -= entry._Max_Value;
                droid.Runtime.Utilities.Drawing.NeodroidUtilities.DrawString(text : i.ToString(),
                  world_pos : pos_down,
                  color : new UnityEngine.Color(1,
                                                1,
                                                1,
                                                1));
                var pos_right = position;
                pos_right.x -= entry._Max_Value;
                droid.Runtime.Utilities.Drawing.NeodroidUtilities.DrawString(text : i.ToString(),
                  world_pos : pos_right,
                  color : new UnityEngine.Color(1,
                                                1,
                                                1,
                                                1));
                var pos_backward = position;
                pos_backward.z -= entry._Max_Value;
                droid.Runtime.Utilities.Drawing.NeodroidUtilities.DrawString(text : i.ToString(),
                  world_pos : pos_backward,
                  color : new UnityEngine.Color(1,
                                                1,
                                                1,
                                                1));
              }
            }
          }
        }
      }
    }
    #endif
    public override void Setup() { throw new System.NotImplementedException(); }
  }
}