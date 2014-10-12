using System;
using System.Text;
/*
 * *********************Author :Gau fung*****************************
 * *********************Email:gaufung@foxmail.com*****************
 * ********************Description: Matrix Tools**********************
 * ********************Github:www.github.com/gaofengGit/Matrix***
 * *********************All Rights Reserved ***************************
 */
namespace Gaufung.MatrixTool
{
    /// <summary>
    /// 矩阵类，使用二维数组存储矩阵数据
    /// </summary>
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
        
        #endregion

        #region 重载操作符
        public double this[int row, int col]
        {
            get
            {
                if (row>Row || col>Column ||row<0||col<0)
                {
                    throw new Exception("指数出界！");
                }
                return _elements[row,col];
            }
            set
            {
                if (row > Row || col > Column || row < 0 || col < 0)
                {
                    throw new Exception("指数出界！");
                }
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

        /// <summary>
        /// 格式化输出
        /// </summary>
        /// <returns></returns>
        public string ToString(IFormatProvider formatProvider)
        {
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Column; j++)
                {
                    stringBuilder.Append(this[i, j].ToString(formatProvider) + " ");
                }
                stringBuilder.Append("\n");
            }
            return stringBuilder.ToString();
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
        /// 判断该矩阵是否为奇异矩阵
        /// </summary>
        /// <returns>true:奇异矩阵，false：非奇异矩阵</returns>
        public bool IsSingularMatrix()
        {
            if (Row != Column)
                return true;
            //保存原始数据，用一个临时变量进行判断
            var temp = new Matrix(this);
            //采用高斯约旦消去法确定是否为奇异矩阵
            for (int i = 0; i < Row; i++)
            {
                //先获得最大的主元值
                var value = temp[i, i];
                var maxIndex = i;
                for (int j = i+1; j < Row; j++)
                {
                    //与最大值相比较,如果大
                    if (this[j,i]>value)
                    {
                        maxIndex = j;
                    }
                }
                //取得该列的最大值，如果该列的最大值等于零，则不可逆
                if (Math.Abs(value) < Eps) return true;
                //如果不是非奇异矩阵，交换该两行的数据
                for (int j = 0; j < Column; j++)
                {
                    var tempValue = temp[i, j];
                    temp[i, j] = temp[maxIndex, j];
                    temp[maxIndex, j] = tempValue;
                }
                //进行列主消元
                for (int j = i+1; j < Row; j++)
                {
                    var factor = temp[j, i]/temp[i, i];
                    for (int k = 0; k < Column; k++)
                    {
                        temp[j, k] = temp[j, k] - factor*temp[i, k];
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// LU分解
        /// </summary>
        /// <param name="lPart">L部分</param>
        /// <param name="uPart">U部分</param>
        /// <returns>是否成功，分解后原始矩阵仍在</returns>
        public bool SpiltLu(Matrix lPart,Matrix uPart)
        {
            if (Row!=Column)
                throw new Exception("行列数不相等！");
            if (IsSingularMatrix())
            {
                throw  new Exception("奇异矩阵，无法分解!");
            }
            lPart.Init();
            uPart.Init();
            var temp = new Matrix(this);
            //从第一列开始
            for (int i = 0; i < Column; i++)
            {
                if (Math.Abs(this[i, i]) < Eps) return false;
                //设置对角线的值
                lPart[i, i] = 1.0;
                for (int j = i+1; j < Row; j++)
                {
                    var factor = temp[j, i] / temp[i, i];
                    lPart[j, i] = factor;
                    for (int k = 0; k < Column; k++)
                    {
                        temp[j, k] = temp[j, k] - temp[i, k] * factor;
                    }
                }
            }
            //获取Upart
            for (int i = 0; i < Row; i++)
            {
                for (int j = i; j < Column; j++)
                {
                    uPart[i, j] = temp[i, j];
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
            if (IsSingularMatrix())
            {
                throw new Exception("奇异矩阵，无法进行PLU分解！");
            }
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
                //交换数据
                for (int j = 0; j < Column; j++)
                {
                    var temp = temMatrix[maxIndex, j];
                    temMatrix[maxIndex, j] = temMatrix[i, j];
                    temMatrix[i, j] = temp;
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
            //LU分解
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

        /// <summary>
        /// 下三角矩阵求解方程组:Ax=b,A为下三角矩阵
        /// </summary>
        /// <param name="consequense">b</param>
        /// <returns>矩阵：x</returns>
        public Matrix DecomposeDown(Matrix consequense)
        {
            #region 异常处理
            if (Row != Column)
                throw new Exception("非阶阵！");
            if (consequense.Column != 1)
                throw new Exception("结果矩阵列数不为1");
            if (Row != consequense.Row)
                throw new Exception("三角阵与结果向量的行不相等！");
            #endregion
            //结果矩阵
            var result = new Matrix(Row, 1);
            result[0, 0] = consequense[0, 0]/this[0, 0];
            for (int i = 1; i < Row; i++)
            {
                var value = 0.0;
                for (int j = 0; j < i; j++)
                {
                    value += this[i, j]*result[j, 0];
                }
                result[i, 0] = (consequense[i, 0] - value)/this[i, i];
            }
            return result;
        }
        /// <summary>
        /// 上三角矩阵求解方程组：Ax=b;A为上三角矩阵
        /// </summary>
        /// <param name="consequense">b</param>
        /// <returns>解：x</returns>
        public Matrix DecomposeUp(Matrix consequense)
        {
            #region 异常处理
            if (Row != Column)
                throw new Exception("A非阶阵！");
            if (consequense.Column != 1)
                throw new Exception("b矩阵列数不为1");
            if (Row != consequense.Row)
                throw new Exception("三角阵与结果向量的行不相等！");
            #endregion
            //解的结果矩阵
            var result = new Matrix(Row, 1);
            result[Row - 1, 0] = consequense[Row - 1, 0]/this[Row - 1, Column - 1];
            for (int i = Row-2; i >= 0; i--)
            {
                var value = 0.0;
                for (int j = Row-1; j >i; j--)
                {
                    value += this[i, j] * result[j, 0];
                }
                result[i, 0] = (consequense[i, 0] - value)/this[i,i];
            }
            return result;
        }
        #endregion

    }
}
