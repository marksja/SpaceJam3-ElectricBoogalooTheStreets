using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class on : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void onClick(string level_name){
		if (level_name == "Quit") {
			Application.Quit ();
		}
		SceneManager.LoadScene (level_name);
	}


}
