namespace droid.Samples.MultiArmedBandit.Evaluation {
  using Enumerable = System.Linq.Enumerable;

  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName :
                                 droid.Runtime.Prototyping.ObjectiveFunctions.EvaluationComponentMenuPath
                                      ._ComponentMenuPath
                                 + "MultiArmedBandit"
                                 + droid.Runtime.Prototyping.ObjectiveFunctions.EvaluationComponentMenuPath
                                        ._Postfix)]
  public class MultiArmedBanditObjective : droid.Runtime.Prototyping.ObjectiveFunctions.EpisodicObjective {
    [UnityEngine.SerializeField] droid.Samples.MultiArmedBandit.Actuators.MultiArmedBanditActuator _arms;
    [UnityEngine.SerializeField] float[] _normalised_values;

    [UnityEngine.SerializeField]
    droid.Samples.MultiArmedBandit.Displayers.TextBarPlotDisplayer _text_bar_plot_displayer = null;

    void Update() { this.ComputeNormalisedValues(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void RemotePostSetup() {
      if (this._arms == null) {
        this._arms = FindObjectOfType<droid.Samples.MultiArmedBandit.Actuators.MultiArmedBanditActuator>();
      }

      this.ComputeNormalisedValues();
    }

    void ComputeNormalisedValues() {
      /*var sum = this._arms.WinAmounts.Sum();
this._normalised_values = new Single[this._arms.WinAmounts.Length];
for (var i = 0; i < this._arms.WinAmounts.Length; i++) {
  this._normalised_values[i] = this._arms.WinAmounts[i] / sum;
}*/

      var values = Enumerable.ToArray(Enumerable.Zip(this._arms.WinAmounts,
                                                     second : this._arms.WinLikelihoods,
                                                     resultSelector : (f, f1) => f * f1));
      var values_sum = Enumerable.Sum(values);

      this._normalised_values = new float[values.Length];
      for (var i = 0; i < values.Length; i++) {
        this._normalised_values[i] = values[i] / values_sum;
      }

      this._text_bar_plot_displayer.Display(values : this._normalised_values);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void InternalReset() { this.ComputeNormalisedValues(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    /// <exception cref="T:System.NotImplementedException"></exception>
    public override float InternalEvaluate() {
      if (this._arms.Won) {
        return this._arms.WinAmounts[this._arms.LastIndex];
      }

      return 0;
    }
  }
}