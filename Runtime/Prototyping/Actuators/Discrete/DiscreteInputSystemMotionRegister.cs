#if INPUT_SYSTEM_EXISTS

// FOR discrete input system emulation


namespace droid.Runtime.Prototyping.Actuators.Discrete {
  /// <inheritdoc />
  /// <summary>
  ///   A register of functionality to performed on transforms
  /// </summary>
  public class DiscreteInputSystemMotionRegister : UnityEngine.MonoBehaviour {

    void Exam() {
      // Create free-standing Actions.
      
      /*var look_action = new UnityEngine.InputSystem.InputAction("look", binding : "<Gamepad>/leftStick");
      var move_action = new UnityEngine.InputSystem.InputAction("move", binding : "<Gamepad>/rightStick");
*/

      //lookAction.AddBinding("<Mouse>/delta");
      //moveAction.AddCompositeBinding("Dpad").With("Up", "<Keyboard>/w").With("Down", "<Keyboard>/s").With("Left", "<Keyboard>/a").With("Right", "<Keyboard>/d");
    }

    void Exam1() {
// Create an Action Map with Actions.
      /*var map = new UnityEngine.InputSystem.InputActionMap("Gameplay");*/
      
      
      
      //var lookAction = map.AddAction("look");
      //lookAction.AddBinding("<Gamepad>/leftStick");
    }

    void Exam2(){
// Create an Action Asset.

/*
    var asset = UnityEngine.ScriptableObject.CreateInstance<UnityEngine.InputSystem.InputActionAsset>();
    var gameplay_map = new UnityEngine.InputSystem.InputActionMap("gameplay");
    */
    
    
    //asset.AddActionMap(gameplayMap);
    //var lookAction = gameplayMap.AddAction("look", "<Gamepad>/leftStick");
    
    
    }
    
    void Start()
    {
      // Create an instance of the default actions.
      /*
      var actions = new UnityEngine.InputSystem.DefaultInputActions();
      actions.Player.Look.performed += this.OnLook;
      actions.Player.Move.performed += this.OnMove;
      actions.Enable();
      */
    }

    /*
    
    void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext obj) { throw new System.NotImplementedException(); }

    void OnLook(UnityEngine.InputSystem.InputAction.CallbackContext obj) { throw new System.NotImplementedException(); }
    
    */
  }
}
#endif