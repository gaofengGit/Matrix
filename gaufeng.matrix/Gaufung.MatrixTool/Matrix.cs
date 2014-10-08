using System;
using System.Text;
namespace Gaufung.MatrixTool
{
    public class Matrix
    {
        #region 私有成员和公共属性
        /// <summary>
        /// 矩阵的存储
        /// </summary>
        private double[,] _elements;
        /// <summary>
        /// 列
        /// </summary>
        public int Column { get; private set; }
        /// <summary>
        /// 行
        /// </summary>
        public int Row { get; private set; }
        /// <summary>
        /// 精度
        /// </summary>
        public double Eps { get; set; }

        #endregion

        #region 构造函数
        /// <summary>
        /// 不带参数的构造函数
        /// </summary>
        public Matrix()
        {
            Row = Column = 1;
            Init();
        }
        /// <summary>
        /// 从一个矩阵从Copy
        /// </summary>
        /// <param name="elements">Copy的矩阵</param>
        public Matrix(double[,] elements)
        {
            Row = elements.GetLength(0);
            Column = elements.GetLength(1);
            Init();
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Column; j++)
                {
                    _elements[i, j] = elements[i, j];
                }
            }
        }
        /// <summary>
        /// 已知的行和列的构造函数
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="column">列</param>
        public Matrix(int row, int column)
        {
            Row = row;
            Column = column;
            Init();
        }
        /// <summary>
        /// 已知阶数的构造函数
        /// </summary>
        /// <param name="size">阶数</param>
        public Matrix(int size)
        {
            Row = Column = size;
            Init();
        }
        /// <summary>
        /// 初始化矩阵，所有值为0.0
        /// </summary>
        private void Init()
        {
            _elements = new double[Row, Column];
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Column; j++)
                {
                    _elements[i, j] = 0.0;
                }
            }
        }
        /// <summary>
        /// 赋值构造函数
        /// </summary>
        /// <param name="matrix">赋值矩阵</param>
        public Matrix(Matrix matrix)
        {
            Row = matrix.Column;
            Column = matrix.Column;
            Init();
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Column; j++)
                {
                    _elements[i, j] = matrix[i, j];
                }
            }
        }
        #endregion

        #region 重载操作符
        public double this[int row, int col]
        {
            get
            {
                return _elements[row,col];
            }
            set
            {
                _elements[row, col] = value;
            }
        }

        /// <summary>
        /// 重载运算符+
        /// </summary>
        /// <param name="m1">加数1</param>
        /// <param name="m2">加数2</param>
        /// <returns>运算结果</returns>
        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            return m1.Add(m2);
        }
        /// <summary>
        /// 重载运算符-
        /// </summary>
        /// <param name="m1">被减数1</param>
        /// <param name="m2">减数2</param>
        /// <returns>计算结果</returns>
        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            return m1.Subtract(m2);
        }

        /// <summary>
        /// 重载运算符×
        /// </summary>
        /// <param name="m1">乘数1</param>
        /// <param name="m2">乘数2</param>
        /// <returns>运算结果</returns>
        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            return m1.Multiply(m2);
        }

        /// <summary>
        /// 实现矩阵的加法
        /// </summary>
        /// <param name="other">与指定矩阵相加的矩阵</param>
        /// <returns>指定矩阵与other相加之和,如果矩阵的行/列数不匹配，则会抛出异常</returns>
        public Matrix Add(Matrix other)
        {
            // 首先检查行列数是否相等
            if (Column != other.Column ||
                Row != other.Row)
                throw new Exception("矩阵的行/列数不匹配。");

            // 构造结果矩阵
            var result = new Matrix(this);		// 拷贝构造
            // 矩阵加法
            for (int i = 0; i < Row; ++i)
            {
                for (int j = 0; j < Column; ++j)
                result[i, j] = result[i, j] + other[i, j];
            }
            return result;
        }

        /// <summary>
        /// 实现矩阵的减法
        /// </summary>
        /// <param name="other">与指定矩阵相减的矩阵</param>
        /// <returns>Matrix型，指定矩阵与other相减之差,如果矩阵的行/列数不匹配，则会抛出异常</returns>
        public Matrix Subtract(Matrix other)
        {
            if (Column != other.Column ||
                Row != other.Row)
                throw new Exception("矩阵的行/列数不匹配。");

            // 构造结果矩阵
            var result = new Matrix(this);		// 拷贝构造

            // 进行减法操作
            for (int i = 0; i < Row; ++i)
            {
                for (int j = 0; j < Column; ++j)
                    result[i, j] = result[i, j] - other[i, j];
            }
            return result;
        }

        /// <summary>
        /// 实现矩阵的乘法
        /// </summary>
        /// <param name="other">与指定矩阵相乘的矩阵</param>
        /// <returns>指定矩阵与other相乘之积,如果矩阵的行/列数不匹配，则会抛出异常</returns>
        public Matrix Multiply(Matrix other)
        {
            // 首先检查行列数是否符合要求
            if (Column != other.Row)
                throw new Exception("矩阵的行/列数不匹配。");

            // ruct the object we are going to return
            var result = new Matrix(Row, other.Column);

            /* 矩阵乘法，即
            //
            // [A][B][C]   [G][H]     [A*G + B*I + C*K][A*H + B*J + C*L]
            // [D][E][F] * [I][J] =   [D*G + E*I + F*K][D*H + E*J + F*L]
            //             [K][L]
            */
            double value;
            for (int i = 0; i < result.Row; ++i)
            {
                for (int j = 0; j < other.Column; ++j)
                {
                    value = 0.0;
                    for (int k = 0; k < Column; ++k)
                    {
                        value += this[i, k] * other[k, j];
                    }
                    result[i, j] = value;
                }
            }
            return result;
        }

        public Matrix Multiply(double value)
        {
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Column; j++)
                {
                    this[i, j] =this[i,j]* value;
                }
            }
            return this;
        }
        #endregion
        #region 其他辅助函数
        /// <summary>
        /// 将矩阵都设置为零
        /// </summary>
        public void InitMatrix()
        {
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Column; j++)
                {
                    this[i, j] = 0.0;
                }
            }
        }
        #endregion

        #region 数值计算中使用的方法
        /// <summary>
        /// 矩阵转置
        /// </summary>
        /// <returns>转置结果</returns>
        public Matrix Transpose()
        {
            // 构造目标矩阵
            var trans = new Matrix(Column, Row);

            // 转置各元素
            for (int i = 0; i < Row; ++i)
            {
                for (int j = 0; j < Column; ++j)
                    //Trans.SetElement(j, i, GetElement(i, j));
                    trans[j, i] = this[i, j];
            }
            return trans;
        }

        /// <summary>
        /// LU分解
        /// </summary>
        /// <param name="lPart">L部分</param>
        /// <param name="uPart">U部分</param>
        /// <returns>是否成功</returns>
        public bool SpiltLu(Matrix lPart,Matrix uPart)
        {
            if (Row!=Column)
                throw new Exception("行列数不相等！");
            //todo 检查是否为奇异矩阵
            lPart.Init();
            uPart.Init();
            //从第一列开始
            for (int i = 0; i < Column; i++)
            {
                if (Math.Abs(this[i, i]) < Eps) return false;
                //设置对角线的值
                lPart[i, i] = 1.0;
                for (int j = i+1; j < Row; j++)
                {
                   var factor=this[j, i]/this[i, i];
                    lPart[j, i] = factor;
                    for (int k = 0; k < Column; k++)
                    {
                        this[j, k] = this[j, k] - this[i, k]*factor;
                    }
                }
            }
            //获取Upart
            for (int i = 0; i < Row; i++)
            {
                for (int j = i; j < Column; j++)
                {
                    uPart[i, j] = this[i, j];
                }
            }
            return true;
        }

        /// <summary>
        /// PLU分解
        /// </summary>
        /// <param name="lPart">L矩阵部分</param>
        /// <param name="uPart">U矩阵部分</param>
        /// <param name="pPart">置换矩阵部分</param>
        /// <returns>返回是否成功</returns>
        public bool SpiltPlu(Matrix lPart,Matrix uPart,Matrix pPart)
        {
            if (Row != Column)
                throw new Exception("行列数不相等！");
            //初始化P矩阵
            pPart.InitMatrix();
            var temMatrix = new Matrix(this);
            var swapIndex=new int[Row];
            for (int i = 0; i < Row; i++)
            {
                swapIndex[i] = i;
            }
            //开始处理
            for (int i = 0; i < Row; i++)
            {
                var value = temMatrix[i, i];
                var maxIndex = i;
                //获取该数据最大的一行
                for (int j = i; j < Row; j++)
                {
                    if (this[j,i]>value)
                    {
                        maxIndex = j;
                        value = temMatrix[j, i];
                    }
                }
                //交换该行和数据
                var row = new double[Column];
                //保存第一行数据
                for (int j = 0; j < Column; j++)
                {
                    row[j] = temMatrix[maxIndex, j];
                }
                //设置第一行数据
                for (int j = 0; j < Column; j++)
                {
                    temMatrix[maxIndex, j] = temMatrix[i, j];
                }
                //设置第二行数据
                for (int j = 0; j < Column; j++)
                {
                    temMatrix[i, j] = row[j];
                }
                SwapIndex(swapIndex,maxIndex,i);
                //开始处理
                for (int j = i+1; j < Row; j++)
                {
                    var factor = temMatrix[j, i] / temMatrix[i, i];
                    for (int k = 0; k < Column; k++)
                    {
                        temMatrix[j, k] = temMatrix[j, k] - factor * temMatrix[i, k];
                    }
                }
            }
            for (int i = 0; i <swapIndex.Length; i++)
            {
                pPart[i, swapIndex[i]] = 1;
            }
            //置换矩阵与A原始矩阵相乘，在进行LU分解。
            var pa = pPart*this;
            pa.SpiltLu(lPart, uPart);
            return true;
        }
        /// <summary>
        /// 交换两列
        /// </summary>
        /// <param name="index">数组</param>
        /// <param name="index1">序列1</param>
        /// <param name="index2">序列2</param>
        private void SwapIndex(int[] index,int index1,int index2)
        {
            var value = index[index1];
            index[index1] = index[index2];
            index[index2] = value;
        }
        #endregion

        #region 输出函数

        public override string ToString()
        {
            var stringBuilder=new StringBuilder();
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Column; j++)
                {
                    stringBuilder.Append(this[i, j].ToString("f")+" ");
                }
                stringBuilder.Append("\n");
            }
            return stringBuilder.ToString();
        }

        public double GetElement(int row,int col)
        {
            return this[row, col];
        }

        #endregion
    }
}
