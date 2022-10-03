namespace droid.Samples.MultiArmedBandit.Actuators {
  using Enumerable = System.Linq.Enumerable;

  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName :
                                 droid.Runtime.Prototyping.Actuators.ActuatorComponentMenuPath
                                      ._ComponentMenuPath
                                 + "MultiArmedBandit"
                                 + droid.Runtime.Prototyping.Actuators.ActuatorComponentMenuPath._Postfix)]
  public class MultiArmedBanditActuator : droid.Runtime.Prototyping.Actuators.Actuator {
    [UnityEngine.SerializeField] UnityEngine.Color _inactive_color = UnityEngine.Color.yellow;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    protected UnityEngine.Material[] _Indicators;

    [UnityEngine.SerializeField] int _last_index;
    [UnityEngine.SerializeField] UnityEngine.Color _lose_color = UnityEngine.Color.red;

    [UnityEngine.SerializeField] protected float[] _Win_Amounts;

    [UnityEngine.SerializeField] UnityEngine.Color _win_color = UnityEngine.Color.green;

    [UnityEngine.SerializeField] protected float[] _Win_Likelihoods;

    [UnityEngine.SerializeField] bool _won;

    /// <summary>
    /// </summary>
    public float[] WinAmounts { get { return this._Win_Amounts; } set { this._Win_Amounts = value; } }

    /// <summary>
    /// </summary>
    public int LastIndex { get { return this._last_index; } set { this._last_index = value; } }

    /// <summary>
    /// </summary>
    public bool Won { get { return this._won; } set { this._won = value; } }

    /// <summary>
    /// </summary>
    public float[] WinLikelihoods {
      get { return this._Win_Likelihoods; }
      set { this._Win_Likelihoods = value; }
    }

    public override string[] InnerMotionNames {
      get { return Enumerable.ToArray(Enumerable.Select(this._Indicators, m => this.Identifier)); }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Setup() {
      var mvs = this.MotionSpace;
      mvs.Min = 0;
      mvs.Max = 2;
      mvs.DecimalGranularity = 0;
      this.MotionSpace = mvs;
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

    public void UpdatePayoutArm1(UnityEngine.UI.Text amount) {
      this.UpdatePayoutArm(0, amount : float.Parse(s : amount.text));
    }

    public void UpdatePayoutArm2(UnityEngine.UI.Text amount) {
      this.UpdatePayoutArm(1, amount : float.Parse(s : amount.text));
    }

    public void UpdatePayoutArm3(UnityEngine.UI.Text amount) {
      this.UpdatePayoutArm(2, amount : float.Parse(s : amount.text));
    }

    public void
        GetPayoutArm1(droid.Runtime.GameObjects.StatusDisplayer.EventRecipients.DataPoller recipient) {
      recipient.PollData(data : this.GetPayoutArm(0));
    }

    public void
        GetPayoutArm2(droid.Runtime.GameObjects.StatusDisplayer.EventRecipients.DataPoller recipient) {
      recipient.PollData(data : this.GetPayoutArm(1));
    }

    public void
        GetPayoutArm3(droid.Runtime.GameObjects.StatusDisplayer.EventRecipients.DataPoller recipient) {
      recipient.PollData(data : this.GetPayoutArm(2));
    }

    public void GetPctArm1(droid.Runtime.GameObjects.StatusDisplayer.EventRecipients.DataPoller recipient) {
      recipient.PollData(data : this.GetPctArm(0));
    }

    public void GetPctArm2(droid.Runtime.GameObjects.StatusDisplayer.EventRecipients.DataPoller recipient) {
      recipient.PollData(data : this.GetPctArm(1));
    }

    public void GetPctArm3(droid.Runtime.GameObjects.StatusDisplayer.EventRecipients.DataPoller recipient) {
      recipient.PollData(data : this.GetPctArm(2));
    }

    public void UpdatePercentageArm1(UnityEngine.UI.Text amount) {
      this.UpdatePercentageArm(0, amount : float.Parse(s : amount.text));
    }

    public void UpdatePercentageArm2(UnityEngine.UI.Text amount) {
      this.UpdatePercentageArm(1, amount : float.Parse(s : amount.text));
    }

    public void UpdatePercentageArm3(UnityEngine.UI.Text amount) {
      this.UpdatePercentageArm(2, amount : float.Parse(s : amount.text));
    }

    void UpdatePayoutArm(int index, float amount) { this._Win_Amounts[index] = amount; }

    string GetPayoutArm(int index) { return this._Win_Amounts[index].ToString("F"); }

    string GetPctArm(int index) { return this._Win_Likelihoods[index].ToString("F"); }

    void UpdatePercentageArm(int index, float amount) { this._Win_Likelihoods[index] = amount; }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    protected override void InnerApplyMotion(droid.Runtime.Interfaces.IMotion motion) {
      foreach (var indicator in this._Indicators) {
        indicator.color = this._inactive_color;
      }

      var index = (int)motion.Strength;

      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log(message : $"MultiArmedBandit got index {index}");
      }
      #endif

      this._last_index = index;

      var random_value = UnityEngine.Random.Range(0f, 1f);
      if (random_value < this._Win_Likelihoods[this._last_index]) {
        this._Indicators[this._last_index].color = this._win_color;
        this._won = true;
      } else {
        this._Indicators[this._last_index].color = this._lose_color;
        this._won = false;
      }
    }
  }
}