using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public string levelToLoad = "Level2";

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            if (levelToLoad == "Finish Screen")
            {
                GameControl.control.gameWin = true;
                SceneManager.LoadScene(levelToLoad);
            }
            else 
            {
                GameControl.control.deadEnemies.Clear();
                SceneManager.LoadScene(levelToLoad); 
            }
            
        }
    }
    
}
