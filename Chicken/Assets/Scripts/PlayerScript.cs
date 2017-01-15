using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
    //stats
    public int HP = 3;
    public int SCORE = 0;   //0-3
    public string PLAYERSTATE = "neutral";

    public float maxSpeed;
    public float acceleration;
    public float airSpeed;
    public float airAccel;
    public float speedMult = 1f;
    //Hype
    public float Hype;

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
    public bool damaged;
    public bool crouching;
	int direction;
	Rigidbody rb;
	GameObject hb;
	public Lazor projectile;
	public Quick hitbox_quick;
	public Swipe hitbox_arc;
	public Dash hitbox_dash;
    public float x_tol;
    public UnityEngine.UI.Image healthBar;
    public Sprite[] barImages;
    public float spawnPosX;
    public float spawnPosY;
    public AudioSource sliceWoosh;
    public AudioSource laserFire;
    public AudioSource idle;
    public AudioSource zap;

    bool falling = false;
    bool grounded = false;
    bool noJumping = false;
    bool inDropThrough = false;
    bool dashing = false;
    BoxCollider feet;
    int jumps = 2;
    
    public string A;
    public string B;
    public string X;
    public string Y;
    public string LeftX;
    public string LeftY;
    public float drag;

    public Animator anim;
    public GameObject child;
	// Use this for initialization
	void Start()
    {
    	moveable = true;
    	damageable = false;
        damaged = false;
        crouching = false;
        rb = GetComponent<Rigidbody>();
		can_attack = true;
        feet = transform.GetChild(0).GetComponent("BoxCollider") as BoxCollider;
        Physics.IgnoreLayerCollision(0, 8, true);
        direction = -1;
        x_tol = 0.6f;
        anim = child.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        //update health bar
        healthBar.sprite = barImages[HP];

        //anim = GetComponentInChildren<Animator>();
        //Read Input
        float x = Input.GetAxis(LeftX) * Time.deltaTime * 5f;

        float y = -Input.GetAxis(LeftY);

        //Set tolerance on x
        if(Mathf.Abs(x) > x_tol || crouching == true)
        {
           // x = 0;
        }
        print("X:"+x);

        // Set running if on the ground and unhurt
        if(x!=0)
        {
            // Flip character based on velocity
            child.transform.localScale = new Vector3(-Mathf.Sign(x)* 2.144844f, 2.144844f, 0);
            if(damaged == false && grounded == true){
                //print("Running");
                anim.SetBool("Running", true);
            }
            
        }
        else
        {
            anim.SetBool("Running", false);
        }

        if (y > 0.3f && damaged == false)
        {
            anim.SetBool("Jumping", true);
        }else if(y < -0.3f && damaged == false && falling == false){
            anim.SetBool("Crouching", true);
            crouching = true;
        }
        else
        {
            anim.SetBool("Jumping", false);
            anim.SetBool("Crouching", false);
            crouching = false;
        }

        if (grounded)
            // Animate jumps
            //anim.SetBool("Jumping", true);
            jumps = 2;
        //Check if attack charge is done
        if (currently_charging){
            if (falling && rb.velocity.y == 0 && !dashing)
            {
                grounded = true;
                falling = false;
                //anim.SetBool("Jumping", false);
            }
            if (rb.velocity.y > 0)
                feet.enabled = false;
            else
                feet.enabled = true;

            // Start charge and the attack animations
            charge_time += Time.deltaTime;
			if(charge_time > charging[last_used]){
				currently_charging = false;

				can_attack = true;
				switch (last_used){
					case 0:
                        if (grounded)
                            rb.velocity = new Vector3(0, 0, 0);
                        anim.SetBool("Miding", true);
                        anim.SetBool("Shorting", true);
                        A_Attack();
                        
                        break;
					case 1:
                        if (grounded)
                            rb.velocity = new Vector3(0, 0, 0);
                        anim.SetBool("Charging", true);
                        moveable = false;
						B_Attack();


                        break;
					case 2:
                        if (grounded)
                            rb.velocity = new Vector3(0, 0, 0);
                        anim.SetBool("Dashing", true);
                         X_Attack();


                         break;
					case 3:
                        if (grounded)
                            rb.velocity = new Vector3(0, 0, 0);
                        anim.SetBool("Miding", true);
                         Y_Attack();
                        

                         break;
				}
                //anim.SetBool("Charging", false);
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
				case 0:// Short
					//do a thing for quick attack
					break;
				case 1:
					//Nothing
					break;
				case 2:
					moveable = false;
					rb.velocity = new Vector3(20f, 0, 0) * direction;;
					if(attacking == false) {
						rb.velocity = new Vector3(0,0,0);
						moveable = true;
					}
					break;
				case 3:
					hb.transform.Rotate(-Vector3.forward * 90 * Time.deltaTime / length[3] * direction);
					break;
			}	
            // Stop the attack animations
            if(attacking == false)
            {
                print("ERRRRRRRRRR");
                anim.SetBool("Shorting", false);
                anim.SetBool("Miding", false);
                
                anim.SetBool("Dashing", false);
                
            }	
		}

		if(moveable){
			//Update can_attack
			cool_time += Time.deltaTime;
			if(cool_time > cooldown[last_used]) can_attack = true;

            //Move
            float newVX = 1f;
            if (grounded)
            {
                float dv = 5 * x * acceleration;
                newVX = rb.velocity.x + dv;
                if (Mathf.Abs(newVX) > maxSpeed)
                    newVX = maxSpeed * (newVX/Mathf.Abs(newVX));
            }
            else
            {
                float dv = 5 * x * airAccel;
                newVX = rb.velocity.x + dv;
                if (Mathf.Abs(newVX) > airSpeed)
                    newVX = airSpeed * (newVX / Mathf.Abs(newVX));
            }
            rb.velocity = new Vector3(newVX * speedMult / drag, rb.velocity.y, 0);

            if (x > 0){
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
				if(Input.GetButtonDown(A)){
					if(a_disabled){
						StartCoroutine(Delayed_Damage(.5f));
					}
					last_used = 0;
					currently_charging = true;
				}
				if(Input.GetButtonDown(B)){
					if(b_disabled){
						StartCoroutine(Delayed_Damage(.5f));
					}
					last_used = 1;
					currently_charging = true;
				}
				if(Input.GetButtonDown(X)){
					if(x_disabled){
						StartCoroutine(Delayed_Damage(.5f));
					}
					last_used = 2;
					currently_charging = true;
				}
				if(Input.GetButtonDown(Y)){
					if(y_disabled){
						StartCoroutine(Delayed_Damage(.5f));
					}
					last_used = 3;
					currently_charging = true;
				}
			}
		}
        if (grounded && !attacking)
            if (idle.isPlaying == false)
            {
                if (Random.value % 10f == 0)
                    idle.Play();
            }
    }

	void OnCollisionEnter() {
//		can_jump = true;
	}

	void A_Attack(){
        if (a_disabled)
            zap.Play();
		can_attack = false;
		cool_time = 0.0f;
        attacking = true;
		Debug.Log("A attack used");
		Quick hitbox = (Quick)Instantiate(hitbox_quick, transform.position, Quaternion.identity);
		hitbox.transform.position += new Vector3(1f, 0, 0) * direction;
		hitbox.GetComponent<Quick>().player_owner = name[6];
		hitbox.GetComponent<Quick>().direction = direction;
		hitbox.transform.parent = transform;
		Destroy(hitbox.gameObject, length[0]);
        StartCoroutine(ACoroutine());
        print("REEEEEEEEE");
	}
	void B_Attack()
    {
        if (b_disabled)
            zap.Play();
        moveable = true;
		can_attack = false;
        attacking = true;
		cool_time = 0.0f;
		Debug.Log("B attack used");
		Lazor proj = (Lazor)Instantiate(projectile, transform.position + Vector3.right*1.5f*direction, Quaternion.identity);
		proj.GetComponent<Rigidbody>().AddForce(500f * direction, 0, 0);
		proj.GetComponent<Lazor>().player_owner = name[6];
		Destroy(proj.gameObject, 5);
        StartCoroutine(BCoroutine());
	}
	void X_Attack()
    {
        if (x_disabled)
            zap.Play();
        can_attack = false;
		cool_time = 0.0f;
		Debug.Log("X attack used");
		attacking = true;
		Dash hitbox = (Dash)Instantiate(hitbox_dash, transform.position, Quaternion.identity);
		hitbox.transform.parent = transform;
		hitbox.GetComponent<Dash>().player_owner = name[6];
		Destroy(hitbox.gameObject, length[2]);
		attack_counter = 0.0f;
		StartCoroutine(XCoroutine());
	}
	void Y_Attack()
    {
        if (y_disabled)
            zap.Play();
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
        StartCoroutine(YCoroutine());
	}

    private IEnumerator Fall()
    {
        grounded = false;
        jumps -= 1;
        inDropThrough = true;
        rb.AddForce(0, -30f, 0);
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
    
    private IEnumerator ACoroutine()
    {
        
        moveable = false;
        yield return new WaitForSeconds(length[0]);
        moveable = true;
    }
    private IEnumerator BCoroutine()
    {
        laserFire.Play();
        moveable = false;
        yield return new WaitForSeconds(length[1] + charging[1]);
        moveable = true;
    }
    private IEnumerator YCoroutine()
        {
        sliceWoosh.Play();
        moveable = false;
        yield return new WaitForSeconds(length[3]);
        moveable = true;
        }


    private IEnumerator XCoroutine()
    {
        moveable = false;
        dashing = true;
        yield return new WaitForSeconds(length[2]);
        dashing = false;
        moveable = true;
    }

    private IEnumerator Delayed_Damage(float time){
    	yield return new WaitForSeconds(time);
    	//take damage animation
    	HP--;
    }
}
 