using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public enum GameMode{
    idle,
    playing,
    levelEnd
}
public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S;
    [Header("Inscribed")]
    public Text uitLevel;
    [SerializeField]
    public TextMeshProUGUI uitShots;
    public Vector3 castlePos;
    public GameObject[] castles;
    public GameObject SealImage;
    public FollowCam followCam; 
    private int shotsRemaining=3; 
    private bool transition =false;

    [Header("Dynamic")]
    public int level;
    public int levelMax;
    public int shotsTaken;
    public GameObject castle;
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot";
    // Start is called before the first frame update
    void Start()
    {
        S = this;
        level = 0;
        shotsTaken = 0;
        levelMax = castles.Length;
        StartLevel();
    }
    void StartLevel()
    {
        if(castle != null)
        {
            Destroy(castle);   
        }
        shotsTaken=0;
        Projectile.DESTROY_PROJECTILES();
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        Goal.goalMet = false;
        UpdateGUI();
        mode = GameMode.playing;
        FollowCam.SWITCH_VIEW(FollowCam.eView.both);
    }

    void UpdateGUI()
    {
        uitLevel.text = "Level: "+(level+1)+" of " + levelMax;
        uitShots.text = "Shots Left: "+ (shotsRemaining-shotsTaken);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGUI();

        if((mode == GameMode.playing) && Goal.goalMet == true)
        {
            mode = GameMode.levelEnd;
            FollowCam.SWITCH_VIEW(FollowCam.eView.both);
            Invoke("SealProtocall", 0.1f);
            Invoke("NextLevel", 1f);
        }
        if(shotsTaken>=(shotsRemaining+1))
        {
            //restart level
            shotsTaken=0;  
            StartLevel();
        }
        
    }

    void NextLevel()
    {
        level++;

        if(level == levelMax)
        {
            LoadMainMenu();
        }
        StartLevel();
    }

    void SealProtocall()
    {
        followCam.SwitchView(FollowCam.eView.castle);
        Invoke("SealDone", 3f);
    }

    void SealDone()
    {
        followCam.SwitchView(FollowCam.eView.slingshot);
    }
    static public void SHOT_FIRED()
    {
        S.shotsTaken++;
    }
    static public GameObject GET_CASTLE()
    {
        return S.castle;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("_MainMenu");
    }
}
