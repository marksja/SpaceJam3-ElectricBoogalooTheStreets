using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
    //stats
    public int HP = 3;
    public int SCORE = 0;   //0-3
    public string PLAYERSTATE = "neutral";

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
	public bool moveable;
	public bool damageable;
	int direction;
	Rigidbody rb;
	GameObject hb;
	public Lazor projectile;
	public Quick hitbox_quick;
	public Swipe hitbox_arc;
	public Dash hitbox_dash;

    bool falling = false;
    bool grounded = false;
    bool noJumping = false;
    bool inDropThrough = false;
    BoxCollider feet;
    int jumps = 2;
    
    public string A;
    public string B;
    public string X;
    public string Y;
    public string LeftX;
    public string LeftY;

	// Use this for initialization
	void Start ()
    {
    	moveable = true;
    	damageable = false;
        rb = GetComponent<Rigidbody>();
		can_attack = true;
        feet = transform.GetChild(0).GetComponent("BoxCollider") as BoxCollider;
        Physics.IgnoreLayerCollision(0, 8, true);
        direction = 1;
	}
	
	// Update is called once per frame
	void Update () {
        //Read Input
        var x = Input.GetAxis(LeftX) * Time.deltaTime * 10f;
        var y = -Input.GetAxis(LeftY);
        if (grounded)
            jumps = 2;
        //Check if attack charge is done
        if (currently_charging){
            if (falling && rb.velocity.y == 0)
            {
                grounded = true;
                falling = false;
            }
            if (rb.velocity.y > 0)
                feet.enabled = false;
            else
                feet.enabled = true;


            charge_time += Time.deltaTime;
			if(charge_time > charging[last_used]){
				currently_charging = false;
				can_attack = true;
				switch (last_used){
					case 0: 
						A_Attack();
						break;
					case 1:
						moveable = false;
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
            if (falling && rb.velocity.y == 0)
            {
                grounded = true;
                falling = false;
            }
            if (rb.velocity.y > 0)
                feet.enabled = false;
            else
                feet.enabled = true;


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
					moveable = false;
					rb.velocity = new Vector3(50f, 0, 0) * direction;;
					if(attacking == false) {
						rb.velocity = new Vector3(0,0,0);
						moveable = true;
					}
					break;
				case 3:
					hb.transform.Rotate(-Vector3.forward * 90 * Time.deltaTime / length[3] * direction);
					break;
			}	
		}

		if(moveable){
			//Update can_attack
			cool_time += Time.deltaTime;
			if(cool_time > cooldown[last_used]) can_attack = true;
			
            //Move
			transform.Translate(x, 0, 0);

			if(x > 0){
				direction = 1;
			}
			if(x < 0){
				direction = -1;
			}

			if(y > .5 && jumps > 0 && !noJumping)
	        {
	            StartCoroutine("Jump");
	            noJumping = true;
			}

	        if (y < -.9 && !inDropThrough)
	        {
	            StartCoroutine("Fall");
	        }
            if (rb.velocity.y < 0)
            {
                falling = true;
                grounded = false;
            }
            else if (rb.velocity.y > 0)
            {
                falling = false;
            }
            if (noJumping && y <= 0)
	            noJumping = false;
	        if (falling && rb.velocity.y == 0)
	        {
	            grounded = true;
	            falling = false;
			}
            if (rb.velocity.y > 0)
                feet.enabled = false;
            else     
                feet.enabled = true;
            if (!grounded && jumps == 2)
                jumps = 1;

			//Attacks
			if(can_attack){
				if(Input.GetButtonDown(A) && !a_disabled){
					last_used = 0;
					currently_charging = true;
				}
				if(Input.GetButtonDown(B) && !b_disabled){
					last_used = 1;
					currently_charging = true;
				}
				if(Input.GetButtonDown(X) && !x_disabled){
					last_used = 2;
					currently_charging = true;
				}
				if(Input.GetButtonDown(Y) && !y_disabled){
					last_used = 3;
					currently_charging = true;
				}
			}
		}
	}

	void OnCollisionEnter() {
//		can_jump = true;
	}

	void A_Attack(){
		can_attack = false;
		cool_time = 0.0f;
		Debug.Log("A attack used");
		Quick hitbox = (Quick)Instantiate(hitbox_quick, transform.position, Quaternion.identity);
		hitbox.transform.position += new Vector3(1f, 0, 0) * direction;
		hitbox.GetComponent<Quick>().player_owner = name[6];
		hitbox.GetComponent<Quick>().direction = direction;
		hitbox.transform.parent = transform;
		Destroy(hitbox.gameObject, length[0]);
	}
	void B_Attack(){
		moveable = true;
		can_attack = false;
		cool_time = 0.0f;
		Debug.Log("B attack used");
		Lazor proj = (Lazor)Instantiate(projectile, transform.position + Vector3.right*1.5f*direction, Quaternion.identity);
		proj.GetComponent<Rigidbody>().AddForce(500f * direction, 0, 0);
		proj.GetComponent<Lazor>().player_owner = name[6];
		Destroy(proj.gameObject, 5);
	}
	void X_Attack(){
		can_attack = false;
		cool_time = 0.0f;
		Debug.Log("X attack used");
		attacking = true;
		Dash hitbox = (Dash)Instantiate(hitbox_dash, transform.position, Quaternion.identity);
		hitbox.transform.parent = transform;
		hitbox.GetComponent<Dash>().player_owner = name[6];
		Destroy(hitbox.gameObject, length[2]);
		attack_counter = 0.0f;
	}
	void Y_Attack(){
		can_attack = false;
		cool_time = 0.0f;
		Debug.Log("Y attack used");
		Swipe hitbox = (Swipe)Instantiate(hitbox_arc, transform.position, Quaternion.Euler(0,0, 45 + (-(direction - 1) * 45)));
		hitbox.transform.parent = transform;
		hitbox.GetComponent<Swipe>().player_owner = name[6];
		hitbox.GetComponent<Swipe>().direction = direction;
		attacking = true;
		hb = hitbox.gameObject;
		Destroy(hitbox.gameObject, length[3] + .05f);
		attack_counter = 0.0f;
	}

    private IEnumerator Fall()
    {
        grounded = false;
        jumps -= 1;
        inDropThrough = true;
        rb.AddForce(0, -10f, 0);
        feet.gameObject.layer = 0;
        Debug.Log("Falling through platform");
        falling = true;
        yield return new WaitForSeconds(0.5f);
        feet.gameObject.layer = 8;
        Debug.Log("Fall complete");
        inDropThrough = false;
	}

    private IEnumerator Jump()
    {
        grounded = false;
        jumps -= 1;
        rb.velocity = new Vector3(rb.velocity.x, 8f);
        Debug.Log("Jumping");
        yield return null;
    }
    /*
    public string RestrictButtons()
    {
        string btns = "";
        while (btns.Length < 2)
        {
            if (Input.GetButtonDown(A))
                btns += 'a';
            else if (Input.GetButtonDown(B))
                btns += 'b';
            else if (Input.GetButtonDown(X))
                btns += 'x';
            else if (Input.GetButtonDown(Y))
                btns += 'y';
        }
        return btns;
    }*/
}
 
/*=======
﻿using UnityEngine;
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
	public Quick hitbox_quick;
	public Swipe hitbox_arc;
	public Dash hitbox_dash;


    bool falling = false;
    bool grounded = false;
    bool noJumping = false;
    BoxCollider feet;
    int jumps = 2;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		can_attack = true;
        feet = gameObject.GetComponentInChildren(typeof(BoxCollider)) as BoxCollider;
        Physics.IgnoreLayerCollision(0, 8, true);
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

		if(y > 0 && jumps > 0 && !noJumping)
        {
            StartCoroutine("Jump");
            noJumping = true;
		}
        if (y < 0)
        {
            StartCoroutine("Fall");
        }
        if (noJumping && y <= 0)
            noJumping = false;
        if (falling && rb.velocity.y == 0)
        {
            grounded = true;
            falling = false;
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
		Quick hitbox = (Quick)Instantiate(hitbox_quick, transform.position, Quaternion.identity);
		hitbox.transform.position += new Vector3(1f, 0, 0);
		hitbox.transform.parent = transform;
		Destroy(hitbox.gameObject, length[0]);
			}
	void B_Attack(){
				can_attack = false;
		cool_time = 0.0f;
				Debug.Log("B attack used");
				Lazor proj = (Lazor)Instantiate(projectile, transform.position, Quaternion.identity);
				proj.GetComponent<Rigidbody>().AddForce(500f, 0, 0);
		Destroy(proj.gameObject, 5);
			}
	void X_Attack(){
				can_attack = false;
		cool_time = 0.0f;
				Debug.Log("X attack used");
		attacking = true;
		Dash hitbox = (Dash)Instantiate(hitbox_dash, transform.position, Quaternion.identity);
		hitbox.transform.parent = transform;
		Destroy(hitbox.gameObject, length[2]);
		attack_counter = 0.0f;
			}
	void Y_Attack(){
				can_attack = false;
		cool_time = 0.0f;
				Debug.Log("Y attack used");
		attacking = true;
		Swipe hitbox = (Swipe)Instantiate(hitbox_arc, transform.position, Quaternion.Euler(0,0,90));
		hitbox.transform.position += new Vector3(0, 1.5f, 0);
		hitbox.transform.parent = transform;
		Slash(hitbox.gameObject);
		Destroy(hitbox.gameObject, length[3]);
		attack_counter = 0.0f;
	}

	IEnumerator Slash(GameObject hitbox){
		float time = 0.0f;
		while(time < length[3]){
	        Quaternion target = Quaternion.Euler(0, 0, -90);
	        hitbox.transform.rotation = Quaternion.Slerp(hitbox.transform.rotation, target, Time.deltaTime * .03f);			
	        time += Time.deltaTime;
			}
		return null;
		}

    private IEnumerator Fall()
    {
        rb.AddForce(0, -10f, 0);
        feet.isTrigger = true;
        Debug.Log("Falling through platform");
        falling = true;
        yield return new WaitForSeconds(0.5f);
        feet.isTrigger = false;
        Debug.Log("Fall complete");
	}

    private IEnumerator Jump()
    {
        jumps -= 1;
        rb.velocity = new Vector3(rb.velocity.x, 8f);
        Debug.Log("Jumping");
        falling = false;
        while (!grounded)
            yield return null;
        jumps = Mathf.Max(2, jumps + 1);
        Debug.Log("Jump Complete");
    }
}

    */