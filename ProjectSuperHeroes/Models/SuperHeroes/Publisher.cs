using System;
using System.Collections.Generic;

namespace ProjectSuperHeroes.Models.SuperHeroes;

public partial class Publisher
{
    public int Id { get; set; }

    public string? PublisherName { get; set; }

    public virtual ICollection<Superhero> Superheroes { get; set; } = new List<Superhero>();
}
