using UnityEngine;
using System.Collections;

public class Lazor : MonoBehaviour {

	public char player_owner;
	PlayerScript enemy_script;
	PlayerScript owner_script;
	Rigidbody rb;
    public AudioSource hit, clank;
	public GameObject x;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		Debug.Log(player_owner);
		string temp = "GAME/Player" + player_owner;
		Debug.Log(temp);
		x = GameObject.Find(temp);
		if(x == null){
			Debug.Log("Fuuuuuck");
	}
        transform.localScale = new Vector3(-Mathf.Sign(rb.GetComponent<Rigidbody>().velocity.x) * 2.144844f, 2.144844f, 0);
        owner_script = x.GetComponent<PlayerScript>();
	}
	
	// Update is called once per frame
	void Update () {
		
        
        transform.localScale = new Vector3(-Mathf.Sign(rb.GetComponent<Rigidbody>().velocity.x) * 2.144844f, 2.144844f, 0);
        
	}

	void OnTriggerEnter(Collider other){
	//	Debug.Log("???");
		if(other.name.Length > 6 && other.name.Substring(0,6) == "Player"){
 //           hit.Play();
			if(other.name[6] == player_owner){
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
			owner_script.Hype += 4;
			Destroy(this.gameObject);
			Destroy(other.gameObject);

        }
		else if(other.name.Contains("Dash") || other.name.Contains("Quick") || other.name.Contains("Swipe")){
			owner_script.Hype += 3;
			Destroy(other.gameObject);
        }
		
	}
}
