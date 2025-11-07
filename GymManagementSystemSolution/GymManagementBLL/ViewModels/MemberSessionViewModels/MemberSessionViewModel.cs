using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MemberSessionViewModels
{
    public class MemberSessionViewModel
    {
        public int Id { get; set; }
        public int MemberId { get; set; }

        public string MemberName { get; set; } = null!;
        public string SessionDescription { get; set; } = null!;
        public bool IsAttended { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
