using Application.Contracts.DTOs.Request;
using Application.Contracts.DTOs.Response;
using Application.Contracts.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class PlanMedicoService : IPlanMedicoService
{
    private readonly IPlanMedicoRepository repository;
    public PlanMedicoService(IPlanMedicoRepository repository)
    {
        this.repository = repository;
    }
    public async Task<PlanMedicoResponse> AddAsync(PlanMedicoRequest planMedicoCreateDto)
    {
        PlanMedico planMedico = new PlanMedico
        {
            Nombre = planMedicoCreateDto.nombre,
            Descripcion = planMedicoCreateDto.descripcion,
            CostoMensual = planMedicoCreateDto.costoMensual,
            Moneda = planMedicoCreateDto.moneda,
            Baja = null
        };
        PlanMedico createdPlanMedico = await repository.AddAsync(planMedico);
        return new PlanMedicoResponse
        {
            id = createdPlanMedico.Id,
            nombre = createdPlanMedico.Nombre,
            descripcion = createdPlanMedico.Descripcion,
            costoMensual = createdPlanMedico.CostoMensual,
            moneda = createdPlanMedico.Moneda,
            activa = createdPlanMedico.Baja == null
        };

    }

    public async Task<PlanesMedicosResponse> GetAllAsync()
    {
        PlanesMedicosResponse response = new PlanesMedicosResponse();
        List<PlanMedico> planesMedicos = await repository.GetAllAsync();
        planesMedicos.ForEach(planMedico =>
        {
            response.Add(new PlanMedicoResponse
            {
                id = planMedico.Id,
                nombre = planMedico.Nombre,
                descripcion = planMedico.Descripcion,
                costoMensual = planMedico.CostoMensual,
                moneda = planMedico.Moneda,
                activa = planMedico.Baja == null
            });
        });
        return response;
    }

    public async Task<bool> ToggleStatusAsync(int id)
    {
        bool opExitosa = await repository.ToggleStatusAsync(id);
        if (!opExitosa)
        {
            throw new Exception("No se pudo cambiar el estado del Plan Medico");
        }
        return opExitosa;
    }

    public async Task<PlanMedicoResponse> UpdateAsync(int id, PlanMedicoRequest planMedicoUpdateDto)
    {
        PlanMedico planMedico = new PlanMedico
        {
            Id = id,
            Nombre = planMedicoUpdateDto.nombre,
            Descripcion = planMedicoUpdateDto.descripcion,
            CostoMensual = planMedicoUpdateDto.costoMensual,
            Moneda = planMedicoUpdateDto.moneda
        };
        PlanMedico updatedPlanMedico = await repository.UpdateAsync(planMedico);
        return new PlanMedicoResponse
        {
            id = updatedPlanMedico.Id,
            nombre = updatedPlanMedico.Nombre,
            descripcion = updatedPlanMedico.Descripcion,
            costoMensual = updatedPlanMedico.CostoMensual,
            moneda = updatedPlanMedico.Moneda,
            activa = updatedPlanMedico.Baja == null
        };
    }
}
