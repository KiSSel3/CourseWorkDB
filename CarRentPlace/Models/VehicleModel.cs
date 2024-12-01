using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentPlace.Models
{
	public class VehicleModel
	{
		public int Id { get; set; }
		public int ProducerId { get; set; }
		public int VehicleTypeId { get; set; }
		public int WheelDriveTypeId { get; set; }
		public int TransmissionTypeId { get; set; }
		public int UserId { get; set; }
		public string Model { get; set; }
		public decimal Mileage { get; set; }
		public decimal EngineDisplacement { get; set; }
		public int Horsepower { get; set; }
		public DateTime ProductionYear { get; set; }
	}
}
