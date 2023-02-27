using UnityEngine;

public class StartupScreenLayout : BaseLayout
{
    [Header("Scene specific")] [SerializeField] 
    private StartupAnimation startupAnimation;
    
    [SerializeField] private RectTransform vpSpelNaamRect;
    [SerializeField] private RectTransform vpSpellenHouderRect;
    [SerializeField] private RectTransform vp2048Rect;
    [SerializeField] private RectTransform vpMijnenvegerRect;
    [SerializeField] private RectTransform vpSolitaireRect;
    [SerializeField] private RectTransform vpSudokuRect;

    public override void SetLayout()
    {
        if (!startupAnimation.finished) return;
        SetAnimationLayout();
    }

    private void SetAnimationLayout()
    {
        vpSpellenHouderRect.localScale =
            Vector3.one * Mathf.Min(screenSafeAreaHeightInUnits / 350f, screenSafeAreaWidthInUnits / 350f);

        vp2048Rect.anchoredPosition = startupAnimation.endPosRot2048[0];
        vp2048Rect.localEulerAngles = startupAnimation.endPosRot2048[1];
        vp2048Rect.localScale = Vector3.one;

        vpMijnenvegerRect.anchoredPosition = startupAnimation.endPosRotMijnenveger[0];
        vpMijnenvegerRect.localEulerAngles = startupAnimation.endPosRotMijnenveger[1];
        vpMijnenvegerRect.localScale = Vector3.one;

        vpSolitaireRect.anchoredPosition = startupAnimation.endPosRotSolitaire[0];
        vpSolitaireRect.localEulerAngles = startupAnimation.endPosRotSolitaire[1];
        vpSolitaireRect.localScale = Vector3.one;

        vpSudokuRect.anchoredPosition = startupAnimation.endPosRotSudoku[0];
        vpSudokuRect.localEulerAngles = startupAnimation.endPosRotSudoku[1];
        vpSudokuRect.localScale = Vector3.one;

        vpSpelNaamRect.anchoredPosition = new Vector3(0, (screenSafeAreaXInUnits / 2f), 0);
        vpSpelNaamRect.localEulerAngles = Vector3.zero;
        vpSpelNaamRect.sizeDelta = new Vector2(screenSafeAreaWidthInUnits * 0.75f, screenSafeAreaHeightInUnits / 2.05f);
    }
}