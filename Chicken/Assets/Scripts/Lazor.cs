using UnityEngine;
using System.Collections;

public class Lazor : MonoBehaviour {

	public int player_owner;
	PlayerScript enemy_script;
	Rigidbody rb;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
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
			}
			//Apply Knockback
			other.GetComponent<Rigidbody>().AddForce(rb.velocity * 100 + Vector3.up*300f);
			Destroy(this.gameObject);
		}
		if(other.name == "Lazor_Prefab(Clone)"){
			//delete other projectile
		}
		
	}
}
