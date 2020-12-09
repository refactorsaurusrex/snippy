using System.Linq;
using System.Management.Automation;

namespace Snippy.Infrastructure
{
    internal class CmdletName<T>
    {
        public static implicit operator string(CmdletName<T> name) => name.ToString();

        public override string ToString()
        {
            var cmdlet = typeof(T).GetCustomAttributes(false).OfType<CmdletAttribute>().Single();
            return $"{cmdlet.VerbName}-{cmdlet.NounName}";
        }
    }
}