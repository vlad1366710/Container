using System;
using System.Collections.Generic;
using System.Diagnostics;
class Program
{
   static int minBinCount = int.MaxValue;
   static  int[] bestArrangement;
    static int count_BF = 0;
    static int count_FFS = 0;
    static int count_P = 0;
    static void Main()
    {
        
        int count;
        int n = 0;// количество предметов
        int M = 0;// вместимость одного контейнера
        int[] m;
        bool flag = true;
        do
        {
            Console.WriteLine("Нажмите 1 для ручного ввода и 2 для рандомных значений и 3 для теста");
            count = Convert.ToInt32(Console.ReadLine());
        } while (count != 1 && count != 2 && count != 3);
        if (count == 1)
        {

            Console.WriteLine("Введите количество предметов");
            n = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Введите вместимость одного контейнера ");
            M = Convert.ToInt32(Console.ReadLine());
            m = new int[n]; // массы предметов

            for (int i = 0; i < m.Length; i++)
            {
                Console.WriteLine("Введите массу  предмета " + (i + 1));
                m[i] = Convert.ToInt32(Console.ReadLine());
            }
            Console.WriteLine("количество предметов " + n);
            Console.WriteLine("вместимость одного контейнера " + M);
            Console.WriteLine("BF");
            BF(n, M, m);
            Console.WriteLine("FFS");
            FFS(n, M, m);
            Console.WriteLine("перебор");
            enumeration(n, M, m);


        }
        else if (count == 2)


        {
            n = generateRandInt(3,4);
            M = generateRandInt(3,4);
            m = new int[n]; // массы предметов

            for (int i = 0; i < m.Length; i++)
            {
                m[i] = generateRandInt(1, M);
            }
            Console.WriteLine("количество предметов " + n);
            Console.WriteLine("вместимость одного контейнера " + M);
            Console.WriteLine("BF");
            BF(n, M, m);
            Console.WriteLine("FFS");
            FFS(n, M, m);
            Console.WriteLine("перебор");
            enumeration(n, M, m);

        }
        else if(count == 3)


        {
            Console.WriteLine("Введите количество предметов");
            n = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Введите вместимость одного контейнера ");
            M = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Введите желаемое количество тестов ");
            int test;
            test= Convert.ToInt32(Console.ReadLine());
            m = new int[n]; // массы предметов
            
            int l = 0;

            while (l < test)
            {
                minBinCount = int.MaxValue;

                m = new int[n]; // массы предметов

                for (int i = 0; i < m.Length; i++)
                {
                    m[i] = generateRandInt(1, M);
                }

                Console.WriteLine("Тест " + l);
                Console.WriteLine("количество предметов " + n);
                Console.WriteLine("вместимость одного контейнера " + M);
                Console.WriteLine("BF");
                BF(n, M, m);
                Console.WriteLine("FFS");
                FFS(n, M, m);
                Console.WriteLine("перебор");
                enumeration(n, M, m);
                Console.WriteLine("--------------------------------------------------");
                l++;
            }

        }
       
        Console.WriteLine("bf " + count_BF);
        Console.WriteLine("ffs " + count_FFS);
         Console.WriteLine("p " + count_P);
    }

    public static int generateRandInt(int minValue, int maxValue)
    {
        Random r = new Random();
        return r.Next(minValue, maxValue);
    }

    static void BF(int n, int M, int[] m)
    {

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        List<List<int>> containers = new List<List<int>>(); // список контейнеров
        containers.Add(new List<int>()); // добавляем первый контейнер
        for (int i = 0; i < n; i++)
        {
            int jMin = -1;
            for (int j = 0; j < containers.Count; j++)
            {
                int remainingSpace = M - containers[j].Sum();
                if (m[i] <= remainingSpace)
                {
                    // выбираем контейнер, в который, после добавления текущего элемента, остается минимум места
                    if (jMin == -1 || remainingSpace < M - containers[jMin].Sum())
                    {
                        jMin = j;
                    }
                }
            }

            // добавляем элемент либо в наилучший контейнер, либо создаем новый, если подходящего не нашлось
            if (jMin == -1)
            {
                containers.Add(new List<int> { m[i] });
            }
            else
            {
                containers[jMin].Add(m[i]);
            }
        }
        stopwatch.Stop();
        
        // выводим результаты
        Console.WriteLine("Количество использованных контейнеров: " + containers.Count);
        count_BF += containers.Count;
        for (int i = 0; i < containers.Count; i++)
        {

            Console.WriteLine("Контейнер " + (i + 1) + ": " + string.Join(", ", containers[i].Select(x => x)));
        }

        double vad = stopwatch.ElapsedTicks;
        Console.WriteLine("Time: " + vad / 10000 + " ms");
    }

        static void FFS(int n, int M, int[] m)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        List<int> weights = new List<int>();

        for (int i = 0; i < n; i++)
        {

            int weight = m[i];
            weights.Add(weight);
        }

        // Сортировка предметов по убыванию массы
        weights.Sort();
        weights.Reverse();

        List<List<int>> containers = new List<List<int>>();
        containers.Add(new List<int>());

        foreach (int weight in weights)
        {
            bool placed = false;

            // Поиск контейнера, в котором предмет может быть размещен
            foreach (List<int> container in containers)
            {
                if (container.Sum() + weight <= M)
                {
                    container.Add(weight);
                    placed = true;
                    break;
                }
            }

            // Если ни один контейнер не подходит, создаем новый контейнер
            if (!placed)
            {
                List<int> newContainer = new List<int>();
                newContainer.Add(weight);
                containers.Add(newContainer);
            }
        }
        stopwatch.Stop();

        Console.WriteLine();
        Console.WriteLine("Количество использованных контейнеров: " + containers.Count);
        count_FFS += containers.Count;

        for (int i = 0; i < containers.Count; i++)
        {
            Console.WriteLine("Предметы в контейнере " + (i + 1) + ": " + string.Join(", ", containers[i]));
        }
        
        double vad = stopwatch.ElapsedTicks;
        Console.WriteLine("Time: " + vad/10000 + " ms");
    }

    static void enumeration(int n, int M, int[] weights)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        Permute(weights, 0, weights.Length - 1, M);
        stopwatch.Stop();
        Console.WriteLine("Минимальное количество контейнеров: " + minBinCount);
        count_P += minBinCount;
        for (int i = 0; i < bestArrangement.Length; i++)
        {
            Console.Write(bestArrangement[i] + " ");
        }
        Console.WriteLine();
        double vad = stopwatch.ElapsedTicks;
        Console.WriteLine("Time: " + vad / 10000 + " ms");
    }
    
    static void Permute(int[] arr, int startIndex, int endIndex, int M)
        {
        if (startIndex == endIndex)
        {
            int binCount = ApplyFirstFit(arr, M);
            if (binCount < minBinCount)
            {
                minBinCount = binCount;
                bestArrangement = new int[arr.Length];
                Array.Copy(arr, bestArrangement, arr.Length);
            }
            
        }
        else
        {
            for (int i = startIndex; i <= endIndex; i++)
            {
                Swap(ref arr[startIndex], ref arr[i]);
                Permute(arr, startIndex + 1, endIndex, M);
                Swap(ref arr[startIndex], ref arr[i]);
            }
        }
    }

        static void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }
        static int ApplyFirstFit(int[] weights, int M)
        {
            int[] bins = new int[weights.Length];
            int binCount = 0;

            foreach (var weight in weights)
            {
                
                int j;
                for (j = 0; j < binCount; j++)
                {
                    if (bins[j] >= weight)
                    {
                        bins[j] -= weight;
                        break;
                    }
                }

                
                if (j == binCount)
                {
                    bins[binCount] = M - weight;
                    binCount++;
                }
            }

            return binCount;
        }
    }

    





