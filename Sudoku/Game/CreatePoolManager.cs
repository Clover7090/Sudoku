using CloverGame.Utils;
using System;
using System.Collections.Generic;
using System.Linq;


namespace CloverGame.Cube.Sudoku
{
    public class CreatePoolManager : Singleton<CreatePoolManager>
    {
        private int[][] mDatas;
        private int[]   mIndex;
        private int     mRows;
        private const int INIT_VALUE = -100;
        public void Init()
        {
            mRows = DataPoolManager.Instance.GetRows();

            mDatas = new int[mRows][];
            for (int i = 0; i < mDatas.Length; i++)
            {
                mDatas[i] = new int[mRows];

                for(int k= 0; k <mDatas[i].Length; k++)
                {
                    mDatas[i][k] = INIT_VALUE;
                }
            }

            mIndex = new int[mRows];
            for(int i= 0; i < mIndex.Length; i++)
            {
                mIndex[i] = i;
            }
        }

        public int Generate()
        {
            int row = 0;
            for (; row < mRows; row++)
            {
                int col = 0;
                for (; col < mRows; col++)
                {
                    if (mDatas[row][col] != INIT_VALUE)
                    {
                        continue;
                    }

                    var notInData = CollectInRowCol(row, col);
                    var includeData = ExceptData(mIndex, notInData);

                    var notInRowData = CollectInRow(row);
                    var includeRowData = ExceptData(mIndex, notInRowData);

                    var behindData = CollectBehind(row, col, includeData, includeRowData);
                    var seedData = SeedData(includeData, behindData);

                    if (seedData == -1)
                    {
                        break;
                    }

                    mDatas[row][col] = seedData;
                }
                if (col != mRows)
                {
                    for (int m = 0; m < mRows; m++)
                    {
                        mDatas[row][m] = INIT_VALUE;
                    }
                    //break;
                    return 0;
                }
            }

            PrintData();

            return 1;

           // List<List<int>> list = CollectBehindRows(row);
           /*
            for (; row < mRows; row++)
            {
                List<List<int>> list = CollectBehindRows(row);

                for (int col = 0; col < mRows; col++)
                {
                    var notInRowData = CollectInRow(row);
                    var includeRowData = ExceptData(mIndex, notInRowData);

                    var newArr = includeRowData.Intersect(list[col]).ToArray();

                    if (newArr.Length != 0)
                    {
                        mDatas[row][col] = newArr[0];
                    }
                    else
                    {
                        col = -1;
                        for (int m = 0; m < mRows; m++)
                        {
                            mDatas[row][m] = INIT_VALUE;
                        }
                    }
                }

                for (int col = 0; col < mRows; col++)
                {
                    list[col].Remove(mDatas[row][col]);
                }
            }

            PrintData();
            */
        }

        public List<List<int>> CollectBehindRows(int initRow)
        {
            List<List<int>> list = new List<List<int>>();
            for (int col = 0; col < mRows; col++)
            {
                list.Add(new List<int>());

                int[] colData = new int[initRow];

                for (int row = 0; row < initRow; row++)
                {
                    colData[row] = mDatas[row][col];
                }

                var includeData = ExceptData(mIndex, colData);

                list[col].AddRange(includeData);
            }
            return list;
        }

        public void PrintData()
        {
            Console.WriteLine();
            for (int i = 0; i < mRows; i++)
            {
                for (int k = 0; k < mRows; k++)
                {
                    Console.Write(mDatas[i][k] +",");
                }
                Console.WriteLine();
            }
        }

        public int[] CollectInRowCol(int row, int col)
        {
            List<int> list = new List<int>((mRows - 1) * 2);

            for (int i = 0; i < mRows; i++)
            {
                if(mDatas[row][i] != INIT_VALUE)
                {
                    list.Add(mDatas[row][i]);
                }                
            }

            for (int i = 0; i < mRows; i++)
            {
                if (mDatas[i][col] != INIT_VALUE)
                { 
                    list.Add(mDatas[i][col]);
                }
            }           

            return list.ToArray();
        }

        public int[] CollectInRow(int row)
        {
            List<int> list = new List<int>((mRows - 1) * 2);

            for (int i = 0; i < mRows; i++)
            {
                if (mDatas[row][i] != INIT_VALUE)
                {
                    list.Add(mDatas[row][i]);
                }
            }

            return list.ToArray();
        }

        public Dictionary<int, List<int>> CollectBehind(int row, int col, int[] includeData, int[] includeRowData)
        {
            if(row <= 0 || mRows - 1 - col <= 0)
            {
                return null;
            }

            List<int> includeList = new List<int>(includeData);

            List<int> includeRowList = new List<int>();

            for (int i = col + 1; i < mRows; i++)
            {
                includeRowList.Clear();
                includeRowList.AddRange(includeRowData);

                List<int> colList = new List<int>();
                for (int k = 0; k < row; k++)
                {
                    var key = mDatas[k][i];

                    if (includeRowList.Contains(key))
                    {
                        includeRowList.Remove(key);
                    }
                }    

                if(includeRowList.Count == 1)
                {
                    mDatas[row][i] = includeRowList[0];
                    includeList.Remove(includeRowList[0]);
                }
                else if (includeRowList.Count == 0)
                {
                    int m = 0;
                }
            }            

            Dictionary<int,int> dic = new Dictionary<int, int>();

            for (int i = col + 1; i < mRows; i++)
            {
                if(mDatas[row][i] != INIT_VALUE)
                {
                    continue;
                }

                List<int> colList = new List<int>();
                for (int k = 0; k < row; k++)
                {
                    var key = mDatas[k][i];

                    if (includeList.Contains(key))
                    {
                        colList.Add(key);
                    }
                }

                int count = colList.Count;

                foreach (int key in colList)
                {
                    if (dic.ContainsKey(key))
                    {
                        dic[key] = dic[key] + 10000 + i * 100 + count;
                    }
                    else
                    {
                        dic.Add(key, 10000 + count);
                    }
                }  
            }

            Dictionary<int, List<int>> dicList = new Dictionary<int, List<int>>();

            foreach (KeyValuePair<int, int> kv in dic)
            {
                if(!dicList.ContainsKey(kv.Value))
                {
                    dicList.Add(kv.Value, new List<int>());                    
                }
                dicList[kv.Value].Add(kv.Key);
            }

            Dictionary<int, List<int>> dicSortedByKey = dicList.OrderByDescending(p => p.Key).ToDictionary(p => p.Key, o => o.Value);

            return dicSortedByKey;
        }


        /// <summary>
        /// 集合求补集
        /// </summary>
        /// <param name="arr1"></param>
        /// <param name="arr2"></param>
        /// <returns></returns>
        public int[] ExceptData(int[] arr1, int[] arr2)
        {
            var newArr = arr1.Except(arr2).ToArray();

            return newArr;
        }

        /// <summary>
        /// 集合求交集
        /// </summary>
        /// <param name="arr1"></param>
        /// <param name="arr2"></param>
        /// <returns></returns>
        public int[] IntersectData(int[] arr1, Dictionary<int, List<int>> arr2)
        {

            foreach (KeyValuePair<int, List<int>> kv in arr2)
            {
                var newArr = arr1.Intersect(kv.Value).ToArray();

                if(newArr.Length != 0)
                {
                    return newArr;
                }
            }

            return null;
        }


        /// <summary>
        /// 随机一个值
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public int SeedData(int[] arr1, Dictionary<int, List<int>> arr2)
        {
            int[] newArr = null;

            if(arr2 == null)
            {
                newArr = arr1;
            }
            else
            {
                newArr = IntersectData(arr1, arr2);

                if (newArr == null || newArr.Length == 0)
                {
                    newArr = arr1;
                }
            }

            if(newArr.Length == 0)
            {
               // PrintData();
                return -1;
            }

            var index = SeedManager.Instance.Seed(newArr);
            return newArr[index];
        } 
    }
}
