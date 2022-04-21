using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public SkillLevel playerSkill = SkillLevel.BEGINNER;
    CameraController playerCamControl;
    [SerializeField]
    public GameObject UICanvas;
    [SerializeField]
    GameObject lockPickCanvasInst;

    string skillLevelTxt = "";
    [SerializeField]
    int timerIncrease = 0;

    GameObject TriggerTxtObj;
    Text playerSkillTxt;

    bool inRedBox = false;
    bool inYellowBox = false;
    bool inGreenBox = false;
    void Start()
    {
        playerCamControl = GetComponentInChildren<CameraController>();
        SetSkillLevel();

        for(int i = 0; i < UICanvas.transform.childCount; i++)
        {
            if(UICanvas.transform.GetChild(i).gameObject.name == "PlayerSkillTxt")
            {
                var childObj = UICanvas.transform.GetChild(i).gameObject;
                playerSkillTxt = childObj.GetComponent<Text>();
            }
            else if(UICanvas.transform.GetChild(i).gameObject.name == "TriggerTxt")
            {
                var childObj = UICanvas.transform.GetChild(i).gameObject;
                TriggerTxtObj = childObj;
            }
        }
        //UICanvas = GameObject.FindObjectOfType<Canvas>().gameObject;
        playerSkillTxt.text = skillLevelTxt;

        TriggerTxtObj.SetActive(false);
        //lockPickCanvasInst = LockUIManager.instance.canvasParentPrefab.gameObject;
        lockPickCanvasInst.SetActive(false);
    }

    void SetSkillLevel()
    {
        switch(playerSkill)
        {
            case SkillLevel.BEGINNER:
                skillLevelTxt = "Beginner";
                timerIncrease = 0;
                break;
            case SkillLevel.INTERMEDIATE:
                skillLevelTxt = "Intermediate";
                timerIncrease = 2;
                break;
            case SkillLevel.MASTER:
                skillLevelTxt = "Master";
                timerIncrease = 4;
                break;
        }
    }

    void Update()
    {
        if(inRedBox || inGreenBox || inYellowBox)
        {
            TriggerTxtObj.SetActive(true);
        }

        if(Input.GetKeyDown(KeyCode.P) && inRedBox)
        {
            InstantiateCombinationUI(Difficulty.HARD);
        }
        if(Input.GetKeyDown(KeyCode.P) && inYellowBox)
        {
            InstantiateCombinationUI(Difficulty.MEDIUM);
        }
        if(Input.GetKeyDown(KeyCode.P) && inGreenBox)
        {
            InstantiateCombinationUI(Difficulty.EASY);
        }

        if(lockPickCanvasInst.activeSelf == false)
        {
            playerCamControl.enabled = true;
        }
    }

    void InstantiateCombinationUI(Difficulty setting)
    {
        lockPickCanvasInst.SetActive(true);
        var lockController = LockUIManager.instance.lockControllerObj.GetComponent<LockController>();
        lockController.difficultyState = setting; //Difficulty.MEDIUM;
        LockController.timeTillComboReset = LockController.timeTillComboReset + timerIncrease;
        playerCamControl.enabled = false;

        TriggerTxtObj.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag("RedBox"))
        {
            Debug.Log("Red Trigger");
            inRedBox = true;
        }
        
        if(other.gameObject.CompareTag("YellowBox"))
        {
            Debug.Log("Yellow Trigger");
            inYellowBox = true;
        }
        
        if(other.gameObject.CompareTag("GreenBox"))
        {
            Debug.Log("Green Trigger");
            inGreenBox = true;
        }
    }

    void OnTriggerExit(Collider other) 
    {
        TriggerTxtObj.SetActive(false);
        if(other.gameObject.CompareTag("RedBox"))
        {
            Debug.Log("Red Trigger Exit");
            inRedBox = false;
        }
        
        if(other.gameObject.CompareTag("YellowBox"))
        {
            Debug.Log("Yellow Trigger Exit");
            inYellowBox = false;
        }
        
        if(other.gameObject.CompareTag("GreenBox"))
        {
            Debug.Log("Green Trigger Exit");
            inGreenBox = false;
        }
    }
}