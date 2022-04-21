using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockUIManager : MonoBehaviour
{
    public static LockUIManager instance;
    RectTransform rect;
    public Canvas canvasParentPrefab;

    [SerializeField]
    GameObject combinationSetPrefab;
    [SerializeField]
    public GameObject lockControllerObj;

    Text resultTxt;
    Text FeedbackTxt;
    Text TimerTxt;
    Text HintTxt;
    Text DifficultyTxt;
    Button exitButton;

    void Awake() 
    {
        canvasParentPrefab = GetComponentInParent<Canvas>();
        if(instance != null)
        {
            Destroy(canvasParentPrefab);
        }
        else
        {
            instance = this;
        }

        // finding all text UI
        Text[] allchildren = GetComponentsInChildren<Text>(true);
        foreach(Text t in allchildren)
        {
            if(t.gameObject.name == "ResultTxt")
            {
                resultTxt = t.gameObject.GetComponent<Text>();
            }
            else if(t.gameObject.name == "FeedbackTxt")
            {
                FeedbackTxt = t.gameObject.GetComponent<Text>();
            }
            else if(t.gameObject.name == "TimerTxt")
            {
                TimerTxt = t.gameObject.GetComponent<Text>();
            }
            else if(t.gameObject.name == "HintTxt")
            {
                HintTxt = t.gameObject.GetComponent<Text>();
            }
            else if(t.gameObject.name == "DifficultyTxt")
            {
                DifficultyTxt = t.gameObject.GetComponent<Text>();
            }
        }

        exitButton = GetComponentInChildren<Button>();
        exitButton.onClick.AddListener(QuitGame);


        rect = GetComponent<RectTransform>();
    }

    void OnEnable() 
    {
        InstantiateCombinationSet();
        HintTxt.text = "Click on your number to guess combination number";
        resultTxt.gameObject.SetActive(false);
        FeedbackTxt.text = "";
        TimerTxt.text = "0";
    }

    void OnDisable() 
    {
        if(lockControllerObj) Destroy(lockControllerObj);
    }

    void InstantiateCombinationSet()
    {
        if(lockControllerObj == null)
        {
            //lockControllerObj = Instantiate(combinationSetPrefab, new Vector3(0, 40, 0), Quaternion.identity,
            //                                this.transform);
            lockControllerObj = Instantiate(combinationSetPrefab, this.transform);
            lockControllerObj.transform.localPosition = rect.anchoredPosition3D + new Vector3(0, 40, 0);
        }
        
    }

    void Start()
    {
        DifficultyTxt.text = "EASY";
    }

    public void SetDifficultyText(string message)
    {
        DifficultyTxt.text = message;
    }

    public void UpdateTimerText(int time)
    {
        TimerTxt.text = time.ToString();
    }

    public void UpdateFeedbackText(string message)
    {
        FeedbackTxt.text = message;
    }

    public void UpdateHintText(string message)
    {
        HintTxt.text = message;
    }

    public void ShowResultText()
    {
        resultTxt.gameObject.SetActive(true);
    }

    void QuitGame()
    {
        canvasParentPrefab.gameObject.SetActive(false);
    }
}
