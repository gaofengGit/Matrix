using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Gaufung.MatrixTool
{
    public static class MatrixHelper
    {
        #region Household分解
        /// <summary>
        /// Householder变换
        /// </summary>
        /// <param name="value">向量</param>
        /// <param name="u">u向量</param>
        /// <param name="beta">系数beta</param>
        /// <param name="rho">系数rho</param>
        /// <returns>householder矩阵H</returns>
        public static Matrix HouseholderTrans(Matrix value, Matrix u, out double beta, out double rho)
        {
            //先进行判断，如果数据不合规范
            if (value.Column != 1 || u.Column != 1)
                throw new Exception("非向量变换！");
            rho = VectorNorm(value) * Sgn(value[0, 0]) * (-1.0);
            beta = 1 / (VectorNorm(value) * (VectorNorm(value) + Math.Abs(value[0, 0])));
            u[0, 0] = value[0, 0] - rho;
            for (int i = 1; i < u.Row; i++)
            {
                u[i, 0] = value[i, 0];
            }
            // Console.WriteLine("");
            var unitaryMatrix = new Matrix(value.Row);
            unitaryMatrix.Unitary();
            return unitaryMatrix - (u * (u.Transpose())).Multiply(beta);
        }
        /// <summary>
        /// 求解向量的范数
        /// </summary>
        /// <param name="vector">向量</param>
        /// <returns>范数</returns>
        private static double VectorNorm(Matrix vector)
        {
            if (vector.Row != 1 && vector.Column != 1)
                throw new Exception("非向量！");
            var value = 0.0;
            if (vector.Row == 1)
            {
                for (int i = 0; i < vector.Column; i++)
                {
                    value += vector[0, i] * vector[0, i];
                }
                return Math.Sqrt(value);
            }
            for (int i = 0; i < vector.Row; i++)
            {
                value += vector[i, 0] * vector[i, 0];
            }
            return Math.Sqrt(value);
        }

        /// <summary>
        ///根据正负，返回1和-1
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>1或者-1</returns>
        private static double Sgn(double value)
        {
            if (value >= 0)
            {
                return 1.0;
            }
            return -1.0;
        }
        #endregion

        #region 最小二乘求解
        /// <summary>
        /// 满秩最小二乘求解Ax=b
        /// </summary>
        /// <param name="value">系数矩阵A</param>
        /// <param name="result">结果值b</param>
        /// <returns>结算结果x矩阵</returns>
        public static Matrix LeastSquares(Matrix value,Matrix result)
        {
                //对数据有效性进行检查
            var ata = (value.Transpose())*value;
            if (ata.IsSingularMatrix())
                throw new Exception("非满秩矩阵！");
            var atb = (value.Transpose())*result;
            var p = new Matrix(ata.Row);
            var l = new Matrix(ata.Row);
            var u = new Matrix(ata.Row);
            //plu分解
            ata.SpiltPlu(l, u, p);
            var pb = p * atb;
            var y = l.DecomposeDown(pb);
            return u.DecomposeUp(y);
        }
        #endregion



    }
}
