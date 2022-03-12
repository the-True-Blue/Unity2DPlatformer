using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameControl : MonoBehaviour
{
    public static GameControl control;
    public List<GameObject> deadEnemies;
    public int livesCounter = 3;
    public TextMeshProUGUI livesText;
    public int hp = 100;
    public TextMeshProUGUI hpText;

    public bool gameWin = false;

    void Awake()
    {
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        livesText = GameObject.Find("lives").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (livesText != null) { livesText.text = livesCounter.ToString(); }
        else 
        {
            if (GameObject.Find("lives") != null)
            {
                livesText = GameObject.Find("lives").GetComponent<TextMeshProUGUI>();
            } 
        }

        if (hpText != null) { hpText.text = hp.ToString(); }
        else 
        {
            if(GameObject.Find("health") != null)
            {
                hpText = GameObject.Find("health").GetComponent<TextMeshProUGUI>();
            }
        }
        
        if (livesCounter == 0)
        {
            //load menu scene
            livesCounter = 3;
            gameWin = false;
            deadEnemies.Clear();
            SceneManager.LoadScene("Finish Screen");
        }

        if (hp == 0)
        {
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if(livesCounter == 1) {
                    livesCounter -= 1;
                }
                else {
                    livesCounter -= 1;
                    player.transform.position = player.GetComponent<SpeedPlayerController>().startPos;
                }
                hp = 100;
            }
            
        }
    }
}
