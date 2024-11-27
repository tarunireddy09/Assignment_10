using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CarRentalSystem.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

//[Authorize] 
[ApiController]
[Route("api/[controller]")]
public class CarController : ControllerBase
{
    private readonly CarRentalService _carRentalService;
    private readonly CarRepository _carRepository;

    public CarController(CarRentalService carRentalService, CarRepository carRepository)
    {
        _carRentalService = carRentalService;
        _carRepository = carRepository;
    }
    
    [HttpGet("check-availability")]
    [Route("cars")]
    public async Task<IActionResult> GetAvailableCars()
    {
        var availableCars = await _carRepository.GetAvailableCars();
        return Ok(availableCars);
    }

    [HttpGet("check-availability/{id}")]
    public async Task<IActionResult> CheckAvailability(int id)
    {
        bool isAvailable = await _carRentalService.CheckCarAvailability(id);
        return Ok(isAvailable);
    }

    //[Authorize(Roles = "Admin")] 
    [HttpPost]
    public async Task<IActionResult> AddCar([FromBody] Car car)
    {
        if (car == null)
        {
            return BadRequest("Car data is required.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _carRepository.AddCar(car);
        return CreatedAtAction(nameof(GetAvailableCars), new { id = car.Id }, car);
    }

    [Authorize(Roles = "Admin")] 
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCar(int id, [FromBody] Car updatedCar)
    {
        if (id != updatedCar.Id)
        {
            return BadRequest("Car ID mismatch.");
        }

        var existingCar = await _carRepository.GetCarById(id);
        if (existingCar == null)
        {
            return NotFound("Car not found.");
        }

        existingCar.Make = updatedCar.Make;
        existingCar.Model = updatedCar.Model;
        existingCar.Year = updatedCar.Year;
        existingCar.PricePerDay = updatedCar.PricePerDay;
        existingCar.IsAvailable = updatedCar.IsAvailable;

        await _carRepository.UpdateCarAvailability(id, updatedCar.IsAvailable);
        return NoContent(); 
    }

    [Authorize(Roles = "Admin")] 
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCar(int id)
    {
        var car = await _carRepository.GetCarById(id);
        if (car == null)
        {
            return NotFound("Car not found.");
        }

        await _carRepository.DeleteCar(id); 
        return NoContent(); 
    }

    [HttpPost("rent/{id}")]
    public async Task<IActionResult> RentCar(int id, [FromBody] RentCarRequest request)
    {
        if (request == null)
        {
            return BadRequest("Request body is required.");
        }

        var result = await _carRentalService.RentCar(id, request.UserName, request.RentalDays);
        return Ok(result);
    }
}