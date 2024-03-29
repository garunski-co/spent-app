﻿using Spent.Commons.Dtos.Identity;
using Spent.Commons.Extensions;
using Spent.Server.Models.Identity;

namespace Spent.Server.Controllers.Identity;

[Route("api/[controller]/[action]")]
[ApiController]
public partial class UserController : AppControllerBase<UserController>
{
    [AutoInject]
    private readonly UserManager<User> _userManager = default!;
    
    [HttpGet]
    public async Task<UserDto> GetCurrentUser(CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        var user = await _userManager.Users.FirstOrDefaultAsync(user => user.Id == userId, cancellationToken);

        if (user is null)
        {
            throw new ResourceNotFoundException();
        }

        return user.Map();
    }

    [HttpPut]
    public async Task<UserDto> Update(EditUserDto userDto, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        var user = await _userManager.Users.FirstOrDefaultAsync(user => user.Id == userId, cancellationToken);

        if (user is null)
        {
            throw new ResourceNotFoundException();
        }

        userDto.Patch(user);

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            throw new ResourceValidationException(result.Errors
                .Select(err => new LocalizedString(err.Code, err.Description)).ToArray());
        }

        return await GetCurrentUser(cancellationToken);
    }

    [HttpDelete]
    public async Task Delete(CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        var user = await _userManager.Users.FirstOrDefaultAsync(user => user.Id == userId, cancellationToken)
                   ?? throw new ResourceNotFoundException();

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            throw new ResourceValidationException(result.Errors
                .Select(err => new LocalizedString(err.Code, err.Description)).ToArray());
        }
    }
}
