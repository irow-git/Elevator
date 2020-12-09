using System;
using System.Linq;

namespace Elevator
{
    class Program
    {
        static void Main(string[] args)
        {
            const int workers = 5;
            string[] securityCheck = new string[workers];
            for (int i = 0; i < workers; i++)
            {
                Random rand = new Random();
                int val = rand.Next(3);
                if (val == 1) securityCheck[i] = "Confidential";
                else if (val == 2) securityCheck[i] = "Secret";
                else securityCheck[i] = "TopSecret";
            }
            ElevatorClass elevator = new ElevatorClass();
            var agents =
                Enumerable.Range(0, workers)
                .Select(i => new AgentClass { Name = "Agent " + (i+1).ToString(), Security = securityCheck[i],  Elevator = elevator })
                .ToList();
            
            foreach(var agent in agents)
            {
                agent.WorkDays();
            }
            while(agents.Any (a => ! a.AtHome))
            {

            }
            Console.WriteLine("Work day is over.");
            Console.ReadLine();
        }
    }
}
