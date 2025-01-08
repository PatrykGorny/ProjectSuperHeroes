namespace ProjectSuperHeroes.Models;

public class SuperHeroCreateModel
{
        public string SuperheroName { get; set; }
        public string FullName { get; set; }
        public int HeightCm { get; set; }
        public int WeightKg { get; set; }
        public List<int> SelectedSuperpowers { get; set; } = new List<int>();
    
}