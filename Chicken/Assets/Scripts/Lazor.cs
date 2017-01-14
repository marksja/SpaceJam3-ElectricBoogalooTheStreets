using UnityEngine;
using System.Collections;

public class Lazor : MonoBehaviour {

	bool hit;
	// Use this for initialization
	void Start () {
		hit = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollsionEnter(){
		Destroy(this);
	}
}
