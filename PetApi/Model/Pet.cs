namespace PetApi
{
    public class Pet
    {
        private string name;
        private string type;
        private string color;
        private int price;

        public Pet(string name, string type, string color, int price)
        {
            this.Name = name;
            this.Type = type;
            this.Color = color;
            this.Price = price;
        }

        public Pet()
        {
        }

        public int Price { get => price; set => price = value; }
        public string Color { get => color; set => color = value; }
        public string Type { get => type; set => type = value; }
        public string Name { get => name; set => name = value; }

        public override bool Equals(object? obj)
        {
            var pet = obj as Pet;
            return pet != null &&
                Name.Equals(pet.Name) &&
                Type.Equals(pet.Type) &&
                Price.Equals(pet.Price) &&
                Color.Equals(pet.Color);    
        }
    }
}