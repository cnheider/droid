namespace droid.Runtime.Inference {
  public class TorchInference : UnityEngine.MonoBehaviour {
    void Start() { InitNetwork(); }

    [System.Runtime.InteropServices.DllImportAttribute("networks")]
    static extern void InitNetwork();

    [System.Runtime.InteropServices.DllImportAttribute("networks")]
    static extern void ApplyNetwork(ref float data, ref float output);

    void SomeFunction() {
      var input = new float[1 * 3 * 64 * 64];
      var output = new float[1 * 5 * 64 * 64];

      // Load input with whatever data you want

      ApplyNetwork(data : ref input[0], output : ref output[0]);

      // Do whatever you want with the output
    }
  }
}