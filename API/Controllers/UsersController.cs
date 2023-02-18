using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
public class UsersController : BaseApiController
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UsersController(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {
        var users = await _userRepository.GetMembersAsync();
        
        return Ok(users);
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
       return await _userRepository.GetMemberAsync(username);
    }

    [HttpPut] // Updating so use put
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto) // No need to specify the return when updating since it's client side
    {
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // We get username from the auth token that gets sent in every request
        var user = await _userRepository.GetUserByUsernameAsync(username);

        if (user == null) return NotFound();

        _mapper.Map(memberUpdateDto, user); // EF updates and overwrites User db as it changes, but doesn't save yet

        if (await _userRepository.SaveAllAsync()) return NoContent(); // Saves changes to db and returns 204, meaning Ok but no response to send back

        return BadRequest("Failed to update user");


    }
}