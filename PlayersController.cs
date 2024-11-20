using FpsMatchmaker.CreateRequests;
using FpsMatchmaker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FpsMatchmaker
{

    [ApiController]
    [Route("[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly GameStatsContext _context;

        public PlayersController(GameStatsContext context)
        {
            _context = context;
        }

        [HttpPost("create")]
        public async Task<ActionResult<Player>> CreatePlayer(CreatePlayerRequest request)
        {
            if (await _context.Players.AnyAsync(p => p.Nickname == request.Nickname))
            {
                return BadRequest("Nickname must be unique");
            }

            var player = new Player
            {
                Id = Guid.NewGuid(),
                Nickname = request.Nickname,
                Wins = 0,
                Losses = 0,
                Elo = 0,
                HoursPlayed = 0,
                RatingAdjustment = 50
            };

            _context.Players.Add(player);
            await _context.SaveChangesAsync();

            var response = new Player
            {
                Id = player.Id,
                Nickname = player.Nickname,
                Wins = player.Wins,
                Losses = player.Losses,
                Elo = player.Elo,
                HoursPlayed = player.HoursPlayed,
                TeamId = null,
                RatingAdjustment = 0
            };

            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayer(Guid id)
        {
            var player = await _context.Players.FindAsync(id);

            if (player == null)
            {
                return NotFound();
            }

            return Ok(player);
        }
    }


}
