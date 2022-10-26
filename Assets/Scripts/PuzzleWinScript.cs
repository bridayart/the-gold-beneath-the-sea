using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class PuzzleWinScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject gamePanelFailed;
    [SerializeField] private GameObject gamePanelWon;

    [SerializeField] private AudioSource _source;

    [SerializeField] private float timeLimit;
    private float currentTime = 0;

    private int pointsToWin;
    private int currentPoints;
    public GameObject puzzlePieces;

    void Start()
    {
        _source = GetComponent<AudioSource>();
        _source.Play();

        currentTime = timeLimit;
        timerText.text = "" + currentTime;
        pointsToWin = puzzlePieces.transform.childCount;
    }

    void Update()
    {

        if (currentPoints >= pointsToWin)
        {
            _source.Stop();

            gamePanelWon.SetActive(true);
            Destroy(this);
        }

        currentTime -= Time.deltaTime;
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        timerText.text = time.ToString("m':'ss");
        
        if (currentTime <= 0)
        {
            _source.Stop();

            gamePanelFailed.SetActive(true);
            GetComponentInChildren<BoxCollider>().enabled = false;
            Destroy(this);
        }
    }

    public void AddPoints()
    {
        currentPoints++; 
    }
}
