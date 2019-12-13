using PolyWars.Server.WebService.ConsoleClient.ServiceReferenceWCF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyWars.Server.WebService.ConsoleClient{
    class Program{
        static void Main(string[] args){
            FlipCaseServiceClient client = new FlipCaseServiceClient("BasicHttpBinding_IFlipCaseService");
            StringData sd = new StringData();
            Console.WriteLine("Enter some text below. Our service will flip the upper and lower case for you!");
            sd.OriginalString = Console.ReadLine();
            sd = client.FlipTheCase(sd);
            Console.WriteLine("The flipped case is " + sd.FlippedCaseString);
            Console.ReadLine();
        }
    }
}
