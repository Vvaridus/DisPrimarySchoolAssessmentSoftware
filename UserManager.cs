using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    public static UserManager instance;

    [SerializeField] private GameObject loginUI;
    [SerializeField] private GameObject registerUI;

    public static UserManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UserManager>();
            }

            return instance;
        }
    }

    public void LoginScreen()
    {
        loginUI.SetActive(true);
        registerUI.SetActive(false);
    }

    public void RegisterScreen()
    {
        loginUI.SetActive(false);
        registerUI.SetActive(true);
    }
}
