using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using TimerApi.Models;

using TimerContext.Models.Context;

namespace TimerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsContoller : ControllerBase
    {
        private readonly TimerDbContext _context;

        public StatisticsContoller(TimerDbContext context)
        {
            _context = context;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<ICollection<StatisticsComposite>>> GetStatisticsAsync()
        {
            try
            {
                var result = (from user in _context.Users.AsEnumerable()
                              join stat in _context.Statistics on user.StatisticsId equals stat.StatisticsId
                              select new StatisticsComposite
                              {
                                  DateRegister = user.DateRegister,
                                  LastSeen = user.LastSeen,
                                  Username = user.Username,
                                  UserPic = user.UserPic,
                                  UserStatus = user.UserStatus,
                                  SecondsTotalCount = stat.SecondsTotalCount,
                                  SecondsMonthCount = stat.SecondsMonthCount,
                                  SecondsWeekCount = stat.SecondsWeekCount,
                                  SecondsTodayCount = stat.SecondsTodayCount,
                              }).ToList();

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
