using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	public bool can_jump;
	public bool a_disabled;
	public bool b_disabled;
	public bool x_disabled;
	public bool y_disabled;
	public Rigidbody rb;
	// Use this for initialization
	void Start () {
		x_disabled = true;
		a_disabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		//Movement
		var x = Input.GetAxis("LeftX") * Time.deltaTime * 10f;
		var y = -Input.GetAxis("LeftY");
		transform.Translate(x, 0, 0);

		if(y > 0 && can_jump){
			rb.AddForce(0, 450f, 0);
			can_jump = false;
		}

		//Attacks
		if(Input.GetButtonDown("A") && !a_disabled){
			Debug.Log("A attack used");
		}
		if(Input.GetButtonDown("B") && !b_disabled){
			Debug.Log("B attack used");
		}
		if(Input.GetButtonDown("X") && !x_disabled){
			Debug.Log("X attack used");
		}
		if(Input.GetButtonDown("Y") && !y_disabled){
			Debug.Log("Y attack used");
		}
	}

	void OnCollisionEnter() {
		can_jump = true;
	}
}
