using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManagement
{
    public class GameSettings
    {
        #region Singleton Pattern
        public static GameSettings Instance { get; private set; }
        static GameSettings()
        {
            new GameSettings();
        }

        private GameSettings()
        {
            Instance = this;
        }
        #endregion

        private float mouseSensitivity = 1f;
        public float MouseSensitivity
        {
            get { return mouseSensitivity; }
            set { mouseSensitivity = value; }
        }

    }
}