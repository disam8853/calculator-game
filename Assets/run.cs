using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class run : MonoBehaviour
{
    // Start is called before the first frame update
    public Button exit;
    public Button start;
    public Button restart;
    public Text title;
    public Button game_start_player1;
    public Button game_start_player2;
    public GameObject player_picture1;
    public GameObject player_picture2;
    public Text time;
    public Text show_equation;
    public InputField player1_answerUI;
    public InputField player2_answerUI;
    public Text player1_score;
    int player1_score_count = 0;
    public Text player2_score;
    int player2_score_count = 0;
    int time_int = 30;
    int game_count_player1= 0;
    int game_count_player2= 0;
    //bool Game_count_plus = true;
    // 這邊開始是新增的
    
    int a = 0;
    int b = 0;
    int op = 0;
    // 題目答案
    int true_answer = 0;
    int player_answer = 0;
    public void build_answer()
    {
        
        op = UnityEngine.Random.Range(0,4);
        a = UnityEngine.Random.Range(1, 25);
        b = UnityEngine.Random.Range(1, 25);
        
        
        if (op==0)
        {
            this.true_answer = Convert.ToInt32(a) + Convert.ToInt32(b) ;
            this.show_equation.text = Convert.ToString(a) + "+" + Convert.ToString(b) + "= ";
            
            
            
        }
        if (op==1)
        {
            this.true_answer = Convert.ToInt32(a) - Convert.ToInt32(b) ;
            this.show_equation.text = Convert.ToString(a) + "-" + Convert.ToString(b) + "= ";
            
            
            
            
        }
        if(op==2)
        {
            this.true_answer = Convert.ToInt32(a) * Convert.ToInt32(b) ;
            this.show_equation.text = Convert.ToString(a) + "X" + Convert.ToString(b) + "= ";
            
            
            
            
        }
        if(op==3)
        {
            this.true_answer = Convert.ToInt32(a) / Convert.ToInt32(b) ;
            this.show_equation.text = Convert.ToString(a) + "/" + Convert.ToString(b) + "= ";
        
            
            
            
        }
        
    }
    public void check_answer_player1()
    {
        player_answer = int.Parse(player1_answerUI.text);
        //test的code
        show_equation.text = player1_answerUI.text + "  " + Convert.ToString(this.true_answer);
        if (Convert.ToInt32(player_answer)==true_answer)
        {
            this.player1_score_count = this.player1_score_count+1;
            player1_score.text = Convert.ToString(player1_score_count);
            build_answer();
        }

    }
    public void check_answer_player2()
    {
        player_answer = int.Parse(player2_answerUI.text);
        show_equation.text = player2_answerUI.text + "  " + Convert.ToString(this.true_answer);
        if (Convert.ToInt32(player_answer)==true_answer)
        {
            this.player2_score_count = this.player2_score_count+1;
            player2_score.text = Convert.ToString(player2_score_count);
            build_answer();   
        }
    }
    public void game_count_plus()
    {    
        game_count_player1 = game_count_player1 + 1;
           
    }
    public void game_count_plus_player2()
    {    
        game_count_player2 = game_count_player2 + 1;
    }

    //以上是新增的
    


    public void game_start_button_player1()
    {
        build_answer();
        player1_answerUI.gameObject.SetActive(true);
        game_start_player1.gameObject.SetActive(false);
        time.gameObject.SetActive(true);
        show_equation.gameObject.SetActive(true);
    }
    public void game_start_button_player2()
    {
        build_answer();
        player2_answerUI.gameObject.SetActive(true);
        game_start_player2.gameObject.SetActive(false);
        show_equation.gameObject.SetActive(true);
    }
    public void timer_ui()
    {
        
        InvokeRepeating("timer", 1, 1);
        ;
    }
    private void timer()
    {
        
        time_int -= 1;
        time.text = Convert.ToString(time_int);
        if (time_int == 0)
        {
            //game_count = game_count + 1;
            time.text = "停";
            show_equation.text = " ";
            CancelInvoke("timer");
            this.time_int = 30;
            // 這行怪怪的
            player1_answerUI.gameObject.SetActive(false);
            
            //這段程式碼很有問題
            if (this.game_count_player1>=1)
            {
                game_start_player2.gameObject.SetActive(true);
                show_equation.gameObject.SetActive(false);
                // 修改過的
                //this.Game_count_plus = false;
                //this.game_count = this.game_count + 1 ;
                //game_count = game_count + 1;
            }
            
            if (this.game_count_player2>=1)
            {
                player2_answerUI.gameObject.SetActive(false);
                exit.gameObject.SetActive(true);
                show_who_win();
                restart.gameObject.SetActive(true);      
            }
            
        }
    }
    void show_who_win()
    {
        player1_score_count = Convert.ToInt32(player1_score.text);
        player2_score_count = Convert.ToInt32(player2_score.text);
        if (player1_score_count > player2_score_count)
        {
            show_equation.text = " Player1 win!!!";

        }
        if (player1_score_count < player2_score_count)
        {
            show_equation.text = " Player2 win!!!";
        }
        if (player1_score_count == player2_score_count)
        {
            show_equation.text = " It's tie!!!";    
        }

    }
    public void restart_game()
    {
        restart.gameObject.SetActive(false);
        game_start_player1.gameObject.SetActive(true);
        show_equation.gameObject.SetActive(false);
        exit.gameObject.SetActive(false);
        restart_score();
        // 按下產生新答案
        //build_answer();
    }
    void restart_score()
    {
        this.player1_score_count = 0;
        player1_score.text = Convert.ToString(player1_score_count);

        this.player2_score_count = 0;
        player2_score.text = Convert.ToString(player2_score_count);    
    }

    //public void button_exit_close()
    //{
    //    exit.gameObject.SetActive(false);
    //}
    // 按最一開始的鍵
    public void initial()
    {
        // 這行出現時，會跑一次build_answer()
        game_start_player1.gameObject.SetActive(true);
        title.gameObject.SetActive(false);
        start.gameObject.SetActive(false);
        exit.gameObject.SetActive(false);
        player_picture1.SetActive(true);
        player_picture2.SetActive(true);
        
        
        
        
        
        
        
    }
    void Start()
    {
        //title.gameObject.SetActive(false);
        // 似乎不能隱藏物件才能執行東西
        //this.true_answer = 0;
        //this.show_equation.text = " ";
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
