using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	int last_used;
	float cool_time;
	public bool currently_charging;
	float charge_time;
	float attack_counter;
	public bool attacking;
	public float[] length;
	public float[] charging;
	public float[] cooldown;
	public bool can_attack;
	public bool can_jump;
	public bool a_disabled;	
	public bool b_disabled;
	public bool x_disabled;
	public bool y_disabled;
	Rigidbody rb;
	public Lazor projectile;
	//public Quick hitbox_quick;
	//public Med hitbox_arc;
	public Dash hitbox_dash;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		can_attack = true;
	}
	
	// Update is called once per frame
	void Update () {
		//Check if attack charge is done
		if(currently_charging){
			charge_time += Time.deltaTime;
			if(charge_time > charging[last_used]){
				currently_charging = false;
				can_attack = true;
				switch (last_used){
					case 0: 
						A_Attack();
						break;
					case 1:
						B_Attack();
						break;
					case 2:
					 	X_Attack();
					 	break;
					case 3:
					 	Y_Attack();
					 	break;
				}
				charge_time = 0.0f;
			}
		}
		else if(attacking){
			attack_counter += Time.deltaTime;
			if(attack_counter > length[last_used]) attacking = false;
			switch (last_used){
				case 0:
					//do a thing for quick attack
					break;
				case 1:
					//Nothing
					break;
				case 2:
					rb.velocity = new Vector3(50f, 0, 0);
					if(attacking == false) rb.velocity = new Vector3(0,0,0);
					break;
				case 3:
					//Do a thing for slash
					break;
			}	
		}
		else{
			//Update can_attack
			cool_time += Time.deltaTime;
			if(cool_time > cooldown[last_used]) can_attack = true;
			
			//Movement
			var x = Input.GetAxis("LeftX") * Time.deltaTime * 10f;
			var y = -Input.GetAxis("LeftY");
			transform.Translate(x, 0, 0);

			if(y > 0 && can_jump){
				rb.AddForce(0, 450f, 0);
				can_jump = false;
			}

			//Attacks
			if(can_attack){
				if(Input.GetButtonDown("A") && !a_disabled){
					last_used = 0;
					currently_charging = true;
				}
				if(Input.GetButtonDown("B") && !b_disabled){
					last_used = 1;
					currently_charging = true;
				}
				if(Input.GetButtonDown("X") && !x_disabled){
					last_used = 2;
					currently_charging = true;
				}
				if(Input.GetButtonDown("Y") && !y_disabled){
					last_used = 3;
					currently_charging = true;
				}
			}
		}
	}

	void OnCollisionEnter() {
		can_jump = true;
	}

	void A_Attack(){
		can_attack = false;
		cool_time = 0.0f;
		Debug.Log("A attack used");
		attacking = true;
	}
	void B_Attack(){
		can_attack = false;
		cool_time = 0.0f;
		Debug.Log("B attack used");
		Lazor proj = (Lazor)Instantiate(projectile, transform.position, Quaternion.identity);
		proj.GetComponent<Rigidbody>().AddForce(500f, 0, 0);
		Destroy(proj, 5);
	}
	void X_Attack(){
		can_attack = false;
		cool_time = 0.0f;
		Debug.Log("X attack used");
		attacking = true;
		Dash hitbox = (Dash)Instantiate(hitbox_dash, transform.position, Quaternion.identity);
		hitbox.transform.parent = transform;
		Destroy(hitbox, length[2]);
		attack_counter = 0.0f;
	}
	void Y_Attack(){
		can_attack = false;
		cool_time = 0.0f;
		Debug.Log("Y attack used");
		attacking = true;
	}
}
