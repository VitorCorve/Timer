using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Globalization;

using TimerApi.Models;

using TimerContext.Models;
using TimerContext.Models.Context;

namespace TimerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly TimerDbContext _context;
        private static Calendar _calendar = new CultureInfo("en-US").Calendar;

        public DataController(TimerDbContext context)
        {
            _context = context;
        }

        [HttpPost, Authorize]
        public async Task<ActionResult<Statistics>> UpdateDate(int secondsTodayCount)
        {
            var userIdentifier = User.Claims.FirstOrDefault(x => x.Type.Contains("serial"))?.Value;

            if (string.IsNullOrEmpty(userIdentifier))
                return BadRequest("Authorizaton error");

            else
            {
                var statisticId = _context.Users.FirstOrDefault(x => x.UserIdentifier.ToString().Equals(userIdentifier))?.StatisticsId;

                Statistics statistics = _context.Statistics.FirstOrDefault(x => x.StatisticsId.Equals(statisticId));

                if (statistics is null)
                {
                    statistics = new Statistics();
                    return BadRequest("Invalid registration error");
                }

                else
                {
                    int timeDifference = (int)(DateTime.UtcNow - statistics.LastUpdateTime).TotalSeconds;

                    if (timeDifference < 50)
                    {
                        return BadRequest("Too much requests");
                    }

                    else
                    {

                        if (statistics.SecondsTodayCount + timeDifference + 60 < secondsTodayCount)
                        {
                            return BadRequest("Cheat attempt registered");
                        }

                        else
                        {
                            SetStatistics(statistics, secondsTodayCount);
                            _context.Entry(statistics).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                            await _context.SaveChangesAsync();

                            return Ok(statistics);
                        }
                    }
                }
            }
        }

        [HttpPost("Update"), Authorize]
        public async Task<ActionResult> UpdateDate(string userStatus, byte[] userPic)
        {
            var userIdentifier = User.Claims.FirstOrDefault(x => x.Type.Contains("serial"))?.Value;

            if (string.IsNullOrEmpty(userIdentifier))
                return BadRequest("Authorizaton error");

            else
            {
                User user = _context.Users.FirstOrDefault(x => x.UserIdentifier.Equals(Guid.Parse(userIdentifier)));

                if (user is null)
                {
                    return BadRequest("User not found");
                }
                else
                {
                    user.UserStatus = userStatus;
                    user.UserPic = userPic;

                    _context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                    await _context.SaveChangesAsync();

                    return Ok();
                }
            }
        }

        private static void SetStatistics(Statistics statistics, int secondsTodayCount)
        {
            statistics.SecondsTotalCount = statistics.SecondsTotalCount - statistics.SecondsTodayCount + secondsTodayCount;
            statistics.SecondsWeekCount = statistics.SecondsWeekCount - statistics.SecondsTodayCount + secondsTodayCount; ;
            statistics.SecondsMonthCount = statistics.SecondsMonthCount - statistics.SecondsTodayCount + secondsTodayCount;
            statistics.SecondsTodayCount = secondsTodayCount;

            int week = _calendar.GetWeekOfYear(DateTime.UtcNow, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            int day = _calendar.GetDayOfYear(DateTime.UtcNow);

            if (DateTime.UtcNow.Month > statistics.LastInitMonth)
            {
                statistics.LastInitMonth = DateTime.UtcNow.Month;
                statistics.SecondsMonthCount = 0;
            }

            if (week > statistics.LastInitWeekNumber)
            {
                statistics.LastInitWeekNumber = week;
                statistics.SecondsWeekCount = 0;
            }

            if (day > statistics.LastInitDay)
            {
                statistics.LastInitDay = day;
                statistics.SecondsTodayCount = 0;
            }

            statistics.LastUpdateToken = Guid.NewGuid();
            statistics.LastUpdateTime = DateTime.UtcNow;
        }

        [HttpGet("Bio"), Authorize]
        public async Task<ActionResult<StatisticsComposite>> GetBio(string userIdentifier)
        {
            User user = _context.Users.FirstOrDefault(x => x.UserIdentifier.Equals(Guid.Parse(userIdentifier)));

            if (user is null)
            {
                return BadRequest("User not found");
            }
            else
            {
                Statistics statistics = _context.Statistics.FirstOrDefault(x => x.StatisticsId.Equals(user.StatisticsId));

                if (statistics is null)
                {
                    return BadRequest("User not found");
                }

                return Ok( new StatisticsComposite
                {
                    DateRegister = user.DateRegister,
                    LastSeen = user.LastSeen,
                    Username = user.Username,
                    UserPic = user.UserPic,
                    UserStatus = user.UserStatus,
                    SecondsTotalCount = statistics.SecondsTotalCount,
                    SecondsMonthCount = statistics.SecondsMonthCount,
                    SecondsWeekCount = statistics.SecondsWeekCount,
                    SecondsTodayCount = statistics.SecondsTodayCount,
                    LastInitDay = statistics.LastInitDay,
                    LastInitMonth = statistics.LastInitMonth,
                    LastInitWeekNumber = statistics.LastInitWeekNumber
                });
            }
        }

        [HttpGet("UserInfo"), Authorize]
        public async Task<ActionResult<StatisticsComposite>> UserInfo()
        {
            string userIdentifier = User.Claims.FirstOrDefault(x => x.Type.Contains("serial"))?.Value;

            if (string.IsNullOrEmpty(userIdentifier))
                return BadRequest("Authorizaton error");

            else
            {
                var statisticId = _context.Users.FirstOrDefault(x => x.UserIdentifier.ToString().Equals(userIdentifier))?.StatisticsId;

                Statistics statistics = _context.Statistics.FirstOrDefault(x => x.StatisticsId.Equals(statisticId));
                User user = _context.Users.FirstOrDefault(x => x.UserIdentifier.Equals(Guid.Parse(userIdentifier)));

                if (user is null)
                {
                    return BadRequest("User not found");
                }
                else
                {
                    if (statistics is null)
                    {
                        return BadRequest("User not found");
                    }

                    return Ok(new StatisticsComposite
                    {
                        DateRegister = user.DateRegister,
                        LastSeen = user.LastSeen,
                        Username = user.Username,
                        UserPic = user.UserPic,
                        UserStatus = user.UserStatus,
                        SecondsTotalCount = statistics.SecondsTotalCount,
                        SecondsMonthCount = statistics.SecondsMonthCount,
                        SecondsWeekCount = statistics.SecondsWeekCount,
                        SecondsTodayCount = statistics.SecondsTodayCount,
                        LastInitDay = statistics.LastInitDay,
                        LastInitMonth = statistics.LastInitMonth,
                        LastInitWeekNumber = statistics.LastInitWeekNumber
                    });
                }
            }
        }
    }
}
