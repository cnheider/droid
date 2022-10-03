namespace droid.Runtime.GameObjects.NeodroidCamera {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class ConfigurableSampleToggleList : UnityEngine.MonoBehaviour {
    [UnityEngine.SerializeField] UnityEngine.UI.Toggle _sample_toggle_button_prefab = null;
    droid.Runtime.Prototyping.Configurables.Configurable[] _all_configurables = null;

    void Awake() {
      this._all_configurables = FindObjectsOfType<droid.Runtime.Prototyping.Configurables.Configurable>();
      //this._all_configurables = FindObjectOfType<PrototypingEnvironment>().Configurables;

      foreach (var configurable in this._all_configurables) {
        if (configurable.enabled) {
          var button = Instantiate(original : this._sample_toggle_button_prefab, parent : this.transform);
          button.isOn = configurable.RandomSamplingPhaseEnum
                        == droid.Runtime.Enums.RandomSamplingPhaseEnum.On_tick_;
          button.onValueChanged.AddListener(value => Set(configurable : configurable, value : value));
          var text = button.GetComponentInChildren<UnityEngine.UI.Text>();
          button.name = configurable.Identifier;
          text.text = configurable.Identifier;
        }
      }
    }

    void Toggle(droid.Runtime.Interfaces.IConfigurable configurable) {
      if (configurable.RandomSamplingPhaseEnum != droid.Runtime.Enums.RandomSamplingPhaseEnum.Disabled_) {
        configurable.RandomSamplingPhaseEnum = droid.Runtime.Enums.RandomSamplingPhaseEnum.Disabled_;
      } else {
        configurable.RandomSamplingPhaseEnum = droid.Runtime.Enums.RandomSamplingPhaseEnum.On_tick_;
      }
    }

    static void Set(droid.Runtime.Interfaces.IConfigurable configurable, bool value) {
      if (value) {
        configurable.RandomSamplingPhaseEnum = droid.Runtime.Enums.RandomSamplingPhaseEnum.On_tick_;
      } else {
        configurable.RandomSamplingPhaseEnum = droid.Runtime.Enums.RandomSamplingPhaseEnum.Disabled_;
      }
    }
  }
}