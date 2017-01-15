using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button_Activator : MonoBehaviour {

	public string link;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collider other){
		if(!other.name.Contains("Player")){
			SceneManager.LoadScene(link);
		}
	}
}
