using GameManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Signals
{
    public class SignalManager
    {
        #region Singleton Pattern
        public static SignalManager Instance { get; private set; }
        static SignalManager()
        {
            new SignalManager();
        }

        private SignalManager()
        {
            Instance = this;

            this.Initialize();
        }
        #endregion

        Dictionary<SignalChannel, bool> SignalStates { get; set; } = new Dictionary<SignalChannel, bool>();

        private void Initialize()
        {
            LevelLoader.Instance.PreLoadScene += this.ResetSignals;

            this.ResetSignals();
        }

        private void ResetSignals()
        {
            SignalChannel[] channels = (SignalChannel[])Enum.GetValues(typeof(SignalChannel));

            foreach (SignalChannel channel in channels)
            {
                this.SignalStates[channel] = false;
            }
        }

        public void LoadLevelState(params SignalChannel[] signalChannels)
        {

        }

        public void Set(SignalChannel signalChannel, bool value)
        {
            this.SignalStates[signalChannel] = value;
        }

        public bool Toggle(SignalChannel signalChannel)
        {
            bool current = this.GetSignal(signalChannel);

            this.SignalStates[signalChannel] = !current;

            return this.SignalStates[signalChannel];
        }

        public bool GetSignal(SignalChannel signalChannel)
        {
            if (!this.SignalStates.ContainsKey(signalChannel))
                this.SignalStates[signalChannel] = false;

            return this.SignalStates[signalChannel];
        }
    }

    [System.Serializable]
    public class SignalValue
    {
        [SerializeField] private SignalChannel signal;
        public SignalChannel Signal { get { return signal; } private set { signal = value; } }

        [SerializeField] private bool value;
        public bool Value { get { return value; } private set { this.value = value; } }


        public SignalValue(SignalChannel signal, bool value)
        {
            this.Signal = signal;
            this.Value = value;
        }
    }

    public enum SignalChannel
    {
        Red, 
        Green,
        Blue,
        Yellow,
        Magenta,
        Cyan,
        Gray
    }

    public enum SignalState
    {
        Active,
        Inactive
    }
}
