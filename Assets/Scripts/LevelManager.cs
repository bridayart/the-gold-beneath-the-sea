using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [SerializeField] private float timeLimit;

    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _collectClip;

    [SerializeField] private List<HiddenObjectData> hiddenObjectsList;

    private List<HiddenObjectData> activeHiddenObjectsList;

    [SerializeField] private int maxActiveHiddenObjectsCount = 4;

    private int totalHiddenObjectsFound = 0;
    private float currentTime = 0;
    private GameStatus gameStatus = GameStatus.NEXT;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != null) Destroy(gameObject);
    }

    private void Start()
    {
        _source = GetComponent<AudioSource>();
        _source.Play();

        activeHiddenObjectsList = new List<HiddenObjectData>();
        AssignHiddenObjects();
    }

    void AssignHiddenObjects()
    {
        currentTime = timeLimit;
        UIManager.instance.TimerText.text = "" + currentTime;

        totalHiddenObjectsFound = 0;

        activeHiddenObjectsList.Clear();
        for (int i = 0; i < hiddenObjectsList.Count; i++)
        {
            hiddenObjectsList[i].hiddenObject.GetComponent<BoxCollider2D>().enabled = false;
        }

        int k = 0;
        while(k < maxActiveHiddenObjectsCount)
        {
            int randomVal = UnityEngine.Random.Range(0, hiddenObjectsList.Count);

            if (!hiddenObjectsList[randomVal].makeHidden)
            {
                hiddenObjectsList[randomVal].hiddenObject.name = "" + k;
                hiddenObjectsList[randomVal].makeHidden = true;
                hiddenObjectsList[randomVal].hiddenObject.GetComponent<BoxCollider2D>().enabled = true;

                activeHiddenObjectsList.Add(hiddenObjectsList[randomVal]);

                k++;
            }
        }

        UIManager.instance.PopulateHiddenObjectIcon(activeHiddenObjectsList);
        gameStatus = GameStatus.PLAYING;
    }

    private void Update()
    {
        if (gameStatus == GameStatus.PLAYING)
        {

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(pos, Vector3.zero);

                if (hit && hit.collider != null)
                {
                    //Debug.Log("Object Name:" + hit.collider.gameObject.name);

                    hit.collider.gameObject.SetActive(false);
                    UIManager.instance.CheckSelectedHiddenObject(hit.collider.gameObject.name);

                    for (int i = 0; i < activeHiddenObjectsList.Count; i++)
                    {
                        if (activeHiddenObjectsList[i].hiddenObject.name == hit.collider.gameObject.name)
                        {
                            _source.PlayOneShot(_collectClip);
                            activeHiddenObjectsList.RemoveAt(i);
                            break;
                        }
                    }

                    totalHiddenObjectsFound++;

                    if (totalHiddenObjectsFound >= maxActiveHiddenObjectsCount)
                    {
                        _source.Stop();
                        Debug.Log("Level Complete!");
                        UIManager.instance.GamePanelWon.SetActive(true);
                        gameStatus = GameStatus.NEXT;
                    }
                }
            }

            currentTime -= Time.deltaTime;
            TimeSpan time = TimeSpan.FromSeconds(currentTime);
            UIManager.instance.TimerText.text = time.ToString("m':'ss");

            if (currentTime <= 0)
            {
                _source.Stop();
                Debug.Log("Level Lost!");
                UIManager.instance.GamePanelFailed.SetActive(true);
                gameStatus = GameStatus.NEXT;
            }
        }
    }

}

[System.Serializable]
public class HiddenObjectData
{
    public string name;
    public GameObject hiddenObject;
    public bool makeHidden = false;

}

public enum GameStatus
{
    PLAYING,
    NEXT
}
