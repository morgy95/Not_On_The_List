using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LosePage : MonoBehaviour
{
    public static string MenuScene = "Menu";

    RectTransform title;
    Vector2 titleBase;
    float startTime;

    public static void Show(int moneyWon, int moneyLeft)
    {
        var go = new GameObject("LosePage");
        go.AddComponent<LosePage>().Build(moneyWon, moneyLeft);
    }

    void Build(int moneyWon, int moneyLeft)
    {
        // Meilleur "argent gagne" sauvegarde
        int best = PlayerPrefs.GetInt("bestMoney", 0);
        if (moneyWon > best)
        {
            best = moneyWon;
            PlayerPrefs.SetInt("bestMoney", best);
            PlayerPrefs.Save();
        }

        Time.timeScale = 0f;
        startTime = Time.unscaledTime;

        var canvas = UIKit.CreateCanvas("LoseCanvas", 100);
        canvas.transform.SetParent(transform, false);

        var bg = UIKit.Panel(canvas.transform, "Background", UIKit.Night);
        UIKit.Stretch(bg);

        var card = UIKit.Panel(bg, "Card", UIKit.Club, UIKit.Border);
        UIKit.Place(card, 0, 0, 1100, 760);

        // Titre + sous-titre
        var t = UIKit.Label(card, "YOU'RE FIRED", 100, UIKit.Red, TextAlignmentOptions.Center, FontStyles.Bold);
        t.characterSpacing = 8;
        title = t.rectTransform;
        UIKit.Place(title, 0, 270, 1000, 120);
        titleBase = title.anchoredPosition;

        var sub = UIKit.Label(card, "The boss found someone better at the door.", 30, UIKit.Muted);
        UIKit.Place(sub.rectTransform, 0, 185, 1000, 45);

        // Liste des invites, ton nom est barre
        var paper = UIKit.Panel(card, "GuestList", UIKit.Paper, UIKit.PaperDark);
        UIKit.Place(paper, -300, 0, 340, 300);
        paper.localRotation = Quaternion.Euler(0, 0, -2f);

        var head = UIKit.Label(paper, "GUEST LIST", 32, UIKit.Ink, TextAlignmentOptions.Center, FontStyles.Bold);
        UIKit.Place(head.rectTransform, 0, 105, 300, 45);
        PaperLine(paper, "Lea M.", UIKit.Ink, false, 45);
        PaperLine(paper, "Karim B.", UIKit.Ink, false, -5);
        PaperLine(paper, "You (bouncer)", UIKit.Red, true, -55);
        PaperLine(paper, "Sofia R.", UIKit.Ink, false, -105);

        // Argent gagne
        var winLabel = UIKit.Label(card, "MONEY WON", 32, UIKit.Muted, TextAlignmentOptions.Left);
        UIKit.Place(winLabel.rectTransform, 160, 60, 340, 45);
        var winValue = UIKit.Label(card, "$" + moneyWon.ToString("N0"), 64, UIKit.Cyan, TextAlignmentOptions.Left, FontStyles.Bold);
        UIKit.Place(winValue.rectTransform, 160, 5, 340, 80);

        // Argent restant (rouge si tu n'as plus rien)
        var leftLabel = UIKit.Label(card, "MONEY LEFT", 32, UIKit.Muted, TextAlignmentOptions.Left);
        UIKit.Place(leftLabel.rectTransform, 160, -75, 340, 45);
        var leftValue = UIKit.Label(card, "$" + moneyLeft.ToString("N0"), 64,
            moneyLeft > 0 ? UIKit.Yellow : UIKit.Red, TextAlignmentOptions.Left, FontStyles.Bold);
        UIKit.Place(leftValue.rectTransform, 160, -130, 340, 80);

        var bestLabel = UIKit.Label(card, "BEST: $" + best.ToString("N0"), 30, UIKit.Muted);
        UIKit.Place(bestLabel.rectTransform, 0, -195, 1000, 45);

        // Boutons
        var retry = UIKit.MakeButton(card, "RETRY", true, UIKit.Night, Retry);
        UIKit.Place((RectTransform)retry.transform, -170, -285, 290, 85);
        var menu = UIKit.MakeButton(card, "MENU", false, UIKit.Muted, Menu);
        UIKit.Place((RectTransform)menu.transform, 170, -285, 290, 85);
    }

    void PaperLine(Transform parent, string text, Color color, bool strike, float y)
    {
        var l = UIKit.Label(parent, text, 30, color, TextAlignmentOptions.Center,
            strike ? FontStyles.Strikethrough : FontStyles.Normal);
        UIKit.Place(l.rectTransform, 0, y, 300, 45);
    }

    // Petit tremblement du titre pendant 0,6 s (timeScale = 0, donc unscaledTime)
    void Update()
    {
        if (title == null) return;
        bool shaking = Time.unscaledTime - startTime < 0.6f;
        title.anchoredPosition = shaking ? titleBase + Random.insideUnitCircle * 6f : titleBase;
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(MenuScene);
    }
}
