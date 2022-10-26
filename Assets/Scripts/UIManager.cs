using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{

    public static UIManager instance;

    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject hiddenObjectIconHolder;
    [SerializeField] private GameObject hiddenObjectIconPrefab;
    [SerializeField] private GameObject gamePanelFailed;
    [SerializeField] private GameObject gamePanelWon;

    private List<GameObject> hiddenObjectIconList;

    public GameObject GamePanelFailed { get { return gamePanelFailed; } }
    public GameObject GamePanelWon { get { return gamePanelWon; } }
    public TextMeshProUGUI TimerText { get { return timerText; } }

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != null) Destroy(gameObject);

        hiddenObjectIconList = new List<GameObject>();
    }

    public void PopulateHiddenObjectIcon(List<HiddenObjectData> activeHiddenObjectList)
    {
        hiddenObjectIconList.Clear();

        for (int i = 0; i < activeHiddenObjectList.Count; i++)
        {
            GameObject icon = Instantiate(hiddenObjectIconPrefab, hiddenObjectIconHolder.transform);
            icon.name = activeHiddenObjectList[i].hiddenObject.name;
            Image childImg = icon.transform.GetChild(0).GetComponent<Image>();

            childImg.sprite = activeHiddenObjectList[i].hiddenObject.GetComponent<SpriteRenderer>().sprite;

            hiddenObjectIconList.Add(icon);
        }
    }

    public void CheckSelectedHiddenObject(string objectName)
    {
        for (int i = 0; i < hiddenObjectIconList.Count; i++)
        {
            if(hiddenObjectIconList[i].name == objectName)
            {
                hiddenObjectIconList[i].SetActive(false);
                break;
            }
        }
    }

}
