using System.Text.Json;

namespace PhoneStore;

public class Phone
{
    private DateOnly _releaseDate;

    public string Model { get; init; }
    public int Price { get; init; }

    public DateOnly ReleaseDate
    {
        get => _releaseDate;
        init
        {
            var now = DateOnly.FromDateTime(DateTime.Now);
            if (value > now)
            {
                _releaseDate = now;
            }
            else
            {
                _releaseDate = value;
            }
        }
    }

    public Phone(string model, int price, DateOnly releaseDate)
    {
        Model = model;
        Price = price;
        ReleaseDate = releaseDate;
    }

    public override string ToString()
    {
        return String.Format($"{Model} - {ReleaseDate.Year}.{ReleaseDate.Month}.{ReleaseDate.Day}");
    }
}

public class StoreService
{
    public string Path { get; init; }
    public List<Phone> Phones { get; private set; }

    public StoreService(string path)
    {
        Path = path;
        if (File.Exists(path))
        {
            try
            {
                var jsonString = File.ReadAllText(Path);
                var phonesList = JsonSerializer.Deserialize<List<Phone>>(jsonString);
                Phones = phonesList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        else
        {
            Phones = new List<Phone>();
        }
    }
    public Phone? PhoneByIndex(int index)
    {
        try
        {
            var phone = Phones.ElementAt(index);
            return phone;
        }
        catch (ArgumentOutOfRangeException)
        {
            Console.WriteLine("You need to enter the correct index!");
            return null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    public List<Phone> PhonesByModel(string model)
    {
        return Phones.Where(phone => phone.Model.ToLower().Contains(model.ToLower())).ToList();
    }
    public List<Phone> PhonesByYear(int year)
    {
        return Phones.Where(phone => phone.ReleaseDate.Year == year).ToList();
    }
    public void AddPhone(Phone phone)
    {
        Phones.Add(phone);
    }
    public void RemovePhone(Phone phone)
    {
        Phones.Remove(phone);
    }
    public void RemovePhoneByIndex(int index)
    {
        Phones.RemoveAt(index);
    }
    public void Save()
    {
        var jsonString = JsonSerializer.Serialize(Phones);
        File.WriteAllText(Path, jsonString);
    }
}