using Levels;
using UnityEngine;
using UnityEngine.UI;

namespace GameManagement
{
    public class GameEntryManager : MonoBehaviour
    {
        [SerializeField] private Camera gameCamera;
        public Camera GameCamera { get { return gameCamera; } }

        [SerializeField] private Transform cameraTarget;
        public Transform CameraTarget { get { return cameraTarget; } }


        [SerializeField] private Vector3[] positions;
        public Vector3[] Positions { get { return positions; } }

        [SerializeField] private Slider mouseSensitivity;
        public Slider MouseSensitivity { get { return mouseSensitivity; } }

        [SerializeField] private Text mouseSensitivityDisplay;
        public Text MouseSensitivityDisplay { get { return mouseSensitivityDisplay; } }

        [Header("References / Level Selection")]
        [SerializeField] private Transform levelSelectionChaptersContent;
        public Transform LevelSelectionChaptersContent { get { return levelSelectionChaptersContent; } }

        [SerializeField] private Transform levelSelectionLevelsContent;
        public Transform LevelSelectionLevelsContent { get { return levelSelectionLevelsContent; } }

        [SerializeField] private LevelUI levelUIPrefab;
        public LevelUI LevelUIPrefab { get { return levelUIPrefab; } }



        float CameraTime { get; set; }
        int Index { get; set; }


        private void Awake()
        {
            if (PlayerPrefs.HasKey(GameSettingsKeys.MouseSensitivity))
            {
                GameSettings.Instance.MouseSensitivity = PlayerPrefs.GetFloat(GameSettingsKeys.MouseSensitivity);
            }
            else
            {
                GameSettings.Instance.MouseSensitivity = 1f;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            GameSettings.Instance.ShowCursor = true;

            this.MouseSensitivity.value = GameSettings.Instance.MouseSensitivity;
            this.MouseSensitivityDisplay.text = $"{GameSettings.Instance.MouseSensitivity:N2}";

            for (int i = 0; i < LevelDataStore.Levels.Length; i++)
            {
                LevelData data = LevelDataStore.Levels[i];
                LevelUI levelUI = GameObject.Instantiate(this.LevelUIPrefab);
                levelUI.SetData(data);
                levelUI.transform.SetParent(this.levelSelectionLevelsContent, false);
            }
        }

        // Update is called once per frame
        void Update()
        {
            this.CameraTime += Time.deltaTime * (1 / 4f);

            if (this.CameraTime > 1)
            {
                this.CameraTime = 0;
                this.Index++;

                if (this.Index >= this.Positions.Length)
                    this.Index = 0;
            }

            int idxC = this.Index;
            int idxN = (idxC + 1) % this.Positions.Length;

            this.GameCamera.transform.position = Vector3.Slerp(this.Positions[idxC], this.Positions[idxN], this.CameraTime);
            this.GameCamera.transform.LookAt(this.CameraTarget);
        }

        public void StartGame()
        {
            PlayerPrefs.SetFloat(GameSettingsKeys.MouseSensitivity, GameSettings.Instance.MouseSensitivity);

            LevelLoader.Instance.Start(GameScene.Level001);
        }

        public void ExitGame()
        {
            PlayerPrefs.SetFloat(GameSettingsKeys.MouseSensitivity, GameSettings.Instance.MouseSensitivity);

            Application.Quit();
        }


        public void MouseSensitivityChanged(float value)
        {
            GameSettings.Instance.MouseSensitivity = Mathf.Clamp(value, .1f, 4f);
            this.MouseSensitivityDisplay.text = $"{GameSettings.Instance.MouseSensitivity:N2}";
        }


        public void LoadLevel(int index)
        {

        }
    }
}