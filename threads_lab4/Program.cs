using System;

namespace lab4_threads
{
    class Program
    {
        public static  Semaphore[] forks = new Semaphore[5];
        public static Semaphore waiter = new Semaphore(4,4);
        public static Thread[] philosophers = new Thread[5];
        static void Main(string[] args)
        {
            Program program = new Program();
            program.Start();
        }
        public void Start()
        {
            for (int i = 0; i < forks.Length; i++)
            {
                forks[i] = new Semaphore(1, 1);
            }
            for (int i = 0; i < 5; i++)
            {
                philosophers[i] = new Thread(Dinner);
                philosophers[i].Start(new Philosopher(i));
            }
        }
        public void getFork(int id)
        {
            forks[id].WaitOne();
        }
        public void putFork(int id)
        {
            forks[id].Release();
        }
        private void Dinner(object obj)
        {
            if (obj is Philosopher philosopher)
            {
                philosopher = (Philosopher)obj;

                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine("Philosopher" + philosopher.Id + " думає " + (i + 1) + "-й раз");
                    waiter.WaitOne();
                    getFork(philosopher.rightFork);
                    getFork(philosopher.leftFork);

                    Console.WriteLine("Philosopher" + philosopher.Id + " поїв " + (i + 1) + "-й раз");
                    putFork(philosopher.leftFork);
                    putFork(philosopher.rightFork);
                    waiter.Release();
                }
            }
        }
    }

    class Philosopher
    {
        public Philosopher(int id)
        {
            Id = id;
            rightFork = id;
            leftFork = (id + 1) % 5;
        }
        public int Id { get; set; }
        public int leftFork { get; set; }
        public int rightFork { get; set; }
    }

}

