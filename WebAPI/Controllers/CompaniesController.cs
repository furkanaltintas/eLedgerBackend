using Application.Features.Companies.CreateCompany;
using Application.Features.Companies.DeleteCompany;
using Application.Features.Companies.GetAllCompanies;
using Application.Features.Companies.MigrateAllCompanies;
using Application.Features.Companies.UpdateCompany;
using Application.Features.Companies.UserCompanies;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Abstractions;

namespace WebAPI.Controllers;

public class CompaniesController : ApiController
{
    public CompaniesController(IMediator mediator) : base(mediator) { }


    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        await Send(new GetAllCompaniesQuery());

    [AllowAnonymous]
    [HttpPost("user-companies")]
    public async Task<IActionResult> UserCompanies(UserCompaniesQuery userCompaniesQuery) =>
        await Send(userCompaniesQuery);

    [HttpPost]
    public async Task<IActionResult> Create(CreateCompanyCommand createCompanyCommand) =>
        await Send(createCompanyCommand);

    [HttpPut]
    public async Task<IActionResult> Update(UpdateCompanyCommand updateCompanyCommand) =>
        await Send(updateCompanyCommand);

    [HttpDelete]
    public async Task<IActionResult> Delete(DeleteCompanyCommand deleteCompanyCommand) =>
        await Send(deleteCompanyCommand);

    [HttpPost("migrateAll")]
    public async Task<IActionResult> MigrateAll(MigrateAllCompaniesCommand migrateAllCompaniesCommand) =>
    await Send(migrateAllCompaniesCommand);
}