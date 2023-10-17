using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{
  
    public class AccountsController : ApiBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService,IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }
        //login
        [HttpPost("login")]//post :/api/accounts/login
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user is null)
              return Unauthorized(new ApiResponse(401));
            var result = await _signInManager.CheckPasswordSignInAsync(user,model.Password,false);
            if(!result.Succeeded)
                return Unauthorized(new ApiResponse(401));

            return Ok(new UserDto()
            {
                DisplyName = user.DisplayName,
                Email = user.Email,
                Token =await _tokenService.GetTokenAsync(user , _userManager)
            }) ;
        }

        //Register
        [HttpPost("register")]//post :/api/accounts/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            if(CheckEmailExists(model.Email).Result.Value)
                return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { "this email is already in user!!"} });

            var user = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,//haidy.ehab@gmail.com
                UserName = model.Email.Split('@')[0],//should be unique,haidy.ehab
                PhoneNumber = model.PhoneNumber,
            };
            var result = await _userManager.CreateAsync(user,model.Password);
            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400));
            return Ok(new UserDto()
            {
                DisplyName=user.DisplayName,
                Email=user.Email,
                Token= await _tokenService.GetTokenAsync(user, _userManager)
            });

        }

        //Get Current User
        [Authorize]
        [HttpGet]//Get :/api/accounts/getcurrentuser
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
           var email= User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            return Ok(new UserDto()
            {
                DisplyName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.GetTokenAsync(user, _userManager)
            }); ;
        }

        //Get Address of user
        [Authorize]
        [HttpGet("address")]//Get : /api/account/address
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManager.FindUserWithAddressAsync(User);
            var address = _mapper.Map<Address, AddressDto>(user.Address);
            return Ok(address);
        }

        //update address
        [HttpPut("address")]//Put : /api/accounts/address

        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto updatedAddress)
        {
            var address = _mapper.Map<AddressDto, Address>(updatedAddress);
          
            var user = await _userManager.FindUserWithAddressAsync(User);
            //user.Address = address;//this will add as new address
            //1.the first way to update
             address.Id = user.Address.Id;
              user.Address = address;

            //2.the second way to update
            //user.Address.FirstName = address.FirstName;
            //user.Address.LastName = address.LastName;
            //user.Address.Street = address.Street;
            //user.Address.City = address.City;
            //user.Address.Country = address.Country;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400));

            return Ok(updatedAddress);
       }

        //check email
        [HttpGet("emailexists")]//Get :/api/accounts/emailexists?email=ahmed.nasr@linkdev.com
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null; 
        }




    }
}
