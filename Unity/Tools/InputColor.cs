using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ATF {
    namespace Colors {

        public enum Colors {
            Red, Green, Blue, Yellow, Orange, White, Black, Cyan, Aqua, Custom
        }

        [System.Serializable]
        public class InputColor {

            [SerializeField] private Colors color = Colors.Custom;
            [SerializeField] [ConditionalField("color", Colors.Custom)] private Color customColor;
            [SerializeField] [Range(0, 1)] private float intesivity;

            private Color ListColors(Colors color) {
                byte alpha = (byte)(intesivity * 255);
                switch (color) {
                    case Colors.Red:
                        return new Color32(255, 0, 0, alpha);
                    case Colors.Blue:
                        return new Color32(0, 0, 255, alpha);
                    case Colors.Green:
                        return new Color32(0, 255, 0, alpha);
                    case Colors.Yellow:
                        return new Color32(255, 255, 0, alpha);
                    case Colors.Orange:
                        return new Color32(255, 165, 0, alpha);
                    case Colors.White:
                        return new Color32(255, 255, 255, alpha);
                    case Colors.Black:
                        return new Color32(0, 0, 0, alpha);
                    case Colors.Cyan:
                        return new Color32(0, 255, 255, alpha);
                    case Colors.Aqua:
                        return new Color32(164, 232, 237, alpha);
                    case Colors.Custom:
                        return customColor;
                }
                return customColor;
            }

            public Color GetColor() {
                return ListColors(color);
            }
        }
    }
}