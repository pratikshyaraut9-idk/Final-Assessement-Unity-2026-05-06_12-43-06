using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [Header("UI Documents")]
    [SerializeField] private UIDocument _hudDoc;
    [SerializeField] private UIDocument _scenarioDoc;

    private Label _scoreLabel;
    
    private VisualElement _overlay;
    private VisualElement _decisionPanel;
    private VisualElement _feedbackPanel;
    
    private Label _titleLabel;
    private Label _descLabel;
    private Button _choiceA;
    private Button _choiceB;
    
    private Label _feedbackDesc;
    private Button _continueButton;
    private VisualElement _resultIcon;

    private ScenarioData _currentData;
    private bool _lastChoiceWasSafe;

    private void OnEnable()
    {
        InitializeHUD();
        InitializeScenarioUI();
        
        if (ScenarioController.Instance != null)
        {
            ScenarioController.Instance.OnScenarioTriggered.AddListener(ShowScenario);
        }
    }

    private void Update()
    {
        if (_scoreLabel != null && GameManager.Instance != null)
        {
            _scoreLabel.text = $"Score: {GameManager.Instance.Score}";
        }
    }

    private void InitializeHUD()
    {
        if (_hudDoc == null) return;
        _scoreLabel = _hudDoc.rootVisualElement.Q<Label>("scoreLabel");
    }

    private void InitializeScenarioUI()
    {
        if (_scenarioDoc == null) return;
        var root = _scenarioDoc.rootVisualElement;
        
        _overlay = root.Q<VisualElement>("overlay");
        _decisionPanel = root.Q<VisualElement>("decisionPanel");
        _feedbackPanel = root.Q<VisualElement>("feedbackPanel");
        
        _titleLabel = root.Q<Label>("scenarioTitle");
        _descLabel = root.Q<Label>("scenarioDesc");
        _choiceA = root.Q<Button>("choiceA");
        _choiceB = root.Q<Button>("choiceB");
        
        _feedbackDesc = root.Q<Label>("feedbackDesc");
        _continueButton = root.Q<Button>("continueButton");
        _resultIcon = root.Q<VisualElement>("resultIcon");

        _choiceA.clicked += () => HandleChoice(true);
        _choiceB.clicked += () => HandleChoice(false);
        _continueButton.clicked += Continue;
    }

    private void ShowScenario(ScenarioData data)
    {
        _currentData = data;
        _titleLabel.text = data.ScenarioTitle;
        _descLabel.text = data.Description;
        _choiceA.text = data.ChoiceAText;
        _choiceB.text = data.ChoiceBText;

        _overlay.RemoveFromClassList("hidden");
        _decisionPanel.RemoveFromClassList("hidden");
        _feedbackPanel.AddToClassList("hidden");

        // Unlock cursor for UI interaction
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
    }

    private void HandleChoice(bool isA)
    {
        _lastChoiceWasSafe = isA ? _currentData.IsSafeA : _currentData.IsSafeB;
        _feedbackDesc.text = isA ? _currentData.FeedbackA : _currentData.FeedbackB;

        _decisionPanel.AddToClassList("hidden");
        _feedbackPanel.RemoveFromClassList("hidden");

        // Set feedback styling
        _feedbackPanel.RemoveFromClassList("feedback-safe");
        _feedbackPanel.RemoveFromClassList("feedback-risky");
        if (_resultIcon != null)
        {
            _resultIcon.RemoveFromClassList("safe-icon");
            _resultIcon.RemoveFromClassList("risky-icon");

            if (_lastChoiceWasSafe)
            {
                _feedbackPanel.AddToClassList("feedback-safe");
                _resultIcon.AddToClassList("safe-icon");
            }
            else
            {
                _feedbackPanel.AddToClassList("feedback-risky");
                _resultIcon.AddToClassList("risky-icon");
            }
        }
    }

    private void Continue()
    {
        _overlay.AddToClassList("hidden");
        
        // Re-lock cursor before scene transition/game resume
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;

        ScenarioController.Instance.CompleteScenario(_lastChoiceWasSafe, _currentData.NextSceneName);
    }
}
