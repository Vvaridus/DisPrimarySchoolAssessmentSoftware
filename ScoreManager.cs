using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public static ScoreManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ScoreManager>();
            }

            return instance;
        }
    }

    private int textScore;
    private int imageScore;
    private int comboScore;

    public int TextScore { get => textScore; set => textScore = value; }
    public int ImageScore { get => imageScore; set => imageScore = value; }
    public int ComboScore { get => comboScore; set => comboScore = value; }
}
