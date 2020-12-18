using System.Collections.Generic;

namespace acmManager.Users.Dto
{
    public class GetAllUserOutput
    {
        public IEnumerable<UserDto> Users { get; set; }
        public long AllResultCount { get; set; }
    }
}