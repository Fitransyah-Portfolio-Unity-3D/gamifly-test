using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Gameplay")]
    [Space(10)]
    [SerializeField] TutorialManager tutorialManager;
    [SerializeField] Batter batter;
    [SerializeField] Pitcher pitcher;
    [SerializeField] TutorialFiveManager tutorialFive;
    [SerializeField] TMP_Text loadingScreenText;
    [SerializeField] Image ballsUiPanel;
    [SerializeField] Image[] ballsUI;
    [SerializeField] Button hitButton;
    [SerializeField] Button throwButton;
    [SerializeField] Button targetButton;

    [SerializeField] int ballUiIndex;

    [Space(30)]
    [Header("Tutorial Text")]
    [Space(10)]
    [SerializeField] CanvasGroup tutorialOnetext;
    [SerializeField] CanvasGroup tutorialTwotext;
    [SerializeField] CanvasGroup tutorialThreetext;
    [SerializeField] CanvasGroup tutorialFourtext;
    [SerializeField] CanvasGroup tutorialFivetext;

    private void Start()
    {
        tutorialManager.OnTutorialStart += ToggleLoadingScreenText;
        tutorialManager.OnLoadingScreenComplete += ToggleLoadingScreenText;
        tutorialManager.OnTutorialOneComplete += EnableBallUi;
        tutorialManager.OntutorialTwoComplete += DisableBallUi;
        tutorialManager.OnTutorialUpdateFromTutorialManager += UpdateButton;
        tutorialManager.OnTutorialFiveStarted += EnableBallUi;
        tutorialManager.OnTutorialFiveComplete += DisableBallUi;

        tutorialManager.OnTutorialUpdateFromTutorialManager += SetTutorialText;

        batter.OnTutorialTwoCount += RemoveBallUiImage;
        tutorialFive.OnTutorialUpdateFromTutorialFive += SetButtonInteractionTutFive;
        pitcher.OnTutorialFivecount += RemoveBallUiImage;
    }

    private void OnDestroy()
    {
        tutorialManager.OnTutorialStart -= ToggleLoadingScreenText;
        tutorialManager.OnLoadingScreenComplete -= ToggleLoadingScreenText;
        tutorialManager.OnTutorialOneComplete -= EnableBallUi;
        tutorialManager.OntutorialTwoComplete -= DisableBallUi;
        tutorialManager.OnTutorialUpdateFromTutorialManager -= UpdateButton;
        tutorialManager.OnTutorialFiveStarted -= EnableBallUi;
        tutorialManager.OnTutorialFiveComplete -= DisableBallUi;

        tutorialManager.OnTutorialUpdateFromTutorialManager += SetTutorialText;

        batter.OnTutorialTwoCount -= RemoveBallUiImage;
        tutorialFive.OnTutorialUpdateFromTutorialFive -= SetButtonInteractionTutFive;
        pitcher.OnTutorialFivecount -= RemoveBallUiImage;
    }
    private void ToggleLoadingScreenText()
    {
        if (loadingScreenText.gameObject.activeSelf)
        {
            loadingScreenText.gameObject.SetActive(false);
        }
        else
        {
            loadingScreenText.gameObject.SetActive(true);
        }
    }

    private void EnableBallUi()
    {
        if (!ballsUiPanel.gameObject.activeSelf) ballsUiPanel.gameObject.SetActive(true);
    }
    private void DisableBallUi()
    {
        ResetBallUiImage();
        if (ballsUiPanel.gameObject.activeSelf) ballsUiPanel.gameObject.SetActive(false);
    }
    private void RemoveBallUiImage()
    {
        ballsUI[ballUiIndex].sprite = null;
        ballsUI[ballUiIndex].color = new Color(0,0,0,0);
        ballUiIndex++;
    }

    private void ResetBallUiImage()
    {
        // reset ball UI
        if (ballUiIndex > 2)
        {
            ballsUiPanel.gameObject.SetActive(false);
            ballUiIndex = 0;
            foreach (Image ball in ballsUI)
            {
                ball.sprite = Resources.Load<Sprite>("baseballimg");
                ball.color = Color.white;
            }
        }
    }

    private void UpdateButton(Tutorial ongoingTutorial)
    {
        SetButtonInteraction(ongoingTutorial);
    }

    public void SetButtonInteraction(Tutorial ongoingTutorial)
    {
        switch (ongoingTutorial)
        {
            case Tutorial.LoadingScreen:
                // TO DO
                hitButton.interactable = false;
                throwButton.interactable = false;
                targetButton.interactable = false;
                break; 
            case Tutorial.TutorialOne:
                // TO DO
                hitButton.interactable = true; 
                throwButton.interactable = false;
                targetButton.interactable = false;
                break;
            case Tutorial.TutorialTwo:
                // TO DO
                hitButton.interactable = true;
                throwButton.interactable = false;
                targetButton.interactable = false;
                break;
            case Tutorial.TutorialThree:
                // TO DO
                hitButton.interactable = false;
                throwButton.interactable = false;
                targetButton.interactable = true;
                break;
            case Tutorial.TutorialFour:
                // TO DO
                hitButton.interactable = false;
                throwButton.interactable = true;
                targetButton.interactable = false;
                break;
            case Tutorial.TutorialEnd:
                // TO DO
                hitButton.interactable = false;
                throwButton.interactable = false;
                targetButton.interactable = false;
                break;
        }
    }

    public void SetButtonInteractionTutFive(Pitcher.PitcherState ongoingPitcherState)
    {
        switch (ongoingPitcherState)
        {
            case Pitcher.PitcherState.Targeting:
                hitButton.interactable = false;
                throwButton.interactable = false;
                targetButton.interactable = true;
                break;
            case Pitcher.PitcherState.Preparing:
                hitButton.interactable = false;
                throwButton.interactable = true;
                targetButton.interactable = false;
                break;
            case Pitcher.PitcherState.Throwing:
                hitButton.interactable = false;
                throwButton.interactable = false;
                targetButton.interactable = false;
                break;
        }
    }

    public void SetTutorialText(Tutorial ongoingTutorial)
    {
        switch (ongoingTutorial)
        {

            case Tutorial.TutorialOne:
                // TO DO
                tutorialOnetext.alpha = 1f;
                tutorialTwotext.alpha = 0;
                tutorialThreetext.alpha = 0;
                tutorialFourtext.alpha = 0;
                tutorialFivetext.alpha = 0;
                break;
            case Tutorial.TutorialTwo:
                // TO DO
                tutorialOnetext.alpha = 0;
                tutorialTwotext.alpha = 1f;
                tutorialThreetext.alpha = 0;
                tutorialFourtext.alpha = 0;
                tutorialFivetext.alpha = 0;
                break;
            case Tutorial.TutorialThree:
                // TO DO
                tutorialOnetext.alpha = 0;
                tutorialTwotext.alpha = 0;
                tutorialThreetext.alpha = 1f;
                tutorialFourtext.alpha = 0;
                tutorialFivetext.alpha = 0;
                break;
            case Tutorial.TutorialFour:
                // TO DO
                tutorialOnetext.alpha = 0;
                tutorialTwotext.alpha = 0;
                tutorialThreetext.alpha = 0;
                tutorialFourtext.alpha = 1f;
                tutorialFivetext.alpha = 0;
                break;
            case Tutorial.TutorialFive:
                // TO DO
                tutorialOnetext.alpha = 0;
                tutorialTwotext.alpha = 0;
                tutorialThreetext.alpha = 0;
                tutorialFourtext.alpha = 0;
                tutorialFivetext.alpha = 1f;
                break;
            case Tutorial.TutorialEnd:
                // TO DO

                break;
        }
    }

}
