using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionDatabase : MonoBehaviour
{
    public MyPlayer myPlayer;
    public string[] missionString = {
        "Collect 50 coins in a single game",    //1
        "Run for 500 meters without collecting any coins",  //2
        "Score 1000 points in a single game",   //3
        "Collect 1000 coins in total",  //4
        "Watch 5 advertisements",   //5
        "Break your best record"    //6
    };

    void Booleans(){
        
        //Внеигровые
        if(myPlayer.coins >= 1000){}
        if(myPlayer.score >= 600){}

    }
    
}

/* 
Разделим миссии на игровые и внеигровые
Игровые миссии можно выполнить в одной сессии игры, а
Внеигровые требуют достижения определенных показателей 
*/