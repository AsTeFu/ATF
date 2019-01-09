namespace ATF {
    namespace Timer {
        using UnityEngine;

        /// <summary>
        /// Класс с таймерами
        /// </summary>
        public class DelayTimer : MonoBehaviour {

            /// <summary>
            /// Делегат для передачи метода
            /// </summary>
            public delegate void NameMethod();

            /// <summary>
            /// Задержка между действиями
            /// </summary>
            /// <param name="canDoing">останавливает действие</param>
            /// <param name="currentTime">Текущее время</param>
            /// <param name="delay">Максимальное время задержки</param>
            /// /// <param name="method">Метод, вызываемый при достижении максимальной задержки</param>
            public static void Delay(ref bool canDoing, ref float currentTime, float maxDelay, NameMethod method) {
                if (!canDoing && currentTime < maxDelay) {
                    currentTime += Time.deltaTime;
                } else if (!canDoing && currentTime > maxDelay) {
                    currentTime = 0;
                    canDoing = true;
                    if (method != null) {
                        //Вызвать желаеммый метод
                        NameMethod m = new NameMethod(method);
                        m();
                    }
                }
            }
        }
    }
}
