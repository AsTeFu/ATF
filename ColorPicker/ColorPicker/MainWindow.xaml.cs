using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ColorPicker {
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            Background = defColor;
            ColorText(defColor.Color);
        }

        private SolidColorBrush defColor = new SolidColorBrush(Color.FromRgb(255, 204, 0));

        char[] validate = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', '#', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        private void hexUp(object sender, EventArgs e) {
            string hex = this.hex.Text.ToUpper();
            if (hex.Trim(validate) == "" && hex.Length == 7) {
                byte[] rgbColor = HexToRGB(hex.Replace("#", ""));

                Color color = Color.FromRgb(rgbColor[0], rgbColor[1], rgbColor[2]);
                rgb.Text = $"{rgbColor[0]}, {rgbColor[1]}, {rgbColor[2]}";
                Background = new SolidColorBrush(color);
                ColorText(color);
            } else {
                rgb.Text = "rgb";
                Background = defColor;
                ColorText(defColor.Color);
            }

            
        }

        char[] validateRGB = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ',', ' ' };
        private void rgbUp(object sender, EventArgs e) {

            if (rgb.Text.Trim(validateRGB) == "") {
                byte[] rgbColor = new byte[3];

                byte n;
                string tmp = "";
                int colorIndex = 0;
                for (int i = 0; i < rgb.Text.Length; i++) {
                    if (rgb.Text[i] != ',') {
                        tmp += rgb.Text[i];
                    }
                    else {
                        if (byte.TryParse(tmp, out n)) {
                            rgbColor[colorIndex] = byte.Parse(tmp);
                            tmp = "";
                            colorIndex++;
                        } else {
                            hex.Text = "#";
                            Background = defColor;
                            ColorText(defColor.Color);
                            break;
                        }
                    }
                    
                    if (i == rgb.Text.Length - 1 && byte.TryParse(tmp, out n)) {
                        rgbColor[colorIndex] = n;
                    }
                }

                Color color = Color.FromRgb(rgbColor[0], rgbColor[1], rgbColor[2]);
                hex.Text = RGBtoHex(rgbColor);
                Background = new SolidColorBrush(color);
                ColorText(color);
            } else {
                hex.Text = "#";
                Background = defColor;
                ColorText(defColor.Color);
            }
        }

        private byte[] HexToRGB(string hex) {
            try {
                byte[] rgbColor = new byte[3];
                for (int i = 0; i < 3; i++) {
                    rgbColor[i] = HexToDec(hex[2 * i] + "" + hex[2 * i + 1]);
                }
                
                return rgbColor;
            } catch {
                return new byte[] { defColor.Color.R, defColor.Color.G, defColor.Color.B };
            }
        }
        private byte HexToDec(string hex) {
            byte num = 0;
            for (int i = 0; i < hex.Length; i++) {
                char tmp = hex[i];

                byte n;
                if (byte.TryParse(tmp.ToString(), out n)) {
                    num += (byte)(n * (i == 0 ? 16 : 1));
                } else {
                    num += (byte)((tmp - 'A' + 10) * (i == 0 ? 16 : 1));
                }
            }
            return num;
        }

        private string RGBtoHex(byte[] rgb) {
            return $"#{rgb[0]:X2}{rgb[1]:X2}{rgb[2]:X2}";
        }

        private void ColorText(Color color) {
            byte r = color.R;
            byte g = color.G;
            byte b = color.B;

            if (1 - (0.299 * r + 0.587 * g + 0.114 * b) / 255 < 0.5) {
                labelRGB.Foreground = new SolidColorBrush(Color.FromRgb(50, 50, 50));
                labelHEX.Foreground = new SolidColorBrush(Color.FromRgb(50, 50, 50));
            } else {
                labelRGB.Foreground = new SolidColorBrush(Color.FromRgb(240, 240, 240));
                labelHEX.Foreground = new SolidColorBrush(Color.FromRgb(240, 240, 240));
            }
  

                
        }
    }
}
