using UnityEngine;
using System.Collections;

public class Swipe : MonoBehaviour {

	public int player_owner;
	PlayerScript enemy_script;
	PlayerScript owner_script;
	public int direction;
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
			//Apply Knockback
			other.GetComponent<Rigidbody>().AddForce(new Vector3(direction*Mathf.Cos(transform.eulerAngles.x) * 1000, direction*Mathf.Sin(transform.eulerAngles.y) * 1000));
		}
		if(other.name.Contains("Swipe")){
			Destroy(this.gameObject);
			Destroy(other.gameObject);
			owner_script.Hype += 4;
		}
		else if(other.name.Contains("Quick")){
			Destroy(other.gameObject);
			owner_script.Hype += 3;
		}
	}
}
