using System;
using System.Collections.Generic;
using System.Diagnostics;
namespace clustering
{
    class Program
    {

        static void Main(string[] args)
        {
           

            const int size = 100000;
            int dim = 2;
            var data = new List<Point>(size);
            var rand = new Random();

            int x, y;
            for (int i = 0; i < size; ++i)
            {
                  x = rand.Next(0, 255);
                  y = rand.Next(0, 255);
                data.Add(new Point(dim, x, y));
            }
            int k = 64;
            
            var model = new Kmeans(k,dim);

            var timer = new Stopwatch();
            timer.Start();
            var labels = model.Fit(data);
            timer.Stop();

            TimeSpan timeTaken = timer.Elapsed;
            Console.WriteLine($"elapsed time:{ timeTaken.ToString(@"m\:ss\.fff")}");



        }
    }
}
