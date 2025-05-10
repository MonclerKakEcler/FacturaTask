using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Factura.Camera;
using Factura.Car;
using Factura.Enemy;
using Factura.Finish;

public class BaseScenario : MonoBehaviour
{
    [Header("UI Components: ")]
    [SerializeField] private Image _startScreen;
    [SerializeField] private Image _finishScreen;
    [SerializeField] private Image _loseScreen;
    [SerializeField] private Button _startGameButton;
    [SerializeField] private Button _finishGameButton;
    [SerializeField] private Button _tryAgainButton;

    [Header("Scripts: ")]
    [SerializeField] private CameraSwitcher _cameraSwitcher;
    [SerializeField] private FinishView _finishView;


    [Inject] ICarController _carController;
    [Inject] IEnemyService _enemyService;

    private void OnEnable()
    {
        _startGameButton.onClick.AddListener(StartGame);
        _finishGameButton.onClick.AddListener(RestartGame);
        _tryAgainButton.onClick.AddListener(RestartGame);

        _finishView.OnLevelFinished += StopGame;
    }

    private void OnDisable()
    {
        _startGameButton.onClick.RemoveListener(StartGame);
        _finishGameButton.onClick.RemoveListener(RestartGame);
        _tryAgainButton.onClick.RemoveListener(RestartGame);
        _finishView.OnLevelFinished -= StopGame;
    }

    private void StartGame()
    {
        _enemyService.SpawnEnemies();
        _startScreen.gameObject.SetActive(false);
        _cameraSwitcher.IsActivePreviewCamera(false);
        _carController.StartMoving();
    }

    private void StopGame()
    {
        _finishScreen.gameObject.SetActive(true);
        _carController.StopMoving();
        _enemyService.ClearEnemies();
    }

    private void RestartGame()
    {
        Time.timeScale = 1;
        _enemyService.ClearEnemies();
        _finishScreen.gameObject.SetActive(false);
        _loseScreen.gameObject.SetActive(false);

        _cameraSwitcher.IsActivePreviewCamera(true);
        _startScreen.gameObject.SetActive(true);
        _carController.SetCarOnStart();
    }
}

