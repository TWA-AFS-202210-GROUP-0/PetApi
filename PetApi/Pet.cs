using System;

namespace PetApi
{
    public class Pet
    {
        public Pet(string name, string type, string color, int price)
        {
            Name = name;
            Type = type;
            Color = color;
            Price = price;
        }

        public Pet()
        {
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }
        public int Price { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Pet pet &&
                   Name == pet.Name &&
                   Type == pet.Type &&
                   Color == pet.Color &&
                   Price == pet.Price;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Type, Color, Price);
        }
    }
}