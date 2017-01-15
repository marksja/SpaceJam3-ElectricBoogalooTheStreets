using UnityEngine;
using System.Collections;

public class Dash : MonoBehaviour {

	public char player_owner;
	PlayerScript enemy_script;	
    public AudioSource hit, clank;
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
	
	}

	void OnTriggerEnter(Collider other){
		Debug.Log("???");
		if(other.name.Length > 6 && other.name.Substring(0,6) == "Player"){
            hit.Play();
			if(other.name[6] == player_owner){
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
            if (!clank.isPlaying)
                clank.Play();
        }
		else if(other.name.Contains("Quick") || other.name.Contains("Swipe")){
			owner_script.Hype += 3;
			Destroy(other.gameObject);
            if (!clank.isPlaying)
                clank.Play();
        }
		
	}
}
