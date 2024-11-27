using CarRentalSystem.Models;
using Microsoft.EntityFrameworkCore;

public class CarRepository
{
    private readonly CarRentalContext _context;

    public CarRepository(CarRentalContext context)
    {
        _context = context;
    }

    public async Task<Car> GetCarById(int id)
    {
        return await _context.Cars.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<Car>> GetAvailableCars()
    {
        return await _context.Cars.Where(c => c.IsAvailable).ToListAsync();
    }

    public async Task AddCar(Car car)
    {
        await _context.Cars.AddAsync(car);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCarAvailability(int carId, bool isAvailable)
    {
        var car = await _context.Cars.FirstOrDefaultAsync(c => c.Id == carId);
        if (car != null)
        {
            car.IsAvailable = isAvailable;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteCar(int carId)
    {
        var car = await _context.Cars.FirstOrDefaultAsync(c => c.Id == carId);
        if (car != null)
        {
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
        }
    }
}
