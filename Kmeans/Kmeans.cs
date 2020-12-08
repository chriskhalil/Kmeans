using System;
using System.Collections.Generic;
using System.Text;

namespace clustering
{
    public struct Point
    {
        public double[] dimension;
        public Point(int dim_num)
        {
            dimension = new double[dim_num];
        }

        public Point(int dim_num,params double[] dim)
        {
            dimension = new double[dim_num];
            SetPoint(dim);
        }
        public void SetPoint(params double[] dim)
        {
            for (int i = 0; i < dim.Length; ++i)
            {
                dimension[i] = dim[i];
            }
        }

        public Point MakeCopy()
        {
            var tmp = new Point(dimension.Length);
            tmp.SetPoint(dimension);
            return tmp;
        }
        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append("(");
            for (int i = 0; i < dimension.Length; ++i)
            {
                if (i == (dimension.Length - 1))
                    str.Append($"{dimension[i]}");
                else
                    str.Append($"{dimension[i]},");
            }
            str.Append(")");
            return str.ToString();
        }
    }

    public class Kmeans
    {
        public Point[] Centroids { get; private set; }
        private int max_iteration;
        private int k;
        private Random rand;
        int dimensions;
        private double FastExp(double x ,int n)
        {
            if (n==0) return 1;
            if (n==1) return x;
            if (n % 2 == 0) return FastExp(x * x, n / 2);
            //if (n % 2 != 0) 
                return x * FastExp(x * x, (n - 1) / 2);
        }

        private double l2_distance(Point x, Point y)
        {
          //this function does not check for range
          //make sure of range before calling the function
            double val = 0.0;
            for (int i = 0; i < dimensions; ++i)
            {
                //faster than  Math.pow(), Math.pow has alot of overhead in multi itr
                  val += FastExp(x.dimension[i] - y.dimension[i], 2);

            }
            return val;
        }

        public Kmeans(int k, int dim, int max_iter = 250)
        {
            rand = new Random();
            this.k = k;
            Centroids = new Point[k];
            max_iteration = max_iter;
            dimensions = dim;
        }

        public int[] Fit(List<Point> data)
        {

            //should not pass reference create new point for centroid
            for (int i = 0; i < k; ++i)
                Centroids[i] = data[rand.Next(0, data.Count)].MakeCopy();

            var labels = new int[data.Count];


            var new_means = new Point[k];
            for (int i = 0; i < Centroids.Length; ++i)
            {
                new_means[i] = new Point(dimensions);
            }
            var counts = new int[k];

            for (int itr = 0; itr < max_iteration; ++itr)
            {
                for (int point = 0; point < data.Count; ++point)
                {
                    double best_distance = Double.MaxValue;
                    int best_cluster = 0;
                    for (int cluster = 0; cluster < k; ++cluster)
                    {
                        double dist = l2_distance(data[point], Centroids[cluster]);
                        if (dist < best_distance)
                        {
                            best_distance = dist;
                            best_cluster = cluster;
                        }
                    }
                    labels[point] = best_cluster;
                }

                for (int point = 0; point < data.Count; ++point)
                {
                    var cluster = labels[point];

                    for (int i = 0; i < dimensions; ++i)
                    {
                        new_means[cluster].dimension[i] += data[point].dimension[i];
                    }
                    counts[cluster] += 1;
                }

                // Divide sums by counts to get new centroids.
                for (int cluster = 0; cluster < k; ++cluster)
                {
                    // Turn 0/0 into 0/1 to avoid zero division.
                    var count = Math.Max(1, counts[cluster]);
                    counts[cluster] = 0;
                    for (int i = 0; i < dimensions; ++i)
                    {
                        Centroids[cluster].dimension[i] = new_means[cluster].dimension[i] / count;

                        //opti reset here rather  than doing nested loops
                         new_means[cluster].dimension[i] = 0;
                    }
                }
            }

            return labels;
        }
    }
}
