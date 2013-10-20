using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxSum
{
    public class MaxSum
    {
        int row = 0;
        int col = 0;
        int ans = -0x3f3f3f3f;
        int start = 0;
        int end = 0;
        int x1 = 0;
        int y1 = 0;
        int x2 = 0;
        int y2 = 0;
        int[,] map = null;
        int[,] sum = null;
        int[,] mark = null;
        public int getsum(int x1, int y1, int x2, int y2)
        {
            if (x1 == 0 && y1 == 0)
            {
                return sum[x2, y2];
            }
            else if (x1 == 0)
            {
                return sum[x2, y2] - sum[x2, y2 - 1];
            }
            else if (y1 == 0)
            {
                if (x1 > x2)
                {
                    return sum[row - 1, y1] - getsum(x2, y2, x1, y1) + map[x2, y2] + map[x1, y1];
                }
                else
                {
                    return sum[x2, y2] - sum[x1 - 1, y1];
                }
            }
            else
            {
                if (x1 > x2)
                {
                    return sum[row - 1, y2] - sum[row - 1, y2 - 1] - getsum(x2, y2, x1, y1) + map[x1, y1] + map[x2, y2];
                }
                else
                {
                    return sum[x2, y2] - sum[x2, y2 - 1] - sum[x1 - 1, y2] + sum[x1 - 1, y1 - 1];
                }
            }
        }

        public int maxsum1(int[] a, int n)
        {
            int thisans = -0x3f3f3f3f;
            int thissum = 0;
            int pos = 0;
            start = 0;
            end = 0;
            for (int i = 0; i < n; i++)
            {
                thissum += a[i];
                if (thissum > thisans)
                {
                    thisans = thissum;
                    start = pos;
                    end = i;
                }
                if (thissum < 0)
                {
                    thissum = 0;
                    pos = i + 1;
                }
            }
            return thisans;
        }

        public void maxsum()
        {
            ans = 0;
            start = 0;
            end = 0;
            int[] b = new int[col + 1];
            for (int i = 0; i < row; i++)
            {
                for (int j = i; j < row; j++)
                {
                    for (int k = 0; k < col; k++)
                    {
                        b[k] = getsum(i, k, j, k);
                    }
                    int temp = maxsum1(b, col);
                    if (ans < temp)
                    {
                        ans = temp;
                        x1 = i;
                        x2 = j;
                        y1 = start;
                        y2 = end;
                    }
                }
            }
            for (int i = x1; i <= x2; i++)
            {
                for (int j = y1; j <= y2; j++)
                {
                    mark[i, j] = 1;
                }
            }
            //      MessageBox.Show(Convert.ToString(x1)+""+Convert.ToString(y1)+""+Convert.ToString(x2)+""+Convert.ToString(y2)+"");
        }
        
    }
}
