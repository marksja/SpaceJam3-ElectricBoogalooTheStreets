﻿using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	int last_used;
	public bool can_attack;
	public bool can_jump;
	public bool a_disabled;
	float a_time;
	public static a_charge;
	public static a_cooldown;	
	public bool b_disabled;
	float b_time;
	public static b_charge;
	public static b_cooldown;
	public bool x_disabled;
	float x_time;
	public static x_charge;
	public static x_cooldown;
	public bool y_disabled;
	float y_time;
	public static y_charge;
	public static y_cooldown;
	Rigidbody rb;
	public Lazor projectile;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		x_disabled = true;
		a_disabled = true;
		can_attack = true;
	}
	
	// Update is called once per frame
	void Update () {
		//Update can_attack
		/*switch (last_used){
			case 0:
				if(cool_time > a_cooldown) can_attack = true;
				break;
			case 1:
				if(cool_time > b_cooldown) can_attack = true;
				break:
			case 2:
				if(cool_time > x_cooldown) can_attack = true;
				break;
			case 3:
				if(cool_time > y_cooldown) can_attack = true;
				break;
		}*/

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
				can_attack = false;
				Debug.Log("A attack used");
			}
			if(Input.GetButtonDown("B") && !b_disabled){
				last_used = 1;
				can_attack = false;
				Debug.Log("B attack used");
				Lazor proj = (Lazor)Instantiate(projectile, transform.position, Quaternion.identity);
				proj.GetComponent<Rigidbody>().AddForce(500f, 0, 0);
			}
			if(Input.GetButtonDown("X") && !x_disabled){
				last_used = 2;
				can_attack = false;
				Debug.Log("X attack used");
			}
			if(Input.GetButtonDown("Y") && !y_disabled){
				last_used = 3;
				can_attack = false;
				Debug.Log("Y attack used");
			}
		}
	}

	void OnCollisionEnter() {
		can_jump = true;
	}
}
