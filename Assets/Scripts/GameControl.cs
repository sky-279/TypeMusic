﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameControl : MonoBehaviour
{

    public static GameControl instance;

    public VideoPlayer player;
    public VideoPlayer lbwnbPlayer;
    public RawImage lbwnbImage;
    public Image groupCodeImage;
    public Text scoreText;
    public Toggle singleMode;
    public Text startTip;

    public BlockGenerator generator;
    public bool gameOver = true;
    public bool lbwnb = false;
    public float scrollingSpeed = 300.0F;
    public TextBlock currentBlock = null;
    public float playTime;
    public int score;

    private string lbwnbInputBuffer = "     ";

    // Use this for initialization
    void Start()
    {
        lbwnbImage.enabled = false;
        generator = GetComponent<BlockGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lbwnb && lbwnbPlayer.frame > 0 && ((ulong)lbwnbPlayer.frame >= lbwnbPlayer.frameCount))
        {
            lbwnbPlayer.Stop();
            lbwnbImage.enabled = false;
            GameStart();
        }
        if (gameOver)
        {
            string input = Input.inputString;
            if (input.Length > 0)
            {
                lbwnbInputBuffer += input;
                if (lbwnbInputBuffer.Length > 5)
                {
                    lbwnbInputBuffer = lbwnbInputBuffer.Substring(lbwnbInputBuffer.Length - 5);
                }
                if (lbwnbInputBuffer.EndsWith("lbwnb"))
                {
                    lbwnb = true;
                    lbwnbImage.enabled = true;
                    lbwnbPlayer.Play();
                }
            }
            if (Input.GetKeyDown(KeyCode.Space) && !lbwnbImage.enabled)
            {
                GameStart();
            }
        }
        else
        {
            playTime += Time.deltaTime;
            if (Input.anyKeyDown)
            {
                if (currentBlock != null && Input.GetKeyDown(currentBlock.key))
                {
                    Destroy(currentBlock.gameObject);
                    UpdateScore(1);
                    if (currentBlock.last)
                    {
                        GameOver();
                    }
                    currentBlock = null;
                }
                else
                {
                    UpdateScore(-3);
                }
            }
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void GameStart()
    {
        if (gameOver)
        {
            groupCodeImage.enabled = false;
            startTip.enabled = false;
            generator.ClearAll();
            score = 50;
            scoreText.text = score.ToString();
            player.Play();
            playTime = 0;
            gameOver = false;
            generator.StartGen();
        }
    }

    public void GameOver()
    {
        if (!gameOver)
        {
            gameOver = true;
            player.Stop();
            startTip.enabled = true;
            lbwnb = false;
        }
    }

    public void UpdateScore(int scoreIncr)
    {
        score += scoreIncr;
        scoreText.text = score.ToString();
        if (score <= 0)
        {
            GameOver();
        }
    }

}
