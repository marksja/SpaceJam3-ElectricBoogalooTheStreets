using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round : MonoBehaviour {

    int POSLIMIT;
    int RESLIMIT;
    int FIGHTLIMIT;

    bool roundOver;

    public string P2_res;
    public string P1_res;
    public float speedMult;
    public float timeRemaining;
    public int currentRound;
    public int currentPhase;

//    Transform gameTransform;
    public GameObject Player1;
    public GameObject Player2;
    public PlayerScript P1S;
    public PlayerScript P2S;

    // Use this for initialization
    void Start (int pos_in, int res_in, int fight_in) {
        POSLIMIT = pos_in;
        RESLIMIT = res_in;
        FIGHTLIMIT = fight_in;

        currentPhase = 0;
        timeRemaining = POSLIMIT;
        //        gameTransform = GetComponentInParent(typeof(Transform)) as Transform;
        P1S = Player1.GetComponent<PlayerScript>() as PlayerScript;
        P2S = Player2.GetComponent<PlayerScript>() as PlayerScript;

    }

    // Update is called once per frame
    void FixedUpdate () {
        if (timeRemaining <= 0)
        {
            if (currentPhase == 0)
            {
                currentPhase = 1;
                timeRemaining = RESLIMIT;
            }
            else if (currentPhase == 1)
            {
                currentPhase = 2;
                timeRemaining = FIGHTLIMIT;
            }
            else if (currentPhase == 2)
            {
                currentPhase = 3;       //game over
            }
        }
        timeRemaining -= Time.deltaTime;
        switch (currentPhase)
        {
            case 0:
                {
                    break;
                }
            case 1:
                {
                    StartCoroutine(RestrictButtons());

                    break;
                }
            case 2:
                {
                    break;
                }
        }
    }

    private IEnumerator RestrictButtons()
    {
        //button restriction screen
        //show button screen
            //code
        //Await button presses
        while (P2_res.Length < 2 && P1_res.Length < 2)
        {
            //display struck buttons
                //do code to show what's struck

            //get button inputs
            if (Input.GetButton(P1S.A) && P2_res.Length < 2 && !P2_res.Contains("A"))
                P2_res += P1S.A;
            if (Input.GetButton(P1S.A) && P2_res.Length < 2 && !P2_res.Contains("B"))
                P2_res += P1S.B;
            if (Input.GetButton(P1S.A) && P2_res.Length < 2 && !P2_res.Contains("X"))
                P2_res += P1S.X;
            if (Input.GetButton(P1S.A) && P2_res.Length < 2 && !P2_res.Contains("Y"))
                P2_res += P1S.Y;
            
            if (Input.GetButton(P2S.A) && P1_res.Length < 2 && !P1_res.Contains("A"))
                P1_res += P1S.A;
            if (Input.GetButton(P2S.A) && P1_res.Length < 2 && !P1_res.Contains("B"))
                P1_res += P1S.A;
            if (Input.GetButton(P2S.A) && P1_res.Length < 2 && !P1_res.Contains("X"))
                P1_res += P1S.A;
            if (Input.GetButton(P2S.A) && P1_res.Length < 2 && !P1_res.Contains("Y"))
                P1_res += P1S.A;
        }
        if (P1_res.Contains("A"))
            P1S.a_disabled = true;
        if (P1_res.Contains("B"))
            P1S.b_disabled = true;
        if (P1_res.Contains("X"))
            P1S.x_disabled = true;
        if (P1_res.Contains("Y"))
            P1S.y_disabled = true;

        if (P2_res.Contains("A"))
            P2S.a_disabled = true;
        if (P2_res.Contains("B"))
            P2S.b_disabled = true;
        if (P2_res.Contains("X"))
            P2S.x_disabled = true;
        if (P2_res.Contains("Y"))
            P2S.y_disabled = true;

        return null;
    }
}