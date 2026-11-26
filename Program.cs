using System;
using System.Text;

namespace ConsoleApp5
{
    public abstract class Animal
    {
        public static double TotalFoodConsumed { get; protected set; } = 0;

        public string Nickname { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public int Energy { get; set; }
        public decimal Price { get; set; }
        public int MealQuantity { get; set; }

        protected int energyLossPerPlay;
        protected int energyGainOnSleep;
        protected int energyGainOnEat;
        protected decimal priceIncreaseOnEat;

        protected Animal(string nickname, int age, string gender, int energy, decimal price, int mealQuantity,
            int lossPlay, int gainSleep, int gainEat, decimal priceInc)
        {
            Nickname = nickname;
            Age = age;
            Gender = gender;
            Energy = Math.Clamp(energy, 0, 100);
            Price = price;
            MealQuantity = mealQuantity;
            energyLossPerPlay = lossPlay;
            energyGainOnSleep = gainSleep;
            energyGainOnEat = gainEat;
            priceIncreaseOnEat = priceInc;
        }

        public virtual void Play()
        {
            if (Energy <= 0)
            {
                Console.WriteLine($"{Nickname} enerjisiz olduğu üçün oynaya bilmir, yatmağa gedir...");
                Sleep();
                return;
            }

            Energy -= energyLossPerPlay;
            Console.WriteLine($"{Nickname} ({GetType().Name}) oynadı. Yeni enerji: {Math.Max(0, Energy)}");

            if (Energy <= 0)
            {
                Energy = 0;
                Console.WriteLine($"{Nickname} enerjisi 0 olduğu üçün yatmağa getdi!");
                Sleep();
            }
        }

        public virtual void Sleep()
        {
            Energy = Math.Min(100, Energy + energyGainOnSleep);
            Console.WriteLine($"{Nickname} yatdı və enerjisi artdı. Cari enerji: {Energy}");
        }

        public virtual void Eat()
        {
            int portion = Math.Max(1, MealQuantity);
            MealQuantity += 1;
            TotalFoodConsumed += portion;

            if (MealQuantity % 3 == 0)
            {
                Age += 1;
            }

            Price += priceIncreaseOnEat;
            Energy = Math.Min(100, Energy + energyGainOnEat);

            Console.WriteLine($"{Nickname} yemək yedi (+{portion} vahid yemək). Yaş: {Age}, Enerji: {Energy}, Yeni Qiymət: {Price} AZN");
        }

        public override string ToString()
        {
            return $"[{GetType().Name}] Ad: {Nickname} | Yaş: {Age} | Cins: {Gender} | Enerji: {Energy} | Qiymət: {Price} AZN | Porsiya: {MealQuantity}";
        }
    }

    public class Cat : Animal
    {
        public Cat(string nickname, int age, string gender, int energy = 60, decimal price = 250m, int mealQuantity = 1)
            : base(nickname, age, gender, energy, price, mealQuantity, 25, 45, 20, 35m)
        {
        }
    }

    public class Dog : Animal
    {
        public Dog(string nickname, int age, string gender, int energy = 75, decimal price = 400m, int mealQuantity = 2)
            : base(nickname, age, gender, energy, price, mealQuantity, 30, 50, 25, 45m)
        {
        }
    }

    public class Bird : Animal
    {
        public Bird(string nickname, int age, string gender, int energy = 50, decimal price = 120m, int mealQuantity = 1)
            : base(nickname, age, gender, energy, price, mealQuantity, 20, 40, 15, 15m)
        {
        }
    }

    public class Fish : Animal
    {
        public Fish(string nickname, int age, string gender, int energy = 40, decimal price = 60m, int mealQuantity = 1)
            : base(nickname, age, gender, energy, price, mealQuantity, 15, 30, 10, 10m)
        {
        }
    }

    public class PetShop
    {
        public Cat[] Cats;
        public Dog[] Dogs;
        public Bird[] Birds;
        public Fish[] Fishes;

        public PetShop(int initialCapacity = 10)
        {
            Cats = new Cat[initialCapacity];
            Dogs = new Dog[initialCapacity];
            Birds = new Bird[initialCapacity];
            Fishes = new Fish[initialCapacity];
        }

        public void AddCat(Cat cat) => AddToArray(ref Cats, cat);
        public void AddDog(Dog dog) => AddToArray(ref Dogs, dog);
        public void AddBird(Bird bird) => AddToArray(ref Birds, bird);
        public void AddFish(Fish fish) => AddToArray(ref Fishes, fish);

        private void AddToArray<T>(ref T[] array, T item) where T : Animal
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == null)
                {
                    array[i] = item;
                    return;
                }
            }

            Array.Resize(ref array, array.Length + 1);
            array[^1] = item;
        }

        public bool RemoveByNickName(string nickname)
        {
            return RemoveFromArray(Cats, nickname) ||
                   RemoveFromArray(Dogs, nickname) ||
                   RemoveFromArray(Birds, nickname) ||
                   RemoveFromArray(Fishes, nickname);
        }

        private bool RemoveFromArray<T>(T[] array, string nickname) where T : Animal
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] != null && string.Equals(array[i].Nickname, nickname, StringComparison.OrdinalIgnoreCase))
                {
                    array[i] = null;
                    return true;
                }
            }
            return false;
        }

        public decimal GetTotalCatsPrice()
        {
            decimal total = 0;
            foreach (var cat in Cats)
            {
                if (cat != null) total += cat.Price;
            }
            return total;
        }

        public decimal GetTotalAllAnimalsPrice()
        {
            return SumArrayPrices(Cats) + SumArrayPrices(Dogs) + SumArrayPrices(Birds) + SumArrayPrices(Fishes);
        }

        private decimal SumArrayPrices<T>(T[] array) where T : Animal
        {
            decimal sum = 0;
            foreach (var item in array)
            {
                if (item != null) sum += item.Price;
            }
            return sum;
        }

        public void PrintAllAnimals()
        {
            Console.WriteLine("\n--- PetShop Heyvan Siyahısı ---");
            PrintArray(Cats);
            PrintArray(Dogs);
            PrintArray(Birds);
            PrintArray(Fishes);
        }

        private void PrintArray<T>(T[] array) where T : Animal
        {
            foreach (var item in array)
            {
                if (item != null) Console.WriteLine(item);
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            PetShop shop = new PetShop(5);

            Cat tom = new Cat("Tom", 1, "Erkək", energy: 50, price: 200m, mealQuantity: 1);
            Cat luna = new Cat("Luna", 2, "Dişi", energy: 30, price: 220m, mealQuantity: 1);
            Dog rex = new Dog("Rex", 3, "Erkək", energy: 60, price: 450m, mealQuantity: 2);
            Bird tweety = new Bird("Tweety", 1, "Dişi", energy: 40, price: 100m, mealQuantity: 1);
            Fish nemo = new Fish("Nemo", 1, "Erkək", energy: 30, price: 50m, mealQuantity: 1);

            shop.AddCat(tom);
            shop.AddCat(luna);
            shop.AddDog(rex);
            shop.AddBird(tweety);
            shop.AddFish(nemo);

            
            tom.Play();
            tom.Play();
            tom.Play();

            
            tom.Eat();
            luna.Eat();
            rex.Eat();
            tweety.Eat();
            nemo.Eat();

            shop.PrintAllAnimals();

            Console.WriteLine($"\nÜmumi yedizdirilən yemək miqdarı: {Animal.TotalFoodConsumed} vahid");
            Console.WriteLine($"Pişiklərin ümumi qiyməti: {shop.GetTotalCatsPrice()} AZN");
            Console.WriteLine($"Bütün heyvanların ümumi qiyməti: {shop.GetTotalAllAnimalsPrice()} AZN");

            if (shop.RemoveByNickName("Tweety"))
            {
                Console.WriteLine("\n'Tweety' PetShop-dan silindi.");
            }

            shop.PrintAllAnimals();
        }
    }
}