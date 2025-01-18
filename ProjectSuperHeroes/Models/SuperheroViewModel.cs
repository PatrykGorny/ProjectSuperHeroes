using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ProjectSuperHeroes.Models.SuperHeroes;

public class SuperheroViewModel
{
    public int Id { get; set; }
    public string SuperheroName { get; set; }
    public int? WeightKg { get; set; }
    public int? HeightCm { get; set; }
    public string FullName { get; set; }


    public int? GenderId { get; set; }
    public int? EyeColourId { get; set; }
    public int? HairColourId { get; set; }
    public int? SkinColourId { get; set; }
    public int? RaceId { get; set; }
    public int? PublisherId { get; set; }
    public int? AlignmentId { get; set; }

    
    public List<Gender> Genders { get; set; }
    
    public List<Colour> EyeColours { get; set; }
    
    public List<Colour> HairColours { get; set; }
    
    public List<Colour> SkinColours { get; set; }
    
    public List<Race> Races { get; set; }
    
    public List<Publisher> Publishers { get; set; }
    
    public List<Alignment> Alignments { get; set; }

  
    public List<Superpower> Superpowers { get; set; }
  
    
    public List<int> SelectedPowerIds { get; set; } 
}