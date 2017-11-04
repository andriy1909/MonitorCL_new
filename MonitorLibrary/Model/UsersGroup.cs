using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorLibrary.Model
{
    public class UsersGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Information { get; set; }
        public int Level { get; set; }
        public UsersGroup Parent { get; set; }
        
        public List<Client> Users { get; set; }
    }
}
