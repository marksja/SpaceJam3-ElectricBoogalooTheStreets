using UnityEngine;
using System.Collections;

public class Dash : MonoBehaviour {

	public int player_owner;
	PlayerScript enemy_script;
    public AudioSource hit, clank;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		Debug.Log("???");
		if(other.name.Length > 6 && other.name.Substring(0,6) == "Player"){
            hit.Play();
			if(other.name[6] == (char)(player_owner + '0')){
				return;
			}
			enemy_script = other.GetComponent<PlayerScript>();
			Debug.Log("Hit enemy");
			if(enemy_script.damageable){
				enemy_script.HP--;
			}
			other.GetComponent<Rigidbody>().AddForce(new Vector3(0, 520, 0));
			Destroy(this.gameObject);
			//Apply Knockback
		}
		if(other.name == "Dash_Prefab(Clone)"){
            if (!clank.isPlaying)
                clank.Play();
			//delete other projectile
		}
	}
}
