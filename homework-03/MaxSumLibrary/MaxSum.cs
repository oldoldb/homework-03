using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxSumLibrary
{
    public class MaxSum
    {
        int start = 0;
        int end = 0;
        int startmin = 0;
        int endmin = 0;
        int x1 = 0;
        int y1 = 0;
        int x2 = 0;
        int y2 = 0;
        int[,] sum = null;
        int[,] visit = null;
        int[,] tempmark = null;
        int[] dx = new int[4] { 0, 1, 0, -1 };
        int[] dy = new int[4] { 1, 0, -1, 0 };

        public MaxSum(int[,] map,int row,int col,ref int ans,ref int[,] mark,int mode)
        {
            mark = new int[row, col];
            initsum(map,row,col);
            switch (mode)
            {
                case 1:
                    maxsum(map, row, col, ref mark, ref ans);
                    break;
                case 2:
                    maxsum_h(map, row, col, ref mark, ref ans);
                    break;
                case 3:
                    maxsum_v(map, row, col, ref mark, ref ans);
                    break;
                case 4:
                    maxsum_vh(map, row, col, ref mark, ref ans);
                    break;
                case 5:
                    maxsum_a(false, map, row, col, ref mark, ref ans);
                    break;
                case 6:
                    maxsum_a(true, map, row, col, ref mark, ref ans);
                    break;
                default:
                    break;
            }    
        }

        public void initsum(int[,] map,int row,int col)
        {
            sum = new int[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        sum[i, j] = map[i, j];
                    }
                    else if (i == 0)
                    {
                        sum[i, j] = sum[i, j - 1] + map[i, j];
                    }
                    else if (j == 0)
                    {
                        sum[i, j] = sum[i - 1, j] + map[i, j];
                    }
                    else
                    {
                        sum[i, j] = sum[i - 1, j] + sum[i, j - 1] - sum[i - 1, j - 1] + map[i, j];
                    }
                }
            }
        }

        public int getsum(int x1, int y1, int x2, int y2,int[,] map,int row,int col)
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
                    return sum[row - 1, y1] - getsum(x2, y2, x1, y1,map,row,col) + map[x2, y2] + map[x1, y1];
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
                    return sum[row - 1, y2] - sum[row - 1, y2 - 1] - getsum(x2, y2, x1, y1,map,row,col) + map[x1, y1] + map[x2, y2];
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

        public int minsum(int[] a, int n)
        {
            int thisans = 0x3f3f3f3f;
            int thissum = 0;
            int pos = 0;
            startmin = 0;
            endmin = 0;
            for (int i = 0; i < n; i++)
            {
                thissum += a[i];
                if (thissum < thisans)
                {
                    thisans = thissum;
                    startmin = pos;
                    endmin = i;
                }
                if (thissum > 0)
                {
                    thissum = 0;
                    pos = i + 1;
                }
            }
            return thisans;
        }
        
        public int maxsum(int[,] map,int row,int col,ref int[,] mark,ref int ans)
        {
            start = 0;
            end = 0;
            int[] b = new int[col + 1];
            for (int i = 0; i < row; i++)
            {
                for (int j = i; j < row; j++)
                {
                    for (int k = 0; k < col; k++)
                    {
                        b[k] = getsum(i, k, j, k,map,row,col);
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
            return ans;
        }

        public void maxsum_h(int[,] map,int row,int col,ref int[,] mark,ref int ans)
        {
            int all = 0;
            ans = 0;
            start = 0;
            end = 0;
            int[] b = new int[col + 1];
            for (int i = 0; i < row; i++)
            {
                for (int j = i; j < row; j++)
                {
                    all = 0;
                    for (int k = 0; k < col; k++)
                    {
                        b[k] = getsum(i, k, j, k,map,row,col);
                        all += b[k];
                    }
                    int tempmax = maxsum1(b, col);
                    int tempmin = minsum(b, col);
                    int tempans;
                    int tempstart;
                    int tempend;
                    if (tempmax > all - tempmin)
                    {
                        tempans = tempmax;
                        tempstart = start;
                        tempend = end;
                    }
                    else
                    {
                        tempans = all - tempmin;
                        tempstart = -startmin;
                        tempend = -endmin;
                    }
                    if (ans < tempans)
                    {
                        ans = tempans;
                        x1 = i;
                        x2 = j;
                        y1 = tempstart;
                        y2 = tempend;
                    }
                    if (tempmin == all)
                    {
                        if (ans < tempmax)
                        {
                            ans = tempmax;
                            y1 = tempstart;
                            y2 = tempend;
                        }
                    }
                }
            }
            if (y1 < 0 || y2 < 0)
            {
                for (int i = x1; i <= x2; i++)
                {
                    for (int j = 0; j < -y1; j++)
                    {
                        mark[i, j] = 1;
                    }
                    for (int j = -y2 + 1; j < col; j++)
                    {
                        mark[i, j] = 1;
                    }
                }
            }
            else
            {
                for (int i = x1; i <= x2; i++)
                {
                    for (int j = y1; j <= y2; j++)
                    {
                        mark[i, j] = 1;
                    }
                }
            }
        }

        public void maxsum_v(int[,] map,int row,int col,ref int[,] mark,ref int ans)
        {
            ans = 0;
            start = 0;
            end = 0;
            int[] b = new int[col + 1];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    for (int k = 0; k < col; k++)
                    {
                        b[k] = getsum(i, k, j, k,mark,row,col);
                    }
                    int temp = maxsum1(b, col);
                    if (ans < temp)
                    {
                        ans = temp;
                        if (j < i)
                        {
                            x1 = -i;
                            x2 = -j;
                        }
                        else
                        {
                            x1 = i;
                            x2 = j;
                        }
                        y1 = start;
                        y2 = end;
                    }
                }
            }
            if (x1 < 0 || x2 < 0)
            {
                for (int j = y1; j <= y2; j++)
                {
                    for (int i = 0; i <= -x2; i++)
                    {
                        mark[i, j] = 1;
                    }
                    for (int i = -x1; i < row; i++)
                    {
                        mark[i, j] = 1;
                    }
                }
            }
            else
            {
                for (int i = x1; i <= x2; i++)
                {
                    for (int j = y1; j <= y2; j++)
                    {
                        mark[i, j] = 1;
                    }
                }
            } 
        }

        public void maxsum_vh(int[,] map,int row,int col,ref int[,] mark,ref int ans)
        {
            int all = 0;
            ans = 0;
            start = 0;
            end = 0;
            int[] b = new int[col + 1];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    all = 0;
                    for (int k = 0; k < col; k++)
                    {
                        b[k] = getsum(i, k, j, k,map,row,col);
                        all += b[k];
                    }
                    int tempmax = maxsum1(b, col);
                    int tempmin = minsum(b, col);
                    int tempans;
                    int tempstart;
                    int tempend;
                    if (tempmax > all - tempmin)
                    {
                        tempans = tempmax;
                        tempstart = start;
                        tempend = end;
                    }
                    else
                    {
                        tempans = all - tempmin;
                        tempstart = -startmin;
                        tempend = -endmin;
                    }
                    if (ans < tempans)
                    {
                        ans = tempans;
                        if (j < i)
                        {
                            x1 = -i;
                            x2 = -j;
                        }
                        else
                        {
                            x1 = i;
                            x2 = j;
                        }
                        y1 = tempstart;
                        y2 = tempend;
                    }
                    if (tempmin == all)
                    {
                        if (ans < tempmax)
                        {
                            ans = tempmax;
                            y1 = tempstart;
                            y2 = tempend;
                        }
                    }
                }
            }
            if ((x1 < 0 || x2 < 0) && (y1 < 0 || y2 < 0))
            {
                for (int i = 0; i <= -x2; i++)
                {
                    for (int j = 0; j < -y1; j++)
                    {
                        mark[i, j] = 1;
                    }
                    for (int j = -y2 + 1; j < col; j++)
                    {
                        mark[i, j] = 1;
                    }
                }
                for (int i = -x1; i < row; i++)
                {
                    for (int j = 0; j < -y1; j++)
                    {
                        mark[i, j] = 1;
                    }
                    for (int j = -y2 + 1; j < col; j++)
                    {
                        mark[i, j] = 1;
                    }
                }
            }
            else if (y1 < 0 || y2 < 0)
            {
                for (int i = x1; i <= x2; i++)
                {
                    for (int j = 0; j < -y1; j++)
                    {
                        mark[i, j] = 1;
                    }
                    for (int j = -y2 + 1; j < col; j++)
                    {
                        mark[i, j] = 1;
                    }
                }
            }
            else if (x1 < 0 || x2 < 0)
            {
                for (int j = y1; j <= y2; j++)
                {
                    for (int i = 0; i <= -x2; i++)
                    {
                        mark[i, j] = 1;
                    }
                    for (int i = -x1; i < row; i++)
                    {
                        mark[i, j] = 1;
                    }
                }
            }
            else
            {
                for (int i = x1; i <= x2; i++)
                {
                    for (int j = y1; j <= y2; j++)
                    {
                        mark[i, j] = 1;
                    }
                }
            }
        }

        public int check(int x, int y,int row,int col)
        {
            if (x < 0 || x >= row || y < 0 || y >= col)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        public void dfs(int x, int y, bool choice,int row,int col)
        {
            visit[x, y] = 1;
            for (int i = 0; i < 4; i++)
            {
                int tx = choice ? (x + dx[i] + row) % row : x + dx[i];
                int ty = choice ? (y + dy[i] + col) % col : y + dy[i];
                if (check(tx, ty,row,col) == 1 && visit[tx, ty] == 0 && tempmark[tx, ty] == 1)
                {
                    dfs(tx, ty, choice,row,col);
                }
            }
        }

        public void maxsum_a(bool choice, int[,] map, int row, int col, ref int[,] mark, ref int ans)
        {
            int n = row * col;
            ans = -0x3f3f3f3f;
            for (int i = 0; i < (1 << n); i++)
            {
                visit = new int[row, col];
                tempmark = new int[row, col];
                for (int j = 0; j < n; j++)
                {
                    tempmark[j / col, j % col] = (i & (1 << j)) >> j;
                }
                bool ok = true;
                for (int j = 0; j < row; j++)
                {
                    if (!ok)
                    {
                        break;
                    }
                    for (int k = 0; k < col; k++)
                    {
                        if (tempmark[j, k] == 1)
                        {
                            dfs(j, k, choice,row,col);
                            ok = false;
                            break;
                        }
                    }
                }
                ok = true;
                for (int j = 0; j < row; j++)
                {
                    if (!ok)
                    {
                        break;
                    }
                    for (int k = 0; k < col; k++)
                    {
                        if (tempmark[j, k] == 1 && visit[j, k] == 0)
                        {
                            ok = false;
                            break;
                        }
                    }
                }
                int s = -0x3f3f3f3f;
                if (ok)
                {
                    s = 0;
                    for (int j = 0; j < row; j++)
                    {
                        for (int k = 0; k < col; k++)
                        {
                            if (tempmark[j, k] == 1)
                            {
                                s += map[j, k];
                            }
                        }
                    }
                    if (s > ans)
                    {
                        ans = s;
                        for (int j = 0; j < row; j++)
                        {
                            for (int k = 0; k < col; k++)
                            {
                                mark[j, k] = tempmark[j, k];
                            }
                        }
                    }
                }
            }
        }
        
        
    }
}
