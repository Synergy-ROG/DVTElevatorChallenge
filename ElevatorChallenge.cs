using System;
using System.Collections.Generic;
using System.Text;

namespace ElevatorChallenge
{

    // Enum to represent the direction of elevator movement
    public enum Direction
    {
        Up,
        Down,
        Stopped
    }

    public class Elevator
    {
        public int Id { get; private set; }
        public int CurrentFloor { get; set; }
        public bool IsMoving { get; set; }
        public Direction CurrentDirection { get; private set; }
        public int Capacity { get; set; }
        public int NumPeople { get; private set; }

        public Elevator(int id, int capacity)
        {
            Id = id;
            CurrentFloor = 1;
            IsMoving = false;
            CurrentDirection = Direction.Stopped;
            Capacity = capacity;
            NumPeople = 0;
        }

        public void MoveToFloor(int TargetFloor)
        {
            if (TargetFloor > CurrentFloor)
                CurrentDirection = Direction.Up;
            else if (TargetFloor < CurrentFloor)
                CurrentDirection = Direction.Down;

            Console.WriteLine($"Elevator {Id} is moving from floor {CurrentFloor} to floor {TargetFloor}.");
            CurrentFloor = TargetFloor;
            Console.WriteLine($"Elevator {Id} has arrived at floor {CurrentFloor}.");

            // Set IsMoving property after reaching the target floor
            IsMoving = false;
        }

        public void Enter(int numPeople)
        {
            if (NumPeople + numPeople <= Capacity)
            {
                NumPeople += numPeople;
                Console.WriteLine($"Entered {numPeople} people in elevator {Id}.");
            }
            else
            {
                Console.WriteLine($"Elevator {Id} is at full capacity.");
            }
        }

        public void Exit(int numPeople)
        {
            if (NumPeople - numPeople >= 0)
            {
                NumPeople -= numPeople;
                Console.WriteLine($"Exited {numPeople} people from elevator {Id}.");
            }
            else
            {
                Console.WriteLine($"There are not enough people in elevator {Id}.");
            }
        }
    }

    public class ElevatorSystem
    {
        private List<Elevator> elevators;


        public ElevatorSystem(int numElevators, int elevatorCapacity)
        {
            elevators = new List<Elevator>();
            for (int i = 1; i <= numElevators; i++)
            {
                elevators.Add(new Elevator(i, elevatorCapacity));
            }
        }

        public List<Elevator> GetElevators()
        {
            return elevators;
        }

        public void ShowElevatorStatus()
        {
            foreach (var elevator in elevators)
            {
                Console.WriteLine($"Elevator {elevator.Id}:");
                Console.WriteLine($"  Current floor: {elevator.CurrentFloor}");
                Console.WriteLine($"  Is moving: {elevator.IsMoving}");
                Console.WriteLine($"  Direction: {elevator.CurrentDirection}");
                Console.WriteLine($"  Number of people: {elevator.NumPeople}");
                Console.WriteLine();
            }
        }

        public void CallElevator(int floor, int numPeople)
        {
            bool allElevatorsBusy = true;

            foreach (var elevator in elevators)
            {
                if (!elevator.IsMoving)
                {
                    allElevatorsBusy = false;
                    break;
                }
            }

            if (allElevatorsBusy)
            {
                Console.WriteLine("All elevators are currently busy. Please wait.");
                return;
            }

            Elevator nearestElevator = null;
            int minDistance = int.MaxValue;

            foreach (var elevator in elevators)
            {
                if (!elevator.IsMoving)
                {
                    int distance = Math.Abs(elevator.CurrentFloor - floor);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestElevator = elevator;
                    }
                }
            }

            if (nearestElevator != null)
            {
                nearestElevator.MoveToFloor(floor);
                nearestElevator.Enter(numPeople);
                nearestElevator.IsMoving = true; // Set IsMoving to true after the elevator starts moving
            }
        }



        //xUnit testing methods called
        public Elevator GetElevator(int elevatorId)
        {
            return elevators.FirstOrDefault(elevator => elevator.Id == elevatorId);
        }
        public bool AllElevatorsBusy()
        {
            foreach (var elevator in elevators)
            {
                if (!elevator.IsMoving)
                {
                    return false; // At least one elevator is not busy
                }
            }
            return true; // All elevators are busy
        }
        //xUnit testing methods called
    }

    class ElevatorChallenge
    {
        static void Main(string[] args)
        {
            // Create an elevator system with 3 elevators, each with a capacity of 10 people
            ElevatorSystem elevatorSystem = new ElevatorSystem(3, 10);
            Console.WriteLine("Welcome to Dynamic Visual Technologies Elevator Challenge!!!");
            while (true)
            {

                Console.WriteLine("1. Show elevator status");
                Console.WriteLine("2. Call elevator");
                Console.WriteLine("3. Exit");
                Console.Write("Please enter your choice: ");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        elevatorSystem.ShowElevatorStatus();
                        break;

                    case 2:
                        Console.Write("Please enter the floor to call the elevator: ");
                        int floor = int.Parse(Console.ReadLine());
                        Console.Write("Please enter the number of people: ");
                        int numPeople = int.Parse(Console.ReadLine());
                        elevatorSystem.CallElevator(floor, numPeople);
                        break;

                    case 3:
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }

                Console.WriteLine();
            }
        }
    }
}