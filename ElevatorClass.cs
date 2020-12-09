using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Elevator
{
    class ElevatorClass
    {
        const int maxCapacity = 1;
        Semaphore semaphore;
        List<AgentClass> agents;

        public ElevatorClass()
        {
            semaphore = new Semaphore(maxCapacity, maxCapacity);
            agents = new List<AgentClass>();
        }
        public void Enter (AgentClass agent)
        {
            semaphore.WaitOne();
            lock (agents)
            {
                agents.Add(agent);
            }
        }
        public void Leave(AgentClass agent)
        {
            semaphore.Release();
            lock (agents)
            {
                agents.Remove(agent);
            }
        }
    }
}
