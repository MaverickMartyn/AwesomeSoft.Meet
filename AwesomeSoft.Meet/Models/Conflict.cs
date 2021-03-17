using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwesomeSoft.Meet.Models
{
    /// <summary>
    /// A simple container object containing a <see cref="Meeting"/> and all the <see cref="Meeting"/>s it conflicts with.
    /// </summary>
    public class Conflict
    {
        public Conflict(Meeting meeting, List<Meeting> conflictingMeetings)
        {
            this.Meeting = meeting;
            this.Conflicts = conflictingMeetings;
        }

        public Meeting Meeting { get; set; }

        public List<Meeting> Conflicts { get; set; }
    }
}
