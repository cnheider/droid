using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnChildCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public delegate void OnCollisionDelegate(Collision collision);

  OnCollisionDelegate _collision_delegate;
  public OnCollisionDelegate CollisionDelegate {
    set { _collision_delegate = value; }
  }

  private void OnCollisionEnter(Collision collision) {
    _collision_delegate(collision);
  }
}
