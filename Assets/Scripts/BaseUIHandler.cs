using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-49)]
public abstract class BaseUIHandler : MonoBehaviour
{
    protected GegevensHouder gegevensHouder;
    protected SaveScript saveScript;
    protected BaseLayout baseLayout;

    [Header("Menu UI")]
    [SerializeField] protected GameObject menuUIObj;
    [SerializeField] protected Transform menuUITransform;
    [SerializeField] protected RectTransform menuUIRect;
    [SerializeField] protected Transform showMenuButtonTransform;
    [SerializeField] protected RectTransform showMenuButtonRect;
    [SerializeField] protected RectTransform backToMenuButtonRect;
    [SerializeField] protected RectTransform menuNewGameButtonRect;
    [SerializeField] protected RectTransform menuNewGameOptionRect;

    [Header("Canvases")]
    [SerializeField] protected GameObject helpUICanvasObj;
    [SerializeField] protected GameObject generalCanvasObj;
    [SerializeField] protected GameObject menuCanvasObj;
    [SerializeField] protected GameObject gameSpecificRootObj;
    [SerializeField] protected GameObject settingsCanvasObj;
    [SerializeField] protected GameObject finishedGameUIObj;

    protected virtual void Start()
    {
        saveScript = SaveScript.Instance;
        if (saveScript == null) return;
        gegevensHouder = GegevensHouder.Instance;
        baseLayout = BaseLayout.Instance;
    }

    protected abstract void SetLayout();

    public void BackToMenu()
    {
        SceneManager.LoadScene("SpellenOverzicht");
    }

    public void OpenMenu()
    {
        bool vertical = baseLayout.screenSafeAreaWidth < baseLayout.screenSafeAreaHeight;
        bool appear;
        if (showMenuButtonTransform.localEulerAngles == Vector3.zero || showMenuButtonTransform.localEulerAngles == new Vector3(0, 0, 270))
        {
            showMenuButtonTransform.localEulerAngles += new Vector3(0, 0, 180);
            appear = false;
        }
        else
        {
            showMenuButtonTransform.localEulerAngles -= new Vector3(0, 0, 180);
            appear = true;
        }
        if (appear)
        {
            menuUIObj.SetActive(true);
            float schaal = Mathf.Min(Mathf.Min(baseLayout.screenSafeAreaHeight, baseLayout.screenSafeAreaWidth) / 1080f, Mathf.Max(baseLayout.screenSafeAreaHeight, baseLayout.screenSafeAreaWidth) / 2520f);
            if (vertical)
            {
                schaal *= 1.1f;
                menuUIRect.sizeDelta = new Vector2(baseLayout.screenWidth, baseLayout.screenSafeAreaY + baseLayout.screenSafeAreaHeight * 0.15f);
                menuUIRect.anchoredPosition = new Vector2(0, -menuUIRect.sizeDelta.y / 2f + baseLayout.screenSafeAreaY);
                float xNewGameButton = 0;
                if (menuNewGameOptionRect != null)
                {
                    xNewGameButton = menuUIRect.sizeDelta.x / 4f;
                    menuNewGameOptionRect.anchoredPosition = new Vector2(-menuUIRect.sizeDelta.x / 4f, baseLayout.screenSafeAreaY / 2f);
                }
                menuNewGameButtonRect.anchoredPosition = new Vector2(xNewGameButton, baseLayout.screenSafeAreaY / 2f);
            }
            else
            {
                menuUIRect.sizeDelta = new Vector2(baseLayout.screenSafeAreaX + baseLayout.screenSafeAreaWidth * 0.2f, baseLayout.screenHeight);
                menuUIRect.anchoredPosition = new Vector2(baseLayout.screenSafeAreaX - menuUIRect.sizeDelta.x / 2f - (baseLayout.screenWidth / 2f), baseLayout.screenHeight / 2f);
                backToMenuButtonRect.transform.SetParent(menuUITransform);
                float size = Mathf.Min(baseLayout.screenSafeAreaWidth / 12f, baseLayout.screenSafeAreaHeight / 12f);
                backToMenuButtonRect.sizeDelta = Vector2.one * size;
                backToMenuButtonRect.anchoredPosition = new Vector2(baseLayout.screenSafeAreaX - menuUIRect.sizeDelta.x / 2f + size / 2f, -baseLayout.screenSafeAreaY / 2f + baseLayout.screenHeight / 2f - size / 2f);
                float yNewGameButton = 0;
                if (menuNewGameOptionRect != null)
                {
                    yNewGameButton = -baseLayout.screenSafeAreaHeight / 8f;
                    menuNewGameOptionRect.anchoredPosition = new Vector2(baseLayout.screenSafeAreaX / 2f, baseLayout.screenSafeAreaHeight / 8f);
                }
                menuNewGameButtonRect.anchoredPosition = new Vector2(baseLayout.screenSafeAreaX / 2f, yNewGameButton);
            }
            menuNewGameButtonRect.localScale = new Vector3(schaal, schaal, 1);
            if (menuNewGameOptionRect != null)
                menuNewGameOptionRect.localScale = new Vector3(schaal, schaal, 1);
        }
        StartCoroutine(ShowMenu(appear, vertical));
    }

    private IEnumerator ShowMenu(bool appear, bool vertical)
    {
        float speed = 50f;
        menuUIObj.SetActive(true);
        if (vertical)
        {
            if (appear)
            {
                showMenuButtonTransform.Translate(Vector3.up * speed);
                menuUITransform.Translate(Vector3.up * speed);
                if (menuUIRect.anchoredPosition.y > menuUIRect.sizeDelta.y / 2f)
                {
                    showMenuButtonRect.anchoredPosition = new Vector2(0, menuUIRect.sizeDelta.y + showMenuButtonRect.sizeDelta.y / 2f);
                    menuUIRect.anchoredPosition = new Vector2(0, menuUIRect.sizeDelta.y / 2f);
                    StopAllCoroutines();
                }
            }
            else
            {
                showMenuButtonTransform.Translate(1.5f * speed * Vector3.up);
                menuUITransform.Translate(1.5f * speed * Vector3.down);
                if (menuUIRect.anchoredPosition.y < -menuUIRect.sizeDelta.y / 2f + Screen.safeArea.y)
                {
                    showMenuButtonRect.anchoredPosition = new Vector2(0, Screen.safeArea.y + showMenuButtonRect.sizeDelta.y / 2f);
                    menuUIObj.SetActive(false);
                    StopAllCoroutines();
                }
            }
        }
        else
        {
            if (appear)
            {
                showMenuButtonTransform.Translate(Vector3.up * speed);
                menuUITransform.Translate(Vector3.right * speed);
                if (menuUIRect.anchoredPosition.x > menuUIRect.sizeDelta.x / 2f - (Screen.width / 2))
                {
                    showMenuButtonRect.anchoredPosition = new Vector2(menuUIRect.sizeDelta.x - (Screen.width / 2f) + showMenuButtonRect.sizeDelta.y / 2f, Screen.height / 2f);
                    menuUIRect.anchoredPosition = new Vector2((menuUIRect.sizeDelta.x / 2f) - (Screen.width / 2f), Screen.height / 2f);
                    StopAllCoroutines();
                }
            }
            else
            {
                showMenuButtonTransform.Translate(1.5f * speed * Vector3.up);
                menuUITransform.Translate(1.5f * speed * Vector3.left);
                if (menuUIRect.anchoredPosition.x < -menuUIRect.sizeDelta.x / 2f - (Screen.width / 2) + Screen.safeArea.x)
                {
                    showMenuButtonRect.anchoredPosition = new Vector2(Screen.safeArea.x - (Screen.width / 2f) + showMenuButtonRect.sizeDelta.y / 2f, Screen.height / 2f);
                    menuUIObj.SetActive(false);
                    StopAllCoroutines();
                }
            }
        }
        yield return gegevensHouder.wachtHonderdste;
        StartCoroutine(ShowMenu(appear, vertical));
    }

    public virtual void OpenHelpUI()
    {
        bool helpUIActive = helpUICanvasObj.activeSelf;
        helpUICanvasObj.SetActive(!helpUIActive);
        gameSpecificRootObj.SetActive(helpUIActive);
        generalCanvasObj.SetActive(helpUIActive);
        menuCanvasObj.SetActive(helpUIActive);
        SetLayout();
    }

    public virtual void OpenSettings()
    {
        bool settingObjActive = settingsCanvasObj.activeSelf;
        settingsCanvasObj.SetActive(!settingObjActive);
        gameSpecificRootObj.SetActive(settingObjActive);
        generalCanvasObj.SetActive(settingObjActive);
        menuCanvasObj.SetActive(settingObjActive);
        SetLayout();
    }
}