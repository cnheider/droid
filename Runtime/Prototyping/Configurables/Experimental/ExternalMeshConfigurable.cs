﻿using droid.Runtime.Environments;
using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Utilities.Misc;
using droid.Runtime.Utilities.Structs;
using UnityEngine;

namespace droid.Runtime.Prototyping.Configurables.Experimental {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ConfigurableComponentMenuPath._ComponentMenuPath
                    + "ExternalMesh"
                    + ConfigurableComponentMenuPath._Postfix)]
  public class ExternalMeshConfigurable : Configurable {
    /// <summary>
    ///   Red
    /// </summary>
    string _texture_str;

    [SerializeField] Texture _texture = null;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() { this._texture_str = this.Identifier + "Texture"; }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          NeodroidUtilities.RegisterComponent((PrototypingEnvironment)this.ParentEnvironment,
                                              (Configurable)this,
                                              this._texture_str);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>n
    protected override void UnRegisterComponent() {
      this.ParentEnvironment?.UnRegister(this, this._texture_str);
    }

    /// <summary>
    /// </summary>
    /// <param name="configuration"></param>
    public override void ApplyConfiguration(IConfigurableConfiguration configuration) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        DebugPrinting.ApplyPrint(this.Debugging, configuration, this.Identifier);
      }
      #endif

      if (configuration.ConfigurableName == this._texture_str) {
        if (this._texture) {
          this._texture.anisoLevel = (int)configuration.ConfigurableValue;
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override IConfigurableConfiguration SampleConfiguration() {
      return new Configuration(this._texture_str, Space1.ZeroOne.Sample());
    }
  }
}
