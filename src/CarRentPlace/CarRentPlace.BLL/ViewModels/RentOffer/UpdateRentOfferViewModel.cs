using System.ComponentModel.DataAnnotations;

namespace CarRentPlace.BLL.ViewModels.RentOffer;

public class UpdateRentOfferViewModel
{
    [Required(ErrorMessage = "Id is required.")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "CarId is required.")]
    public Guid CarId { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "PricePerDay must be non-negative.")]
    public decimal PricePerDay { get; set; }

    [Required(ErrorMessage = "Location is required.")]
    [StringLength(255, ErrorMessage = "Location must not exceed 255 characters.")]
    public string Location { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description must not exceed 500 characters.")]
    public string? Description { get; set; }

    public bool IsAvailable { get; set; } = true;
}
