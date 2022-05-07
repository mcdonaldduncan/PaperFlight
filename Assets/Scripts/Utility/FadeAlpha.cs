using Liminal.Core.Fader;
using System.Collections;
using UnityEngine;

public class FadeAlpha : MonoBehaviour
{
    private Timer timer;

    private bool hasFadedToBlack;
    private bool hasFadedToClear;
    private bool isWaitingToFade;

    private GameObject blackBox;

    [SerializeField] private GameObject player;

    void Start()
    {
        timer = GetComponent<Timer>();
        timer.OnTimerEnd += OnTimerEnd; // bind delegate
        timer.InitializeTimer(8, "FadeToBlack");
        timer.StartTimer();
       
        blackBox = transform.GetChild(0).gameObject;
    }

    // MoveTowards called in update, lerp called as coroutine
    void Update()
    {
        if (hasFadedToBlack && !ScreenFader.Instance.IsFading)
        {
            if (!isWaitingToFade)
            {
                timer.InitializeTimer(4, "FadeToClear");
                timer.StartTimer();
                blackBox.SetActive(false);
                isWaitingToFade = true;
            }
        }
    }

    private void OnTimerEnd()
    {
        if (timer.currentHandle.name.Equals("FadeToBlack"))
        {
            if (!hasFadedToBlack)
            {
                ScreenFader.Instance.FadeTo(Color.black, duration: 3);
                hasFadedToBlack = true;
            }
        }
        else if (timer.currentHandle.name.Equals("FadeToClear"))
        {
            if (!hasFadedToClear)
            {
                player.GetComponent<SplineWalker>().enabled = true;
                ScreenFader.Instance.FadeToClearFromBlack(duration: 5);
                hasFadedToClear = true;
            }
        }
    }
}
