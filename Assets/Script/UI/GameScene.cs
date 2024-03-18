using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    [SerializeField]
    private Transform overlayPanel;
    [SerializeField]
    private Transform winPanel;
    [SerializeField]
    private Transform losePanel;
    [SerializeField]
    private Button replayButton;
    [SerializeField]
    private Button homeButton;
    [SerializeField]
    private TMP_Text _coinLose;
    [SerializeField]
    private TMP_Text _coinWin;
    [SerializeField]
    private TMP_Text _coinText;

    public void UpdateCoin()
    {
        int lastCoin = int.Parse(_coinText.text);
        int coin = GameManager.instance.GetCoin();
        ShortcutExtensionsTMPText.DOCounter(_coinText, lastCoin, coin, .5f);
    }

    private void SetFinalCoin()
    {
        int coin = GameManager.instance.GetCoin();
        ShortcutExtensionsTMPText.DOCounter(_coinLose, 0, coin, .5f);
        ShortcutExtensionsTMPText.DOCounter(_coinWin, 0, coin, .5f);
        Debug.Log("SetFinalCoin" + coin);
    }

    public void PopupWinPanelGameScene()
    {
        overlayPanel.gameObject.SetActive(true);
        winPanel.gameObject.SetActive(true);
        FadePanelInScene(overlayPanel.GetComponent<CanvasGroup>(), winPanel.GetComponent<RectTransform>());
        homeButton.interactable = false;
        replayButton.interactable = false;
        SetFinalCoin();
    }

    public void PopupLosePanelGameScene()
    {
        overlayPanel.gameObject.SetActive(true);
        losePanel.gameObject.SetActive(true);
        FadePanelInScene(overlayPanel.GetComponent<CanvasGroup>(), losePanel.GetComponent<RectTransform>());
        homeButton.interactable = false;
        replayButton.interactable = false;
        SetFinalCoin();
    }

    private void FadePanelInScene(CanvasGroup canvasGroup, RectTransform rectTransform)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1, .3f).SetUpdate(true);

        rectTransform.localScale = Vector3.zero;
        rectTransform.DOScale(1, .3f).SetEase(Ease.OutBack).SetUpdate(true);
    }
    private void OnApplicationQuit()
    {
        DOTween.KillAll();
    }
}
