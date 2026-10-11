using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public static class UIKit
{
    public static Color Hex(string h) { ColorUtility.TryParseHtmlString(h, out var c); return c; }

    public static readonly Color Night = Hex("#0B1020");
    public static readonly Color Club = Hex("#150C1C");
    public static readonly Color Border = Hex("#3A1A3F");
    public static readonly Color Cyan = Hex("#5DE0E6");
    public static readonly Color Purple = Hex("#B57BFF");
    public static readonly Color Red = Hex("#FF4D6D");
    public static readonly Color Yellow = Hex("#FFD166");
    public static readonly Color Muted = Hex("#9AA4C7");
    public static readonly Color Paper = Hex("#E8DFC4");
    public static readonly Color PaperDark = Hex("#8A7A52");
    public static readonly Color Ink = Hex("#2A2418");

    // Assigne ta police pixel ici (optionnel). Sinon la police TMP par defaut est utilisee.
    public static TMP_FontAsset Font;

    public static Canvas CreateCanvas(string name, int sortingOrder = 0)
    {
        var go = new GameObject(name, typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        var canvas = go.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = sortingOrder;

        var scaler = go.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0.5f;

        EnsureEventSystem();
        return canvas;
    }

    static void EnsureEventSystem()
    {
        if (UnityEngine.Object.FindFirstObjectByType<EventSystem>() != null) return;
        var es = new GameObject("EventSystem", typeof(EventSystem));
#if ENABLE_INPUT_SYSTEM
        es.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
#else
        es.AddComponent<StandaloneInputModule>();
#endif
    }

    public static RectTransform Panel(Transform parent, string name, Color fill, Color? outline = null)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(Image));
        go.transform.SetParent(parent, false);
        go.GetComponent<Image>().color = fill;
        if (outline.HasValue)
        {
            var o = go.AddComponent<Outline>();
            o.effectColor = outline.Value;
            o.effectDistance = new Vector2(4, -4);
        }
        return go.GetComponent<RectTransform>();
    }

    public static void Stretch(RectTransform rt)
    {
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    // Place un element par rapport au centre de son parent.
    public static void Place(RectTransform rt, float x, float y, float w, float h)
    {
        rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = new Vector2(w, h);
        rt.anchoredPosition = new Vector2(x, y);
    }

    public static TMP_Text Label(Transform parent, string text, float size, Color color,
        TextAlignmentOptions align = TextAlignmentOptions.Center, FontStyles style = FontStyles.Normal)
    {
        var go = new GameObject("Label", typeof(RectTransform));
        go.transform.SetParent(parent, false);
        var t = go.AddComponent<TextMeshProUGUI>();
        if (Font != null) t.font = Font;
        t.text = text;
        t.fontSize = size;
        t.color = color;
        t.alignment = align;
        t.fontStyle = style;
        t.raycastTarget = false;
        return t;
    }

    public static Button MakeButton(Transform parent, string label, bool primary, Color textColor, Action onClick)
    {
        var rt = Panel(parent, label + " Button", primary ? Cyan : Club, primary ? Night : Border);
        var btn = rt.gameObject.AddComponent<Button>();

        var colors = btn.colors;
        colors.normalColor = new Color(0.9f, 0.9f, 0.9f);
        colors.highlightedColor = Color.white;
        colors.pressedColor = new Color(0.7f, 0.7f, 0.7f);
        colors.selectedColor = new Color(0.9f, 0.9f, 0.9f);
        btn.colors = colors;
        btn.onClick.AddListener(() => onClick());

        var le = rt.gameObject.AddComponent<LayoutElement>();
        le.preferredHeight = 90;

        var t = Label(rt, label, 40, textColor);
        Stretch(t.rectTransform);
        return btn;
    }

    public static Slider MakeSlider(Transform parent, float value, Action<float> onChange)
    {
        var root = new GameObject("Slider", typeof(RectTransform), typeof(Slider));
        root.transform.SetParent(parent, false);

        var bg = Panel(root.transform, "Background", Border);
        bg.anchorMin = new Vector2(0, 0.3f);
        bg.anchorMax = new Vector2(1, 0.7f);
        bg.offsetMin = bg.offsetMax = Vector2.zero;

        var fillArea = new GameObject("Fill Area", typeof(RectTransform)).GetComponent<RectTransform>();
        fillArea.SetParent(root.transform, false);
        fillArea.anchorMin = new Vector2(0, 0.3f);
        fillArea.anchorMax = new Vector2(1, 0.7f);
        fillArea.offsetMin = fillArea.offsetMax = Vector2.zero;

        var fill = Panel(fillArea, "Fill", Cyan);
        fill.anchorMin = Vector2.zero;
        fill.anchorMax = Vector2.one;
        fill.offsetMin = fill.offsetMax = Vector2.zero;

        var handleArea = new GameObject("Handle Slide Area", typeof(RectTransform)).GetComponent<RectTransform>();
        handleArea.SetParent(root.transform, false);
        Stretch(handleArea);
        handleArea.offsetMin = new Vector2(15, 0);
        handleArea.offsetMax = new Vector2(-15, 0);

        var handle = Panel(handleArea, "Handle", Yellow, Night);
        handle.anchorMin = Vector2.zero;
        handle.anchorMax = new Vector2(0, 1);
        handle.sizeDelta = new Vector2(30, 0);

        var slider = root.GetComponent<Slider>();
        slider.fillRect = fill;
        slider.handleRect = handle;
        slider.targetGraphic = handle.GetComponent<Image>();
        slider.direction = Slider.Direction.LeftToRight;
        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.value = value;
        slider.onValueChanged.AddListener(v => onChange(v));
        return slider;
    }
}
