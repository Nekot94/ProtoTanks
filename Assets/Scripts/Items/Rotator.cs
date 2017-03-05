using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {


	
	// Update is called once per frame
	void Update () {
		transform.Rotate(50f*Time.deltaTime,50f*Time.deltaTime,0);
	}
}
