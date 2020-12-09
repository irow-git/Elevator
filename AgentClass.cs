using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Elevator
{
    enum AgentActionOutside { Walk, Elevator, Leave};
    enum AgentActionInside { TravelToG, TravelToS, TravelToT1, TravelToT2 };

    class AgentClass
    {
        public string Name { get; set; }
        public string Security { get; set; }
        public ElevatorClass Elevator { get; set; }
        Random rand = new Random();
        ManualResetEvent eventLeave = new ManualResetEvent(false);

        private AgentActionOutside GetRandomOutdoorAction()
        {
            int val = rand.Next(10);
            if (val < 7) return AgentActionOutside.Elevator;
            else return AgentActionOutside.Leave;
        }

        private AgentActionInside GetRandomElevatorAction()
        {
            int val = rand.Next(8);
            if (val < 2) return AgentActionInside.TravelToG;
            else if (val < 4) return AgentActionInside.TravelToS;
            else if (val < 6) return AgentActionInside.TravelToT1;
            else return AgentActionInside.TravelToT2;
        }

        private void GoToWork()
        {

            Console.WriteLine(Name + " (" + Security + ")" + " now walks towards the elevator.");
            Thread.Sleep(200);
        }
        


        private void EnterElevator()
        {

            Console.WriteLine(Name + " (" + Security + ")" + " is calling the elevator.");
            Elevator.Enter(this);
            Console.WriteLine(Name + " (" + Security + ")" + " entered the elevator.");
            while (true)
            {
                var elevatorAction = GetRandomElevatorAction();
                switch (elevatorAction)
                {
                    case AgentActionInside.TravelToG:
                        Console.WriteLine(Name + " (" + Security + ")" + " is going to Ground floor.");
                        Thread.Sleep(1000);
                        Console.WriteLine(Name + " (" + Security + ")" + " left the elevator.");
                        Elevator.Leave(this);
                        return;
                    case AgentActionInside.TravelToS:
                        Console.WriteLine(Name + " (" + Security + ")" + " is going to Secret floor.");
                        Thread.Sleep(1000);
                        if (Security == "Secret" || Security == "TopSecret")
                        {
                            Console.WriteLine(Name + " (" + Security + ")" + " left the elevator.");
                            Elevator.Leave(this);
                            return;
                        }
                        else
                        {
                            Console.WriteLine(Name + " (" + Security + ")" + " - Permission denied!");
                            break;
                        }
                    case AgentActionInside.TravelToT1:
                        Console.WriteLine(Name + " (" + Security + ")" + " is going to TopSecret floor 1.");
                        Thread.Sleep(1000);
                        if (Security != "TopSecret")
                        {
                            Console.WriteLine(Name + " (" + Security + ")" + " - Permission denied!");
                            break;
                        }
                        else
                        {
                            Console.WriteLine(Name + " (" + Security + ")" + " left the elevator.");
                            Elevator.Leave(this);
                            return;
                        }
                    case AgentActionInside.TravelToT2:
                        Console.WriteLine(Name + " (" + Security + ")" + " is going to TopSecret floor 2.");
                        Thread.Sleep(1000);
                        if (Security != "TopSecret")
                        {
                            Console.WriteLine(Name + " (" + Security + ")" + " - Permission denied!");
                            break;
                        }
                        else
                        {
                            Console.WriteLine(Name + " (" + Security + ")" + " left the elevator.");
                            Elevator.Leave(this);
                            return;
                        }
                    default:
                        throw new ArgumentException(elevatorAction + " action is not supported!");
                }
            }

        }
        private void WorkDay()
        {
            while (true)
            {
                GoToWork();
                var outdoorAction = GetRandomOutdoorAction();
                switch (outdoorAction)
                {
                    case AgentActionOutside.Elevator:
                        EnterElevator();
                        break;
                    case AgentActionOutside.Leave:
                        Console.WriteLine(Name + " (" + Security + ")" + " randomly desided to leave the base.");
                        eventLeave.Set();
                        return;
                    default:
                        throw new ArgumentException(outdoorAction + " action is not supported!");
                }
            }
        }

        public void WorkDays()
        {
            Thread t = new Thread(WorkDay);
            t.Start();
        }

        public bool AtHome 
        {
            get
            {
                return eventLeave.WaitOne(0);
            }
        
        }
    }
}
