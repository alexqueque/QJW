using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace ClayObject.Extensions
{
    /// <summary>
    /// ExpandoObject 对象拓展
    /// </summary>
    public static class ExpandoObjectExtensions
    {
        /// <summary>
        /// 将对象转 ExpandoObject 类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ExpandoObject ToExpandoObject(this object value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            //if (value is not ExpandoObject expando)
            //{
            //    expando = new ExpandoObject();
            //    var dict = (IDictionary<string, object>)expando;

            //    var dictionary = value.ToDictionary();
            //    foreach (var kvp in dictionary)
            //    {
            //        dict.Add(kvp);
            //    }
            //}
            // 检查 value 是否为 ExpandoObject，否则创建新实例并填充数据
            var expando = value as ExpandoObject;
            if (expando == null)
            {
                expando = new ExpandoObject();
                var dict = (IDictionary<string, object>)expando;

                // 假设 ToDictionary() 返回 IDictionary<string, object>
                //var dictionary = value.ToDictionary();
                // 使用反射将对象属性转换为字典
                var dictionary = value.GetType()
                                      .GetProperties()
                                      .ToDictionary(
                                           prop => prop.Name,
                                           prop => prop.GetValue(value, null)
                                      );
                foreach (var kvp in dictionary)
                {
                    dict.Add(kvp.Key, kvp.Value); // 需显式指定 Key 和 Value
                }
            }


            return expando;
        }

        /// <summary>
        /// 移除 ExpandoObject 对象属性
        /// </summary>
        /// <param name="expandoObject"></param>
        /// <param name="propertyName"></param>
        public static void RemoveProperty(this ExpandoObject expandoObject, string propertyName)
        {
            if (expandoObject == null)
                throw new ArgumentNullException(nameof(expandoObject));

            if (propertyName == null)
                throw new ArgumentNullException(nameof(propertyName));

            ((IDictionary<string, object>)expandoObject).Remove(propertyName);
        }

        /// <summary>
        /// 判断 ExpandoObject 是否为空
        /// </summary>
        /// <param name="expandoObject"></param>
        /// <returns></returns>
        public static bool Empty(this ExpandoObject expandoObject)
        {
            return !((IDictionary<string, object>)expandoObject).Any();
        }

        /// <summary>
        /// 判断 ExpandoObject 是否拥有某属性
        /// </summary>
        /// <param name="expandoObject"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static bool HasProperty(this ExpandoObject expandoObject, string propertyName)
        {
            if (expandoObject == null)
                throw new ArgumentNullException(nameof(expandoObject));

            if (propertyName == null)
                throw new ArgumentNullException(nameof(propertyName));

            return ((IDictionary<string, object>)expandoObject).ContainsKey(propertyName);
        }

        /// <summary>
        /// 实现 ExpandoObject 浅拷贝
        /// </summary>
        /// <param name="expandoObject"></param>
        /// <returns></returns>
        public static ExpandoObject ShallowCopy(this ExpandoObject expandoObject)
        {
            return Copy(expandoObject, false);
        }

        /// <summary>
        /// 实现 ExpandoObject 深度拷贝
        /// </summary>
        /// <param name="expandoObject"></param>
        /// <returns></returns>
        public static ExpandoObject DeepCopy(this ExpandoObject expandoObject)
        {
            return Copy(expandoObject, true);
        }

        /// <summary>
        /// 拷贝 ExpandoObject 对象
        /// </summary>
        /// <param name="original"></param>
        /// <param name="deep"></param>
        /// <returns></returns>
        private static ExpandoObject Copy(ExpandoObject original, bool deep)
        {
            var clone = new ExpandoObject();

            var _original = (IDictionary<string, object>)original;
            var _clone = (IDictionary<string, object>)clone;

            foreach (var kvp in _original)
            {
                _clone.Add(
                    kvp.Key,
                    deep && kvp.Value is ExpandoObject eObject ? DeepCopy(eObject) : kvp.Value
                );
            }

            return clone;
        }
    }
}

