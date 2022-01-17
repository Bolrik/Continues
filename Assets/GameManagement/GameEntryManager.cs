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




        float CameraTime { get; set; }
        int Index { get; set; }


        // Start is called before the first frame update
        void Start()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            this.MouseSensitivity.value = GameSettings.Instance.MouseSensitivity;
            this.MouseSensitivityDisplay.text = $"{GameSettings.Instance.MouseSensitivity:N2}";
        }

        public void MouseSensitivityChanged(float value)
        {
            GameSettings.Instance.MouseSensitivity = Mathf.Clamp(value, .1f, 4f);
            this.MouseSensitivityDisplay.text = $"{GameSettings.Instance.MouseSensitivity:N2}";
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
            LevelLoader.Instance.Start(GameScene.Level001);
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}