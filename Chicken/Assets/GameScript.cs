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

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void FixedUpdate () {
        

	}

    float Find_Delta_Hype(){
        float Comeback_Hype;
        float Close_Game_Hype;
        float Round_Hype;
         
    }

    
}
