using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public static Manager Instance;
    private Keyboard keyboard = Keyboard.current;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ChangeScene();
    }

    void ChangeScene()
    {
        if (keyboard.rKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("Main");
        }
    }
}
