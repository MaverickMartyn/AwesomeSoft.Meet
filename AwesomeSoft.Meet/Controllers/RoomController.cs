using AwesomeSoft.Meet.Helpers;
using AwesomeSoft.Meet.Models;
using AwesomeSoft.Meet.Models.ViewModels;
using AwesomeSoft.Meet.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwesomeSoft.Meet
{
    [Authorize]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly RoomService _roomService;

        public RoomController(RoomService roomService)
        {
            _roomService = roomService;
        }

        #region Actions
        /// <summary>
        /// Gets all the possible <see cref="Room"/>s.
        /// </summary>
        /// <returns>A collection of <see cref="Room"/>s.</returns>
        [HttpGet("api/[controller]")]
        [ProducesResponseType(typeof(IEnumerable<Room>), StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            return Ok(_roomService.GetRooms());
        }

        /// <summary>
        /// Gets all the current users scheduled Meetings.
        /// </summary>
        /// <returns>The requested room.</returns>
        [HttpGet("api/[controller]/{id?}")]
        [ProducesResponseType(typeof(Room), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Room), StatusCodes.Status404NotFound)]
        public IActionResult Get(uint id)
        {
            var room = _roomService.GetRoomById(id);
            if (room is null)
            {
                return NotFound();
            }
            return Ok(room);
        }

        /// <summary>
        /// Creates a new Room.
        /// </summary>
        /// <param name="model">The room data.</param>
        /// <returns>Returns the finished Room along with its ID.</returns>
        /// <response code="201">On a successful creation.</response>
        /// <response code="400">Data was malformed or otherwise invalid.</response>
        [HttpPost("api/[controller]")]
        [ProducesResponseType(typeof(Room), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post([FromBody] AddRoomViewModel model)
        {
            if (ModelState.IsValid)
            {
                var room = _roomService.Add(new Room()
                {
                    Name = model.Name
                });
                return Created(Url.Action(nameof(Get), new { Id = room.Id }), room);
            }
            return BadRequest();
        }

        /// <summary>
        /// Modifies a Room.
        /// </summary>
        /// <param name="id">Id of the Room.</param>
        /// <param name="model">The model containing the modified data.</param>
        /// <returns>Returns the modified Room.</returns>
        /// <response code="200">On success.</response>
        /// <response code="404">If the <see cref="Room"/> was not found.</response>
        /// <response code="400">Data was malformed or otherwise invalid.</response>
        [HttpPut("api/[controller]/{id?}")]
        [ProducesResponseType(typeof(Room), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(uint id, [FromBody] EditRoomViewModel model)
        {
            if (ModelState.IsValid)
            {
                var room = _roomService.GetRoomById(id);
                if (room is null)
                {
                    return NotFound();
                }

                room.Name = model.Name;
                return Ok(_roomService.Update(room));
            }
            return BadRequest();
        }

        /// <summary>
        /// Deletes a <see cref="Room"/>.
        /// </summary>
        /// <param name="id">Id of the <see cref="Room"/>.</param>
        /// <returns>Returns an <see cref="ActionResult"/> indicating the status.</returns>
        /// <response code="204">On a successful deletion.</response>
        /// <response code="404">If the <see cref="Room"/> was not found.</response>
        [HttpDelete("api/[controller]/{id?}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(uint id)
        {
            var room = _roomService.GetRoomById(id);
            if (room is null)
            {
                return NotFound();
            }
            _roomService.Delete(room);
            return NoContent();
        }
        #endregion
    }
}
