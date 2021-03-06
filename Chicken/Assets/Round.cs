﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Round : MonoBehaviour {

    public int POSLIMIT;
    public int RESLIMIT;
    public int FIGHTLIMIT;

    bool roundOver;

    public int round_num;
    public int P1_wins;
    public int P2_wins;
    public string P2_res = "";
    public string P1_res = "";
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
    public UnityEngine.UI.Image hypeBar;
    public Sprite[] scoreBar;
    public Sprite[] resBar;
    public Sprite[] hypeFill;
    bool countdown = false;
    public AudioSource deathSound;

    //Hype Algoritm
    float P1_Previous_Hype;
    float P2_Previous_Hype;
    public float total_hype;
    public UnityEngine.UI.Text HYPELABEL;
    public int maxHype = 10000000;
    public int[] hypeTiers;
    public int hypeTier;

    // Use this for initialization
    void Start () {
        P1_wins = 0;
        P2_wins = 0;
        round_num = 0;
        currentRound = 0;
        currentPhase = 0;
        total_hype = 0;
        timeRemaining = POSLIMIT;
        Update_UI();
        //gameTransform = GetComponeP2ResBar.enabled = false;ntInParent(typeof(Transform)) as Transform;
        //P1S = Player1.GetComponent<PlayerScript>() as PlayerScript;
        //P2S = Player2.GetComponent<PlayerScript>() as PlayerScript;

        juicyPhaseName.text = "POSITION!";
        descriptivePhase.text = "Move to your starting position of choice.";
    }

    // Update is called once per frame
    void Update ()
    {
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
            P2S.moveable = false;
        }
        if (!countdown)
            timer.text = timeRemaining.ToString("0.00");
        if (currentRound != 0)
            roundLbl.text = "Round " + currentRound.ToString();
        else
            roundLbl.text = "Round 1";
        if (timeRemaining <= 0)
        {
            if (currentPhase == 0)
            {
                juicyPhaseName.text = "RESTRICT!";
                descriptivePhase.text = "choose 2 attacks that your foe may not use.";
                P1_res = "";
                P2_res = "";
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
                    if (P2_res.Length == 0)
                        P2_res = "AB";
                    else if (P2_res.Length == 1)
                        if (P2_res[0] != 'A')
                            P2_res += "A";
                        else
                            P2_res += "B";
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
                    if (P2_res.Length == 0)
                        P2_res = "AB";
                    else if (P2_res.Length == 1)
                        if (P2_res[0] != 'A')
                            P2_res += "A";
                        else
                            P2_res += "B";
                }
            }
            else if (currentPhase == 2)
            {
                currentPhase = 3;       //game over
                timeRemaining = 3.0f;
            }
            else if(currentPhase == 3)
            {
                timeRemaining = POSLIMIT;
                juicyPhaseName.fontSize = 100;
                juicyPhaseName.text = "POSITION!";
                descriptivePhase.text = "Move to your starting position of choice.";
                P1S.damageable = P2S.damageable = false;
                P1S.a_disabled = P1S.b_disabled = P1S.x_disabled = P1S.y_disabled =
                    P2S.a_disabled = P2S.b_disabled = P2S.x_disabled = P2S.y_disabled = false;
                currentPhase = 0;
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
                    if ((P2S.HP <= 0 && P1S.HP <= 0)|| timeRemaining <= 0)
                    {
                        P1S.moveable = false;
                        P2S.moveable = false;
                        End_Round(0);
                    }
                    if (P1S.HP <= 0){
                        //Player 1 loses
                        deathSound.Play();
                        Debug.Log("Player 1 Loses");
                        P1S.moveable = false;
                        P2S.moveable = false;
                        End_Round(2);
                        //Display victory screen for player 2
                        //Play of the game, etc
                    }
                    if(P2S.HP <= 0){
                        //Player 2 loses
                        deathSound.Play();
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
        total_hype = Mathf.Min(total_hype, maxHype);
        HYPELABEL.text = total_hype.ToString("N");
        if (total_hype > hypeTiers[hypeTier])
        {
            hypeTier++;
            hypeBar.sprite = hypeFill[hypeTier];
            speedMult = 1f + .01f * hypeTier;
            P1S.speedMult = speedMult;
            P2S.speedMult = speedMult;
            P1S.drag += .01f;
            P2S.drag += .01f;

        }
        
    }

    void Update_UI(){
        //Change round number text
        round_label.text = "Round " + currentRound.ToString();
        //Reset health bar
        //Update win numbers
    }

    void End_Round(int winner = 0){
        juicyPhaseName.fontSize = 100;
        Debug.Log("Round Ended");
        if(winner == 0){
            //DRAW
            juicyPhaseName.text = "DRAW!";
            P1_wins++;
            P1S.SCORE++;
            P2_wins++;
            P2S.SCORE++;
            P1Score.sprite = scoreBar[P1S.SCORE];
            P2Score.sprite = scoreBar[P2S.SCORE];
        }
        if(winner == 1){
            juicyPhaseName.text = "Player 1 Wins!";
            P1_wins++;
            P1S.SCORE++;
            P1Score.sprite = scoreBar[P1S.SCORE];
        }
        if(winner == 2){
            juicyPhaseName.text = "Player 2 Wins!";
            P2_wins++;
            P2S.SCORE++;
            P2Score.sprite = scoreBar[P2S.SCORE];
        }
        currentRound++;
        timeRemaining = 3.0f;
    //    currentPhase = 0;
    //    timeRemaining = POSLIMIT;
    //    Update_UI();
        P1S.moveable = true;
        P2S.moveable = true;
        P1S.HP = 3;
        P2S.HP = 3;
        descriptivePhase.text = P1_wins + " - " + P2_wins;
        Update_UI();
        if (P1S.SCORE == 3)
        {
            timer.text = "";
            juicyPhaseName.fontSize = 150;
            juicyPhaseName.text = "Player 1 Wins!";
            WaitForSeconds wait = new WaitForSeconds(5f);
            SceneManager.LoadScene("Main_Menu");
        }
        else if (P2S.SCORE == 3)
        {
            timer.text = "";
            juicyPhaseName.fontSize = 150;
            juicyPhaseName.text = "Player 2 Wins!";
            WaitForSeconds wait = new WaitForSeconds(5f);
            SceneManager.LoadScene("Main_Menu");
        }
    }

    float Get_Hype_Differential(){
        Debug.Log("Hype: " + total_hype);
        if(P1S.Hype == P1_Previous_Hype && P2S.Hype == P2_Previous_Hype) return 0.0f;
        float time_hype = 500f*(20f/timeRemaining);
        float close_game_hype = 500f*(round_num)/(Mathf.Abs(P1_wins - P2_wins) + 1);
        float p1_underdog_hype = (P1S.HP - P1S.HP) * (P2_wins - P1_wins) * 500f;
        float p2_underdog_hype = (P2S.HP - P2S.HP) * (P1_wins - P2_wins) * 500f;
        float P1_hype_diff = P2S.Hype - P1_Previous_Hype;
        float P2_hype_diff = P1S.Hype - P2_Previous_Hype;

        P1_Previous_Hype = P1S.Hype;
        P2_Previous_Hype = P2S.Hype;

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
        timeRemaining = FIGHTLIMIT;
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