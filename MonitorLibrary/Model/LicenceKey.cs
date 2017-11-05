using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorLibrary.Model
{
    public class LicenceKey
    {
        public int Id { get; set; }
        public Client Client { get; set; }
        public string Key { get; set; }
        public string UnicId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpDate { get; set; }
        public bool Active { get; set; }
    }
}
