using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectSuperHeroes.Models;
using ProjectSuperHeroes.Models.SuperHeroes;


namespace ProjectSuperHeroes.Controllers;

public class SuperHeroesController : Controller
{
    private readonly SuperHeroesDbContext _context;

    public SuperHeroesController(SuperHeroesDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> SuperHeroes(int page = 1, int pageSize = 20)
    {
        var totalItems = await _context.Superheroes.CountAsync();
        
        var superheroes = await _context.Superheroes
            .OrderBy(m => m.SuperheroName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
        ViewBag.PageSize = pageSize;

        return View(superheroes);
    }
    public async Task<IActionResult> Powers(int id)
    {
        var heroPowers = await _context.HeroPowers
            .Include(hp => hp.Power)
            .Where(hp => hp.HeroId == id)
            .GroupBy(hp => hp.Power.PowerName)
            .Select(g => new SuperPowerViewModel
            {
                PowerName = g.Key,
                OccurrenceCount = _context.HeroPowers 
                    .Count(hp => hp.Power.PowerName == g.Key)-1
            })
            .ToListAsync();
        
        var hero = await _context.Superheroes.FindAsync(id);

        if (hero == null)
        {
            return NotFound();
        }

        ViewBag.HeroName = hero.SuperheroName;

        return View(heroPowers);
    }
    
    [Authorize]
    [HttpGet("SuperHeroes/Create")]
    public IActionResult Create()
    {
        
        var viewModel = new SuperheroViewModel
        {
            Genders = _context.Genders.ToList(),
            EyeColours = _context.Colours.ToList(),
            HairColours = _context.Colours.ToList(),
            SkinColours = _context.Colours.ToList(),
            Races = _context.Races.ToList(),
            Publishers = _context.Publishers.ToList(),
            Alignments = _context.Alignments.ToList(),
            Superpowers = _context.Superpowers.ToList() 
        };

        return View(viewModel);
    }
   
    [HttpPost("SuperHeroes/Create")]
    
public IActionResult Create(SuperheroViewModel model)
{
    
    
    using (var connection = _context.Database.GetDbConnection())
    {
        connection.Open();
       
        using (var command = connection.CreateCommand())
        {
            command.CommandText = "SELECT IFNULL(MAX(id), 0) + 1 FROM superhero;";
            var result = command.ExecuteScalar();
          
            model.Id = Convert.ToInt32(result);
        }
    }
    ModelState.Remove(nameof(model.Genders));
    ModelState.Remove(nameof(model.EyeColours));
    ModelState.Remove(nameof(model.HairColours));
    ModelState.Remove(nameof(model.SkinColours));
    ModelState.Remove(nameof(model.Races));
    ModelState.Remove(nameof(model.Publishers));
    ModelState.Remove(nameof(model.Alignments));
    ModelState.Remove(nameof(model.Superpowers));
    ModelState.Remove(nameof(model.Id));
    
    if (ModelState.IsValid)
    {
        
        var superhero = new Superhero
        {
            Id = model.Id , 
            SuperheroName = model.SuperheroName,
            FullName = model.FullName,
            WeightKg = model.WeightKg,
            HeightCm = model.HeightCm,
            GenderId = model.GenderId != null ? model.GenderId : 3,
            EyeColourId = model.EyeColourId != null ? model.EyeColourId:1,
            HairColourId = model.HairColourId != null ? model.HairColourId : 1,
            SkinColourId = model.SkinColourId != null ? model.SkinColourId : 1,
            RaceId = model.RaceId != null ? model.RaceId : 1,
            PublisherId = model.PublisherId != null ? model.PublisherId : 1,
            AlignmentId = model.AlignmentId != null ? model.AlignmentId:4,
        };

    
        _context.Superheroes.Add(superhero);
        _context.SaveChanges();
         
        if (model.SelectedPowerIds != null)
        {
            var values = model.SelectedPowerIds
                .Where(id => id != null)
                .Select(id => $"({model.Id}, {id})");

            if (values.Any())
            {
                var sql = $"INSERT INTO hero_power (hero_id, power_id) VALUES {string.Join(", ", values)}";
                _context.Database.ExecuteSqlRaw(sql);
            }
        }

        

        return RedirectToAction("Superheroes"); 
    }

    
    model.Genders = _context.Genders.ToList();
    model.EyeColours = _context.Colours.ToList();
    model.HairColours = _context.Colours.ToList();
    model.SkinColours = _context.Colours.ToList();
    model.Races = _context.Races.ToList();
    model.Publishers = _context.Publishers.ToList();
    model.Alignments = _context.Alignments.ToList();
    model.Superpowers = _context.Superpowers.ToList();

    return View(model);
}
    public IActionResult Details(int id)
    {
        var superhero = _context.Superheroes
            .Include(s => s.Gender)
            .Include(s => s.EyeColour)
            .Include(s => s.HairColour)
            .Include(s => s.SkinColour)
            .Include(s => s.Race)
            .Include(s => s.Publisher)
            .Include(s => s.Alignment)
            .FirstOrDefault(s => s.Id == id);

        if (superhero == null)
        {
            return NotFound();
        }

        var model = new SuperheroViewModel
        {
            Id = superhero.Id,
            SuperheroName = superhero.SuperheroName,
            FullName = superhero.FullName,
            WeightKg = superhero.WeightKg,
            HeightCm = superhero.HeightCm,
            GenderId = superhero.GenderId,
            EyeColourId = superhero.EyeColourId,
            HairColourId = superhero.HairColourId,
            SkinColourId = superhero.SkinColourId,
            RaceId = superhero.RaceId,
            PublisherId = superhero.PublisherId,
            AlignmentId = superhero.AlignmentId,
            

        
            Genders = _context.Genders.ToList(),
            EyeColours = _context.Colours.ToList(),
            HairColours = _context.Colours.ToList(),
            SkinColours = _context.Colours.ToList(),
            Races = _context.Races.ToList(),
            Publishers = _context.Publishers.ToList(),
            Alignments = _context.Alignments.ToList(),
            Superpowers = _context.Superpowers.ToList(),
        };

        return View(model);
    }



}


