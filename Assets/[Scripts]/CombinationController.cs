using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CombinationController : MonoBehaviour
{
    // the min/max values available in combination
    const int MIN = 0;
    const int MAX = 9;

    public bool isCurrent = false;
    
    
    [SerializeField]
    Button upArrowBttn;
    [SerializeField]
    Button downArrowBttn;
    [SerializeField]
    public Text combinationText;
    [SerializeField]
    public Button combinationTxtBttn;
    [SerializeField]
    public Image combinationImg;

    //public delegate void GuessAction();
    //public static event GuessAction onCorrectGuess;
    //public static UnityEvent<CombinationController> onCorrectGuess;
    //public static event Action<CombinationController> onCorrectGuess;
    [SerializeField]
    public int guessedValue = MIN;
    [SerializeField]
    public int randomizedValue = MIN;

    void Awake() 
    {
        isCurrent = false;
        combinationImg = GetComponent<Image>();

        Transform[] allchildren = GetComponentsInChildren<Transform>();

        foreach(Transform t in allchildren)
        {
            if(t.gameObject.name == "UpArrow")
            {
                upArrowBttn = t.gameObject.GetComponent<Button>();
            }
            else if(t.gameObject.name == "DownArrow")
            {
                downArrowBttn = t.gameObject.GetComponent<Button>();
            }
            else if(t.gameObject.name == "CombinationTxt")
            {
                combinationText = t.gameObject.GetComponent<Text>();
                combinationTxtBttn = t.gameObject.GetComponent<Button>();
            }
        }
    }

    void Start()
    {
        randomizedValue = Random.Range(MIN, MAX + 1);
        upArrowBttn.onClick.AddListener(UpArrowPress);
        downArrowBttn.onClick.AddListener(DownArrowPress);
        combinationTxtBttn.onClick.AddListener(onValueComparison);

        if(isCurrent == false)
        {
            upArrowBttn.enabled = false;
            downArrowBttn.enabled = false;
            combinationTxtBttn.enabled = false;
        }
    }

    void Update()
    {
        // enables/disables combination interaction
        if(isCurrent == true)
        {
            upArrowBttn.enabled = isCurrent;
            downArrowBttn.enabled = isCurrent;
            combinationTxtBttn.enabled = isCurrent;
        }
        else if(isCurrent == false)
        {
            upArrowBttn.enabled = isCurrent;
            downArrowBttn.enabled = isCurrent;
            combinationTxtBttn.enabled = isCurrent;
        }

        if(Input.GetKeyDown(KeyCode.Space) && isCurrent == true)
        {
            onValueComparison();
            //combinationTxtBttn.onClick.Invoke();
        }
    }

    void onValueComparison()
    {
        if(isCurrent == false) return;
        //Debug.Log(Mathf.InverseLerp(MIN, MAX, guessedValue));

        // get the distance away from it in percentages
        var guessValPercent = Mathf.InverseLerp(MIN, MAX, guessedValue);
        var correctValPercent = Mathf.InverseLerp(MIN, MAX, randomizedValue);

        float colorChange = Mathf.Abs(guessValPercent - correctValPercent);
        
        var originalColor = combinationImg.color;
        Color finalColor = new Color(colorChange, originalColor.g, colorChange, originalColor.a);
        combinationImg.color = finalColor;
        Debug.Log("combination tested");

        //UIManager.instance.UpdateFeedbackText("Invalid number");
        
        // if(guessedValue == randomizedValue)
        // {
        //     combinationImg.color = Color.white;
        //     //onCorrectGuess?.Invoke(this, EventArgs.Empty);
        //     if(onCorrectGuess != null)
        //     {
        //         onCorrectGuess(this);
        //     }
        // }
    }

    void UpArrowPress()
    {
        guessedValue++;
        if(guessedValue > MAX)
        {
            guessedValue = MIN;
        }
        
        combinationText.text = guessedValue.ToString();
    }

    void DownArrowPress()
    {
        guessedValue--;
        if(guessedValue < MIN)
        {
            guessedValue = MAX;
        }
        
        combinationText.text = guessedValue.ToString();
    }

}