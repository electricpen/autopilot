using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Autopilot
{
    class Bar
    {
        public int left { get; set; }
        public int right { get; set; }
        public int y_position { get; set; }
        public Color color { get; set; }

        public Bar(int start, int end, int height, Color color)
        {
            left = start;
            right = end;
            y_position = height;
            this.color = color;
        }
        public int GetPercent(Bitmap screen)
        {
            int start = left;
            int end = right;
            int mid = GetMidpoint(start, end);
            bool foundEnd;
            Color currentColor;
            do
            {
                currentColor = screen.GetPixel(mid, y_position);
                foundEnd = mid == start || mid == end || IsLastPixel(currentColor, mid, screen) ? true : false;
                if (!foundEnd)
                {
                    if (currentColor.Equals(color))
                    {
                        start = mid;
                        mid = GetMidpoint(start, end);
                    } else
                    {
                        end = mid;
                        mid = GetMidpoint(start, end);
                    }
                } 
            } while (!foundEnd);
            return (mid - left) * 100 / (right - left - 1);
        }

        private bool IsLastPixel(Color color, int position, Bitmap screen)
        {
            if(color.Equals(this.color))
            {
                if (!screen.GetPixel(position + 1, y_position).Equals(this.color))
                {
                    return true;
                }
            } else
            {
                if (screen.GetPixel(position - 1, y_position).Equals(this.color))
                {
                    return true;
                }
            }
            return false;
        }

        private int GetMidpoint(int start, int end)
        {
            return ((end - start) / 2) + start;
        }
    }
}
