using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup mainMenuCanvasGroup;
    [SerializeField] private CanvasGroup quizCanvasGroup;
    [SerializeField] private CanvasGroup settingsCanvasGroup;
    [SerializeField] private CanvasGroup learningCanvasGroup;

    private static MenuManager instance;

    public static MenuManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MenuManager>();
            }

            return instance;
        }
    }

    public void OpenQuiz()
    {
        UIManager.MyInstance.questionNumberCount = 0;
        quizCanvasGroup.alpha = 1;
        quizCanvasGroup.blocksRaycasts = true;
        quizCanvasGroup.interactable = true;
        UIManager.MyInstance.ResetQuestion();
        
    }

    public void CloseQuiz()
    {
        quizCanvasGroup.alpha = 0;
        quizCanvasGroup.blocksRaycasts = false;
        quizCanvasGroup.interactable = false;
    }

    public void OpenMainMenu()
    {
        mainMenuCanvasGroup.alpha = 1;
        mainMenuCanvasGroup.blocksRaycasts = true;
        mainMenuCanvasGroup.interactable = true;
    }

    public void CloseMainMenu()
    {
        mainMenuCanvasGroup.alpha = 0;
        mainMenuCanvasGroup.blocksRaycasts = false;
        mainMenuCanvasGroup.interactable = false;
    }

    public void OpenSettings()
    {
        settingsCanvasGroup.alpha = 1;
        settingsCanvasGroup.blocksRaycasts = true;
        settingsCanvasGroup.interactable = true;
    }

    public void CloseSettings()
    {
        settingsCanvasGroup.alpha = 0;
        settingsCanvasGroup.blocksRaycasts = false;
        settingsCanvasGroup.interactable = false;
    }

    public void OpenLearning()
    {
        learningCanvasGroup.alpha = 1;
        learningCanvasGroup.blocksRaycasts = true;
        learningCanvasGroup.interactable = true;
    }

    public void CloseLearning()
    {
        learningCanvasGroup.alpha = 0;
        learningCanvasGroup.blocksRaycasts = false;
        learningCanvasGroup.interactable = false;
    }    
}
