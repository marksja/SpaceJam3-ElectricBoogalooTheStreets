using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour {
    public int ROUNDS;
    public int CURRENTROUND;
    public int POSLIMIT;
    public int RESLIMIT;
    public int FIGHTLIMIT;
    public float timeRemaining;

    public float HYPELEVEL;
    public int HYPETIER;    //0 - 7
    public int HYPEPERCENT;
    const float baseSpeedMult = .8f;
    float speedMult;

    public int currentPhase;
	// Use this for initialization
	void Start () {
        currentPhase = 0;
        timeRemaining = POSLIMIT;
	}
	
	// Update is called once per frame
	void Update () {
        speedMult = baseSpeedMult + .1f * HYPETIER;
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
                currentPhase = 3;       //game over
        }
        timeRemaining -= Time.deltaTime;
        switch(currentPhase)
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

    
}
