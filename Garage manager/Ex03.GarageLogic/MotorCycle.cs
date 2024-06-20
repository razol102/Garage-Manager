using System;
using System.Collections.Generic;
using System.Text;

namespace Ex03.GarageLogic
{
    public class MotorCycle : Vehicle
    {
        private eLicenseType m_LicenseType;
        private int m_EngineVolume;
        internal const eFuelType k_FuelType = eFuelType.Octan98;
        internal const float k_MaxFuelTank = 5.5f;
        internal const float k_MaxBatterySize = 2.5f;
        private const int k_NumOfWheels = 2;
        private const float k_MaxWheelAirPressure = 33;
        private List<string> m_UniqueAttributesNames;

        public MotorCycle(Engine i_Engine, string i_LicenseNumber) : base(i_Engine, i_LicenseNumber)
        {
            m_LicenseType = 0;
            m_EngineVolume = 0;
            m_Wheels = CreateWheelsList(k_NumOfWheels, k_MaxWheelAirPressure);
            m_UniqueAttributesNames = new List<string>(k_NumOfUniqueAttributes)
            {
                "License type",
                "Engine Volume",
            };
        }

        public override List<string> GetUniqueAttributesNamesToChoose()
        {
            int             indexOfType = 1;
            StringBuilder   licenseTypeSB = new StringBuilder();

            foreach (eLicenseType type in Enum.GetValues(typeof(eLicenseType)))
            {
                licenseTypeSB.AppendFormat("{0} - {1}, ", indexOfType, type);
                indexOfType++;
            }

            licenseTypeSB.Length -= 2;
            licenseTypeSB.AppendFormat(" (1 - {0})", Enum.GetValues(typeof(eLicenseType)).Length);

            return new List<string> { $"License type: {licenseTypeSB}", "Engine Volume" };
        }

        public override List<string> UniqueAttributesNames
        {
            get
            {
                return m_UniqueAttributesNames;
            }
        }

        internal override void SetUniqueAttributes(List<string> i_Attributes)
        {
            int indexOfAttribute1 = (int)eGeneralVehicleAttributes.Unique1 - k_NumOfGeneralAttributes;
            int indexOfAttribute2 = (int)eGeneralVehicleAttributes.Unique2 - k_NumOfGeneralAttributes;

            m_LicenseType = (eLicenseType)Enum.Parse(typeof(eLicenseType), (int.Parse(i_Attributes[indexOfAttribute1]) - 1).ToString());
            m_EngineVolume = int.Parse(i_Attributes[indexOfAttribute2]);
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
            if (!short.TryParse(i_UserInput, out short typeChoice))
            {
                throw new FormatException("Invalid character. Enter a valid option");
            }
            else if (typeChoice < 1 || typeChoice > Enum.GetValues(typeof(eLicenseType)).Length)
            {
                throw new FormatException($"Choice should be between 1 to {Enum.GetValues(typeof(eLicenseType)).Length}");
            }
        }

        internal override void ValidationOfUnique2Attribute(string i_UserInput)
        {
            if (!int.TryParse(i_UserInput, out int engineVolume))
            {
                throw new FormatException("Engine volume should be decimal digits only");
            }

            if (engineVolume < 0)
            {
                throw new ValueOutOfRangeException(default, 0, "Engine volume must be greater or equal to 0!");
            }
        }

        public override List<string> GetUniqueAttributesValues()
        {
            List<string> uniqueAttributes = new List<string>(k_NumOfUniqueAttributes);

            uniqueAttributes.Add(m_LicenseType.ToString());
            uniqueAttributes.Add(m_EngineVolume.ToString());

            return uniqueAttributes;
        }
    }
}
