using UnityEngine;
using System.Collections;

public class Quick : MonoBehaviour {

	public char player_owner;
	public int direction;
	PlayerScript enemy_script;
	PlayerScript owner_script;	
	public GameObject x;
	// Use this for initialization
	void Start () {
		Debug.Log(player_owner);
		string temp = "GAME/Player" + player_owner;
		Debug.Log(temp);
		x = GameObject.Find(temp);
		if(x == null){
			Debug.Log("Fuuuuuck");
		}
		owner_script = x.GetComponent<PlayerScript>();
	}
	
	// Update is called once per frame
	void Update () {
	//eat the booty like groceries
	}

	void OnTriggerEnter(Collider other){
		Debug.Log("???");
		if(other.name.Length > 6 && other.name.Substring(0,6) == "Player"){
			if(other.name[6] == player_owner){
				return;
			}
			enemy_script = other.GetComponent<PlayerScript>();
			Debug.Log("Hit enemy");
			if(enemy_script.damageable){
				enemy_script.HP--;
				owner_script.Hype += 5;
			}
			//Destroy(this.gameObject);
			//Apply Knockback
			other.GetComponent<Rigidbody>().AddForce(new Vector3(600 * direction, 520, 0) );
		}
		if(other.name.Contains("Quick")){
			owner_script.Hype += 3;
			Destroy(this.gameObject);
			Destroy(other.gameObject);
		}
	}
}
