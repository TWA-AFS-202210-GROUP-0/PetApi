namespace PetApi.Dto
{
    public class PetDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }
        public double Price { get; set; }

        public PetDto()
        {
        }

        public override bool Equals(object? obj)
        {
            var pet = obj as PetDto;
            return pet != null &&
                   Name.Equals(pet.Name) &&
                   Type.Equals(pet.Type) &&
                   Color.Equals(pet.Color) &&
                   Price.Equals(pet.Price);
        }
    }
}
