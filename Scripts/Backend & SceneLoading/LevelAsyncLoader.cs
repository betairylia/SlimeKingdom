using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelAsyncLoader : MonoBehaviour
{
    public Text text;

	// Use this for initialization
	void Start ()
    {
        text.text = GameDataKeeper.GetSingleton().targetSceneName + ".unity";
        StartCoroutine(LoadScene());
	}

    IEnumerator LoadScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(GameDataKeeper.GetSingleton().targetSceneName);
        yield return async;
    }
}
