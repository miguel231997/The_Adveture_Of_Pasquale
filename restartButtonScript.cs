using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class restartButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void restartScene()
    {
        TimeLeftScript.timeLeft = 50f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
