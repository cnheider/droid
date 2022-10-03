namespace droid.Runtime.Utilities.Grid {
  public abstract class GridCell : UnityEngine.MonoBehaviour {
    protected UnityEngine.Collider _Col;
    protected UnityEngine.Renderer _Rend;
    public droid.Runtime.Structs.Vectors.IntVector3 GridCoordinates { get; set; }

    public abstract void Setup(string name, UnityEngine.Material mat);
  }

  public class EmptyCell : GridCell {
    public override void Setup(string n, UnityEngine.Material mat) {
      this._Rend = this.GetComponent<UnityEngine.Renderer>();
      this._Col = this.GetComponent<UnityEngine.Collider>();
      this.name = n;
      this._Col.isTrigger = true;
      this._Rend.enabled = false;

      //Destroy (this.GetComponent<Renderer> ());
      //this.GetComponent<Renderer>().material = mat;
    }

    public void SetAsGoal(string n, UnityEngine.Material mat) {
      this.name = n;
      this._Rend.enabled = true;
      this._Rend.material = mat;
      this.tag = "Goal";
    }
  }

  public class FilledCell : GridCell {
    public override void Setup(string n, UnityEngine.Material mat) {
      this.name = n;
      this.GetComponent<UnityEngine.Collider>().isTrigger = false;
      this.GetComponent<UnityEngine.Renderer>().material = mat;
      this.tag = "Obstruction";
    }
  }

  public class GoalCell : EmptyCell {
    public override void Setup(string n, UnityEngine.Material mat) {
      this.name = n;
      this.GetComponent<UnityEngine.Collider>().isTrigger = true;
      this.GetComponent<UnityEngine.Renderer>().material = mat;
      this.tag = "Goal";
    }
  }
}