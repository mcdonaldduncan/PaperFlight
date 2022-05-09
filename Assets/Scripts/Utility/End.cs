using Liminal.Core.Fader;
using Liminal.SDK.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour
{
    [SerializeField] private GameObject EndUI;
    [SerializeField] private GameObject Game;
    [SerializeField] Transform aurora;
    [SerializeField] Transform target;
    [SerializeField] private bool FadeEnd;
    

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
        EndGame();

        //Game.SetActive(false);

        //if(!FadeEnd)
        //{
        //    ScreenFader.Instance.FadeToClear(duration: 1);
        //    EndUI.SetActive(true);
        //}
    }

    void RepositionAurora()
    {
        aurora.transform.position = new Vector3(target.position.x, aurora.transform.position.y, aurora.transform.position.z + 1000f);
    }

    public void EndGame()
    {
        Application.Quit();
        //ExperienceApp.End();
    }
}
