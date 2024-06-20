using System;
using System.Collections.Generic;
using System.Text;

namespace Ex03.GarageLogic
{
    public class Car : Vehicle
    {
        private eColor m_Color;
        private const int k_MinNumOfDoors = 2;
        private const int k_MaxNumOfDoors = 5;
        private int m_NumOfDoors;
        internal const eFuelType k_FuelType = eFuelType.Octan95;
        internal const float k_MaxFuelTank = 45f;
        internal const float k_MaxBatterySize = 3.5f;
        private const int k_NumOfWheels = 5;
        private const float k_MaxWheelAirPressure = 31;
        private List<string> m_UniqueAttributesNames;

        public Car(Engine i_Engine, string i_LicenseNumber) : base(i_Engine, i_LicenseNumber)
        {
            m_Color = eColor.White;
            m_NumOfDoors = 0;
            m_Wheels = CreateWheelsList(k_NumOfWheels, k_MaxWheelAirPressure);
            m_UniqueAttributesNames = new List<string>(k_NumOfUniqueAttributes)
            {
                "Color",
                "Number Of Doors",
            };
        }

        public override List<string> UniqueAttributesNames
        {
            get
            {
                return m_UniqueAttributesNames;
            }
        }

        public override List<string> GetUniqueAttributesNamesToChoose()
        {
            int             indexOfColor = 1;
            StringBuilder   colorsSB = new StringBuilder();

            foreach (eColor color in Enum.GetValues(typeof(eColor)))
            {
                colorsSB.AppendFormat("{0} - {1}, ", indexOfColor, color);
                indexOfColor++;
            }

            colorsSB.Length -= 2;
            colorsSB.AppendFormat(" (1 - {0})", Enum.GetValues(typeof(eColor)).Length);

            return new List<string> { $"Color: {colorsSB}", $"Number Of Doors ({k_MinNumOfDoors} - {k_MaxNumOfDoors})" };
        }

        internal override void SetUniqueAttributes(List<string> i_Attributes)
        {
            int indexOfAttribute1 = (int)eGeneralVehicleAttributes.Unique1 - k_NumOfGeneralAttributes;
            int indexOfAttribute2 = (int)eGeneralVehicleAttributes.Unique2 - k_NumOfGeneralAttributes;

            m_Color = (eColor)Enum.Parse(typeof(eColor), (int.Parse(i_Attributes[indexOfAttribute1]) - 1).ToString());
            m_NumOfDoors = int.Parse(i_Attributes[indexOfAttribute2]);
        }

        internal override float GetMaxFuelAmount()
        {
            return k_MaxFuelTank;
        }

        internal override float GetMaxBatteryAmount()
        {
            return k_MaxBatterySize;
        }

        internal override float GetMaxWheelAirPressure()
        {
            return k_MaxWheelAirPressure;
        }

        internal override void ValidationOfUnique1Attribute(string i_UserInput)
        {
            if (!short.TryParse(i_UserInput, out short colorChoice))
            {
                throw new FormatException("Invalid character. Enter a valid option");
            }
            else if (colorChoice < 1 || colorChoice > Enum.GetValues(typeof(eColor)).Length)
            {
                throw new FormatException($"Choice should be between 1 to {Enum.GetValues(typeof(eColor)).Length}");
            }
        }

        internal override void ValidationOfUnique2Attribute(string i_UserInput)
        {
            if (!short.TryParse(i_UserInput, out short numOfDoors))
            {
                throw new FormatException("Number of doors should be decimal digits only");
            }
            else if (numOfDoors < k_MinNumOfDoors || numOfDoors > k_MaxNumOfDoors)
            {
                throw new FormatException($"Number of doors should be between {k_MinNumOfDoors} to {k_MaxNumOfDoors}");
            }
        }

        public override List<string> GetUniqueAttributesValues()
        {
            List<string> uniqueAttributes = new List<string>(k_NumOfUniqueAttributes);

            uniqueAttributes.Add(m_Color.ToString());
            uniqueAttributes.Add(m_NumOfDoors.ToString());

            return uniqueAttributes;
        }
    }
}
    