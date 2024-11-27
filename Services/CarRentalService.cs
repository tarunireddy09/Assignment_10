using CarRentalSystem.Models;
using System.Threading.Tasks;

public class CarRentalService
{
    private readonly CarRepository _carRepository;

    public CarRentalService(CarRepository carRepository)
    {
        _carRepository = carRepository;
    }

    public async Task<bool> CheckCarAvailability(int carId)
    {
        var car = await _carRepository.GetCarById(carId);
        return car != null && car.IsAvailable;
    }

    public async Task<string> RentCar(int carId, string userName, int rentalDays)
    {
        var car = await _carRepository.GetCarById(carId);

        if (car == null)
        {
            return "Car not found.";
        }

        if (!car.IsAvailable)
        {
            return "Car is not available for rent.";
        }

        decimal totalPrice = car.PricePerDay * rentalDays;

        await _carRepository.UpdateCarAvailability(carId, false);

        return $"Car rented successfully! Total price: ${totalPrice}";
    }
}