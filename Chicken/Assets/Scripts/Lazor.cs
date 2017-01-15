using UnityEngine;
using System.Collections;

public class Lazor : MonoBehaviour {

	public int player_owner;
	PlayerScript enemy_script;
	PlayerScript owner_script;
	Rigidbody rb;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
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
			//Apply Knockback
			other.GetComponent<Rigidbody>().AddForce(rb.velocity * 100 + Vector3.up*300f);
			Destroy(this.gameObject);
		}
		if(other.name.Contains("Lazor")){
			owner_script += 4;
			Destroy(this.gameObject);
			Destroy(other.gameObject);

		}
		else if(other.name.Contains("Dash") || other.name.Contains("Quick") || other.name.Contains("Swipe")){
			owner_script += 3;
			Destroy(other.gameObject);
		}
		
	}
}
