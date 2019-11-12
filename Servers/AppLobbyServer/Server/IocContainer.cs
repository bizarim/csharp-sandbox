using System;
using System.Collections.Generic;

namespace AppLobbyServer.Server
{
    public class IocContainer
    {
        private readonly Dictionary<int, Type> dic = new Dictionary<int, Type>();

        public void Register(int pn, Type type)
        {
            if (true == dic.ContainsKey(pn)) return;
            dic.Add(pn, type);
        }

        public object Resolve(int pn)
        {
            if (false == dic.ContainsKey(pn)) return null;
            return Activator.CreateInstance(dic[pn]);
        }
    }
}
