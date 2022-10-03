namespace droid.Runtime.Sampling {
  public class UniformnessTest : UnityEngine.MonoBehaviour {

    const int _max_cnt = 100000;
    void Start() {
      var total = 0f;
      var a = 0;
      var b = 0;
      var c = 0;
      for (var i = 0; i < _max_cnt; i++) {
        var v = UnityEngine.Random.Range(0, maxInclusive : (float)3);
        total += v;
        switch ((int)v) {
          case 0:
            a++;
            break;
          case 1:
            b++;
            break;
          case 2:
            c++;
            break;
        }
      }

      UnityEngine.Debug.Log(message : total / _max_cnt);
      UnityEngine.Debug.Log(message : a);
      UnityEngine.Debug.Log(message : b);
      UnityEngine.Debug.Log(message : c);
    }
  }
}