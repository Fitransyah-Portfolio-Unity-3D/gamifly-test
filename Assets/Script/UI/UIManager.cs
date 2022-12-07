using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] TutorialManager tutorialManager;
    [SerializeField] Batter batter;
    [SerializeField] TMP_Text loadingScreenText;
    [SerializeField] Image ballsUiPanel;
    [SerializeField] Image[] ballsUI;

    [SerializeField] int ballUiIndex;

    private void Start()
    {
        tutorialManager.OnTutorialStart += ToggleLoadingScreenText;
        tutorialManager.OnLoadingScreenComplete += ToggleLoadingScreenText;
        tutorialManager.OnTutorialOneComplete += EnableBallUi;
        tutorialManager.OnTutorialTwoComplete += DisableBallUi;


        batter.OnTutorialTwoCount += RemoveBallUiImage;
    }

    private void OnDestroy()
    {
        tutorialManager.OnTutorialStart -= ToggleLoadingScreenText;
        tutorialManager.OnLoadingScreenComplete -= ToggleLoadingScreenText;
        tutorialManager.OnTutorialOneComplete -= EnableBallUi;
        tutorialManager.OnTutorialTwoComplete -= DisableBallUi;

        batter.OnTutorialTwoCount -= RemoveBallUiImage;
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
        if (ballsUiPanel.gameObject.activeSelf) ballsUiPanel.gameObject.SetActive(true);
    }
    private void RemoveBallUiImage()
    {
        ballsUI[ballUiIndex].sprite = null;
        ballsUI[ballUiIndex].color = new Color(0,0,0,0);
        ballUiIndex++;

        // reset ball UI
        if (ballUiIndex > 2)
        {
            ballsUiPanel.gameObject.SetActive(false);
            ballUiIndex = 0;
            foreach(Image ball in ballsUI)
            {
                ball.sprite = Resources.Load<Sprite>("baseballimg");
                ball.color = Color.white;
            }
        }
    }
}
