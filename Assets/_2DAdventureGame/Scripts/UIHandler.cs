using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIHandler : MonoBehaviour
{
    public static UIHandler instance { get; private set; }

    public float displayTime = 4.0f;
    private VisualElement m_NonPlayerDialogue;
    private float m_TimerDisplay;

    private VisualElement m_Healthbar;

    private VisualElement m_WinScreen;
    private VisualElement m_LoseScreen;

    private VisualElement m_Root;
    private List<Label> m_HealLabels = new List<Label>();
    private List<float> m_HealTimes = new List<float>();
    private float m_HealthGlowTimer;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        m_Root = uiDocument.rootVisualElement;
        m_Healthbar = uiDocument.rootVisualElement
            .Q<VisualElement>("HealthBar");
        SetHealthValue(1.0f);

        m_NonPlayerDialogue = uiDocument.rootVisualElement
            .Q<VisualElement>("NPCDialogue");
        m_NonPlayerDialogue.style.display = DisplayStyle.None;
        m_TimerDisplay = -1.0f;

        m_LoseScreen = uiDocument.rootVisualElement
            .Q<VisualElement>("LoseScreenContainer");
        m_WinScreen = uiDocument.rootVisualElement
            .Q<VisualElement>("WinScreenContainer");
    }

    public void SetHealthValue(float percentage)
    {
        m_Healthbar.style.width = Length.Percent(100 * percentage);
    }

    private void Update()
    {
        // «+N» піднімається на 80 px і тане за 1 с
        for (int i = m_HealLabels.Count - 1; i >= 0; i--)
        {
            m_HealTimes[i] += Time.deltaTime;
            float t = m_HealTimes[i];
            m_HealLabels[i].style.translate = new Translate(0, -80 * t);
            m_HealLabels[i].style.opacity = 1 - t;
            if (t >= 1)
            {
                m_HealLabels[i].RemoveFromHierarchy();
                m_HealLabels.RemoveAt(i);
                m_HealTimes.RemoveAt(i);
            }
        }

        if (m_HealthGlowTimer > 0)
        {
            m_HealthGlowTimer -= Time.deltaTime;
            if (m_HealthGlowTimer <= 0)
            {
                m_Healthbar.style.unityBackgroundImageTintColor = Color.white;
            }
        }

        if (m_TimerDisplay > 0)
        {
            m_TimerDisplay -= Time.deltaTime;
            if (m_TimerDisplay < 0)
            {
                m_NonPlayerDialogue.style.display = DisplayStyle.None;
            }
        }
    }

    public void ShowHeal(int amount, Vector3 worldPosition)
    {
        Vector2 panelPosition = RuntimePanelUtils.CameraTransformWorldToPanel(
            m_Root.panel, worldPosition + Vector3.up * 1.2f, Camera.main);
        Label label = new Label("+" + amount);
        label.pickingMode = PickingMode.Ignore;
        label.style.position = Position.Absolute;
        label.style.left = panelPosition.x - 30;
        label.style.top = panelPosition.y;
        label.style.fontSize = 48;
        label.style.unityFontStyleAndWeight = FontStyle.Bold;
        label.style.color = new Color(0.45f, 1f, 0.45f);
        m_Root.Add(label);
        m_HealLabels.Add(label);
        m_HealTimes.Add(0);

        m_Healthbar.style.unityBackgroundImageTintColor = new Color(0.6f, 1f, 0.6f);
        m_HealthGlowTimer = 0.4f;
    }

    public void DisplayDialogue()
    {
        m_NonPlayerDialogue.style.display = DisplayStyle.Flex;
        m_TimerDisplay = displayTime;
    }

    public void DisplayWinScreen()
    {
        m_WinScreen.style.opacity = 1.0f;
    }

    public void DisplayLoseScreen()
    {
        m_LoseScreen.style.opacity = 1.0f;
    }
}
