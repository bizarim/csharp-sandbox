using System.Collections.Generic;
using System.Linq;

namespace appLobbyServer.Player
{
    public partial class PlayerShardDBInfo
    {      
        public static string strConnMainDb = "";

        protected static Dictionary<long, long> shardidlist = new Dictionary<long, long>();

        /// <summary>
        /// 샤드정보 얻어오기
        /// </summary>
        /// <param name="accidx">계정번호</param>
        /// <returns></returns>
        public static long GetShardID(long accidx)
        {
            long sid = 0;

            // shardid는 변경이 되지 않으니 풀에 있으면 풀에서 가져오자.
            lock (shardidlist)
            {
                sid = shardidlist.FirstOrDefault(row => row.Key == accidx).Value;
                if (0 < sid) return sid;
            }

            return sid;
        }
    }
}
