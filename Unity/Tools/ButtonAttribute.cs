#if UNITY_EDITOR
using System;

namespace ATF {
    namespace ButtonsEditor {

        public enum Enabled {
            EnabledPlayMode, DisablePlayMode, AlwaysEnabled
        }

        /// <summary>
        /// Создает кнопку в инспекторе, вызывающую метод помеченный атрибутом
        /// </summary>
        [AttributeUsage(AttributeTargets.Method)]
        public sealed class ButtonAttribute : Attribute {

            public string tooltip = "";
            public Enabled enabled;            
            
            public ButtonAttribute(Enabled enabled = Enabled.AlwaysEnabled) {
                this.enabled = enabled;
            }

            public ButtonAttribute(string tooltip, Enabled enabled = Enabled.AlwaysEnabled) {
                this.tooltip = tooltip;
                this.enabled = enabled;
            }
        }
        
    }
}
#endif