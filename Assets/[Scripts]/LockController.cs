using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockController : MonoBehaviour
{
    ContentSizeFitter sizeFitter;
    public Difficulty difficultyState = Difficulty.EASY;

    [SerializeField]
    GameObject combinationPrefab;

    [SerializeField]
    public static int timeTillComboReset = 10;
    float timeCounter;
    [SerializeField]
    int SECOND;


    [SerializeField]
    List<CombinationController> combinations;


    int numCombinations;
    int currentCombinationIndex = 0;

    void Awake()
    {
        Debug.Log("Combination set created");
    }

    void OnDestroy() 
    {
        Debug.Log("Combination set destroyed");
    }

    void Start()
    {
        sizeFitter = GetComponent<ContentSizeFitter>();

        DifficultySetting();
        SECOND = timeTillComboReset;

        for(int i = 0; i < numCombinations; i++)
        {
            GameObject newObj = Instantiate(combinationPrefab, this.transform);
            var newCombination = newObj.GetComponent<CombinationController>();
            newCombination.combinationTxtBttn.onClick.AddListener(onCombinationUnlock);
            combinations.Add(newCombination);
        }

        ResetCurrentCombination();
    }

    void DifficultySetting()
    {
        switch(difficultyState)
        {
            case Difficulty.EASY:
                numCombinations = 3;
                timeTillComboReset = 10;
                LockUIManager.instance.SetDifficultyText("EASY");
                break;
            case Difficulty.MEDIUM:
                numCombinations = 4;
                timeTillComboReset = 10;
                LockUIManager.instance.SetDifficultyText("MEDIUM");
                break;
            case Difficulty.HARD:
                numCombinations = 5;
                timeTillComboReset = 10;
                LockUIManager.instance.SetDifficultyText("HARD");
                break;
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            onCombinationRelock();
        }

        if(currentCombinationIndex > 0 && currentCombinationIndex < combinations.Count)
        {
            TimeCounter();
        }
    }

    void onCombinationUnlock()
    {
        var currentCombination = combinations[currentCombinationIndex];
        // combination doesn't unlock if guessed value is not randomized value
        if(currentCombination.guessedValue != currentCombination.randomizedValue) return;
        
        // disable size fitter to maintain combination position in grid
        if(sizeFitter.enabled == true) sizeFitter.enabled = false;
        
        Debug.Log("Unlocked combination " + currentCombinationIndex);
        currentCombination.gameObject.SetActive(false);
        LockUIManager.instance.UpdateFeedbackText("Unlocked combination " + 
                                            (currentCombinationIndex + 1));
        
        currentCombinationIndex++;
        
        if(currentCombinationIndex >= combinations.Count)
        {
            LockUIManager.instance.ShowResultText();
            return;
        }

        ResetCurrentCombination();

        // Reset timer
        SECOND = timeTillComboReset;

        LockUIManager.instance.UpdateHintText("Unlock the combination before combination resets");
    }

    void onCombinationRelock()
    {
        if(currentCombinationIndex >= combinations.Count) return;

        if(currentCombinationIndex <= 0)
        {
            currentCombinationIndex = 0;
            return;
        }

        var currentCombination = combinations[currentCombinationIndex];
        currentCombination.combinationImg.color = Color.white;
        currentCombination.combinationText.color = Color.black;
        currentCombination.combinationText.fontStyle = FontStyle.Normal;
        currentCombination.guessedValue = 0;
        currentCombination.combinationText.text = 0.ToString();
        currentCombination.isCurrent = false;

        currentCombinationIndex--;

        ResetCurrentCombination();

        LockUIManager.instance.UpdateFeedbackText("Combination " +
                                            (currentCombinationIndex + 1) +
                                            " relocked");
        
        if(currentCombinationIndex == 0)
        {
            LockUIManager.instance.UpdateHintText("Click on your number to guess combination number");
        }
    }

    void ResetCurrentCombination()
    {
        var currentCombination = combinations[currentCombinationIndex];
        currentCombination.gameObject.SetActive(true);

        currentCombination.isCurrent = true;
        currentCombination.combinationImg.color = Color.white;
        currentCombination.combinationText.color = Color.red;
        currentCombination.combinationText.fontStyle = FontStyle.Bold;
    }
    
    void TimeCounter()
    {
        if (timeCounter >= 1)
        {
            SECOND--;
            if (SECOND <= 0)
            {
                onCombinationRelock();
                SECOND = timeTillComboReset;
            }
            timeCounter = 0;
        }
        else
        {
            timeCounter += Time.deltaTime;
        }

        LockUIManager.instance.UpdateTimerText(SECOND);
    }
}