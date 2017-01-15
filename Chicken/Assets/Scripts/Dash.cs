using UnityEngine;
using System.Collections;

public class Dash : MonoBehaviour {

	public int player_owner;
	PlayerScript enemy_script;
	PlayerScript owner_script;	
	// Use this for initialization
	void Start () {
		owner_script = GameObject.Find("Player"+player_owner).GetComponent<PlayerScript>();
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
			if(enemy_script.damageable){
				enemy_script.HP--;
				owner_script.Hype += 5;
			}
			other.GetComponent<Rigidbody>().AddForce(new Vector3(0, 520, 0));
			Destroy(this.gameObject);
			//Apply Knockback
		}
		if(other.name.Contains("Dash")){
			owner_script.Hype += 4;
			Destroy(this.gameObject);
			Destroy(other.gameObject);
		}
		else if(other.name.Contains("Quick") || other.name.Contains("Swipe")){
			owner_script.Hype += 3;
			Destroy(other.gameObject);
		}
		
	}
}
