using DnTool.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Tasks;
namespace DnTool
{
    public class Delegater
    {
        /// <summary>
        /// 重复执行指定代码直到成功
        /// </summary>
        /// <param name="trueFunc">要执行的返回成功的操作</param>
        /// <param name="failAction">每次执行失败时，执行的动作</param>
        /// <param name="maxCount">重试的最大次数，0为一直重试</param>
        /// <returns>是否成功</returns>
        public static bool WaitTrue(Func<bool> trueFunc, Action failAction = null, int maxCount = 0)
        {
            failAction = failAction ?? (() => { });

            if (maxCount == 0)
            {
                while (!trueFunc())
                {
                    failAction();
                }
                return true;
            }
            int count = 0;
            while (!trueFunc() && count < maxCount)
            {

                failAction();
                count++;
            }
            return count < maxCount;
        }

        /// <summary>
        /// 重复执行指定代码直到成功，满足条件时立即结束
        /// </summary>
        /// <param name="trueFunc">要执行的返回成功的操作</param>
        /// <param name="breakFunc">当此代码执行成功时，立即中断并返回</param>
        /// <param name="failAction">每次执行失败时，执行的动作</param>
        /// <param name="maxCount">重试的最大次数，0为一直重试</param>
        /// <returns>最终是否成功</returns>
        public static bool WaitTrue(Func<bool> trueFunc,Func<bool> breakFunc, Action failAction, int maxCount = 0)
        {
            failAction = failAction ?? (() => { });
            if (maxCount == 0)
            {
                while (!trueFunc())
                {
                    if (breakFunc())
                    {
                        return true;
                    }
                    failAction();
                }
                return true;
            }
            int count = 0;
            while (!trueFunc() && count < maxCount)
            {
                if (breakFunc())
                {
                    return trueFunc();
                }
                failAction();
                count++;
            }
            return count < maxCount;
        }
      
    }
}
