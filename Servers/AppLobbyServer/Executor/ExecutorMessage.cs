using System.Collections.Generic;
using System.Linq;

namespace AppLobbyServer.Executor
{
    public interface IRunnable
    {
        void Start();
    }

    public class ExecutorMessage : IRunnable
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ExecutorMessage));

        protected Queue<LibServerCommon.Message.IMessage> queue = new Queue<LibServerCommon.Message.IMessage>();

        protected System.Threading.Thread thread = null;

        public void Start()
        {
            if (null == thread)
            {
                thread = new System.Threading.Thread(Loop);
                thread.Start();
            }
        }

        protected void Loop()
        {
            while (true)
            {
                List<LibServerCommon.Message.IMessage> items = null;
                {
                    lock (queue)
                    {
                        items = queue.ToList();
                        queue.Clear();
                    }
                }

                if (0 < items.Count)
                {
                    foreach (var item in items)
                    {
                        try
                        {
                            item.Process();
                        }
                        catch (System.OutOfMemoryException ex)
                        {
                            logger.Error(ex.Message);
                            logger.Error(ex.StackTrace);
                            System.Environment.Exit(1);
                        }
                        catch (System.Exception ex)
                        {
                            logger.Error(ex.Message);
                            logger.Error(ex.StackTrace);
                        }
                        finally
                        {
                            item.Release();
                        }
                    }
                    items.Clear();

                }

                System.Threading.Thread.Sleep(1);
            }
        }

        public void Add(LibServerCommon.Message.IMessage item)
        {
            if (null == thread) return;

            lock (queue)
            {
                queue.Enqueue(item);
            }
        }
    }
}
