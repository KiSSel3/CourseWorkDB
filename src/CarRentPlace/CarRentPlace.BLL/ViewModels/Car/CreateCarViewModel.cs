using System.ComponentModel.DataAnnotations;

namespace CarRentPlace.BLL.ViewModels.Car;

public class CreateCarViewModel
{
    [Required(ErrorMessage = "BrandId is required.")]
    public Guid BrandId { get; set; }

    [Required(ErrorMessage = "ModelId is required.")]
    public Guid ModelId { get; set; }

    [Required(ErrorMessage = "BodyTypeId is required.")]
    public Guid BodyTypeId { get; set; }

    [Required(ErrorMessage = "TransmissionTypeId is required.")]
    public Guid TransmissionTypeId { get; set; }

    [Required(ErrorMessage = "DriveTypeId is required.")]
    public Guid DriveTypeId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Seats must be greater than 0.")]
    public int Seats { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Mileage must be non-negative.")]
    public int Mileage { get; set; }
}