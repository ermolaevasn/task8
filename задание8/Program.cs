using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace задание8
{
    class Program
    {
        static int[,] Delete(int[,] Array, int edge, ref int edges, int vershina)//удаление ребра
        {
            int[,] ArrayNew = new int[vershina, edges - 1];
            int t = 0;
            for (int i = 0; i < edges; i++)
            {
                if (i != edge)
                {
                    for (int j = 0; j < vershina; j++)
                        ArrayNew[j, t] = Array[j, i];
                    t++;
                }
            }
            edges = edges - 1;//количество ребер сократилось
            return ArrayNew;
        }

        static int FirstPoint(int[,] Array, int vershina, int edge)//первая вершина
        {
            for (int i = 0; i < vershina; i++)
            {
                if (Array[i, edge] == 1) return i;
            }
            return 0;
        }

        static int SecondPoint(int[,] Array, int vershina, int edge)//вторая вершина
        {
            int t = 0;
            for (int i = 0; i < vershina; i++)
            {
                if (Array[i, edge] == 1) t++;
                if (t == 2) return i;
            }
            return 0;
        }

        static bool IsFirstPoint(int[,] Array, int vershina, int edge, int top)//проверяет, является ли данная вершина 
        {                                                                      //первой в матрице в данном ребре
            for (int i = 0; i < vershina; i++)
            {
                if ((Array[i, edge] == 1) && (i == top)) return true;
                else if (Array[i, edge] == 1) return false;
            }
            return false;
        }

        static int[,] MakeArray(int[,] Array, int[,] ArrayNew, int vershina, int edges)
        {
            for (int i = 0; i < vershina; i++)
            {
                for (int j = 0; j < edges; j++)
                    ArrayNew[i, j] = Array[i, j];
            }
            return ArrayNew;
        }

        static bool Way(ref int[,] Array, int vershina, int edges, int point1, int point2)//проверка, можно ли добраться 
        {                                                                                // от вершины point1 до вершины point2
            for (int i = 0; i < edges; i++)
            {
                if (Array[point1, i] == 1)//находим ребро, исходящее из данной вершины
                {
                    if (IsFirstPoint(Array, vershina, i, point1))
                    {
                        if (SecondPoint(Array, vershina, i) == point2) return true;
                        else
                        {
                            int[,] ArrayNew = new int[vershina, edges];
                            ArrayNew = Delete(Array, i, ref edges, vershina);//удаляем ребро между точками, чтобы не пройти по нему второй раз
                            if (Way(ref ArrayNew, vershina, edges, SecondPoint(Array, vershina, i), point2)) return true;//поиск пути до point2 от конца данной грани
                            else return false;
                        }
                    }
                    else//если оно втрое в своем столбц матрицы
                    {
                        if (FirstPoint(Array, vershina, i) == point2) return true;
                        else
                        {
                            int[,] ArrayNew = new int[vershina, edges];
                            ArrayNew = Delete(Array, i, ref edges, vershina);//удаляем ребро между точками, чтобы не пройти по нему второй раз
                            if (Way(ref ArrayNew, vershina, edges, FirstPoint(Array, vershina, i), point2)) return true;
                            else return false;
                        }
                    }
                }
            }
            return false;//если при переборе всех граней, исходящих из вершины, не было найдено пути до point2, то мост
        }

        static bool Bridge(int[,] Array, int edge, int vershina, int edges)
        {//функция, определяющая, является ли данное ребро мостом
            int point1 = FirstPoint(Array, vershina, edge);//первая вершина ребра
            int point2 = SecondPoint(Array, vershina, edge);//вторая вершина ребра
            Array = Delete(Array, edge, ref edges, vershina);//удаляем данное ребро
            int[,] ArrayNew = new int[vershina, edges];
            ArrayNew = MakeArray(Array, ArrayNew, vershina, edges);
            if (Way(ref ArrayNew, vershina, edges, point1, point2))//если можно найти путь от первой вершины до втрой без удаленного ребра, то данное ребро не мост
            {
                Console.WriteLine(edge + " Не мост");
                return false;
            }
            else
            {
                Console.WriteLine(edge + "Мост");
                return true;
            }
        }

        static void Step(int[,] Array, ref int edges, int vershina, int begin)
        {
            int ones = 0;//переменная для подсчета количетсва ребер, исходящих из данной ввершины
            for (int i = 0; i < edges; i++)//подсчитываем количество ребер, исходящих из данного ребра
                if (Array[begin, i] == 1) ones++;
            for (int i = 0; i < edges; i++)
            {
                Bridge(Array, i, vershina, edges);//перебор ребер

            }
        }

        static void WriteArray(int[,] Array, int vershina, int edges)//вывод массива на экран
        {
            for (int i = 0; i < vershina; i++)
            {
                Console.Write(i + " ");
                for (int j = 0; j < edges; j++)
                    Console.Write(Array[i, j] + " ");
                Console.WriteLine();
            }
        }
        static bool Proverka(int[,] Array, int edges, int vershina)//проверка графа
        { 
            for (int i = 1; i < vershina; i++)
            {
                int[,] ArrayNew = new int[vershina, edges];
                ArrayNew = MakeArray(Array, ArrayNew, vershina, edges);
                if (!Way(ref ArrayNew, vershina, edges, 0, i)) return false;
            }

            for (int i = 0; i < vershina; i++)
            {
                int g = 0;//количество единиц в строке (граней)
                for (int j = 0; j < edges; j++)
                    if (Array[i, j] == 1) g++;//если встретили единицу, значит, есть грань
                if ((g == 0) || (g % 2 == 1)) return false;
            }
            return true;
        }

        static int[,] Generator(ref int vershina, ref int edges)
        {
            int[,] Array;
            bool ok;//переменная для проверки, является ли сгенерированный граф матрицей
            do
            {
                Random rnd = new Random();
                vershina = rnd.Next(3, 10);//количество вершин
                edges = rnd.Next(vershina, vershina * (vershina - 1) / 2 + 1);//количество граней не должно быть меньше количества вершин и не должно быть больше количества граней в полном графе с данным количеством вершин
                Array = new int[vershina, edges];//матрица
                for (int i = 0; i < edges; i++)//заполняем матрицу случайными числами
                {
                    int oneFirst;//первая вершина
                    int oneSecond;//вторая вершина
                    do//выбираем две вершины для единиц
                    {
                        oneFirst = rnd.Next(0, vershina);
                        oneSecond = rnd.Next(0, vershina);
                    } while (oneFirst == oneSecond);
                    for (int j = 0; j < vershina; j++)//заполняем столбец матрицы
                    {
                        if ((j == oneFirst) || (j == oneSecond)) Array[j, i] = 1;
                        else Array[j, i] = 0;
                    }
                }
                //делаем проверку на то, нет ли однаковых ребер, удаляем одно, если есть
                for (int i = 0; i < edges; i++)
                {
                    int oneFirst = FirstPoint(Array, vershina, i);//находим первую вершину
                    int oneSecond = SecondPoint(Array, vershina, i);//находим вторую вершину
                    for (int j = i + 1; j < edges; j++)//перебираем последующие ребра
                    {
                        if ((oneFirst == FirstPoint(Array, vershina, j) && (oneSecond == SecondPoint(Array, vershina, j))))
                        {
                            Array = Delete(Array, j, ref edges, vershina);//удаляем ребро
                            j--;
                        }
                    }
                }
                ok = Proverka(Array, edges, vershina);
            } while (!ok);
            return Array;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Нахождение мостов в графе");
            do
            {
                int vershina = 0;//переменная для хранения количества вершин
                int edges = 0;//переменная для хранения количества ребер
                int[,] Array = Generator(ref vershina, ref edges);
                Console.WriteLine("Ребра и вершины:");
                Console.WriteLine();
                WriteArray(Array, vershina, edges);//выводим массив на экран
                Console.WriteLine();
                Console.WriteLine("Список мостов:");
                Step(Array, ref edges, vershina, 0);//ищем и выводим мосты
                Console.ReadLine();
            } while (true);
        }
    }
}
