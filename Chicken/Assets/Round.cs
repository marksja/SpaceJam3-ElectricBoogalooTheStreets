using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round : MonoBehaviour {

    int POSLIMIT;
    int RESLIMIT;
    int FIGHTLIMIT;

    public float speedMult;
    public float timeRemaining;
    public int currentRound;
    public int currentPhase;
    // Use this for initialization
    void Start (int pos_in, int res_in, int fight_in) {
        POSLIMIT = pos_in;
        RESLIMIT = res_in;
        FIGHTLIMIT = fight_in;

        currentPhase = 0;
        timeRemaining = POSLIMIT;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (timeRemaining <= 0)
        {
            if (currentPhase == 0)
            {
                currentPhase = 1;
                timeRemaining = RESLIMIT;
                RestrictButtons();
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
                    //show buttons and ask for restrictions
                    break;
                }
            case 2:
                {
                    break;
                }
        }
    }

    void RestrictButtons()
    {
        //button restriction screen
    }
}
