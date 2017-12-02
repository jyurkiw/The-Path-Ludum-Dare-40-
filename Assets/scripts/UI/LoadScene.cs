using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {
	public void ButtonHandler(string name)
    {
        Debug.Log(name);
        SceneManager.LoadScene(name, LoadSceneMode.Single);
    }
}
