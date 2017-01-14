using UnityEngine;
using System.Collections;

public class Swipe : MonoBehaviour {

	public int player_owner;
	PlayerScript enemy_script;
	public int direction;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		Debug.Log("???");
		if(other.name.Length > 6 && other.name.Substring(0,6) == "Player"){
			if(other.name[6] == (char)(player_owner + '0')){
				return;
			}
			enemy_script = other.GetComponent<PlayerScript>();
			Debug.Log("Hit enemy");
			enemy_script.HP--;
			//Apply Knockback
			other.GetComponent<Rigidbody>().AddForce(new Vector3(direction*Mathf.Cos(transform.eulerAngles.x) * 1000, direction*Mathf.Sin(transform.eulerAngles.y) * 1000));
		}
		if(other.name == "Swipe_Prefab(Clone)"){
			//delete other projectile
		}
	}
}
