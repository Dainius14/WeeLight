using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;
using YeelightAPI;

namespace WeeLight.Models
{

    public class YeeDevice
    {
        #region Properties
        private Device _device { get; set; }
        public string ID { get => _device.Id; }
        public string Name { get => _device.Name; }

        private bool _isPowerOn = false;
        public bool IsPowerOn
        {
            get => _isPowerOn;
            set
            {
                _isPowerOn = value;
                _device.SetPower(value, 100);
                if (value)
                {
                    SetDynamicProperties();
                }
            }
        }

        private int _brightness = 0;
        public int Brightness
        {
            get => _brightness;
            set
            {
                _brightness = value >= 1 ? value : 1;
                _device.SetBrightness(_brightness, 100);
            }
        }

        private int _rgbColor = 0;
        public int RGBColor
        {
            get => _rgbColor;
            set
            {
                if (!CanSetRGBColor)
                {
                    return;
                }

                _rgbColor = value;
                byte[] rgb = Utilities.Converters.RGBIntToBytes(value);
                _device.SetRGBColor(rgb[0], rgb[1], rgb[2], 100);
            }
        }

        private int _temperature = 0;
        public int Temperature
        {
            get => _temperature;
            set
            {
                if (!CanSetTemperature)
                {
                    return;
                }

                _temperature = value;
                _device.SetColorTemperature(value);
            }
        }


        public bool CanSetTemperature { get; private set; }

        public bool CanSetRGBColor { get; private set; }

        public int MinTemperature { get; private set; }
        public int MaxTemperature { get; private set; }

        #endregion

        public YeeDevice(Device device)
        {
            _device = device;
            device.Connect();

            IsPowerOn = (string)device[YeelightAPI.Models.PROPERTIES.power] == "on";
            SetStaticProperties();
            SetDynamicProperties();

            device.OnNotificationReceived += (object sender, NotificationReceivedEventArgs args) =>
            {
                Debug.WriteLine($"Notification for {_device.Name}: " + JsonConvert.SerializeObject(args.Result));
                switch (args.Result.Params.Keys.First().ToString())
                {
                    case "power":
                        break;

                    default:
                        Debug.WriteLine("Unknown params: " + JsonConvert.SerializeObject(args.Result));
                        break;
                }
            };
        }

        private void SetStaticProperties()
        {
            CanSetRGBColor = _device.SupportedOperations.Contains(YeelightAPI.Models.METHODS.SetRGBColor);
            CanSetTemperature = _device.SupportedOperations.Contains(YeelightAPI.Models.METHODS.SetColorTemperature);
            if (CanSetTemperature)
            {
                switch (_device.Model)
                {
                    case YeelightAPI.Models.MODEL.Ceiling:
                    case YeelightAPI.Models.MODEL.DeskLamp:
                    case YeelightAPI.Models.MODEL.Unknown:  // TODO change to ct_bulb when API is updated
                        MinTemperature = 2700;
                        MaxTemperature = 6500;
                        break;
                    case YeelightAPI.Models.MODEL.Color:
                    default:
                        MinTemperature = 1700;
                        MaxTemperature = 6500;
                        break;
                }
            }
        }
        private void SetDynamicProperties()
        {
            _brightness = Convert.ToInt32(_device[YeelightAPI.Models.PROPERTIES.bright]);

            if (CanSetRGBColor)
            {
                _rgbColor = Convert.ToInt32(_device[YeelightAPI.Models.PROPERTIES.rgb]);
            }

            CanSetTemperature = _device.SupportedOperations.Contains(YeelightAPI.Models.METHODS.SetColorTemperature);
            if (CanSetTemperature)
            {
                _temperature = Convert.ToInt32(_device[YeelightAPI.Models.PROPERTIES.ct]);
            }
        }
    }
}
