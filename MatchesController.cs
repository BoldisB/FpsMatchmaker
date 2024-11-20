using FpsMatchmaker.CreateRequests;
using FpsMatchmaker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FpsMatchmaker
{
    [ApiController]
    [Route("[controller]")]
    public class MatchesController : ControllerBase
    {
        private readonly GameStatsContext _context;

        public MatchesController(GameStatsContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> CreateMatch(CreateMatchRequest request)
        {
            if (request.Duration < 1)
            {
                return BadRequest("Duration must be at least 1 hour");
            }

            var team1 = await _context.Teams
                .Include(t => t.Players)
                .FirstOrDefaultAsync(t => t.Id == request.Team1Id);

            var team2 = await _context.Teams
                .Include(t => t.Players)
                .FirstOrDefaultAsync(t => t.Id == request.Team2Id);

            if (team1 == null || team2 == null)
            {
                return BadRequest("One or both teams not found");
            }

            if (request.WinningTeamId != null &&
                request.WinningTeamId != team1.Id &&
                request.WinningTeamId != team2.Id)
            {
                return BadRequest("Invalid winning team");
            }

            // Update hours played for all players
            foreach (var player in team1.Players.Concat(team2.Players))
            {
                player.HoursPlayed += request.Duration;
                player.RatingAdjustment = EloCalculator.DetermineK(player.HoursPlayed);
            }

            // Calculate team average Elos
            double team1AvgElo = team1.Players.Average(p => p.Elo);
            double team2AvgElo = team2.Players.Average(p => p.Elo);

            // Update stats based on match result
            if (request.WinningTeamId != null)
            {
                var (winners, losers) = request.WinningTeamId == team1.Id
                    ? (team1.Players, team2.Players)
                    : (team2.Players, team1.Players);

                foreach (var player in winners)
                {
                    player.Wins++;
                    player.Elo = EloCalculator.CalculateNewElo(
                        player.Elo,
                        request.WinningTeamId == team1.Id ? team2AvgElo : team1AvgElo,
                        1,
                        player.RatingAdjustment);
                }

                foreach (var player in losers)
                {
                    player.Losses++;
                    player.Elo = EloCalculator.CalculateNewElo(
                        player.Elo,
                        request.WinningTeamId == team1.Id ? team1AvgElo : team2AvgElo,
                        0,
                        player.RatingAdjustment);
                }
            }
            else
            {
                // Draw case
                foreach (var player in team1.Players.Concat(team2.Players))
                {
                    player.Elo = EloCalculator.CalculateNewElo(
                        player.Elo,
                        player.TeamId == team1.Id ? team2AvgElo : team1AvgElo,
                        0.5,
                        player.RatingAdjustment);
                }
            }

            var match = new Match
            {
                Id = Guid.NewGuid(),
                Team1Id = request.Team1Id,
                Team2Id = request.Team2Id,
                WinningTeamId = request.WinningTeamId,
                Duration = request.Duration
            };

            _context.Matches.Add(match);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }

}
