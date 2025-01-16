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
    public IActionResult Create()
    {
        var availablePowers = _context.Superpowers.ToList();
        ViewBag.AvailablePowers = availablePowers;
        return View();
    }
   
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Superhero superhero)
    { 
       return View(superhero);
    }
   
   

}


