#if TEXT_MESH_PRO_EXISTS
using TMPro;

namespace droid.Neodroid.Prototyping.Displayers {
  /// <summary>
  ///
  /// </summary>
  [ExecuteInEditMode]
  [AddComponentMenu(
      DisplayerComponentMenuPath._ComponentMenuPath + "TextMesh" + DisplayerComponentMenuPath._Postfix)]
  public class TextMeshDisplayer : Displayer {
    /// <summary>
    ///
    /// </summary>
    TextMeshPro _text;

    /// <summary>
    ///
    /// </summary>
    protected override void Setup() { this._text = this.GetComponent<TextMeshPro>(); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="text"></param>
    public void SetText(string text) {
      #if NEODROID_DEBUG
 if (this.Debugging) {
        Debug.Log("Applying " + text + " To " + this.name);
      }
  #endif

      this._text.SetText(text);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="NotImplementedException"></exception>
    public override void Display(float value) { throw new NotImplementedException(); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="NotImplementedException"></exception>
    public override void Display(Double value) { throw new NotImplementedException(); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="values"></param>
    /// <exception cref="NotImplementedException"></exception>
    public override void Display(float[] values) { throw new NotImplementedException(); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    public override void Display(String value) {
      #if NEODROID_DEBUG
 if (this.Debugging) {
        Debug.Log("Applying " + value + " To " + this.name);
      }
  #endif

      this.SetText(value);
    }
  }
}
#else
namespace droid.Runtime.Prototyping.Displayers {
  /// <summary>
  /// </summary>
  [UnityEngine.ExecuteInEditMode]
  [UnityEngine.AddComponentMenu("Neodroid/Displayers/TextMesh")]
  public class TextMeshDisplayer : Displayer {
    /// <inheritdoc />
    public override void Setup() {
      UnityEngine.Debug
                 .Log("TextMeshPro is not defined in project, add 'TEXT_MESH_PRO_EXISTS' to your unity projects 'define symbols' under the player settings or '-define:TEXT_MESH_PRO_EXISTS' in mcs.rsp to enable TextMeshPro displayer integration");
    }

    /// <summary>
    /// </summary>
    /// <param name="text"></param>
    public void SetText(string text) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log(message : "Applying " + text + " To " + this.name);
      }
      #endif
    }

    //public override void Display(Object o) { throw new NotImplementedException(); }

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public override void Display(float value) { throw new System.NotImplementedException(); }

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public override void Display(double value) { throw new System.NotImplementedException(); }

    /// <summary>
    /// </summary>
    /// <param name="values"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public override void Display(float[] values) { throw new System.NotImplementedException(); }

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    public override void Display(string value) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log(message : "Applying " + value + " To " + this.name);
      }
      #endif
      this.SetText(text : value);
    }

    public override void Display(UnityEngine.Vector3 value) { throw new System.NotImplementedException(); }
    public override void Display(UnityEngine.Vector3[] value) { throw new System.NotImplementedException(); }

    public override void Display(droid.Runtime.Structs.Points.ValuePoint points) {
      throw new System.NotImplementedException();
    }

    public override void Display(droid.Runtime.Structs.Points.ValuePoint[] points) {
      throw new System.NotImplementedException();
    }

    public override void Display(droid.Runtime.Structs.Points.StringPoint point) {
      throw new System.NotImplementedException();
    }

    public override void Display(droid.Runtime.Structs.Points.StringPoint[] points) {
      throw new System.NotImplementedException();
    }

    public override void PlotSeries(droid.Runtime.Structs.Points.ValuePoint[] points) {
      throw new System.NotImplementedException();
    }
  }
}
#endif