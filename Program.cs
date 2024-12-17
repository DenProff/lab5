using System;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Threading.Channels;

namespace lab5
{
    internal class Program
    {
        static Random rand = new Random();
        static void Main(string[] args)
        {
            bool isMatrixFormed = false; //флаг для проверки, сформирована ли матрица
            bool isJaggedArrayFormed = false; //флаг для проверки, сформирован ли рваный массив

            int answer;

            PrintMenu();
            do
            {
                answer = ReadNumber("Введите номер операции: ", "Ошибка при вводе пункта меню.", 1, 4);

                //организация меню
                switch (answer)
                {
                    case 1:
                        WorkMatrix(ref isMatrixFormed);
                        PrintMenu();
                        break;
                    case 2:
                        WorkJaggedArray(ref isJaggedArrayFormed);
                        PrintMenu();
                        break;
                    case 3:
                        WorkString();
                        PrintMenu();
                        break;
                    case 4:
                        Console.WriteLine("\nНажмите любую кнопку для выхода из программы...");
                        break;
                }
            } while (answer != 4);
        }

        //ввод числа
        #region ReadNumber
        static int ReadNumber(string messageFirst, string messageSecond)
        {
            int number = 0;
            bool isConvert = false;
            do
            {
                try
                {
                    Console.Write(messageFirst);
                    number = int.Parse(Console.ReadLine());
                    isConvert = true;
                }
                catch (FormatException) //ошибка неправильного формата входных данных
                {
                    Console.WriteLine("\n" + messageSecond + " Пожалуйста, попробуйте еще раз" + "\n");
                }
                catch (OverflowException) //ошибка переполнения типа int
                {
                    Console.WriteLine("\nВведенное число выходит за пределы типа int. Пожалуйста, попробуйте еще раз\n");
                }
            } while (!isConvert);
            return number;
        }
        #endregion

        //ввод числа с заданным диапазоном
        #region ReadNumber
        static int ReadNumber(string messageFirst, string messageSecond, int leftBorder, int rightBorder)
        {
            int number = 0;
            bool isConvert = false;
            do
            {
                try
                {
                    Console.Write(messageFirst);
                    number = int.Parse(Console.ReadLine());
                    if (number >= leftBorder && number <= rightBorder)
                        isConvert = true;
                    else
                        Console.WriteLine("\n" + messageSecond + $" Пожалуйста, введите целое число от {leftBorder} до {rightBorder}" + "\n");
                }
                catch (FormatException) //ошибка неправильного формата входных данных
                {
                    Console.WriteLine("\n" + messageSecond + " Пожалуйста, попробуйте еще раз" + "\n");
                }
                catch (OverflowException) //ошибка переполнения типа int
                {
                    Console.WriteLine("\nВведенное число выходит за пределы типа int. Пожалуйста, попробуйте еще раз\n");
                }
            } while (!isConvert);

            return number;
        }
        #endregion

        //ввод строки
        #region ReadString
        static string ReadString(string message)
        {
            Console.Write(message);
            string sentence = Console.ReadLine();

            return sentence;
        }
        #endregion

        //работа с матрицей
        #region WorkMatrix
        static void WorkMatrix(ref bool isMatrixFormed)
        {
            int[,] matrix = new int[0, 0]; //инициализация матрицы
            int answer;
            int readAnswer;
            PrintMenuMatrix();
            do
            {
                answer = ReadNumber("Введите номер операции: ", "Ошибка при вводе пункта меню.", 1, 4);
                PrintMenuMatrix();
                //организация меню
                switch (answer)
                {
                    case 1:
                        int rows = ReadNumber("Введите количество строк: ", "Ошибка при вводе количества строк.", 1, 25); //ввод количества строк матрицы
                        Console.WriteLine();
                        int columns = ReadNumber("Введите количество столбцов: ", "Ошибка при вводе количества столбцов.", 1, 25); //ввод количества столбцов матрицы
                        //выбор способа формирования матрицы
                        Console.WriteLine("\n1. Заполнить массив генератором случайных чисел");
                        Console.WriteLine("2. Заполнить массив ручным вводом\n");
                        readAnswer = ReadNumber("Введите номер операции: ", "Ошибка при вводе пункта меню.", 1, 2);
                        PrintMenuMatrix();
                        matrix = CreateMatrix(rows, columns, readAnswer); //формирование матрицы
                        PrintMenuMatrix();
                        Console.WriteLine("Двумерный массив успешно сформирован\n");
                        isMatrixFormed = true; //матрица создана
                        break;
                    case 2:
                        PrintMatrix(isMatrixFormed, matrix); //печать матрицы
                        break;
                    case 3:
                        if (!isMatrixFormed) //если до этой операции массив не был сформирован
                        {
                            Console.WriteLine("Двумерный массив не сформирован\n");
                            break;
                        }
                        if (isMatrixFormed && matrix.GetLength(1) == 25) //если достигнуто ограничение по количеству столбцов
                        {
                            Console.WriteLine("Достигнуто ограничение количества столбцов двумерного массива (25 столбцов)\n");
                            break;
                        }
                        int numberColumns;
                        if (matrix.GetLength(1) == 24) //случай, когда в связи с ограничением можно добавить только один столбец
                        {
                            Console.WriteLine("В массив будет добавлен только 1 столбец, иначе будет превышено ограничение по их количеству");
                            numberColumns = 1;
                        }
                        else
                        {
                            PrintMatrix(isMatrixFormed,matrix);
                            numberColumns = ReadNumber($"Введите количество добавляемых столбцов (от 1 до {25 - matrix.GetLength(1)}): ", "Ошибка при вводе количества столбцов.", 1, 25 - matrix.GetLength(1));
                        }
                        //выбор способа формирования столбцов
                        Console.WriteLine("\n1. Заполнить столбцы генератором случайных чисел");
                        Console.WriteLine("2. Заполнить столбцы ручным вводом\n");
                        readAnswer = ReadNumber("Введите номер операции: ", "Ошибка при вводе пункта меню.", 1, 2);
                        PrintMenuMatrix();
                        matrix = AddColumnsMatrix(numberColumns, readAnswer, matrix); //добавление столбцов в матрицу
                        PrintMenuMatrix();
                        PrintMatrix(isMatrixFormed, matrix); //печать матрицы
                        Console.WriteLine("Столбцы были успешно добавлены\n");
                        break;
                    case 4:
                        isMatrixFormed = false; //выход из меню работы с матрицей, опускаем флаг
                        break;
                }
            } while (answer != 4);
        }

        //формирование матрицы
        #region CreateMatrix
        static int[,] CreateMatrix(int rows, int columns, int readAnswer)
        {
            int[,] matrix = new int[rows, columns]; //формирование матрицы
            //организация меню
            switch (readAnswer)
            {
                case 1:
                    for (int i = 0; i < matrix.GetLength(0); i++) //цикл по строкам
                    {
                        for (int j = 0; j < matrix.GetLength(1); j++) //цикл по столбцам
                           matrix[i, j] = rand.Next(-200, 200); //заполнение случайными числами
                    }
                    break;
                case 2:
                    for (int i = 0; i < matrix.GetLength(0); i++) //цикл по строкам
                    {
                        for (int j = 0; j < matrix.GetLength(1); j++) //цикл по столбцам
                        {
                            int number = ReadNumber($"Введите {j + 1}-й элемент {i + 1}-й строки массива: ", "Ошибка при вводе целого элемента массива.");
                            matrix[i, j] = number;
                        }
                        Console.WriteLine();
                    }
                    break;
            }
            return matrix;
        }
        #endregion

        //печать матрицы
        #region PrintMatrix
        static void PrintMatrix(bool isMatrixFormed, int[,] matrix)
        {
            if (!isMatrixFormed) //если матрица не сформирована
            {
                Console.WriteLine("Двумерный массив не сформирован\n");
                return;
            }
            for (int i = 0; i < matrix.GetLength(0); i++) //цикл по строкам
            {
                for (int j = 0; j < matrix.GetLength(1); j++) //цикл по столбцам
                    Console.Write($"{matrix[i, j],-5}"); //вывод элемента
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        #endregion

        //добавление столбцов
        #region AddColumnsMatrix
        static int[,] AddColumnsMatrix(int numberColumns, int readAnswer, int[,] matrix)
        {
            //формирование дополнительной матрицы с требуемым количеством столбцов
            int[,] extraMatrix = new int[matrix.GetLength(0), matrix.GetLength(1) + numberColumns];
            //заполнение новой матрицы элементами исходной
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                    extraMatrix[i, j] = matrix[i, j];
            }
            //организация меню
            switch (readAnswer)
            {
                case 1:
                    while (numberColumns > 0)
                    {
                        for (int i = 0; i < matrix.GetLength(0); i++) //заполнение новых столбцов
                            extraMatrix[i, extraMatrix.GetLength(1) - numberColumns] = rand.Next(-200, 200); //заполнение случайными числами
                        numberColumns--;
                    }
                    break;
                case 2:
                    while (numberColumns > 0)
                    {
                        for (int i = 0; i < matrix.GetLength(0); i++) //заполнение новых столбцов
                        {
                            int number = ReadNumber($"Введите {i + 1}-й элемент {extraMatrix.GetLength(1) - numberColumns + 1}-ого столбца массива: ", "Ошибка при вводе целого элемента массива.");
                            extraMatrix[i, extraMatrix.GetLength(1) - numberColumns] = number;
                        }
                        Console.WriteLine();
                        numberColumns--;
                    }
                    break;
            }
            return extraMatrix;
        }
        #endregion

        //печать меню для двумерного массива
        #region PrintMenuMatrix
        static void PrintMenuMatrix()
        {
            Console.Clear();
            Console.WriteLine("1. Сформировать двумерный массив");
            Console.WriteLine("2. Распечатать двумерный массив");
            Console.WriteLine("3. Добавить столбцы в конец двумерного массива");
            Console.WriteLine("4. Вернуться назад\n");
        }
        #endregion

        #endregion

        //работа с рваным массивом
        #region WorkJaggedArray
        static void WorkJaggedArray(ref bool isJaggedArrayFormed)
        {
            int[][] jaggedArray = []; //инициализация рваного массива
            int answer;
            int readAnswer;
            PrintMenuJaggedArray();

            do
            {
                answer = ReadNumber("Введите номер операции: ", "Ошибка при вводе пункта меню.", 1, 4);
                PrintMenuJaggedArray();

                //организация меню
                switch (answer)
                {
                    case 1:
                        int rows = ReadNumber("Введите количество строк: ", "Ошибка при вводе количества строк.", 1, 25); //ввод количества строк рваного массива
                        //выбор способа формирования матрицы
                        Console.WriteLine("\n1. Заполнить строки массива генератором случайных чисел");
                        Console.WriteLine("2. Заполнить строки массива ручным вводом\n");
                        readAnswer = ReadNumber("Введите номер операции: ", "Ошибка при вводе пункта меню.", 1, 2);
                        PrintMenuJaggedArray();
                        jaggedArray = CreateJaggedArray(rows, readAnswer); //формирование рваного массива
                        PrintMenuJaggedArray();
                        Console.WriteLine("Рваный массив успешно сформирован\n");
                        isJaggedArrayFormed = true; //рваный массив создан
                        break;
                    case 2:
                        PrintJaggedArray(isJaggedArrayFormed, jaggedArray); //печать рваного массива
                        break;
                    case 3:
                        if (!isJaggedArrayFormed) //если массив не сформирован
                        {
                            Console.WriteLine("Рваный массив не сформирован\n");
                            break;
                        }
                        if (jaggedArray.Length == 0) //если массив пустой
                        {
                            Console.WriteLine("Рваный массив пустой\n");
                            break;
                        }
                        PrintJaggedArray(isJaggedArrayFormed, jaggedArray);
                        //ввод числа, которое должно содержаться в удаляемой строке
                        int number = ReadNumber($"Введите число, которое должно содержаться в удаляемой строке: ", "Ошибка при вводе целого числа.");
                        int rowNumber = FindRow(jaggedArray, number); //поиск строки с заданным числом
                        if (rowNumber == -1) //если строки с числом не найдено
                        {
                            PrintMenuJaggedArray();
                            PrintJaggedArray(isJaggedArrayFormed, jaggedArray);
                            Console.WriteLine($"Строка с числом {number} в массиве не найдена\n");
                            break;
                        }
                        if (jaggedArray.Length == 1) //если строка в массиве одна
                        {
                            jaggedArray = [];
                            PrintMenuJaggedArray();
                            PrintJaggedArray(isJaggedArrayFormed, jaggedArray);
                            Console.WriteLine("Строка с номером 1 была успешно удалена\n");
                        }
                        else
                        {
                            jaggedArray = DeleteRow(jaggedArray,rowNumber); //удаление строки
                            PrintMenuJaggedArray();
                            PrintJaggedArray(isJaggedArrayFormed, jaggedArray);
                            Console.WriteLine($"Строка с номером {rowNumber + 1} была успешно удалена\n");
                        }
                        break;
                    case 4:
                        isJaggedArrayFormed = false; //выход из меню работы с рваным массивом, опускаем флаг
                        break;
                }
            } while (answer != 4);
        }

        //формирование рваного массива
        #region CreateJaggedArray
        static int[][] CreateJaggedArray(int rows, int readAnswer)
        {
            int[][] jaggedArray = new int[rows][]; //формирование рваного массива
            //организация меню
            switch (readAnswer)
            {
                case 1:
                    for (int i = 0; i < jaggedArray.Length; i++)
                        jaggedArray[i] = new int[rand.Next(1, 25)]; //создание строк со случайной длиной
                    for (int i = 0; i < jaggedArray.Length; i++)
                    {
                        for (int j = 0; j < jaggedArray[i].Length; j++)
                            jaggedArray[i][j] = rand.Next(-200, 200); //заполнение строк случайными числами
                    }
                    break;
                case 2:
                    for (int i = 0; i < jaggedArray.Length; i++) //цикл по строкам массива
                    {
                        int lengthRow = ReadNumber($"Введите длину {i + 1}-й строки массива: ", "Ошибка при вводе длины строки массива.", 1, 25);
                        jaggedArray[i] = new int[lengthRow];
                        for (int j = 0; j < lengthRow; j++) //цикл по элементам строк массива
                        {
                            int number = ReadNumber($"Введите {j + 1}-й элемент {i + 1}-й строки массива: ", "Ошибка при вводе целого элемента массива.");
                            jaggedArray[i][j] = number;
                        }
                        Console.WriteLine();
                    }   
                    break;
            }
            return jaggedArray;
        }
        #endregion

        //печать рваного массива
        #region PrintJaggedArray
        static void PrintJaggedArray(bool isJaggedArrayFormed, int[][] jaggedArray)
        {
            if (!isJaggedArrayFormed) //если рваный массив не был сформирован
            {
                Console.WriteLine("Рваный массив не сформирован\n");
                return;
            }
            if (jaggedArray.Length == 0) //если рваный массив пустой
            {
                Console.WriteLine("Рваный массив пустой\n");
                return;
            }
            for (int i = 0; i < jaggedArray.Length; i++) //цикл по строкам массива
            {
                for (int j = 0; j < jaggedArray[i].Length; j++) //цикл по элементам строк
                    Console.Write($"{jaggedArray[i][j],-5}"); //вывод элемента
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        #endregion

        //поиск строки с заданным элементом
        #region FindRow
        static int FindRow(int[][] jaggedArray, int number)
        {
            int rowNumber = -1;
            for (int i = 0; i < jaggedArray.Length; i++)
            {
                rowNumber = Array.IndexOf(jaggedArray[i], number) != -1 ? i : -1;
                if (rowNumber != -1)
                    break;
            }
            return rowNumber;
        }
        #endregion

        //удаление строки
        #region DeleteRow
        static int[][] DeleteRow(int[][] jaggedArray, int rowNumber)
        {
            int[][] extraJaggedArray = new int[jaggedArray.Length - 1][];
            for (int i = 0, j = 0; i < extraJaggedArray.Length; i++, j++)
            {
                if (i == rowNumber)
                    j++;
                extraJaggedArray[i] = new int[jaggedArray[j].Length];
                for (int k = 0; k < jaggedArray[j].Length; k++)
                {
                    extraJaggedArray[i][k] = jaggedArray[j][k];
                }
            }
            return extraJaggedArray;
        }
        #endregion

        //печать меню для рваного массива
        #region PrintMenuJaggedArray
        static void PrintMenuJaggedArray()
        {
            Console.Clear();
            Console.WriteLine("1. Сформировать рваный массив");
            Console.WriteLine("2. Распечатать рваный массив");
            Console.WriteLine("3. Удалить первую строку, в которой встречается заданное число");
            Console.WriteLine("4. Вернуться назад\n");
        }
        #endregion

        #endregion

        //работа со строкой
        #region WorkString
        static void WorkString()
        {
            int answer;
            PrintMenuString();
            do
            {
                answer = ReadNumber("Введите номер операции: ", "Ошибка при вводе пункта меню.", 1, 3);
                string sentence, result; //инициализация строки и результата ее обработки
                PrintMenuString();
                //организация меню
                switch (answer)
                {
                    case 1:
                        Console.WriteLine("Строка будет обработана по следующему принципу: перевернуть каждое предложение, заканчивающееся символом ’!’.\n");
                        sentence = ReadString("Введите строку: "); //ввод строки
                        result = ProcessString(sentence);
                        if (Regex.IsMatch(result, @"^\s*$")) //если строка пустая
                            Console.WriteLine("\nСтрока пустая\n");
                        else
                            Console.WriteLine($"\nПолученный результат: {result}\n"); //вывод обработанной строки
                        break;
                    case 2:
                        sentence = "В лесу родилась елочка! В лесу она росла. Зимой и летом стройная, зеленая была!"; //тестовая строка
                        result = ProcessString(sentence);
                        Console.WriteLine($"Полученный результат: {result}\n");
                        break;
                    case 3:
                        break;
                }
            } while (answer != 3);
        }

        //обработка строки
        #region ProcessString
        static string ProcessString(string input)
        {
            input = Regex.Replace(input, @" {2,}", " "); //удаление повторяющихся пробелов
            input = Regex.Replace(input, @"\t{2,}", "\t"); //удаление повторяющихся пробелов
            input = Regex.Replace(input, @"(\w+)([,:!])(\w+)", "$1$2 $3"); //удаление повторяющихся знаков препинания
            //регулярное выражение для поиска предложений, заканчивающихся на '!'
            string pattern = @"\b[A-ZА-ЯЁ][A-ZА-ЯЁa-zа-яё,:; ]*(?<!,|:|;)!(?:\s|$)";
            //заменяем найденные предложения перевёрнутыми
            string result = Regex.Replace(input, pattern, ReverseSentence);
            return result;
        }
        #endregion

        //переворот строки
        #region ReverseString
        static string ReverseSentence(Match match)
        {
            //получаем предложение из найденных совпадений
            string sentence = match.Value.Trim();
            //делаем первую букву строчной и удаляем восклицательный знак
            sentence = Char.ToLower(sentence[0]) + sentence.Substring(1, sentence.Length - 2);
            //разделяем предложение на слова и разделители (пробелы, запятые, восклицательные знаки)
            string[] words = Regex.Split(sentence, @"(\s)"); //сохраняем разделители

            //переворачиваем массив слов
            Array.Reverse(words);
            //обработка знаков препинания
            for (int i = 0; i < words.Length; i++)
            {
                if (",:;".Contains(words[i].Last()))
                {
                    words[i - 2] = string.Concat(words[i - 2], words[i].Trim().Last());
                    words[i] = Regex.Replace(words[i], "[,:;]", "");
                }
            }

            //собираем предложение обратно
            string reversed = string.Join("", words).Trim();

            //делаем первую букву заглавной
            reversed = Char.ToUpper(reversed[0]) + reversed.Substring(1, reversed.Length - 1);
            return reversed + "! "; //возвращаем полученное предложение и добавляем восклицательный знак
        }
        #endregion

        //печать меню для строки
        #region PrintMenuString
        static void PrintMenuString()
        {
            Console.Clear();
            Console.WriteLine("1. Ввести строку");
            Console.WriteLine("2. Использовать заранее сформированную строку");
            Console.WriteLine("3. Вернуться назад\n");
        }
        #endregion

        #endregion

        //печать меню
        #region PrintMenu
        static void PrintMenu()
        {
            Console.Clear();
            Console.WriteLine("1. Работа с двумерными массивами");
            Console.WriteLine("2. Работа с рваными массивами");
            Console.WriteLine("3. Работа со строками");
            Console.WriteLine("4. Выход\n");
        }
        #endregion
    }
}
