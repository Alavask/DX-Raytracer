using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    class Program
    {
        static void Main(string[] args)
		{
			using (Visualiser visualiser = new Visualiser())
			{
				visualiser.Run();
			}
		}
    }
}
