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

        private SignalValue[] signalValues;
        public SignalValue[] SignalValues { get { return signalValues; } private set { signalValues = value; } }

        private void Initialize()
        {
            SignalChannel[] channels = (SignalChannel[])Enum.GetValues(typeof(SignalChannel));

            this.SignalValues = channels.Select(channel => new SignalValue(channel, 0)).ToArray();
        }

        public void LoadLevelState(params SignalChannel[] signalChannels)
        {

        }

        public float SetSignal(SignalValue signalValue)
        {
            return this.SetSignal(signalValue.Signal, signalValue.Value);
        }

        public float SetSignal(SignalChannel signalChannel, float value)
        {
            var currentSignalValue = this.SignalValues.FirstOrDefault(sv => sv.Signal == signalChannel);

            currentSignalValue?.Set(value);

#warning MultiState Value??? Persistent Value Provider

            return value;
        }
    }

    [System.Serializable]
    public class SignalValue
    {
        [SerializeField] private SignalChannel signal;
        public SignalChannel Signal { get { return signal; } private set { signal = value; } }

        [SerializeField, Range(0, 1)] private float value;
        public float Value { get { return value; } private set { this.value = value; } }

        public bool IsActive { get => this.Value >= .5f; }


        public SignalValue(SignalChannel signal, float value)
        {
            this.Signal = signal;
            this.Value = value;
        }

        public void Set(float value)
        {
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
}
