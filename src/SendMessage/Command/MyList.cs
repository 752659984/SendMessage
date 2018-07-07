using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Command
{
    public class MyList<T>
    {
        private static object LockObject = new object();

        public int Length { get; set; }

        private List<T> Data { get; set; }

        private List<T> OperateList(int type, T entity, Func<T, bool> func)
        {
            lock (LockObject)
            {
                switch (type)
                {
                    case 1:
                        Data.Add(entity);
                        ++Length;
                        return null;
                    case 0:
                        Data.Remove(entity);
                        --Length;
                        return null;
                    case 2:
                        return Data.Where(func).ToList();
                    default:
                        return null;
                }
            }
        }

        public void Add(T entity)
        {
            OperateList(1, entity, null);
        }

        public void Remove(T entity)
        {
            OperateList(0, entity, null);
        }

        public List<T> Where(Func<T, bool> func)
        {
            return OperateList(2, default(T), func);
        }
    }
}
