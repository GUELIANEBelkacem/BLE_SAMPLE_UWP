using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Storage.Streams;

namespace Stati_EP_BT_WIN.StatiBLE
{
    public class StatiBLEManager
    {
        Action<string> _log = null;
        public StatiBLEManager(Action<string> log)
        {
            _log = log;
        }
        //=======================================================================================================

        GattServiceProvider serviceProvider;

        private GattLocalCharacteristic metaDataCharacteristic;

        private GattLocalCharacteristic readCharacteristic;

        private GattLocalCharacteristic writeCharacteristic;

        private bool peripheralSupported;


        public async Task Run()
        {
            if (serviceProvider == null)
            {
                var serviceStarted = await ServiceProviderInitAsync();
                if (serviceStarted) _log("Ouiiiii!!! it's aliiiive!");
                else _log("Ah merde, un problème");
                
            }
            else
            {
                _log("Il tourne déjà le truc");
            }
        }
        public void Stop()
        {
            if (serviceProvider != null)
            {
                if (serviceProvider.AdvertisementStatus != GattServiceProviderAdvertisementStatus.Stopped)
                {
                    serviceProvider.StopAdvertising();
                    _log("Boom! il est mort");
                }
                serviceProvider = null;
            }
        }



        //comme on joue le role d'un peripherique, on doit vérifier si le role est supporté
        private async Task<bool> CheckPeripheralRoleSupportAsync()
        {
            var localAdapter = await BluetoothAdapter.GetDefaultAsync();
            if (localAdapter != null) return localAdapter.IsPeripheralRoleSupported;
            else return false;
        }

        private async Task<bool> ServiceProviderInitAsync()
        {
            if(!await CheckPeripheralRoleSupportAsync())
            {
                _log("Peripheral role is not supported on this device");
                return false;
            }
            GattServiceProviderResult serviceResult = await GattServiceProvider.CreateAsync(Config.EPServiceID);
            if (serviceResult.Error == BluetoothError.Success)
            {
                serviceProvider = serviceResult.ServiceProvider;
            }
            else
            {
                _log($"Could not create service provider: {serviceResult.Error}");
                return false;
            }

            metaDataCharacteristic = await CreateCharacteristic(Config.EPMetaDataCharacteristicID, Config.EPMetaDataParameters);
            if(metaDataCharacteristic == null) return false;
            metaDataCharacteristic.ReadRequested += MetaDataCharacteristic_ReadRequestedAsync;

            readCharacteristic = await CreateCharacteristic(Config.EPReadCharacteristicID, Config.EPReadParameters);
            if(readCharacteristic == null) return false;
            readCharacteristic.ReadRequested += ReadCharacteristic_ReadRequestedAsync;

            writeCharacteristic = await CreateCharacteristic(Config.EPWriteCharacteristicID, Config.EPWriteParameters);
            if(writeCharacteristic == null) return false;
            writeCharacteristic.WriteRequested += WriteCharacteristic_WriteRequestedAsync;


            // TODO: possible de formater les charactéristiques, voila comment faire (les paramertres à chercher)
            //GattPresentationFormat myFormat = GattPresentationFormat.
            //    FromParts(
            //    GattPresentationFormatTypes.Utf8,
            //    PresentationFormats.Exponent,
            //    Convert.ToUInt16(PresentationFormats.Units.Unitless),
            //    Convert.ToByte(PresentationFormats.NamespaceId.BluetoothSigAssignedNumber),
            //    PresentationFormats.Description);

            //Config.EPWriteParameters.PresentationFormats.Add(myFormat);

            metaDataCharacteristic.SubscribedClientsChanged += ResultCharacteristic_SubscribedClientsChanged;

       
            GattServiceProviderAdvertisingParameters advParameters = new GattServiceProviderAdvertisingParameters
            {
                
                IsConnectable = peripheralSupported,
                IsDiscoverable = true
            };
            BluetoothAdapter adapter = await BluetoothAdapter.GetDefaultAsync();

            serviceProvider.AdvertisementStatusChanged += ServiceProvider_AdvertisementStatusChanged;
            serviceProvider.StartAdvertising(advParameters);
            return true;
        }

        private async Task<GattLocalCharacteristic> CreateCharacteristic(Guid id, GattLocalCharacteristicParameters param)
        {
            GattLocalCharacteristicResult result = await serviceProvider.Service.CreateCharacteristicAsync(id, param);
            if (result.Error == BluetoothError.Success) return result.Characteristic;
            else _log($"Could not create operand1 characteristic: {result.Error}");

            return null;
        }

        private async void MetaDataCharacteristic_ReadRequestedAsync(GattLocalCharacteristic sender, GattReadRequestedEventArgs args)
        {
            using (args.GetDeferral())
            {
                GattReadRequest request = await args.GetRequestAsync();
                if (request == null)
                {
                    _log("Access to device not allowed");
                    return;
                }

                var writer = new DataWriter();
                writer.ByteOrder = ByteOrder.LittleEndian;
                writer.WriteString("TODO: les meta datas");
                request.RespondWithValue(writer.DetachBuffer());
            }
        }

        private async void ReadCharacteristic_ReadRequestedAsync(GattLocalCharacteristic sender, GattReadRequestedEventArgs args)
        {
            using (args.GetDeferral())
            {
                GattReadRequest request = await args.GetRequestAsync();
                if (request == null)
                {
                    _log("Access to device not allowed");
                    return;
                }

                var writer = new DataWriter();
                writer.ByteOrder = ByteOrder.LittleEndian;
                writer.WriteString("TODO: l'envoie du zip (3 fichiers) en base64");
                request.RespondWithValue(writer.DetachBuffer());
            }
        }

        private async void WriteCharacteristic_WriteRequestedAsync(GattLocalCharacteristic sender, GattWriteRequestedEventArgs args)
        {
            using (args.GetDeferral())
            {
                
                GattWriteRequest request = await args.GetRequestAsync();
                if (request == null)
                {
                    _log("Umm je ne sais pas trop pourquoi cela peut arriver");
                    return;
                }

                var reader = DataReader.FromBuffer(request.Value);
                reader.ByteOrder = ByteOrder.LittleEndian;
                string val = reader.ReadString(8);
                //TODO: converir la base64 en zip (contenant 1 seul fichier)    

                if (request.Option == GattWriteOption.WriteWithResponse)
                    request.Respond();
            }
        }
        private void ResultCharacteristic_SubscribedClientsChanged(GattLocalCharacteristic sender, object args)
        {
            _log($"New device subscribed {sender.SubscribedClients.Count}");
        }
        private void ServiceProvider_AdvertisementStatusChanged(GattServiceProvider sender, GattServiceProviderAdvertisementStatusChangedEventArgs args)
        {
            _log($"New Advertisement Status: {sender.AdvertisementStatus}");
        }
    }
}
