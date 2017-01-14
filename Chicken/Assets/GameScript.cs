using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour {
    public int POSLIMIT;
    public int RESLIMIT;
    public int FIGHTLIMIT;

    public float HYPELEVEL;
    public int HYPETIER;    //0 - 7
    public int HYPEPERCENT;
    const float baseSpeedMult = .8f;
    public Round[] rounds;

    public int currentRound = 0;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        

	}

    
}
