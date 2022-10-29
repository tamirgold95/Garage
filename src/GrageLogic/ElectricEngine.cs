namespace Ex3.GarageLogic
{
    public class ElectricEngine : Engine
    {
        public ElectricEngine(float i_Remaining = 0)
            : base(i_Remaining)
        {
        }

        public void Recharge(float i_HoursToAdd)
        {
            if (Remaining + i_HoursToAdd <= Max)
            {
                Remaining += i_HoursToAdd;
            }
            else
            {
                float amountBeforeRecharge = Remaining;

                Remaining = Max;
                throw new ValueOutOfRangeException(i_HoursToAdd, amountBeforeRecharge, Max);
            }
        }
    }
}
