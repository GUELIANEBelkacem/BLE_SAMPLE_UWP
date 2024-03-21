using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace Stati_EP_BT_WIN.StatiBLE
{
    public static class Config
    {
        public static readonly GattLocalCharacteristicParameters EPMetaDataParameters = new GattLocalCharacteristicParameters
        {
            CharacteristicProperties = GattCharacteristicProperties.Read,
            WriteProtectionLevel = GattProtectionLevel.Plain,
            UserDescription = "EP MetaData Characteristic"
        };

        public static readonly GattLocalCharacteristicParameters EPReadParameters = new GattLocalCharacteristicParameters
        {
            CharacteristicProperties = GattCharacteristicProperties.Read,
            WriteProtectionLevel = GattProtectionLevel.Plain,
            UserDescription = "EP Read Characteristic"
        };
        
        public static readonly GattLocalCharacteristicParameters EPWriteParameters = new GattLocalCharacteristicParameters
        {
            CharacteristicProperties = GattCharacteristicProperties.Write | GattCharacteristicProperties.WriteWithoutResponse,
            WriteProtectionLevel = GattProtectionLevel.Plain,
            UserDescription = "EP Write Characteristic"
        };


        public static readonly Guid EPServiceID = Guid.Parse("04A36223-3BFC-4A73-9B0E-F67A3CE12637");

        public static readonly Guid EPMetaDataCharacteristicID = Guid.Parse("29A87E6B-E933-474C-9AEC-4774EF5715A5");
        public static readonly Guid EPReadCharacteristicID = Guid.Parse("2E8CBC95-076E-4A89-ACF9-783697B97F09");
        public static readonly Guid EPWriteCharacteristicID = Guid.Parse("3590F528-2806-41D7-A1F9-64E60F1ED00F");

    }
}
