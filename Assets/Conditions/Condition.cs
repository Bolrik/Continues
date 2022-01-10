namespace Conditions
{
    [System.Serializable]
    public class Condition
    {
        public virtual bool Check() => false;
    }
}