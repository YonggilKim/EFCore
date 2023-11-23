using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EFGetStarted
{
    public static class Extensions
    {
        public static IQueryable<GuildDto> MapGuildDto(this IQueryable<Guild> guild)
        {
            return guild.Select(g => new GuildDto
            {
                Name = g.GuildName,
                MemberCount = g.Members.Count
            });
        }
    }
}
