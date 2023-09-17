using MessagingCorp.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Converters
{
    public static class StaticConverter
    {
        /// <summary>
        /// When want to convert e.g. DAO User to normal User, then newObj= Convert>DAOType, UserType>(daoObj);
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TTarget Convert<TSource, TTarget>(TSource source)
            where TTarget : new()
        {
            var target = new TTarget();

            foreach (var sourceProperty in typeof(TSource).GetProperties())
            {
                var targetProperty = typeof(TTarget).GetProperty(sourceProperty.Name);

                if (targetProperty != null && targetProperty.CanWrite)
                {
                    targetProperty.SetValue(target, sourceProperty.GetValue(source));
                }
            }

            return target;
        }
    }
}
