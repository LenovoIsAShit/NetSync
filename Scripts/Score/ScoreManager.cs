using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


/// <summary>
/// 计分板管理器
/// </summary>
public class ScoreManager : MonoBehaviour
{
    TMP_Text score;
    TMP_Text level;
    public int level_now;
    public int score_now;

    public void Add(int score)
    {
        this.score_now += score;
        this.score.text = "Score: " + this.score_now.ToString();
    }

    public void SetScore(int score)
    {
        this.score_now = score;
        this.score.text = "Score: " + this.score_now.ToString();
    }
    public void SetLevel(int level)
    {
        level_now = level;
        this.level.text = "Level: " + level.ToString();
    }

    public void NextLevel()
    {
        level_now++;
        this.level.text = "Level: " + (level_now).ToString();
    }

    public void Defalut_init()
    {
        this.score = transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        this.level = transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
        score.text = "Score: " + (0).ToString();
        level.text = "Level: " + (1).ToString();
        level_now = 1;
    }

    public void GameDataInit(GameData gd)
    {
        Defalut_init();

        SetScore(gd.score);
        SetLevel(gd.level);
    }
}
