using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    [SerializeField] private Button buttonA, buttonB, buttonC;
    [SerializeField] private TextMeshProUGUI textA, textB, textC;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Image questionImage;
    [SerializeField] private Sprite tmpChangeImage;
    [SerializeField] private GameObject apple;
    [SerializeField] private GameObject parentObjectSpawn;
    [SerializeField] private GameObject parentObjectSpawn2;
    [SerializeField] private GameObject questionTextOneImage;
    [SerializeField] private TextMeshProUGUI questionTextOneText;
    [SerializeField] private GameObject questionTextTwoImage;
    [SerializeField] private TextMeshProUGUI questionTextTwoText;
    [SerializeField] private TextMeshProUGUI textOperator;
    [SerializeField] private TextMeshProUGUI textOperator2;

    public int questionNumberCount;
    private int answer;
    private int answer2;
    private int correctSelection;
    private int operatorType;
    private string answerString;
    private int questionType;

    public static UIManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }

    private void Start()
    {
        questionNumberCount = 0;
        operatorType = Random.Range(1, 5);
        answer = Random.Range(1, 10);
        answerString = answer.ToString();
        correctSelection = Random.Range(1, 3);
        switch (correctSelection)
        {
            case 1:
                textA.text = answer.ToString();

                if (answer <= 2)
                {
                    textB.text = (answer + 1).ToString();
                }
                else if (answer > 2)
                {
                    textB.text = (answer - 2).ToString();
                }

                textC.text = (answer + 2).ToString();
                break;
            case 2:
                textB.text = answer.ToString();

                if (answer <= 2)
                {
                    textA.text = (answer + 1).ToString();
                }
                else if (answer > 2)
                {
                    textA.text = (answer - 2).ToString();
                }

                textC.text = (answer + 2).ToString();
                break;

            case 3:
                textC.text = answer.ToString();

                if (answer <= 2)
                {
                    textB.text = (answer + 1).ToString();
                }
                else if (answer > 2)
                {
                    textB.text = (answer - 2).ToString();
                }

                textA.text = (answer + 2).ToString();
                break;
        }

        for (int i = 0; i < answer; i++)
        {
            GameObject tmp = Instantiate(apple) as GameObject;
            tmp.transform.parent = parentObjectSpawn.transform;
        }
    }

    public void ClearPrevious()
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag("Destroy");

        foreach (GameObject item in items)
        {
            GameObject.Destroy(item);
        }
        questionTextOneImage.SetActive(false);
        questionTextTwoImage.SetActive(false);
        textOperator.text = "";
        textOperator2.text = "";
    }    

    public void ResetQuestion()
    {
        Debug.Log("Text Score: " + ScoreManager.MyInstance.TextScore);
        Debug.Log("Image Score: " + ScoreManager.MyInstance.ImageScore);
        Debug.Log("Combo Score: " + ScoreManager.MyInstance.ComboScore);
        questionNumberCount++;

        if (questionNumberCount > 10)
        {
            //debug passing data to database
            int txtSCore = ScoreManager.MyInstance.TextScore;
            int imgSCore = ScoreManager.MyInstance.ImageScore;
            int comboSCore = ScoreManager.MyInstance.ComboScore;

            AuthManager.MyInstance.SaveData(txtSCore,imgSCore,comboSCore);
            MenuManager.MyInstance.CloseQuiz();
            MenuManager.MyInstance.OpenMainMenu();
        }
        questionType = Random.Range(1, 4);
        operatorType = Random.Range(1, 4);
        correctSelection = Random.Range(1, 4);
        ClearPrevious();
        switch (operatorType)
        {
            //Count
            case 1:
                answer = Random.Range(1, 10);
                answerString = answer.ToString();
                switch (questionType)
                {
                    //Text
                    case 1:
                        questionText.text = "\n\n\n\nWhat is this number?";
                        questionTextOneImage.SetActive(true);
                        questionTextOneText.text = answerString;
                        break;

                    //Image
                    case 2:
                        questionText.text = "How many apples do you see?";

                        for (int i = 0; i < answer; i++)
                        {
                            GameObject tmp = Instantiate(apple) as GameObject;
                            tmp.transform.parent = parentObjectSpawn.transform;
                        }
                        break;

                    //Combo
                    case 3:
                        questionText.text = "How many apples do you see \n\n Or \n\n What is this number?";

                        for (int i = 0; i < answer; i++)
                        {
                            GameObject tmp = Instantiate(apple) as GameObject;
                            tmp.transform.parent = parentObjectSpawn.transform;
                        }
                        questionTextOneImage.SetActive(true);
                        questionTextOneText.text = answerString;
                        break;
                }   

                switch (correctSelection)
                {
                    case 1:
                        textA.text = answer.ToString();

                        if (answer <= 2)
                        {
                            textB.text = (answer + 1).ToString();
                        }
                        else if (answer > 2)
                        {
                            textB.text = (answer - 2).ToString();
                        }

                        textC.text = (answer + 2).ToString();
                        break;
                    case 2:
                        textB.text = answer.ToString();

                        if (answer <= 2)
                        {
                            textA.text = (answer + 1).ToString();
                        }
                        else if (answer > 2)
                        {
                            textA.text = (answer - 2).ToString();
                        }

                        textC.text = (answer + 2).ToString();
                        break;

                    case 3:
                        textC.text = answer.ToString();

                        if (answer <= 2)
                        {
                            textB.text = (answer + 1).ToString();
                        }
                        else if (answer > 2)
                        {
                            textB.text = (answer - 2).ToString();
                        }

                        textA.text = (answer + 2).ToString();
                        break;
                }                
                break;
                //Add
            case 2:
                answer = Random.Range(1, 10);
                answer2 = Random.Range(1, 10);
                answerString = (answer + answer2).ToString();

                switch (questionType)
                {
                    //Text
                    case 1:
                        questionText.text = "\n\n\n\nCan you add the numbers together?";
                        questionTextOneImage.SetActive(true);
                        questionTextOneText.text = answer.ToString();
                        questionTextTwoImage.SetActive(true);
                        questionTextTwoText.text = answer2.ToString();
                        textOperator2.text = "+";
                        break;
                    //Image
                    case 2:
                        questionText.text = "Can you add the apples together?";

                        for (int i = 0; i < answer; i++)
                        {
                            GameObject tmp = Instantiate(apple) as GameObject;
                            tmp.transform.parent = parentObjectSpawn.transform;
                        }

                        for (int i = 0; i < answer2; i++)
                        {
                            GameObject tmp2 = Instantiate(apple) as GameObject;
                            tmp2.transform.parent = parentObjectSpawn2.transform;
                        }
                        textOperator.text = "+";
                        break;

                    //Combo
                    case 3:
                        questionText.text = "Can you add the apples together \n\n Or \n\n Can you add the number together?";

                        for (int i = 0; i < answer; i++)
                        {
                            GameObject tmp = Instantiate(apple) as GameObject;
                            tmp.transform.parent = parentObjectSpawn.transform;
                        }

                        for (int i = 0; i < answer2; i++)
                        {
                            GameObject tmp2 = Instantiate(apple) as GameObject;
                            tmp2.transform.parent = parentObjectSpawn2.transform;
                        }
                        questionTextOneImage.SetActive(true);
                        questionTextOneText.text = answer.ToString();
                        questionTextTwoImage.SetActive(true);
                        questionTextTwoText.text = answer2.ToString();
                        textOperator.text = "+";
                        textOperator2.text = "+";
                        break;
                }

                switch (correctSelection)
                {
                    case 1:

                        textA.text = (answer + answer2).ToString();

                        if (answer + answer2 <= 2)
                        {
                            textB.text = (answer + answer2 + 1).ToString();
                        }
                        else if (answer + answer2 > 2)
                        {
                            textB.text = (answer + answer2 - 2).ToString();
                        }

                        textC.text = (answer + answer2 + 2).ToString();

                        break;
                    case 2:

                        textB.text = (answer + answer2).ToString();

                        if (answer + answer2 <= 2)
                        {
                            textA.text = (answer + answer2 + 1).ToString();
                        }
                        else if (answer + answer2 > 2)
                        {
                            textA.text = (answer + answer2 - 2).ToString();
                        }

                        textC.text = (answer + answer2 + 2).ToString();

                        break;

                    case 3:
                        textC.text = (answer + answer2).ToString();

                        if (answer + answer2 <= 2)
                        {
                            textB.text = (answer + answer2 + 1).ToString();
                        }
                        else if (answer + answer2 > 2)
                        {
                            textB.text = (answer + answer2 - 2).ToString();
                        }

                        textA.text = (answer + answer2 + 2).ToString();

                        break;
                }
                break;

                //subtract
            case 3:              
                answer = Random.Range(2, 10);
                answer2 = Random.Range(1, answer);
                answerString = (answer - answer2).ToString();
                switch (questionType)
                {
                    //Text
                    case 1:
                        questionText.text = "\n\n\n\nCan you take the numbers away from each other?";
                        questionTextOneImage.SetActive(true);
                        questionTextOneText.text = answer.ToString();
                        questionTextTwoImage.SetActive(true);
                        questionTextTwoText.text = answer2.ToString();
                        textOperator2.text = "-";
                        break;

                    //Image
                    case 2:
                        questionText.text = "Can you take the apples away from each other?";

                        for (int i = 0; i < answer; i++)
                        {
                            GameObject tmp = Instantiate(apple) as GameObject;
                            tmp.transform.parent = parentObjectSpawn.transform;
                        }

                        for (int i = 0; i < answer2; i++)
                        {
                            GameObject tmp2 = Instantiate(apple) as GameObject;
                            tmp2.transform.parent = parentObjectSpawn2.transform;
                        }
                        textOperator.text = "-";
                        break;

                    //Combo
                    case 3:
                        questionText.text = "Can you take the apples away from each other \n\n Or \n\n Can you take the numbers away from each other?";

                        for (int i = 0; i < answer; i++)
                        {
                            GameObject tmp = Instantiate(apple) as GameObject;
                            tmp.transform.parent = parentObjectSpawn.transform;
                        }

                        for (int i = 0; i < answer2; i++)
                        {
                            GameObject tmp2 = Instantiate(apple) as GameObject;
                            tmp2.transform.parent = parentObjectSpawn2.transform;
                        }
                        questionTextOneImage.SetActive(true);
                        questionTextOneText.text = answer.ToString();
                        questionTextTwoImage.SetActive(true);
                        questionTextTwoText.text = answer2.ToString();
                        textOperator.text = "-";
                        textOperator2.text = "-";
                        break;
                }
                switch (correctSelection)
                {
                    case 1:

                        textA.text = (answer - answer2).ToString();

                        if (answer - answer2 <= 2)
                        {
                            textB.text = (answer - answer2 + 1).ToString();
                        }
                        else if (answer - answer2 > 2)
                        {
                            textB.text = (answer - answer2 - 2).ToString();
                        }

                        textC.text = (answer - answer2 + 2).ToString();

                        break;
                    case 2:

                        textB.text = (answer - answer2).ToString();

                        if (answer - answer2 <= 2)
                        {
                            textA.text = (answer - answer2 + 1).ToString();
                        }
                        else if (answer - answer2 > 2)
                        {
                            textA.text = (answer - answer2 - 2).ToString();
                        }

                        textC.text = (answer - answer2 + 2).ToString();

                        break;

                    case 3:
                        textC.text = (answer - answer2).ToString();

                        if (answer - answer2 <= 2)
                        {
                            textB.text = (answer - answer2 + 1).ToString();
                        }
                        else if (answer - answer2 > 2)
                        {
                            textB.text = (answer - answer2 - 2).ToString();
                        }

                        textA.text = (answer - answer2 + 2).ToString();

                        break;
                }
                break;

            case 4:

                break;

            case 5:

                break;
        }
    }
    public void AnswersAClicked()
    {
        if (answerString == textA.text)
        {
            questionText.text = "Correct! Great Job";
            ScoreUp(questionType);
            ResetQuestion();
        }
        else
        {
            questionText.text = "Not Quite Right, Try Again.";
            ScoreDown(questionType);
            ResetQuestion();
        }
    }
    public void AnswerBClicked()
    {
        if (answerString == textB.text)
        {
            questionText.text = "Correct! Great Job";
            ScoreUp(questionType);
            ResetQuestion();
        }
        else
        {
            questionText.text = "Not Quite Right, Try Again.";
            ScoreDown(questionType);
            ResetQuestion();
        }
    }
    public void AnswerCClicked()
    {
        if (answerString == textC.text)
        {
            questionText.text = "Correct! Great Job";
            ScoreUp(questionType);
            ResetQuestion();
        }
        else
        {
            questionText.text = "Not Quite Right, Try Again.";
            ScoreDown(questionType);
            ResetQuestion();
        }
    }
    public void ScoreUp(int type)
    {
        switch (type)
        {
            case 1:
                ScoreManager.MyInstance.TextScore++;
                break;

            case 2:
                ScoreManager.MyInstance.ImageScore++;
                break;

            case 3:
                ScoreManager.MyInstance.ComboScore++;
                break;
        }
    }
    public void ScoreDown(int type)
    {
        switch (type)
        {
            case 1:
                if (ScoreManager.MyInstance.TextScore > 0)
                {
                    ScoreManager.MyInstance.TextScore--;
                }
                break;

            case 2:
                if (ScoreManager.MyInstance.ImageScore > 0)
                {
                    ScoreManager.MyInstance.ImageScore--;
                }
                break;

            case 3:
                if (ScoreManager.MyInstance.ComboScore > 0)
                {
                    ScoreManager.MyInstance.ComboScore--;
                }
                break;
        }
    }
}
