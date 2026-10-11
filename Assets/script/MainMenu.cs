using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("Config")]
    public string gameScene = "Game";
    public TMP_FontAsset font;          // optionnel : ta police pixel
    public string version = "v0.1";
    public string jamName = "Game jam: Uninvited";

    GameObject howToOverlay;
    GameObject settingsOverlay;

    void Start()
    {
        Time.timeScale = 1f;
        AudioListener.volume = PlayerPrefs.GetFloat("volume", 1f);
        if (font != null) UIKit.Font = font;
        Build();
    }

    void Build()
    {
        var canvas = UIKit.CreateCanvas("MainMenuCanvas");

        var bg = UIKit.Panel(canvas.transform, "Background", UIKit.Night);
        UIKit.Stretch(bg);

        var card = UIKit.Panel(bg, "Card", UIKit.Club, UIKit.Border);
        UIKit.Place(card, 0, 0, 1100, 860);

        // Titre dans un cadre cyan
        var frame = UIKit.Panel(card, "TitleFrame", UIKit.Club, UIKit.Cyan);
        UIKit.Place(frame, 0, 280, 760, 240);
        var l1 = UIKit.Label(frame, "NOT ON", 90, UIKit.Cyan, TextAlignmentOptions.Center, FontStyles.Bold);
        l1.characterSpacing = 8;
        UIKit.Place(l1.rectTransform, 0, 55, 740, 110);
        var l2 = UIKit.Label(frame, "THE LIST", 90, UIKit.Purple, TextAlignmentOptions.Center, FontStyles.Bold);
        l2.characterSpacing = 8;
        UIKit.Place(l2.rectTransform, 0, -55, 740, 110);

        var tag = UIKit.Label(card, "Tonight, you run the door.", 34, UIKit.Muted);
        UIKit.Place(tag.rectTransform, 0, 125, 900, 50);

        // Boutons
        var list = new GameObject("Buttons", typeof(RectTransform), typeof(VerticalLayoutGroup));
        list.transform.SetParent(card, false);
        UIKit.Place((RectTransform)list.transform, 0, -130, 480, 420);
        var v = list.GetComponent<VerticalLayoutGroup>();
        v.spacing = 20;
        v.childControlWidth = true;
        v.childControlHeight = true;
        v.childForceExpandWidth = true;
        v.childForceExpandHeight = false;

        UIKit.MakeButton(list.transform, "PLAY", true, UIKit.Night, Play);
        UIKit.MakeButton(list.transform, "HOW TO PLAY", false, UIKit.Muted, () => howToOverlay.SetActive(true));
        UIKit.MakeButton(list.transform, "SETTINGS", false, UIKit.Muted, () => settingsOverlay.SetActive(true));
#if !UNITY_WEBGL
        UIKit.MakeButton(list.transform, "QUIT", false, UIKit.Red, Quit);
#endif

        // Poteaux de file d'attente
        Pole(card, -330, -150);
        Pole(card, 330, -150);

        // Pied de page
        var best = UIKit.Label(card, "BEST: " + PlayerPrefs.GetInt("best", 0).ToString("N0"), 26, UIKit.Muted, TextAlignmentOptions.Left);
        UIKit.Place(best.rectTransform, -380, -390, 280, 40);
        var jam = UIKit.Label(card, jamName, 26, UIKit.Muted);
        UIKit.Place(jam.rectTransform, 0, -390, 500, 40);
        var ver = UIKit.Label(card, version, 26, UIKit.Muted, TextAlignmentOptions.Right);
        UIKit.Place(ver.rectTransform, 380, -390, 280, 40);

        BuildHowToPlay(bg);
        BuildSettings(bg);
    }

    void Pole(Transform parent, float x, float y)
    {
        var stick = UIKit.Panel(parent, "PoleStick", UIKit.PaperDark);
        UIKit.Place(stick, x, y, 10, 130);
        var ball = UIKit.Panel(parent, "PoleBall", UIKit.Yellow);
        UIKit.Place(ball, x, y + 75, 30, 30);
        var foot = UIKit.Panel(parent, "PoleFoot", UIKit.PaperDark);
        UIKit.Place(foot, x, y - 70, 60, 14);
    }

    GameObject Overlay(Transform parent, string title, out RectTransform card)
    {
        var dim = UIKit.Panel(parent, title + " Overlay", new Color(0.04f, 0.06f, 0.12f, 0.94f));
        UIKit.Stretch(dim);

        card = UIKit.Panel(dim, "Card", UIKit.Club, UIKit.Cyan);
        UIKit.Place(card, 0, 0, 1000, 640);

        var h = UIKit.Label(card, title, 64, UIKit.Cyan, TextAlignmentOptions.Center, FontStyles.Bold);
        UIKit.Place(h.rectTransform, 0, 250, 900, 90);

        var close = UIKit.MakeButton(card, "CLOSE", true, UIKit.Night, () => dim.gameObject.SetActive(false));
        UIKit.Place((RectTransform)close.transform, 0, -255, 320, 80);

        dim.gameObject.SetActive(false);
        return dim.gameObject;
    }

    void BuildHowToPlay(Transform parent)
    {
        howToOverlay = Overlay(parent, "HOW TO PLAY", out var card);
        var body = UIKit.Label(card,
            "1. Check the guest list. If a name isn't on it, turn them away.\n\n" +
            "2. Kick out the drunk guests inside the club.\n\n" +
            "3. Close the windows before intruders climb in.\n\n" +
            "<color=#FF4D6D>Let in too many intruders and you're fired.</color>",
            34, UIKit.Muted, TextAlignmentOptions.TopLeft);
        UIKit.Place(body.rectTransform, 0, 15, 860, 380);
    }

    void BuildSettings(Transform parent)
    {
        settingsOverlay = Overlay(parent, "SETTINGS", out var card);

        var lbl = UIKit.Label(card, "Music volume", 36, UIKit.Muted, TextAlignmentOptions.Left);
        UIKit.Place(lbl.rectTransform, -50, 70, 700, 50);

        var slider = UIKit.MakeSlider(card, AudioListener.volume, val =>
        {
            AudioListener.volume = val;
            PlayerPrefs.SetFloat("volume", val);
        });
        UIKit.Place((RectTransform)slider.transform, 0, -10, 700, 60);
    }

    public void Play()
    {
        SceneManager.LoadScene(gameScene);
    }

    public void Quit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
