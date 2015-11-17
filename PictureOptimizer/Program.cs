using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

namespace PictureOptimizer
{
    public class Program
    {
        static void Main(string[] args)
        {
            JobHost host = new JobHost();
            host.RunAndBlock();
        }
    }
}
