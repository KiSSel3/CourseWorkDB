using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentPlace.Models
{
	public class UserHistoryModel
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public int OperationId { get; set; }
		public DateTime DateTime { get; set; }
	}

}
