using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Pims.Ltsa.Models
{
    public class BctfaOwnershipList
    {
        private readonly IEnumerable<int> pids;
        public BctfaOwnershipList()
        {
            this.pids = new HashSet<int>();
        }
        [DataMember(Name = "order", EmitDefaultValue = false)]
        public IEnumerable<int> Pids { get; }
        
    }
}
