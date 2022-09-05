using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using TMPro;

public class AuthManager : MonoBehaviour
{
    public static AuthManager instance;

    public static AuthManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AuthManager>();
            }

            return instance;
        }
    }

    [SerializeField] private CanvasGroup loginCanvasGroup;
    [SerializeField] private CanvasGroup registerCanvasGroup;
    [SerializeField] private CanvasGroup mainMenuCanvasGroup;

    //Firebase Vars
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;
    public DatabaseReference databaseReference;

    //Login Vars
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text warningLoginText;
    public TMP_Text confirmLoginText;

    //Register Vars
    [Header("Register")]
    public TMP_InputField usernameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField passwordRegisterVerifyField;
    public TMP_Text warningRegisterText;

    [Header("UserData")]
    public TextMeshProUGUI usernameDisplay;

    private void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.Log("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        auth = FirebaseAuth.DefaultInstance;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        
    }

    public void ClearLoginFields()
    {
        emailLoginField.text = "";
        passwordLoginField.text = "";
    }

    public void ClearRegisterFields()
    {
        usernameRegisterField.text = "";
        emailRegisterField.text = "";
        passwordRegisterField.text = "";
        passwordRegisterVerifyField.text = "";
    }

    public void LoginButton()
    {
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
    }

    public void RegisterButton()
    {
        StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
    }

    public void SignOutButton()
    {
        auth.SignOut();
        UserManager.MyInstance.LoginScreen();
        MoveToLogin();
        ClearLoginFields();
        ClearRegisterFields();
    }

    public void SaveData(int textScore, int imageScore, int comboScore)
    {
        StartCoroutine(UpdateUsernameAuth(user.DisplayName));
        StartCoroutine(UpdateUsernameDatabase(user.DisplayName));

        StartCoroutine(UpdateTextScore(textScore));
        StartCoroutine(UpdateImageScore(imageScore));
        StartCoroutine(UpdateComboScore(comboScore));
    }

    private IEnumerator Login(string _email, string _password)
    {
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);

        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseException = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseException.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {                
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            warningLoginText.text = message;
        }
        else
        {
            user = LoginTask.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", user.DisplayName, user.Email);
            warningLoginText.text = "";
            confirmLoginText.text = "Logging in...";

            usernameDisplay.text = user.DisplayName;

            StartCoroutine(LoadUserData());

            yield return new WaitForSeconds(2);

            confirmLoginText.text = "";
            LoginMoveToMain();
            ClearLoginFields();
            ClearRegisterFields();
            Debug.Log(user.DisplayName);
        }
    }

    private void LoginMoveToMain()
    {
        loginCanvasGroup.alpha = 0;
        loginCanvasGroup.blocksRaycasts = false;
        loginCanvasGroup.interactable = false;
        registerCanvasGroup.alpha = 0;
        registerCanvasGroup.blocksRaycasts = false;
        registerCanvasGroup.interactable = false;
        mainMenuCanvasGroup.alpha = 1;
        mainMenuCanvasGroup.blocksRaycasts = true;
        mainMenuCanvasGroup.interactable = true;

        Debug.Log("TScore: " + ScoreManager.MyInstance.TextScore + " IScore: " + ScoreManager.MyInstance.ImageScore + " CScore: " + ScoreManager.MyInstance.ComboScore);
    }

    public void MoveToLogin()
    {
        loginCanvasGroup.alpha = 1;
        loginCanvasGroup.blocksRaycasts = true;
        loginCanvasGroup.interactable = true;
        registerCanvasGroup.alpha = 1;
        registerCanvasGroup.blocksRaycasts = true;
        registerCanvasGroup.interactable = true;
        mainMenuCanvasGroup.alpha = 0;
        mainMenuCanvasGroup.blocksRaycasts = false;
        mainMenuCanvasGroup.interactable = false;
    }

    private IEnumerator Register(string _email, string _password, string _username)
    {
        if (_username == "")
        {
            warningRegisterText.text = "Missing Username";
        }
        else if (passwordRegisterField.text != passwordRegisterVerifyField.text)
        {
            warningRegisterText.text = "Password does not match";
        }
        else
        {
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);
            if (RegisterTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseException = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseException.ErrorCode;
                string message = "Register Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        break;
                }
                warningRegisterText.text = message;
            }
            else
            {
                user = RegisterTask.Result;

                if (user != null)
                {
                    UserProfile profile = new UserProfile { DisplayName = _username };
                    var ProfileTask = user.UpdateUserProfileAsync(profile);
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);
                    if (ProfileTask.Exception != null)
                    {
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseException = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseException.ErrorCode;
                        warningRegisterText.text = "Username Set Failed";
                    }
                    else
                    {
                        UserManager.MyInstance.LoginScreen();
                        warningRegisterText.text = "";
                        ClearLoginFields();
                        ClearRegisterFields();
                    }
                }
            }
        }
    }

    private IEnumerator UpdateUsernameAuth(string _username)
    {
        UserProfile profile = new UserProfile { DisplayName = _username };

        var ProfileTask = user.UpdateUserProfileAsync(profile);

        yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

        if (ProfileTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
        }
        else
        {
            //username is updated
        }
    }

    private IEnumerator UpdateUsernameDatabase(string _username)
    {
        var DBTask = databaseReference.Child("users").Child(user.UserId).Child("username").SetValueAsync(_username);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //database username updated
        }
    }

    private IEnumerator UpdateTextScore(int _textScore)
    {
        Debug.Log(user.DisplayName + " : " + _textScore + " : " + databaseReference);
        var DBTask = databaseReference.Child("users").Child(user.UserId).Child("textscore").SetValueAsync(_textScore);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //text score updated
        }
    }

    private IEnumerator UpdateImageScore(int _imageScore)
    {
        var DBTask = databaseReference.Child("users").Child(user.UserId).Child("imagescore").SetValueAsync(_imageScore);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //image score updated
        }
    }

    private IEnumerator UpdateComboScore(int _comboScore)
    {
        var DBTask = databaseReference.Child("users").Child(user.UserId).Child("comboscore").SetValueAsync(_comboScore);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //combo score updated
        }
    }

    private IEnumerator LoadUserData()
    {
        var DBTask = databaseReference.Child("users").Child(user.UserId).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null)
        {
            ScoreManager.MyInstance.TextScore = 0;
            ScoreManager.MyInstance.ImageScore = 0;
            ScoreManager.MyInstance.ComboScore = 0;
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;

            string txtscore = snapshot.Child("textscore").Value.ToString();
            string imgscore = snapshot.Child("imagescore").Value.ToString();
            string comboscore = snapshot.Child("comboscore").Value.ToString();

            ScoreManager.MyInstance.TextScore = int.Parse(txtscore);
            ScoreManager.MyInstance.ImageScore = int.Parse(imgscore);
            ScoreManager.MyInstance.ComboScore = int.Parse(comboscore);

            
        }
    }
}
