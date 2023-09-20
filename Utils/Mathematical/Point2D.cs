﻿using System;

namespace Utils.Mathematical
{
    public struct Point2D : IEquatable<Point2D>
    {
        public int X;
        public int Y;

        public static Point2D Zero => new(0, 0);

        public Point2D()
        {
            X = 0;
            Y = 0;
        }

        public Point2D(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point2D(int xy)
        {
            X = xy;
            Y = xy;
        }

        public static Point2D operator -(Point2D me)
        {
            return new Point2D(-me.X, -me.Y);
        }

        public static Point2D operator -(Point2D left, Point2D right)
        {
            return new Point2D(left.X - right.X, left.Y - right.Y);
        }

        public static Point2D operator +(Point2D left, Point2D right)
        {
            return new Point2D(left.X + right.X, left.Y + right.Y);
        }

        public static Point2D operator *(Point2D point, int num)
        {
            return new Point2D(point.X * num, point.Y * num);
        }

        public static Point2D operator *(int num, Point2D point)
        {
            return new Point2D(point.X * num, point.Y * num);
        }

        public static Point2D operator /(Point2D point, int num)
        {
            if (num == 0)
                throw new DivideByZeroException("除数不能为0");
            return new Point2D(point.X / num, point.Y / num);
        }

        public static bool operator ==(Point2D left, Point2D right)
        {
            return left.X == right.X && left.Y == right.Y;
        }

        public static bool operator !=(Point2D left, Point2D right)
        {
            return left.X != right.X || left.Y != right.Y;
        }

        public static explicit operator Vector2D(Point2D point)
        {
            return new Vector2D(point.X, point.Y);
        }

        /// <summary>
        /// 曼哈顿距离
        /// </summary>
        public static int Manhattan(Point2D left, Point2D right)
        {
            return Math.Abs(left.X - right.X) + Math.Abs(left.Y - right.Y);
        }

        /// <summary>
        /// 切比雪夫距离
        /// </summary>
        public static int Chebyshev(Point2D left, Point2D right)
        {
            int x = Math.Abs(left.X - right.X);
            int y = Math.Abs(left.Y - right.Y);
            return Math.Max(x, y);
        }

        public readonly bool Equals(Point2D other)
        {
            return this == other;
        }

        public override readonly bool Equals(object? obj)
        {
            return obj is Point2D p && Equals(p);
        }

        public override readonly int GetHashCode()
        {
            return Tuple.Create(X, Y).GetHashCode();
        }

        public override readonly string ToString()
        {
            return $"({X},{Y})";
        }
    }
}
