using UnityEngine;

[RequireComponent(typeof(Collider))]
public class OnChildCollision : MonoBehaviour {

  public delegate void OnCollisionDelegate(Collision collision);

  OnCollisionDelegate _collision_delegate;
  public OnCollisionDelegate CollisionDelegate {
    set { _collision_delegate = value; }
  }

  private void OnCollisionEnter(Collision collision) {
    _collision_delegate(collision);
  }
}
