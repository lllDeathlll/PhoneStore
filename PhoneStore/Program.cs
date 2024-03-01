using PhoneStore;

internal class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the phone store!");

        var path = @"store.json";

        StoreService store = new(path);

        WriteHelp();

        while (true)
        {
            var input = Console.ReadLine() ?? "help";
            if (int.TryParse(input, out var choice))
            {
                switch (choice)
                {
                    case 1:
                        ListPhones(store);
                        break;
                    case 2:
                        ListPhonesByYear(store);
                        break;
                    case 3:
                        AddPhone(store);
                        break;
                    case 4:
                        BuyPhone(store);
                        break;
                    case 5:
                        BuyAllPhones(store);
                        break;
                    case 6:
                        store.Save();
                        break;
                    case 7:
                        Console.Clear();
                        break;
                    case 8:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Choose the option from the list:");
                        WriteHelp();
                        break;
                }
            }
            else
            {
                if (input.ToLower() == "help")
                {
                    WriteHelp();
                }
                else
                {
                    Console.WriteLine("You need to choose the option");
                }
            }
        }
    }

    public static void WriteHelp()
    {
        Console.WriteLine("Options:\n" +
                          "1) List available phones \n" +
                          "2) List phones by release year \n" +
                          "3) Add phone to store\n" +
                          "4) Buy phone from store\n" +
                          "5) Buy all phones\n" +
                          "6) Save\n" +
                          "7) Clear Console\n" +
                          "8) Exit\n" +
                          "Help - list options");
    }

    public static Phone? CreatePhone()
    {
        Console.WriteLine("Enter the model:");
        var model = Console.ReadLine();
        if (model == null)
        {
            Console.WriteLine("You need to enter a phone model!");
            return null;
        }

        Console.WriteLine("Enter the phone price:");
        if (!int.TryParse(Console.ReadLine(), out int price))
        {
            Console.WriteLine("You must enter a correct price!");
            return null;
        }

        Console.WriteLine("Enter the release year:");
        if (!int.TryParse(Console.ReadLine(), out int year))
        {
            Console.WriteLine("You must enter a correct year!");
            return null;
        }
        Console.WriteLine("Enter the release month:");
        if (!int.TryParse(Console.ReadLine(), out int month))
        {
            Console.WriteLine("You must enter a correct year!");
            return null;
        }
        Console.WriteLine("Enter the release day:");
        if (!int.TryParse(Console.ReadLine(), out int day))
        {
            Console.WriteLine("You must enter a correct year!");
            return null;
        }
        DateOnly date = new(year, month, day);

        Phone phone = new(model, price, date);
        return phone;
    }

    public static void ListPhones(StoreService store)
    {
        Console.WriteLine("Phones:");
        for (int i = 1; i <= store.Phones.Count; i++)
        {
            Console.WriteLine($"{i}) {store.Phones.ElementAt(i-1)}");
        }
    }

    private static void ListPhonesByYear(StoreService store)
    {
        Console.WriteLine("Enter the year:");
        if (!int.TryParse(Console.ReadLine(), out int year))
        {
            Console.WriteLine("You must enter a valid year");
            return;
        }
        var phones = store.Phones.Where((phone => phone.ReleaseDate.Year == year)).ToList();
        Console.WriteLine("Phones:");
        for (int i = 1; i <= phones.Count; i++)
        {
            Console.WriteLine($"{i}) {phones.ElementAt(i-1)}");
        }
    }

    public static void AddPhone(StoreService store)
    {
        var phone = CreatePhone();
        if (phone == null) return;
        Console.WriteLine(phone);
        store.AddPhone(phone);
    }

    public static void BuyPhone(StoreService store)
    {
        Console.WriteLine("Find phone by:\n" +
                          "1) Number\n" +
                          "2) Model\n" +
                          "3) Release year");
        int choice;
        try
        {
            choice = Convert.ToInt32(Console.ReadLine());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return;
        }

        Phone? phone;

        switch (choice)
        {
            case 1:
                Console.WriteLine("Phones:");
                for (int i = 1; i <= store.Phones.Count; i++)
                {
                    Console.WriteLine($"{i}) {store.Phones.ElementAt(i-1)}");
                }
                Console.WriteLine("Enter the number:");
                if (int.TryParse(Console.ReadLine(), out var index))
                {
                    phone = store.PhoneByIndex(index+1);
                }
                else
                {
                    return;
                }
                break;
            case 2:
                Console.WriteLine("Enter the model name:");
                var model = Console.ReadLine();
                if (model == null)
                {
                    Console.WriteLine("You must enter a model!");
                    return;
                }
                phone = CorrectPhone(store.PhonesByModel(model));
                break;
            case 3:
                Console.WriteLine("Enter the release year:");
                if (int.TryParse(Console.ReadLine(), out var year))
                {
                    phone = CorrectPhone(store.PhonesByYear(year));
                }
                else
                {
                    Console.WriteLine("You must choose a correct year!");
                    return;
                }
                break;
            default:
                Console.WriteLine("You need to choose one on the options");
                return;
        }

        if (phone == null) return;
        Console.WriteLine($"You bought {phone.Model} made in {phone.ReleaseDate}");
        Console.WriteLine($"Your total is ${phone.Price}");
    }

    public static Phone? CorrectPhone(List<Phone> phones)
    {
        if (!phones.Any()) return null;

        Console.WriteLine("Which phone do you want?");

        for (int i = 1; i <= phones.Count; i++)
        {
            Console.WriteLine($"{i}) {phones.ElementAt(i-1)}");
        }

        if (int.TryParse(Console.ReadLine(), out var index))
        {
            try
            {
                return phones.ElementAt(index);
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("You must choose a correct number!");
            }
        }
        else
        {
            Console.WriteLine("You must enter a number!");
            return null;
        }

        return null;
    }

    public static void BuyAllPhones(StoreService store)
    {
        var phones = store.Phones;
        Console.WriteLine("You bought:");
        foreach (var phone in phones)
        {
            Console.WriteLine($"{phone.Model} made in {phone.ReleaseDate}");
        }
        Console.WriteLine($"Your total is ${phones.Select(phone => phone.Price).Sum()}");
    }
}