using Liminal.Core.Fader;
using Liminal.SDK.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour
{
    public GameObject EndUI;
    public GameObject Game;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<SplineWalker>())
        {
            StartCoroutine(FadeToBlackRoutine());
        }
    }
    private IEnumerator FadeToBlackRoutine()
    {
        ScreenFader.Instance.FadeTo(Color.black, duration: 1);
        yield return ScreenFader.Instance.WaitUntilFadeComplete();
        Game.SetActive(false);
        ScreenFader.Instance.FadeToClear(duration: 1);
        EndUI.SetActive(true);
    }

    public void EndGame()
    {
        ExperienceApp.End();
    }
}
