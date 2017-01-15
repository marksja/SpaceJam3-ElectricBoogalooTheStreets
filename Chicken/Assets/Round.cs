using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round : MonoBehaviour {

    public int POSLIMIT;
    public int RESLIMIT;
    public int FIGHTLIMIT;

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
    public UnityEngine.UI.Text roundLbl;
    public UnityEngine.UI.Text juicyPhaseName;
    public UnityEngine.UI.Text descriptivePhase;
    public UnityEngine.UI.Image P1Score;
    public UnityEngine.UI.Image P2Score;
    public UnityEngine.UI.Image P1Buttons;
    public UnityEngine.UI.Image P2Buttons;
    public UnityEngine.UI.Image P1ButtonSel;
    public UnityEngine.UI.Image P2ButtonSel;
    public UnityEngine.UI.Image P1Pan;
    public UnityEngine.UI.Image P2Pan;
    public UnityEngine.UI.Image P1ResBar;
    public UnityEngine.UI.Image P2ResBar;
    public Sprite[] scoreBar;
    public Sprite[] resBar;
    bool countdown = false;

    //Hype Algoritm
    float P1_Previous_Hype;
    float P2_Previous_Hype;
    float total_hype;

    // Use this for initialization
    void Start () {
        P1_wins = 0;
        P2_wins = 0;
        round_num = 0;
        currentRound = 1;
        currentPhase = 0;
        timeRemaining = POSLIMIT;
        Update_UI();
        //gameTransform = GetComponeP2ResBar.enabled = false;ntInParent(typeof(Transform)) as Transform;
        //P1S = Player1.GetComponent<PlayerScript>() as PlayerScript;
        //P2S = Player2.GetComponent<PlayerScript>() as PlayerScript;

        juicyPhaseName.text = "POSITION!";
        descriptivePhase.text = "Move to your starting position of choice.";

    }

    // Update is called once per frame
    void Update () {
        if (currentPhase == 0)
        {
            P1Pan.enabled = false;
            P2Pan.enabled = false;
            P1ButtonSel.enabled = false;
            P2ButtonSel.enabled = false;
            P1ResBar.enabled = false;
            P2ResBar.enabled = false;
        }
        if (currentPhase == 1)
        {
            P1S.moveable = false;
            P1S.moveable = false;
        }
        if (!countdown)
            timer.text = timeRemaining.ToString("0.00");
        if (round_num != 0)
            roundLbl.text = "Round " + currentRound.ToString();
        else
            roundLbl.text = "Round 1";
        if (timeRemaining <= 0)
        {
            if (currentPhase == 0)
            {
                juicyPhaseName.text = "RESTRICT!";
                descriptivePhase.text = "choose 2 attacks that your foe may not use.";
                currentPhase = 1;
                P1S.moveable = false;
                P2S.moveable = false;
                timeRemaining = RESLIMIT;
            }
            else if (currentPhase == 1)
            {
                StartCoroutine(startFight());

                currentPhase = 2;
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
                    StartCoroutine(RestrictButtons());
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
                    P1ResBar.sprite = resBar[P2_res.Length];
                    P2ResBar.sprite = resBar[P1_res.Length];
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
        total_hype += Get_Hype_Differential();
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
            P1S.SCORE++;
            P1Score.sprite = scoreBar[P1S.SCORE];
        }
        if(winner == 2){
            P2_wins++;
            P2S.SCORE++;
            P2Score.sprite = scoreBar[P2S.SCORE];
        }
        juicyPhaseName.fontSize -= 40;
        currentRound++;
        currentPhase = 0;
        timeRemaining = POSLIMIT;
        Update_UI();
        P1S.moveable = true;
        P2S.moveable = true;
        P1S.HP = 3;
        P2S.HP = 3;
    }

    float Get_Hype_Differential(){
        if(P1S.Hype == P1_Previous_Hype) return 0.0f;
        if(P2S.Hype == P2_Previous_Hype) return 0.0f;
        float time_hype = 500f*(20f/timeRemaining);
        float close_game_hype = 500f*(round_num)/(Mathf.Abs(P1_wins - P2_wins) + 1);
        float p1_underdog_hype = (P1S.HP - P1S.HP) * (P2_wins - P1_wins) * 500f;
        float p2_underdog_hype = (P2S.HP - P2S.HP) * (P1_wins - P2_wins) * 500f;
        float P1_hype_diff = P2S.Hype - P1_Previous_Hype;
        float P2_hype_diff = P1S.Hype - P2_Previous_Hype;

        P1_hype_diff *= time_hype + close_game_hype + p1_underdog_hype;
        P2_hype_diff *= time_hype + close_game_hype + p1_underdog_hype;

        if(P1_hype_diff < 0) P1_hype_diff = 0;
        if(P2_hype_diff < 0) P2_hype_diff = 0;

        return P1_hype_diff + P2_hype_diff;
    }

    private IEnumerator RestrictButtons()
    {
        //button restriction screen
        //show button screen
        P1Pan.enabled = true;
        P2Pan.enabled = true;
        P1ButtonSel.enabled = true;
        P2ButtonSel.enabled = true;
        P1ResBar.enabled = true;
        P2ResBar.enabled = true;
        yield return new WaitForSeconds(RESLIMIT);
        P1Pan.enabled = false;
        P2Pan.enabled = false;
        P1ButtonSel.enabled = false;
        P2ButtonSel.enabled = false;
        P1ResBar.enabled = false;
        P2ResBar.enabled = false;

    }
    
    private IEnumerator startFight()
    {
        countdown = true;
        descriptivePhase.text = "";
        timer.text = FIGHTLIMIT.ToString("0.00");
        juicyPhaseName.fontSize += 10;
        juicyPhaseName.text = "3";
        yield return new WaitForSeconds(1f);
        juicyPhaseName.fontSize += 10;
        juicyPhaseName.text = "2";
        yield return new WaitForSeconds(1f);
        juicyPhaseName.fontSize += 10;
        juicyPhaseName.text = "1";
        yield return new WaitForSeconds(1f);
        countdown = false;
        juicyPhaseName.fontSize += 10;
        juicyPhaseName.text = "FIGHT!";
        timeRemaining = FIGHTLIMIT;
        timer.text = timeRemaining.ToString("0.00");
        Debug.Log("movable");
        P1S.moveable = true;
        P2S.moveable = true;
        yield return new WaitForSeconds(1.5f);
        juicyPhaseName.text = "";
    }
}