namespace droid.Runtime.GameObjects.SimpleMenu {
  public class Toggler : UnityEngine.MonoBehaviour {
    [UnityEngine.SerializeField] UnityEngine.GameObject togglee;

    void Update() {
#if !INPUT_SYSTEM_EXISTS
      if (UnityEngine.Input.GetKey("escape")) {
        if (this.togglee) {
          this.togglee.SetActive(value : !this.togglee.activeSelf);
        }
      }
#endif
    }
  }
}