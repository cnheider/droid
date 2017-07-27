using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Neodroid.Evaluation {
  class EuclideanDistance : ObjectiveFunction {

    public GameObject g1, g2;

    public override float Evaluate() {
      return Vector3.Distance(g1.transform.position, g2.transform.position);
    }
  }
}
