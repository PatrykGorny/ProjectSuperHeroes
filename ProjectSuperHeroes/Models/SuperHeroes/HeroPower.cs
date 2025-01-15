using System;
using System.Collections.Generic;

namespace ProjectSuperHeroes.Models.SuperHeroes;

public partial class HeroPower
{
    public int? HeroId { get; set; }

    public int? PowerId { get; set; }

    public virtual Superhero? Hero{ get; set; }

    public virtual Superpower? Power { get; set; }
}
