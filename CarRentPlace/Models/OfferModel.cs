using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentPlace.Models
{
	public class OfferModel
	{
		public int Id { get; set; }
		public int VehicleId { get; set; }
		public int UserId { get; set; }
		public DateTime PublicationDate { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public string Location { get; set; }
		public bool Show { get; set; }
		public bool Approved { get; set; }
	}

}
