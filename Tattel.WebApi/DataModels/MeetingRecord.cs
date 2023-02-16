using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tattel.WebApi.DataModels
{
    public class MeetingRecord
    {
        public MeetingRecord() { }

        public string Id { get; set; }
        public string SenderId { get; set; }
        public string RecipientId { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
