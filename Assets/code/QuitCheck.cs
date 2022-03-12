using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class QuitCheck : MonoBehaviour
{    
    private void Awake(){
        if(FindObjectsOfType<QuitCheck>().Length>1){
            Destroy(gameObject);
        }
        else{
            DontDestroyOnLoad(gameObject);
        }
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            GameControl.control.deadEnemies.Clear();
            GameControl.control.livesCounter = 3;
            GameControl.control.hp = 100;
            SceneManager.LoadScene("Title");
        }
    }
}
