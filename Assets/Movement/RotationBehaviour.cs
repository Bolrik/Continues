using UnityEngine;

namespace Movement
{
    public class RotationBehaviour : MonoBehaviour, IInputStateReceiver
    {
        [Header("References")]
        [SerializeField] private IInputStateProvider inputStateProvider;
        private IInputStateProvider InputStateProvider { get { return this.inputStateProvider; } set { this.inputStateProvider = value; } }

        [Header("Info")]
        [SerializeField] private Vector2 mousePosition;
        private Vector2 MousePosition { get { return this.mousePosition; } set { this.mousePosition = value; } }

        [SerializeField] private Vector2 mousePositionSmooth;
        private Vector2 MousePositionSmooth { get { return this.mousePositionSmooth; } set { this.mousePositionSmooth = value; } }

        [Header("Settings")]
        [SerializeField] private Vector2 viewClamp = new Vector2(360, 180);
        public Vector2 ViewClamp { get { return this.viewClamp; } }

        [SerializeField] private bool lockCursor;
        public bool LockCursor { get { return this.lockCursor; } }


        [SerializeField] private Vector2 sensitivit = new Vector2(2, 2);
        public Vector2 Sensitivit { get { return this.sensitivit; } }

        [SerializeField] private Vector2 smoothing = new Vector2(3, 3);
        private Vector2 Smoothing { get { return this.smoothing; } set { this.smoothing = value; } }

        [SerializeField] private Vector2 targetDirection;
        private Vector2 TargetDirection { get { return this.targetDirection; } set { this.targetDirection = value; } }

        [SerializeField] private Vector2 targetCharacterDirection;
        private Vector2 TargetCharacterDirection { get { return this.targetCharacterDirection; } set { this.targetCharacterDirection = value; } }

        [Header("View Targets")]
        [SerializeField] private GameObject verticalTargetTransform;
        private GameObject VerticalTargetTransform { get { return this.verticalTargetTransform; } set { this.verticalTargetTransform = value; } }

        [SerializeField] private GameObject horizontalTargetTransform;
        private GameObject HorizontalTargetTransform { get { return this.horizontalTargetTransform; } set { this.horizontalTargetTransform = value; } }

        InputState InputState { get; set; }

        private void Start()
        {
            this.InputStateProvider = this.GetComponent<IInputStateProvider>();

            if (this.InputStateProvider == null)
            {
                this.enabled = false;
                return;
            }

            this.InputStateProvider.InputStateChanged += this.UpdateInputState;

            // Set target direction to the camera's initial orientation.
            if (this.VerticalTargetTransform != null)
                this.targetDirection = this.VerticalTargetTransform.transform.localRotation.eulerAngles;

            // Set target direction for the character body to its inital state.
            if (this.HorizontalTargetTransform != null)
                this.targetCharacterDirection = this.HorizontalTargetTransform.transform.localRotation.eulerAngles;

            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
        }

        void LateUpdate()
        {
            if (this.LockCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            var targetOrientation = Quaternion.Euler(this.TargetDirection);
            var targetCharacterOrientation = Quaternion.Euler(this.TargetCharacterDirection);

            var mouseDelta = this.InputState.Look;

            mouseDelta = Vector2.Scale(mouseDelta, new Vector2(this.Sensitivit.x * this.smoothing.x, this.Sensitivit.y * this.smoothing.y));

            Vector2 mousePositionSmooth = this.MousePositionSmooth;
            mousePositionSmooth.x = Mathf.Lerp(mousePositionSmooth.x, mouseDelta.x, 1f / this.Smoothing.x);
            mousePositionSmooth.y = Mathf.Lerp(mousePositionSmooth.y, mouseDelta.y, 1f / this.Smoothing.y);

            Vector2 mousePosition = this.MousePosition;
            mousePosition += mousePositionSmooth;

            if (this.ViewClamp.x < 360)
                mousePosition.x = Mathf.Clamp(mousePosition.x, -this.ViewClamp.x * 0.5f, this.ViewClamp.x * 0.5f);

            if (this.ViewClamp.y < 360)
                mousePosition.y = Mathf.Clamp(mousePosition.y, -this.ViewClamp.y * 0.5f, this.ViewClamp.y * 0.5f);

            if (this.VerticalTargetTransform != null)
            {
                var rotation = Quaternion.AngleAxis(-mousePosition.y, targetOrientation * Vector3.right) * targetOrientation;
                this.VerticalTargetTransform.transform.localRotation = rotation * targetCharacterOrientation;
            }

            if (this.HorizontalTargetTransform != null)
            {
                var rotation = Quaternion.AngleAxis(mousePosition.x, Vector3.up);
                this.HorizontalTargetTransform.transform.localRotation = rotation * targetCharacterOrientation;
            }

            this.MousePosition = mousePosition;
            this.MousePositionSmooth = mousePositionSmooth;
        }
        

        public void UpdateInputState(InputState inputState)
        {
            this.InputState = inputState;
        }

    }
}