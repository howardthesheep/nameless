using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private Animator transition;
    [SerializeField] 
    private float transitionTime = 1f;
    private void Update()
    {
        // Change to see if player enters certain area
        if (Input.GetKeyDown(KeyCode.P))
        {
            loadCave();
        }
    }

    private void loadCave()
    {
        StartCoroutine(_loadCave(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator _loadCave(int levelIndex)
    {
        //Play Animation
        transition.SetTrigger("Start");
        //Wait
        yield return new WaitForSeconds(transitionTime);
        //Load Scene
        SceneManager.LoadScene(levelIndex);
    }
}
