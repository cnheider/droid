namespace droid.Runtime.Structs.Space {
  public static class Normalisation {
    internal static float Normalise01_(float v, float min, float span) { return (v - min) / span; }
    internal static float Denormalise01_(float v, float min, float span) { return v * span + min; }

    internal static UnityEngine.Vector2 Normalise01_(UnityEngine.Vector2 v,
                                                     UnityEngine.Vector2 min,
                                                     UnityEngine.Vector2 span) {
      return (v - min) / span;
    }

    internal static UnityEngine.Vector2 Denormalise01_(UnityEngine.Vector2 v,
                                                       UnityEngine.Vector2 min,
                                                       UnityEngine.Vector2 span) {
      return v * span + min;
    }

    internal static UnityEngine.Vector3 Normalise01_(UnityEngine.Vector3 v,
                                                     UnityEngine.Vector3 min,
                                                     UnityEngine.Vector3 span) {
      return droid.Runtime.Utilities.Extensions.NeodroidUtilities.Divide(a : v - min, b : span);
    }

    internal static UnityEngine.Vector3 Denormalise01_(UnityEngine.Vector3 v,
                                                       UnityEngine.Vector3 min,
                                                       UnityEngine.Vector3 span) {
      return droid.Runtime.Utilities.Extensions.NeodroidUtilities.Multiply(a : v, b : span) + min;
    }

    internal static UnityEngine.Vector4 Normalise01_(UnityEngine.Vector4 v,
                                                     UnityEngine.Vector4 min,
                                                     UnityEngine.Vector4 span) {
      return droid.Runtime.Utilities.Extensions.NeodroidUtilities.Divide(a : v - min, b : span);
    }

    internal static UnityEngine.Vector4 Denormalise01_(UnityEngine.Vector4 v,
                                                       UnityEngine.Vector4 min,
                                                       UnityEngine.Vector4 span) {
      return droid.Runtime.Utilities.Extensions.NeodroidUtilities.Multiply(a : v, b : span) + min;
    }

    internal static float NormaliseMinusOneOne_(dynamic v, float min, float span) {
      return (Normalise01_(v : v, min : min, span : span) - 0.5f) * 2;
    }

    internal static float DenormaliseMinusOneOne_(float v, float min, float span) {
      return Denormalise01_(v : v / 2 + .5f, min : min, span : span);
    }

    internal static UnityEngine.Vector2 NormaliseMinusOneOne_(UnityEngine.Vector2 v,
                                                              UnityEngine.Vector2 min,
                                                              UnityEngine.Vector2 span) {
      return (Normalise01_(v : v, min : min, span : span) - 0.5f * UnityEngine.Vector2.one) * 2;
    }

    internal static UnityEngine.Vector2 DenormaliseMinusOneOne_(UnityEngine.Vector2 v,
                                                                UnityEngine.Vector2 min,
                                                                UnityEngine.Vector2 span) {
      return Denormalise01_(v : v / 2 + .5f * UnityEngine.Vector2.one, min : min, span : span);
    }

    internal static UnityEngine.Vector3 NormaliseMinusOneOne_(UnityEngine.Vector3 v,
                                                              UnityEngine.Vector3 min,
                                                              UnityEngine.Vector3 span) {
      return (Normalise01_(v : v, min : min, span : span) - 0.5f * UnityEngine.Vector3.one) * 2;
    }

    internal static UnityEngine.Vector3 DenormaliseMinusOneOne_(UnityEngine.Vector3 v,
                                                                UnityEngine.Vector3 min,
                                                                UnityEngine.Vector3 span) {
      return Denormalise01_(v : v / 2 + .5f * UnityEngine.Vector3.one, min : min, span : span);
    }

    internal static UnityEngine.Vector4 NormaliseMinusOneOne_(UnityEngine.Vector4 v,
                                                              UnityEngine.Vector4 min,
                                                              UnityEngine.Vector4 span) {
      return (Normalise01_(v : v, min : min, span : span) - 0.5f * UnityEngine.Vector4.one) * 2;
    }

    internal static UnityEngine.Vector4 DenormaliseMinusOneOne_(UnityEngine.Vector4 v,
                                                                UnityEngine.Vector4 min,
                                                                UnityEngine.Vector4 span) {
      return Denormalise01_(v : v / 2 + .5f * UnityEngine.Vector4.one, min : min, span : span);
    }
  }
}