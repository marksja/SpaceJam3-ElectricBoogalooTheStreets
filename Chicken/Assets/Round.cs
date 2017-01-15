using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round : MonoBehaviour {

    public int POSLIMIT = 10;
    public int RESLIMIT = 10;
    public int FIGHTLIMIT = 100;

    bool roundOver;

    public int round_num;
    public int P1_wins;
    public int P2_wins;
    public string P2_res;
    public string P1_res;
    public float speedMult;
    public float timeRemaining;
    public int currentRound;
    public int currentPhase;

    //    Transform gameTransform;
    public GameObject gameO;
    public GameScript game;
    public GameObject Player1;
    public GameObject Player2;
    public PlayerScript P1S;
    public PlayerScript P2S;
    public Canvas UI;
    public UnityEngine.UI.Text timer;
    public UnityEngine.UI.Text round_label;

    // Use this for initialization
    void Start () {
        P1_wins = 0;
        P2_wins = 0;
        round_num = 1;
        //timer = UI.GetComponentInChildren<UnityEngine.UI.Text>();
        POSLIMIT = 10;
        RESLIMIT = 10;
        FIGHTLIMIT = 100;
        currentRound = 1;
        currentPhase = 0;
        timeRemaining = POSLIMIT;
        Update_UI();
        //gameTransform = GetComponentInParent(typeof(Transform)) as Transform;
        //P1S = Player1.GetComponent<PlayerScript>() as PlayerScript;
        //P2S = Player2.GetComponent<PlayerScript>() as PlayerScript;

    }

    // Update is called once per frame
    void Update () {
        timer.text = timeRemaining.ToString("0.00");
        if (timeRemaining <= 0)
        {
            if (currentPhase == 0)
            {
                currentPhase = 1;
                P1S.moveable = false;
                P2S.moveable = false;
                timeRemaining = RESLIMIT;
            }
            else if (currentPhase == 1)
            {
                currentPhase = 2;
                timeRemaining = FIGHTLIMIT;
                P1S.moveable = true;
                P2S.moveable = true;
                P1S.damageable = true;
                P2S.damageable = true;
                if(P1_res.Length == 2){
                    if (P1_res.Contains("A"))
                        P1S.a_disabled = true;
                    if (P1_res.Contains("B"))
                        P1S.b_disabled = true;
                    if (P1_res.Contains("X"))
                        P1S.x_disabled = true;
                    if (P1_res.Contains("Y"))
                        P1S.y_disabled = true;
                }
                else{
                    //If player 2 has not selected 2 buttons
                }
                if(P2_res.Length == 2){
                    if (P2_res.Contains("A"))
                        P2S.a_disabled = true;
                    if (P2_res.Contains("B"))
                        P2S.b_disabled = true;
                    if (P2_res.Contains("X"))
                        P2S.x_disabled = true;
                    if (P2_res.Contains("Y"))
                        P2S.y_disabled = true;
                }
                else{
                    //If player 1 has not selected 2 buttons
                }
            }
            else if (currentPhase == 2)
            {
                currentPhase = 3;       //game over
                End_Round();
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
                    if (Input.GetButton(P1S.A) && P2_res.Length < 2 && !P2_res.Contains("A"))
                        P2_res += "A";
                    if (Input.GetButton(P1S.B) && P2_res.Length < 2 && !P2_res.Contains("B"))
                        P2_res += "B";
                    if (Input.GetButton(P1S.X) && P2_res.Length < 2 && !P2_res.Contains("X"))
                        P2_res += "X";
                    if (Input.GetButton(P1S.Y) && P2_res.Length < 2 && !P2_res.Contains("Y"))
                        P2_res += "Y";
                    
                    if (Input.GetButton(P2S.A) && P1_res.Length < 2 && !P1_res.Contains("A"))
                        P1_res += "A";
                    if (Input.GetButton(P2S.B) && P1_res.Length < 2 && !P1_res.Contains("B"))
                        P1_res += "B";
                    if (Input.GetButton(P2S.X) && P1_res.Length < 2 && !P1_res.Contains("X"))
                        P1_res += "X";
                    if (Input.GetButton(P2S.Y) && P1_res.Length < 2 && !P1_res.Contains("Y"))
                        P1_res += "Y";
                    break;
                }
            case 2:
                {
                   
                    if(P1S.HP <= 0){
                        //Player 1 loses
                        Debug.Log("Player 1 Loses");
                        P1S.moveable = false;
                        P2S.moveable = false;
                        End_Round(2);
                        //Display victory screen for player 2
                        //Play of the game, etc
                    }
                    if(P2S.HP <= 0){
                        //Player 2 loses
                        Debug.Log("Player 2 Loses");
                        P1S.moveable = false;
                        P2S.moveable = false;
                        End_Round(1);
                        //Display victory screen for player 1
                        //Play of the game, etc
                    }
                    break;
                }
        }
        if(Input.GetKeyDown(KeyCode.R)){
            round_num = 0;
            P1_wins = 0;
            P2_wins = 0;
            timeRemaining = POSLIMIT;
            currentPhase = 0;
            Update_UI();
        }

    }

    void Update_UI(){
        //Change round number text
        round_label.text = "Round " + currentRound.ToString();
        //Reset health bar
        //Update win numbers
    }

    void End_Round(int winner = 0){
        if(winner == 0){
            //No one won the round. What do?
        }
        if(winner == 1){
            P1_wins++;
        }
        if(winner == 2){
            P2_wins++;
        }
        currentRound++;
        currentPhase = 0;
        timeRemaining = POSLIMIT;
        Update_UI();
        P1S.moveable = true;
        P2S.moveable = true;
        P1S.HP = 3;
        P2S.HP = 3;
    }

    private IEnumerator RestrictButtons()
    {
        //button restriction screen
        //show button screen
            //code
        //Await button presses
        while (P2_res.Length < 2 && P1_res.Length < 2)
        {
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