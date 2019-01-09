using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ATF {
    namespace Tools {
         public enum ColorCondition {
             Red, Green, Blue, Yellow, Orange, White, Black, Cyan, Aqua, Custom
         }

         [System.Serializable]
         public class ColorChoice {

            Dictionary<ColorCondition, Color> colors = new Dictionary<ColorCondition, Color> {
                { ColorCondition.Red, new Color(255, 0, 0) },
                { ColorCondition.Blue, new Color(0, 0, 255) },
                { ColorCondition.Green, new Color(0, 255, 0) },
                { ColorCondition.Yellow, new Color(255, 255, 0) },
                { ColorCondition.Orange, new Color(255, 165, 0) },
                { ColorCondition.White, new Color(255, 255, 255) },
                { ColorCondition.Black, new Color(0, 0, 0) },
                { ColorCondition.Cyan, new Color(0, 255, 255) },
                { ColorCondition.Aqua, new Color(164, 232, 237) }
            };


            [SerializeField] private ColorCondition currentColor = ColorCondition.Custom;
            [SerializeField] [ConditionalField("color", ColorCondition.Custom)] private Color customColor;
            [SerializeField] [Range(0, 1)] private float intesivity;
            public Color CurrentColor {
                get {
                    return GetColor();
                }
            }
            
            private Color GetColor() {
                    if (currentColor != ColorCondition.Custom) {
                        Color newColor = colors[currentColor];
                        newColor.a = (byte)(intesivity * 255);
                        return newColor;
                    } else return customColor;
                }

            public string ToHex() {
                    return $"#{(int)(CurrentColor.r * 255):X2}{(int)(CurrentColor.g * 255):X2}{(int)(CurrentColor.b * 255):X2}";
                }
         }
     }
}