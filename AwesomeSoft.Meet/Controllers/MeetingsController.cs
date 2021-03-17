using AwesomeSoft.Meet.Helpers;
using AwesomeSoft.Meet.Models;
using AwesomeSoft.Meet.Models.ViewModels;
using AwesomeSoft.Meet.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AwesomeSoft.Meet.Controllers
{
    /// <summary>
    /// Handles managing of the <see cref="Meeting"/>s.
    /// </summary>
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    public class MeetingsController : ControllerBase
    {
        private readonly ILogger<MeetingsController> _logger;
        private readonly IUserService _userService;
        private readonly MeetingService _meetingService;

        public MeetingsController(ILogger<MeetingsController> logger,
            IUserService userService,
            MeetingService meetingService)
        {
            _logger = logger;
            _userService = userService;
            _meetingService = meetingService;
        }

        #region Actions
        /// <summary>
        /// Gets all the current users scheduled Meetings.
        /// </summary>
        /// <returns>A collection of Meetings.</returns>
        [HttpGet("[controller]")]
        [ProducesResponseType(typeof(IEnumerable<Meeting>), StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            return Ok(_meetingService.GetMeetings(_userService.GetCurrentUser()));
        }
        /// <summary>
        /// Gets all the current users scheduled Meetings.
        /// </summary>
        /// <returns>A collection of Meetings.</returns>
        [HttpGet("[controller]/{id?}")]
        [ProducesResponseType(typeof(Meeting), StatusCodes.Status200OK)]
        public IActionResult Get(uint id)
        {
            Meeting meeting = _meetingService.GetMeetingById(id);
            User user = _userService.GetCurrentUser();
            if (meeting is null)
            {
                return NotFound();
            }

            if (meeting.Owner.Id != user.Id && !meeting.Participants.Any(p => p.Id == user.Id))
            {
                return Unauthorized();
            }

            return Ok(meeting);
        }

        /// <summary>
        /// Creates a new Meeting.
        /// </summary>
        /// <param name="model">The meeting data.</param>
        /// <returns>Returns the finsihed Meeting along with its ID.</returns>
        /// <response code="201">On a successful creation.</response>
        /// <response code="400">Data was malformed or otherwise invalid.</response>
        [HttpPost("[controller]")]
        [ProducesResponseType(typeof(Meeting), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Add(AddMeetingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var meeting = _meetingService.Add(new Meeting()
                {
                    Title = model.Title,
                    Description = model.Description,
                    StartTime = model.StartTime,
                    EndTime = model.EndTime,
                    Owner = _userService.GetCurrentUser(),
                    Participants = model.Participants.Select(p => _userService.GetById(p.Id)).ToList()
                });
                return Created(Url.Action(nameof(Get), new { Id = meeting.Id }), meeting);
            }
            return BadRequest();
        }

        /// <summary>
        /// Modifies a Meeting.
        /// </summary>
        /// <param name="id">Id of the Meeting.</param>
        /// <param name="model">The model containing the modified data.</param>
        /// <returns>Returns the modified Meeting.</returns>
        /// <response code="200">On success.</response>
        /// <response code="404">If the <see cref="Meeting"/> was not found.</response>
        /// <response code="401">If the <see cref="Meeting"/> is not owned by the current user.</response>
        /// <response code="400">Data was malformed or otherwise invalid.</response>
        [HttpPut("[controller]/{id?}")]
        [ProducesResponseType(typeof(Meeting), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Edit(uint id, EditMeetingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var meeting = _meetingService.GetMeetingById(id);
                if (meeting is null)
                {
                    return NotFound();
                }
                if (meeting.Owner != _userService.GetCurrentUser())
                {
                    return Unauthorized();
                }

                meeting.Title = model.Title;
                meeting.Description = model.Description;
                meeting.StartTime = model.StartTime;
                meeting.EndTime = model.EndTime;
                meeting.Participants = model.Participants.Select(p => _userService.GetById(p.Id)).ToList();
                return Ok(_meetingService.Update(meeting));
            }
            return BadRequest();
        }

        /// <summary>
        /// Deletes a <see cref="Meeting"/>.
        /// </summary>
        /// <param name="id">Id of the <see cref="Meeting"/>.</param>
        /// <returns>Returns an <see cref="ActionResult"/> indicating the status.</returns>
        /// <response code="204">On a successful deletion.</response>
        /// <response code="404">If the <see cref="Meeting"/> was not found.</response>
        /// <response code="401">If the <see cref="Meeting"/> is not owned by the current user.</response>
        [HttpDelete("[controller]/{id?}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Delete(uint id)
        {
            var meeting = _meetingService.GetMeetingById(id);
            if (meeting is null)
            {
                return NotFound();
            }

            if (meeting.Owner != _userService.GetCurrentUser())
            {
                return Unauthorized();
            }
            _meetingService.Delete(meeting);
            return NoContent();
        }

        /// <summary>
        /// Gets the current users conflicting Meetings.
        /// </summary>
        /// <returns>A list of <see cref="Conflict"/> objects for every Meeting conflict.</returns>
        [HttpGet("[controller]/[action]")]
        [ProducesResponseType(typeof(IEnumerable<Conflict>), StatusCodes.Status200OK)]
        public IActionResult Conflicts()
        {
            return Ok(_meetingService.GetConflicts(_userService.GetCurrentUser()));
        }
        #endregion
    }
}
