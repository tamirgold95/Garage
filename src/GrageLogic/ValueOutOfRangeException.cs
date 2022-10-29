namespace Ex3.GarageLogic
{
    using System;

    public class ValueOutOfRangeException : Exception
    {
        internal float MaxValue { get; set; }

        internal float MinValue { get; set; } = 0;

        public ValueOutOfRangeException(float i_UserInputValue, float i_CurrentCapacity, float i_MaxCapacity)
            : base(string.Format(
                @"You asked to add: {0}.
You already had: {1}.
That amount ({2}) exceeds the maximum capacity, which is: {3}.
Therefore, maximum capacity was filled.",
                i_UserInputValue,
                i_CurrentCapacity,
                i_CurrentCapacity + i_UserInputValue,
                i_MaxCapacity))
        {
        }
    }
}
