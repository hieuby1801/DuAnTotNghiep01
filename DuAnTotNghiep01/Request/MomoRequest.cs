using DATN_API.Helpers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_API.Request
{
    public class MomoRequest
    {
		public int OrderId { get; set; }
		public string OrderInfo { get; set; }
		public int Amount { get; set; }
	
	}
}
