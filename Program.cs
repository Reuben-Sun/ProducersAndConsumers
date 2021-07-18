using System;
using System.Threading;

namespace ProducersAndConsumers
{
    class MainClass
    {
        static Mutex mutex = new Mutex();   //互斥信号量
        static Semaphore empty = new Semaphore(10, 10);     //Semaphore(初始值，最大值)
        static Semaphore full = new Semaphore(0, 10);
        static int[] buffer = new int[10];      //大小为10的缓冲池
        static Random rand = new Random();

        static void Main(string[] args)
        {
            new Thread(new ThreadStart(Producer)).Start();
            new Thread(new ThreadStart(Customer)).Start();
            Console.Read();
        }

        private static void Producer()  //生产者
        {
            uint ppointer = 0;
            int temp;
            while (true)
            {
                //生产一个产品，这里是模拟，就不写了
                empty.WaitOne();    //P(empty)
                mutex.WaitOne();    //P(mutex)
                //将一个产品送进缓冲区
                temp = rand.Next(1, 100);
                buffer[ppointer] = temp;
                Console.WriteLine("Producer works at {0} with {1}", ppointer, temp);
                ppointer = (ppointer + 1) % 10;
                mutex.ReleaseMutex();   //V(mutex)
                full.Release();         //V(full)
                Thread.Sleep(400);
            }
        }


        private static void Customer()  //消费者
        {
            uint cpointer = 0;
            int temp;
            while (true)
            {
                full.WaitOne();     //P(full)
                mutex.WaitOne();    //P(mutex)
                //从缓冲区取一个产品
                temp = buffer[cpointer];
                Console.WriteLine("Customer gains at {0} with {1}", cpointer, temp);
                cpointer = (cpointer + 1) % 10;
                mutex.ReleaseMutex();   //V(mutex)
                empty.Release();        //V(empty)
                Thread.Sleep(400);
            }
        }
    

    }
}
