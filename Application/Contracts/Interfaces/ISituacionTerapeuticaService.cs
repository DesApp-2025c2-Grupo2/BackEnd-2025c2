using Application.Contracts.DTOs.Request;
using Application.Contracts.DTOs.Response;

namespace Application.Contracts.Interfaces;

public interface ISituacionTerapeuticaService
{
    // Define aquí los métodos que el servicio debe implementar
    // Será un ABM (Alta, Baja, Modificación) de Situaciones Terapéuticas

    SituacionesTerapeuticasResponse GetAll();
    SituacionTerapeuticaResponse ToggleStatus(int id);
    SituacionTerapeuticaResponse Update(int id, SituacionTerapeuticaRequest request);
    SituacionTerapeuticaResponse Add(SituacionTerapeuticaRequest request);
}
