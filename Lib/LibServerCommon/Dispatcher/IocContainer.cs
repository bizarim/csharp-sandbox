using System;
using System.Collections.Generic;

namespace LibServerCommon.Dispatcher
{
    public interface IIocContainer
    {
        void Register(int pn, Type type);
        T Resolve<T>(int pn);
    }

    public class IocContainer : IIocContainer
    {
        private readonly Dictionary<int, Type> dic = new Dictionary<int, Type>();

        public void Register(int pn, Type type)
        {
            if (true == dic.ContainsKey(pn)) return;
            dic.Add(pn, type);
        }

        public T Resolve<T>(int pn)
        {
            if (false == dic.ContainsKey(pn)) return default(T);
            return (T)Activator.CreateInstance(dic[pn]);
        }
    }
}
