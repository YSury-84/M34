using System;
using System.Threading.Tasks;
using AutoMapper;
using HomeApi.Contracts.Models.Devices;
using HomeApi.Contracts.Models.Rooms;
using HomeApi.Data.Models;
using HomeApi.Data.Queries;
using HomeApi.Data.Repos;
using Microsoft.AspNetCore.Mvc;

namespace HomeApi.Controllers
{
    /// <summary>
    /// Контроллер комнат
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RoomsController : ControllerBase
    {
        private IRoomRepository _repository;
        private IRoomRepository _rooms;
        private IMapper _mapper;
        
        public RoomsController(IRoomRepository repository, IRoomRepository rooms, IMapper mapper)
        {
            _repository = repository;
            _rooms = rooms;
            _mapper = mapper;
        }
        
        //TODO: Задание - добавить метод на получение всех существующих комнат
        
        /// <summary>
        /// Добавление комнаты
        /// </summary>
        [HttpPost] 
        [Route("")] 
        public async Task<IActionResult> Add([FromBody] AddRoomRequest request)
        {
            var existingRoom = await _repository.GetRoomByName(request.Name);
            if (existingRoom == null)
            {
                var newRoom = _mapper.Map<AddRoomRequest, Room>(request);
                await _repository.AddRoom(newRoom);
                return StatusCode(201, $"Комната {request.Name} добавлена!");
            }
            
            return StatusCode(409, $"Ошибка: Комната {request.Name} уже существует.");
        }


        /// <summary>
        /// Обновление существующего устройства
        /// </summary>
        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> Edit(
            [FromRoute] Guid id,
            [FromBody] EditRoomsRequest request)
        {
            var room = await _rooms.GetRoomByName(request.NewName);
            if (room == null)
                return StatusCode(400, $"Ошибка: Комната {request.NewName} не существует!");

            room.Area = request.NewArea;
            room.GasConnected = request.NewGasConnected;
            room.Voltage = request.NewVoltage;

            await _rooms.UpdateRoom(room);

            return StatusCode(200, $"Комната обновлена! Имя - {room.Name}");
        }


    }
}