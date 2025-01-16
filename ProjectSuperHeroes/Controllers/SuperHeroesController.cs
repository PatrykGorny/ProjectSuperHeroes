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
    public IActionResult Powers(int id)
    {
        return View();
    }
    [Authorize]
    public IActionResult Create()
    {
        return View();
    }

    public async Task<IActionResult> SuperPowers(int page = 1, int pageSize = 20)
    {
        var totalItems = await _context.HeroPowers
            .GroupBy(hp => hp.Power.PowerName)
            .CountAsync();

        
        var superpowers = await _context.HeroPowers
            .GroupBy(hp => hp.Power.PowerName) 
            .Select(g => new SuperPowerViewModel 
            {
                PowerName = g.Key,
                OccurrenceCount = g.Count() 
            })
            .OrderBy(m => m.PowerName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
        ViewBag.PageSize = pageSize;

        
        return View(superpowers);
    }

    }

