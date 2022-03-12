using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectMenu : MonoBehaviour
{
    public void Level1(){
        SceneManager.LoadScene("Level1 Remake");
    }
     public void Level2(){
        SceneManager.LoadScene("Level2 Remake");
    }
     public void Level3(){
        SceneManager.LoadScene("Level3 Remake");
    }
     public void Level4(){
        SceneManager.LoadScene("Level4 Remake");
    }
     public void Level5(){
        SceneManager.LoadScene("Level5 Remake");
    }
    public void Back(){
        SceneManager.LoadScene("Title");
    }
    

}
