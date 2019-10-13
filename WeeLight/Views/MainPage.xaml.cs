using System;
using WeeLight.Models;
using WeeLight.ViewModels;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WeeLight.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPageVM VM { get; set; }
        public MainPage()
        {
            InitializeComponent();
            VM = new MainPageVM();

            VM.PropertyChanged += VM_PropertyChanged;

            Window.Current.Activated += Window_Activated;
        }

        private void Window_Activated(object sender, WindowActivatedEventArgs e)
        {
            VM.OnWindowActivated();
        }

        private void VM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(VM.SelectedDevice):
                    if (VM.SelectedDevice != null)
                    {
                        if (VM.SelectedDevice.CanSetTemperature)
                        {
                            CreateTemperatureButtons();
                        }

                        if (VM.SelectedDevice.CanSetRGBColor)
                        {
                            CreateColorButtons();
                        }
                    }
                    break;
            }
        }

        private void DeviceList_ItemClick(object sender, ItemClickEventArgs e)
        { 
            VM.SelectDevice(e.ClickedItem as YeeDevice);
        }

        private void CreateTemperatureButtons()
        {
            if (VM.SelectedDevice == null)
            {
                return;
            }
            ControlTemplate template = (ControlTemplate)Resources["TemperatureColorButtonTemplate"];

            TemperatureButtons.Children.Clear();
            foreach (var t in VM.SelectedDevice.PredefinedTemperatures)
            {
                byte[] rgb = Utilities.Converters.TemperatureToRGBBytes(t);
                Button btn = new Button()
                {
                    Content = $"{t} K",
                    Background = new SolidColorBrush(Color.FromArgb(255, rgb[0], rgb[1], rgb[2])),
                    Template = template,
                    Tag = new TemperatureColorButtonTag { Temperature = t },
                    IsEnabled = VM.SelectedDevice.IsPowerOn,
                };

                btn.Click += TemperatureColorButton_Click;

                TemperatureButtons.Children.Add(btn);
            }
        }
        private void CreateColorButtons()
        {
            if (VM.SelectedDevice == null)
            {
                return;
            }
            ControlTemplate template = (ControlTemplate)Resources["TemperatureColorButtonTemplate"];

            ColorButtons.Children.Clear();
            foreach (var c in VM.SelectedDevice.PredefinedColors)
            {
                byte[] rgb = Utilities.Converters.RGBStringToBytes(c);
                Button btn = new Button()
                {
                    Background = new SolidColorBrush(Color.FromArgb(255, rgb[0], rgb[1], rgb[2])),
                    Template = template,
                    Tag = new TemperatureColorButtonTag { RGBColor = Utilities.Converters.RGBBytesToInt(rgb) },
                    IsEnabled = VM.SelectedDevice.IsPowerOn,
                };

                btn.Click += TemperatureColorButton_Click;

                ColorButtons.Children.Add(btn);
            }
        }

        private void DevicePowerSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;

            if (toggleSwitch != null)
            {
                VM.OnPowerToggle(toggleSwitch.IsOn);
            }
        }
        private void TopLevelNavBottom_ItemClick(object sender, ItemClickEventArgs e)
        {
            var button = e.ClickedItem as StackPanel;
            if (button.Name == "RefreshDevicesButton")
            {
                VM.DiscoverDevices();
            }
        }

        private void BrightnessSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            var slider = sender as Slider;
            if (slider != null)
            {
                slider.Value = e.NewValue == 0 ? 1 : e.NewValue;
                VM.SelectedDevice.Brightness = Convert.ToInt32(slider.Value);
            }
        }
        private void TemperatureSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            var slider = sender as Slider;
            if (slider != null)
            {
                VM.SelectedDevice.Temperature = Convert.ToInt32(slider.Value);
            }
        }

        private void TemperatureColorButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (VM.SelectedDevice.IsPowerOn)
            {
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Hand, 0);
            }
        }

        private void TemperatureColorButton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 0);
        }
        private void TemperatureColorButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            TemperatureColorButtonTag tag = (TemperatureColorButtonTag)button.Tag;
            if (tag.Temperature != null)
            {
                VM.SelectedDevice.Temperature = (int)tag.Temperature;
            }
            if (tag.RGBColor != null)
            {
                VM.SelectedDevice.RGBColor = (int)tag.RGBColor;
            }

        }
    }

    class TemperatureColorButtonTag
    {
        public int? Temperature { get; set; }
        public int? RGBColor { get; set; }
    }
}
