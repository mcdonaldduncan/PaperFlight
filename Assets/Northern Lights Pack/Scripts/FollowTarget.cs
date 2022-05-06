using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour {
	public Transform target;

	private bool enable; 
	private Vector3 playerOffset;

	// Use this for initialization
	void Start () {
		if (target == null) {
			if (Camera.main != null) {
				target = Camera.main.transform;
			} else {
				Debug.Log ("FollowTarget needs a target to be assigned"); 
				enable = false;
			}
		} else {
			enable = true;
		}

		if (enable) {
			playerOffset = target.position - transform.position;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (enable) {
			transform.position = target.position - playerOffset;
		}
	}
}
