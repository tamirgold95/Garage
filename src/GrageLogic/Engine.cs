namespace Ex3.GarageLogic
{
    public abstract class Engine
    {
        public float Remaining { get; set; }

        public float Max { get; set; }

        public Engine(float i_Remaining)
        {
            Remaining = i_Remaining;
        }

        internal float GetEnergyPercentage()
        {
            return (Remaining / Max) * 100f;
        }
    }
}
