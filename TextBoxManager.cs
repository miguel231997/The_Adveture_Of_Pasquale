using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TextBoxManager : MonoBehaviour
{

    int currentScene;
    private static TextBoxManager _instance;
    public GameObject textBox;
    public Text theText;
    public TextAsset textFile;
    public string[] textLines;

    public int currentLine;
    public int endAtLine;

    public bool isActive;
    public bool stopPlayerMovement;
    public PlayerControllerScript player;
    public GhostControllerScript ghostPlayer;
    public BossScript boss;

    public static TextBoxManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<TextBoxManager>();
            }

            return _instance;
        }
    }

    void Awake()
    {
        //if this doesn't exist yet...
        if (_instance == null)  //will only happen once!
        {
            //set instance to this
            _instance = this;
        }
        //If instance already exists and it's not this:
        else if (_instance != null && _instance != this)
        {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

    }

    // Start is called before the first frame update
    // This applies to title screen
    void Start()
    {
        boss = FindObjectOfType<BossScript>();
        player = FindObjectOfType<PlayerControllerScript>();
        ghostPlayer = FindObjectOfType<GhostControllerScript>();

        if(textFile != null)
        {
            textLines = (textFile.text.Split('\n'));
        }

        if(endAtLine == 0)
        {
            endAtLine = textLines.Length - 1;
        }

        if (isActive)
        {
            EnableTextBox();
        }
        else
        {
            DisableTextBox();
        }
    }



    // Update is called once per frame
    void Update()

    {

        boss = FindObjectOfType<BossScript>();
        player = FindObjectOfType<PlayerControllerScript>();
        ghostPlayer = FindObjectOfType<GhostControllerScript>();

        if (!isActive)
        {
            return;
        }

        theText.text = textLines[currentLine];

        if((Input.GetKeyDown(KeyCode.Return) )|| (Input.GetKeyDown(KeyCode.Space)))
        {
            currentLine += 1;
        }

        if (currentLine > endAtLine)
        {
            DisableTextBox();
        }

    }

    public void EnableTextBox()
    {
        textBox.SetActive(true);
        isActive = true;
        if(player!=null)
            player.canMove = false;
        if (ghostPlayer != null)
            ghostPlayer.allowMoving = false;

        
    }

    public void DisableTextBox()
    {
        textBox.SetActive(false);
        if (player != null)
            player.canMove = true;
        if (ghostPlayer != null)
            ghostPlayer.allowMoving = true;
        isActive = false;
 

    }

   public void ReloadScript(TextAsset theText)
    {
        if(theText != null)
        {
            textLines = new string[1];
            textLines = (theText.text.Split('\n'));
            currentLine = 0;
            endAtLine = textLines.Length - 1;
        }
    }
    
}
