using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WeeLight.Models;

namespace WeeLight.ViewModels
{
    public class DeviceVM : BaseViewModel
    {
        private YeeDevice _device;


        public string Name
        {
            get => _device.Name;
        }

        public string ID
        {
            get => _device.ID;
        }
        public bool CanSetTemperature
        {
            get => _device.CanSetTemperature;
        }
        public bool CanSetRGBColor
        {
            get => _device.CanSetRGBColor;
        }
        public int MinTemperature
        {
            get => _device.MinTemperature;
        }
        public int MaxTemperature
        {
            get => _device.MaxTemperature;
        }

        public bool IsPowerOn
        {
            get => _device.IsPowerOn;
            set => Set(value, (v) => _device.IsPowerOn = v);
        }

        public int Brightness
        {
            get => _device.Brightness;
            set => Set(value, (v) => _device.Brightness = v);
        }
        public int RGBColor
        {
            get => _device.RGBColor;
            set => Set(value, (v) => _device.RGBColor = v);
        }
        public int Temperature
        {
            get => _device.Temperature;
            set => Set(value, (v) => _device.Temperature = v);
        }

        public ObservableCollection<int> PredefinedTemperatures;
        public ObservableCollection<string> PredefinedColors;

        public DeviceVM(YeeDevice device)
        {
            _device = device;

            if (CanSetTemperature)
            {
                PredefinedTemperatures = new ObservableCollection<int>();
                int[] temperatures = { 1700, 2700, 4000, 5500, 6500 };
                foreach (var t in temperatures)
                {
                    if (t >= MinTemperature && t <= MaxTemperature)
                    {
                        PredefinedTemperatures.Add(t);
                    }
                }
            }

            if (CanSetRGBColor)
            {
                PredefinedColors = new ObservableCollection<string>();
                string[] colors = { "ff0000", "00ff00", "0000ff" };
                foreach (var c in colors)
                {
                    PredefinedColors.Add(c);
                }
            }
        }
    }
}
