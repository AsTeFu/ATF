
# Нейронная сеть 
Распознает цифру из изображения 28x28 пискелей. Обучалась на базе данных  MNIST. Тестирование показывает значение эффективности почти 98%! 

**Структура**

Количество узлов в:
* Входном слое —   28*28=784, каждый соответствует цвету пикселя
* Cкрытом слое — 200
* Выходном слое — 10, каждый соответствует определенной цифре

В качестве функции активации используется сигмоида.
Коэффицент обучения — 0.01. Количество итераций цикла тренировки сети — 10 (Успел сделать всю домашку за эти 2 часа -_-).

**Внутренности**
* **Темные области** — части изображения, которые соответсвуют цифре. По темным участкам можно попытаться обвести число. Например, маркер "3" прослеживается явно.
* **Светлые области** — области, которые наверняка не принадлежат числу. Так, маркер "0" имеет светлую область внутри себя.
* **Серые области** — выступают в роли шума.

![screenshot](https://pp.userapi.com/c851420/v851420636/a0201/lvcnZr9_Zxk.jpg)

Отчетливо прослеживаются, разве что, образ трех и нуля и, возможо, восьми. Самый мистический образ - 9. Думаю, все неправельные ответы были получены именно на нем, хуже всего определяется нейронной сетью. Магия...

**Можно пощупать?**

В приложение, написанном на коленках за ночь, можно нарисовать любимое число или, например, оставить подпись. После чего спросить у *Джарвиса*, что он видит. Видит он, кстати, не очень, часто ошибается. С чем это связано, могу только догадываться.
 
![screenshot](https://pp.userapi.com/c851324/v851324878/9c37e/BDVQY4yZpjM.jpg)


# ATF Library
## Дроби
Представление рациональных чисел, арифмитические операции и математические функции

**Подключение**
```c#
using ATF.RationalNumbers;
```
**Инициализация**
```c#
Rational a = new Rational(1, 4); // Output: 1//4
Rational b = 0.25; // Output: 1//4
Rational c = (1, 4); // Output: 1//4
Rational d = 1 / 4d; // Output: 1//4
```
**Арифмитические операции**
```c#
Rational addition = new Rational(1, 4) + 4; //  Output: 9//4
Rational subtraction = new Rational(5) - (1, 2); // Output:  9//2
Rational multiplication = new Rational(2, 3) * 1.5; //  Output: 1
Rational division = addition / 5.4; //  Output: 15//16
```
**Парсинг**
```c#
Rational rational1 = Rational.Parse("-3/4"); //	output: -3//4

if (Rational.TryParse("2.4", out Rational rational2)) {
	//do smth....
	//output: 12//5
}
```
**Математические операции**

Находятся в классе MathRational

| Название  | Описание | Использование | Результат |
| :-: | :---------: | :-------: | :-: |
| Abs | Возвращает абсолютное значение указанного числа | Abs(-2.5) | 3//2 |
| Floor | Возвращает наибольшее целое число, меньшее или равное указанному числу | Floor(12/5d) | 2 |
| Ceiling | Возвращает наименьшее целое число, большее или равное указанному числу | Floor(12/5d) | 3 |
| Min | Возвращает меньшее из чисел | Min(2.3, 4.5, 1.25) | 5//4 |
| Max | Возвращает большее из чисел | Max(2.3, 4.5, 1.25) | 9//2 |
| Pow | Возвращает указанное число, возведенное в указанную степень | Pow(1.25, 2) | 25//16 |
| Sign | Возвращает значение, указывающее знак числа | Sign((-1, 8)) | -1 |
| Truncate | Возвращает целую часть числа | Truncate(17/4d) | 4 |

## Комплексные числа
Арифмитические операции и больше ничего, учил билет с комлпексными числами по матану прост

**Подключение**
```c#
using ATF.Complex;
```
**Инициализация**
```c#
Complex a = new Complex(5, 3);
Complex b = 5;
Complex c = (1, 4);
```
## Перевод чисел между системами счисления
Переводит числа между системами счисления почему-то только положительных, но с дробной частью

**Подключение**
```c#
using ATF.Сomplex;
```

**Инициализация**

Сам объект неизменяемый, но над ним определенны арифмитические операции
```c#
NumeralSystem number1 = 5;
NumeralSystem number2 = new NumeralSystem("E2", 16);
NumeralSystem number3 = ("1010", 2);

NumeralSystem number4 = number1 + number2;
```
**Перевод**

Возвращает строку
```c#
number1.TranslateSystem(2); //101
number2.TranslateSystem(13); //145
number3.TranslateSystem(9); //1
number4.TranslateSystem(10); //231
```

## Перестановки
Перестановки, разложение в произведение независимых циклов и траспозиций, декремнт, знак, порядок и возведение в степень

```c#
Permutation example = new Permutation(10, 4, 6, 8, 3, 2, 5, 7, 9, 1);

Console.WriteLine(example.PrintCycle());
Console.WriteLine(example.PrintTransposition());

Console.WriteLine("Декремент: " + example.decrement);
Console.WriteLine("Знак: " + example.sign);
Console.WriteLine("Порядок: " + example.order);

Console.WriteLine(example.Pow(87));
```
![screenshot](https://pp.userapi.com/c849336/v849336206/145ef7/ewSkAW_D5NE.jpg)


# А вот тут много всякого нерабочего.exe
## COLOR PICKER
Перевод из HEX в RGB и обратно

![screenshot](https://pp.userapi.com/c846221/v846221972/15a4fa/EpPALQQ0uf8.jpg)
## Решение СЛУ
Устал считать матрицы, огромные дроби? Тогда тебе сюда....
Элементарные преобразования над матрицами, перевод в ступенчатый вид, в модифицированный вид Йорданна-Гауса. Поддерживает дроби и не поддерживает нормальный интерфейс. 

![screenshot](https://pp.userapi.com/c845120/v845120852/15d95e/rJJDkxm2Cao.jpg)
![screenshot](https://pp.userapi.com/c846016/v846016553/16995e/EFkELeNd9RY.jpg)

## Classic 2048
Немного живая 2048 и огромная куча баг... куча фич.

![screenshot](https://pp.userapi.com/c846120/v846120409/166cec/17q7VteMBq8.jpg)

## А также куча, ждущая своего часа

* Недоделанный калькулятор
* Сжатие по Хаффману 
* Шифр Цезаря
* Представление логической функции в виде СДНФ

# Простые числа
Поиск простых чисел до заданного числа N путем немного оптимизированного решета Эратосфена. Довольно медленно, но значительного сокращения времени можно добиться только распараллеливанием этого решета, хотя у меня не получилось. Но я видел алгоритмы, написанные бородатыми дядьками и работающие за секунду для N равного 10^9. Снова магия...

| Заданное N | Кол-во простых чисел | Время (s) | Тиков процессора|
|--|--|--|--|
| 100| 25 | 0 | 34 |
| 1 000| 168 | 0 | 81 |
| 10 000| 1 229 | 0 | 433 |
| 100 000| 9 592 | 0 | 3 898 |
| 1 000 000| 78 498| 0,005 |56056|
| 5 000 000| 348 513 | 0,029  | 292 203|
|10 000 000 |664 579| 0,059 | 598 802|
|100 000 000 |5 761 455| 1,195 |11 956 908|
|1 000 000 000|50 847 534| 14,185  |141 868 722|
|2 000 000 000|98 222 287| 29,264  |98 222 287|

Вообще все эти замеры нужно проделать несколько раз и взять усредненный показатель, а так все это сильно зависит от настроения моего компьютера.
Увеличить N особо не получается из-за различных ограничений по памяти.

# Unity
## 2D Platformer
* Передвижение
* Прыжок 
Много кода там, можно выбирать режимы прыжка и *возможно* корректная обработка приземления
* Плавное *(или не очень)* следование камеры за персонажем
* Встряхивание камеры
* И еще пару крутых *и не очень* скриптов, например pool объектов

## Tools
 * Таймер или классный костыль, лучше не использовать
 * Атрибуты
   * ReadOnly - запрещает редактировать поле в инспекторе
   * MustBeAssigned - оповещает о заданном поле
   * ConditionalField - открывает поля в зависимости от значения
   * Button -  создает кнопку в инспекторе
* Поле типа Color с пресетами
* Контур колайдера
