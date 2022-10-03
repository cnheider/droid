namespace droid.Samples.MultiArmedBandit.Actuators {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName :
                                 droid.Runtime.Prototyping.Actuators.ActuatorComponentMenuPath
                                      ._ComponentMenuPath
                                 + "RestlessMultiArmedBandit"
                                 + droid.Runtime.Prototyping.Actuators.ActuatorComponentMenuPath._Postfix)]
  public class RestlessMultiArmedBanditActuator : MultiArmedBanditActuator {
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Setup() {
      var mvs = this.MotionSpace;
      mvs.Min = 0;
      mvs.Max = this._Indicators.Length - 1;
      this.MotionSpace = mvs;

      this.ReAssignValues();
      this.StartCoroutine(routine : this.ExecuteAfterTime(2));
    }

    protected void ReAssignValues() {
      for (var index = 0; index < this._Win_Likelihoods.Length; index++) {
        this._Win_Likelihoods[index] = UnityEngine.Random.Range(0.1f, 0.9f);
        this._Win_Amounts[index] = UnityEngine.Random.Range(0.1f, 0.9f);
      }

      if (this._Win_Likelihoods == null || this._Win_Likelihoods.Length == 0) {
        this._Win_Likelihoods = new float[this._Indicators.Length];
        for (var index = 0; index < this._Indicators.Length; index++) {
          this._Win_Likelihoods[index] = 0.5f;
        }
      }

      if (this._Win_Amounts == null || this._Win_Amounts.Length == 0) {
        this._Win_Amounts = new float[this._Indicators.Length];
        for (var index = 0; index < this._Indicators.Length; index++) {
          this._Win_Amounts[index] = 10f;
        }
      }
    }

    System.Collections.IEnumerator ExecuteAfterTime(float time) {
      while (true) {
        yield return new UnityEngine.WaitForSeconds(seconds : time);

        this.ReAssignValues();
      }
    }
  }
}