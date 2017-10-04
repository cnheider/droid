using Neodroid.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUpdater : MonoBehaviour {

  public NeodroidAgent _agent;
  Text _status_text;

	// Use this for initialization
	void Start () {
    if(!_agent)
      _agent = FindObjectOfType<NeodroidAgent>();
    _status_text = GetComponent<Text>();
  }
	
	// Update is called once per frame
	void Update () {
    _status_text.text = _agent.GetStatus();
	}
}
