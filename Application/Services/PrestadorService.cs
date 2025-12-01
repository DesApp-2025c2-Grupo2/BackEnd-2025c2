using Application.Contracts.DTOs.Request;
using Application.Contracts.DTOs.Response;
using Application.Contracts.Interfaces;
using Application.Utilities;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using System.Diagnostics;

namespace Application.Services;
public class PrestadorService : IPrestadorService
{
    private readonly IPrestadorRepository prestadorRepo;
    private readonly IAgendaRepository agendaRepo;

    public PrestadorService(IPrestadorRepository prestadorRepository, IAgendaRepository agendaRepository)
    {
        prestadorRepo = prestadorRepository;
        agendaRepo = agendaRepository;
    }

    public async Task<PrestadoresResponse> GetAllAsync()
    {
        PrestadoresResponse response = new();
        List<Prestador> prestadoresE = await prestadorRepo.GetAllAsync();
        prestadoresE.ForEach(pr => response.Add(DTOMapper.PrestadorToResponse(pr)));
        return response;
    }

    public async Task<bool> ToggleStatusAsync(int id)
    {
        return await prestadorRepo.ToggleStatusAsync(id);
    }

    public async Task<PrestadorResponse> SaveAsync(PrestadorRequest prestadorRequest)
    {
        PrestadorResponse response;
        Prestador prestadorMapped = DTOMapper.PrestadorToEntity(prestadorRequest);
        Prestador prestadorDB;
        if (prestadorRequest.Id.HasValue && prestadorRequest.Id.Value > 0) prestadorDB = await prestadorRepo.UpdateAsync(prestadorMapped, prestadorRequest.Especialidades);
        else prestadorDB = await prestadorRepo.CreateAsync(prestadorMapped, prestadorRequest.Especialidades);
        response = DTOMapper.PrestadorToResponse(prestadorDB);
        return response;
    }

    public async Task<AgendaResponse> UpdateAgendaAsync(AgendaRequest request)
    {
        AgendaResponse response;
        Agenda agendaDB;
        if (request.HorariosAtencion.Count > 0)
        {
            agendaDB = await agendaRepo.UpdateAsync(DTOMapper.AgendaToEntity(request));
            Debug.WriteLine(agendaDB);
            response = DTOMapper.AgendaToDTO(agendaDB);

        }
        else
        {
            await agendaRepo.ClearAsync(request.Id);
            response = new AgendaResponse { Id = request.Id, Horarios = new() };
        }
        return response;
    }
}