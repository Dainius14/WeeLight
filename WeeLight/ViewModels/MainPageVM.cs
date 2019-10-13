using System;
using System.Collections.ObjectModel;
using System.Linq;
using WeeLight.Models;
using YeelightAPI;

namespace WeeLight.ViewModels
{
    public class MainPageVM : BaseViewModel
    {
        #region Properties
        public ObservableCollection<YeeDevice> Devices { get; } = new ObservableCollection<YeeDevice>();

        private DeviceVM _selectedDevice = null;
        public DeviceVM SelectedDevice
        {
            get => _selectedDevice;
            set => Set(value, (v) => _selectedDevice = v);
        }
        private int _selectedDeviceIndex;
        public int SelectedDeviceIndex
        {
            get => _selectedDeviceIndex;
            set => Set(value, (v) => _selectedDeviceIndex = v);
        }

        private bool _isDeviceListLoading = true;
        public bool IsDeviceListLoading
        {
            get => _isDeviceListLoading;
            set => Set(value, (v) => _isDeviceListLoading = v);
        }

        private bool _noDevicesFound;
        public bool NoDevicesFound
        {
            get => _noDevicesFound;
            set => Set(value, (v) => _noDevicesFound = v);
        }

        #endregion

        public MainPageVM()
        {
            DiscoverDevices();

        }

        public void OnPowerToggle(bool value)
        {
            SelectedDevice.IsPowerOn = value;
        }


        async public void DiscoverDevices()
        {
            IsDeviceListLoading = true;

            Devices.Clear();
            var devices = await DeviceLocator.Discover();
            foreach (var device in devices)
            {
                Devices.Add(new YeeDevice(device));
            }

            if (SelectedDevice != null)
            {
                SelectDevice(Devices.FirstOrDefault(x => x.ID == SelectedDevice.ID));
            }
            else
            {
                SelectDevice(Devices.FirstOrDefault());
            }

            NoDevicesFound = SelectedDevice == null;

            IsDeviceListLoading = false;
        }

        public void SelectDevice(YeeDevice yeeDevice)
        {
            if (yeeDevice != null)
            {
                SelectedDevice = new DeviceVM(yeeDevice);
                SelectedDeviceIndex = Devices.IndexOf(yeeDevice);
            }
            else
            {
                SelectedDevice = null;
                SelectedDeviceIndex = -1;
            }
        }

        public void OnWindowActivated()
        {
            foreach (var device in Devices)
            {
                device.Connect();
            }
        }
    }
}
