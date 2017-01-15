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

	void OnTriggerEnter(Collider other){
        if (link == "")
        {
            float rand = Random.value * 100 + 40;
            if (Mathf.Floor(rand) % 3 == 0)
                link = "Warzone";
            if (Mathf.Floor(rand) % 3 == 1)
                link = "Ultimate Location";
            if (Mathf.Floor(rand) % 3 == 2)
                link = "Cockfight Arena";
        }
        if (!other.name.Contains("Player")){
			SceneManager.LoadScene(link);
		}
	}
}
