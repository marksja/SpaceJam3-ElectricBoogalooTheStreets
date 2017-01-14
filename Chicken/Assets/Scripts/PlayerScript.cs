using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	int last_used;
	public bool can_attack;
	public bool can_jump;
	public bool a_disabled;
	float a_time;
//	public static a_charge;
//	public static a_cooldown;	
	public bool b_disabled;
	float b_time;
//	public static b_charge;
//	public static b_cooldown;
	public bool x_disabled;
	float x_time;
//	public static x_charge;
//	public static x_cooldown;
	public bool y_disabled;
	float y_time;
//	public static y_charge;
//	public static y_cooldown;
	Rigidbody rb;
	public Lazor projectile;

    bool falling = false;
    bool grounded = false;
    bool noJumping = false;
    BoxCollider feet;
    int jumps = 2;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		x_disabled = true;
		a_disabled = true;
		can_attack = true;
        feet = gameObject.GetComponentInChildren(typeof(BoxCollider)) as BoxCollider;
        Physics.IgnoreLayerCollision(0, 8, true);
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

	void OnCollisionEnter(Collider other) {
     /*  if (rb.velocity.y <= 0 && (other.name.Contains("Plat") || other.name.Contains("Floor")))
        {
            jumps = 2;
        }
        else if (other.name.Contains("Wall") || other.name.Contains("Ceil"))
            jumps += 1;*/
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
