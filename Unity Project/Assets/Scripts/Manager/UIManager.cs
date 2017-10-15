using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIManager : MonoBehaviour {
    public static UIManager Instance = null;
    private Text _Score;
    private Button _BtnLeft;
    private Button _BtnRight;
    private Button _BtnJump;
    private GameObject _RestartPage;
    private Button _BtnRestart;
    private Button _BtnQuit;
    private int _CurrentSocre = 0;
    private CharacterControl _PlayerController;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        _Score = transform.FindChild("Num").GetComponent<Text>();
        _BtnLeft = transform.FindChild("BtnLeft").GetComponent<Button>();
        _BtnRight = transform.FindChild("BtnRight").GetComponent<Button>();
        _BtnJump = transform.FindChild("BtnJump").GetComponent<Button>();
        _RestartPage = transform.FindChild("Restart").gameObject;
        _BtnRestart = transform.FindChild("Restart/BtnRestart").GetComponent<Button>();
        _BtnQuit = transform.FindChild("Restart/BtnQuit").GetComponent<Button>();

        _RestartPage.SetActive(false);
    }

    void Start()
    {
        _PlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterControl>();

        _BtnLeft.GetComponent<PressingListener>().PressingEvent += PlayerMoveLeft;
        _BtnRight.GetComponent<PressingListener>().PressingEvent += PlayerMoveRight;
        _BtnJump.GetComponent<PressingListener>().PointerDownEvent += PlayerJump;
        _BtnRestart.onClick.AddListener(Restart);
        _BtnQuit.onClick.AddListener(GameManager.Quit);
    }
    
	public void AddScore(int score)
    {
        _CurrentSocre += score;
        _Score.text = _CurrentSocre.ToString();
    }

    public void Restart()
    {
        GameManager.Restart();
    }

    public void ShowRestartPage()
    {
        _RestartPage.SetActive(true);
    }

    void PlayerMoveLeft()
    {
        _PlayerController.MoveLeft();
    }

    void PlayerMoveRight()
    {
        _PlayerController.MoveRight();
    }

    void PlayerJump()
    {
        _PlayerController.Jump();
    }
}
