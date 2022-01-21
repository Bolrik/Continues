using Levels;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace GameManagement
{
    public class MainMenuManager : Singleton<MainMenuManager>
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


        protected override void Awake()
        {
            base.Awake();

            if (PlayerPrefs.HasKey(PlayerPrefKeys.MouseSensitivity))
            {
                GameSettings.Instance.MouseSensitivity = PlayerPrefs.GetFloat(PlayerPrefKeys.MouseSensitivity);
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

            this.LoadChapter(1);
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
            PlayerPrefs.SetFloat(PlayerPrefKeys.MouseSensitivity, GameSettings.Instance.MouseSensitivity);

            LevelManager.Instance.Start(LevelDataStore.Instance.Get(level => level.Chapter > 0));
        }

        public void ExitGame()
        {
            PlayerPrefs.SetFloat(PlayerPrefKeys.MouseSensitivity, GameSettings.Instance.MouseSensitivity);

            Application.Quit();
        }

        public void ResetLevelData()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetFloat(PlayerPrefKeys.MouseSensitivity, GameSettings.Instance.MouseSensitivity);
            LevelManager.Instance.ClearLevelTimes();

            LevelManager.Instance.Restart();
        }


        public void MouseSensitivityChanged(float value)
        {
            GameSettings.Instance.MouseSensitivity = Mathf.Clamp(value, .1f, 4f);
            this.MouseSensitivityDisplay.text = $"{GameSettings.Instance.MouseSensitivity:N2}";
        }


        public void LoadLevel(int index)
        {

        }

        public void LoadChapter(int chapter)
        {
            this.LevelSelectionLevelsContent.Clear();

            LevelData[] chapterLevels = LevelDataStore.Instance.GetChapter(chapter);

            for (int i = 0; i < chapterLevels.Length; i++)
            {
                LevelData data = chapterLevels[i];
                LevelUI levelUI = GameObject.Instantiate(this.LevelUIPrefab);
                levelUI.SetData(data);
                levelUI.transform.SetParent(this.levelSelectionLevelsContent, false);
            }
        }
    }
}