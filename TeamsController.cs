using FpsMatchmaker.CreateRequests;
using FpsMatchmaker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FpsMatchmaker
{

    [ApiController]
    [Route("[controller]")]
    public class TeamsController : ControllerBase
    {
        private readonly GameStatsContext _context;

        public TeamsController(GameStatsContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Team>> CreateTeam(CreateTeamRequest request)
        {
            if (request.Players.Count != 5)
            {
                return BadRequest("Team must have exactly 5 players");
            }

            if (await _context.Teams.AnyAsync(t => t.TeamName == request.TeamName))
            {
                return BadRequest("Team name must be unique");
            }

            var players = await _context.Players
                .Where(p => request.Players.Contains(p.Id))
                .ToListAsync();

            if (players.Count != 5)
            {
                return BadRequest("One or more players not found");
            }

            if (players.Any(p => p.TeamId != null))
            {
                return BadRequest("One or more players already belong to a team");
            }

            var team = new Team
            {
                Id = Guid.NewGuid(),
                TeamName = request.TeamName,
                Players = players
            };

            foreach (var player in players)
            {
                player.TeamId = team.Id;
            }

            _context.Teams.Add(team);
            await _context.SaveChangesAsync();

            return Ok(team);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Team>> GetTeam(Guid id)
        {
            var team = await _context.Teams
                .Include(t => t.Players)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (team == null)
            {
                return NotFound();
            }

            team.Players = team.Players.OrderBy(p => p.Nickname).ToList();
            return Ok(team);
        }
    }


}
