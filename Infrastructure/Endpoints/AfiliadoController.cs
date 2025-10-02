using Application.Contracts.DTOs.Internal;
using Application.Contracts.DTOs.Response;
using Application.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Endpoints;


[ApiController]
[Route("[controller]")]
public class AfiliadoController : ControllerBase
{
    private readonly IProjectLogger logger;

    public AfiliadoController(IProjectLogger logger)
    {
        this.logger = logger;
    }

    [HttpGet("all")]
    public ActionResult<AfiliadosResponse> GetAll()
    {
        ActionResult<AfiliadosResponse> response = NotFound("No implementado");
        try
        {
            AfiliadosResponse data = new AfiliadosResponse()
            {
                new AfiliadoResponse
                {
                    numeroAfiliado = 0000001,
                    titular = new PersonaResponse
                    {
                        id = 1,
                        nombre = "Juan",
                        apellido = "Perez",
                        fechaNacimiento = new DateTime(1980, 1, 1),
                        parentesco = 1,
                        alta = new DateTime(2020, 1, 1),
                        baja = null,
                        telefonos = new List<TelefonoDTO>
                            {
                                new TelefonoDTO
                                {
                                    id = 1,
                                    numero = "987654321"
                                }
                            },
                        emails = new List<EmailDTO>
                            {
                                new EmailDTO
                                {
                                    id = 1,
                                    correo = "email@email.com"
                                }
                            },
                        documentacion = new DocumentacionDTO
                        {
                            tipoDocumento = 1,
                            numero = "12345678"
                        },
                        direcciones = new List<DireccionDTO>
                            {
                                new DireccionDTO
                                {
                                    id = 1,
                                    calle = "Calle Falsa",
                                    altura = "123",
                                    provinciaCiudad = "Buenos Aires, Hurlingham"
                                }
                            },
                        situacionesTerapeuticasResponse = new()
                    },
                    integrantes = new List<PersonaResponse>
                    {
                        new PersonaResponse
                        {
                            id = 2,
                            nombre = "Maria",
                            apellido = "Gomez",
                            fechaNacimiento = new DateTime(2010, 1, 1),
                            parentesco = 2,
                            alta = new DateTime(2020, 1, 1),
                            baja = null,
                            telefonos = new List<TelefonoDTO>
                            {
                                new TelefonoDTO
                                {
                                    id = 1,
                                    numero = "987654321"
                                }
                            },
                            emails = new List<EmailDTO>
                            {
                                new EmailDTO
                                {
                                    id = 1,
                                    correo = "email2@email.com"
                                }
                            },
                            documentacion = new DocumentacionDTO
                            {
                                tipoDocumento = 1,
                                numero = "87654321"
                            },
                            direcciones = new List<DireccionDTO>
                            {
                                new DireccionDTO
                                {
                                    id = 1,
                                    calle = "Calle Falsa",
                                    altura = "123",
                                    provinciaCiudad = "Buenos Aires, Hurlingham"
                                }
                            },
                            situacionesTerapeuticasResponse = new SituacionesTerapeuticasResponse
                            {
                                new SituacionTerapeuticaResponse
                                {
                                    id = 9,
                                    nombre = "Embarazo",
                                    descripcion = "Estado de una mujer que lleva un feto en su útero."
                                },
                                new SituacionTerapeuticaResponse
                                {
                                    id = 12,
                                    nombre = "Hipotiroidismo",
                                    descripcion = "Condición en la que la glándula tiroides no produce suficientes hormonas tiroideas."
                                }
                            }
                        }
                    },
                    planMedico = new PlanMedicoDTO
                    {
                        Nombre = "Plan Hierro",
                        CostoMensual = 55000,
                        Moneda = "ARS"
                    }
                },
                new AfiliadoResponse
                {
                    numeroAfiliado = 0000002,
                    titular = new PersonaResponse
                    {
                        id = 3,
                        nombre = "Carlos",
                        apellido = "Lopez",
                        fechaNacimiento = new DateTime(1975, 5, 15),
                        parentesco = 1,
                        alta = new DateTime(2019, 3, 10),
                        baja = null,
                        telefonos = new List<TelefonoDTO>
                            {
                                new TelefonoDTO
                                {
                                    id = 1,
                                    numero = "123456789"
                                }
                            },
                        emails = new List<EmailDTO>
                            {
                                new EmailDTO
                                {
                                    id = 1,
                                    correo = "email3@email.com"
                                }
                        },
                        documentacion = new DocumentacionDTO
                        {
                            tipoDocumento = 1,
                            numero = "11223344"
                        },
                        direcciones = new List<DireccionDTO>
                        {
                            new DireccionDTO
                            {
                                id = 1,
                                calle = "Avenida Siempre Viva",
                                altura = "742",
                                provinciaCiudad = "Buenos Aires, Moron"
                            }
                        },
                        situacionesTerapeuticasResponse = new()
                        {
                            new SituacionTerapeuticaResponse
                            {
                                id = 1,
                                nombre = "Diabetes Tipo 1",
                                descripcion = "Condición crónica en la que el páncreas produce poca o ninguna insulina."
                            }

                        }
                    },
                    integrantes = new List<PersonaResponse>(),
                    planMedico = new PlanMedicoDTO
                    {
                        Nombre = "Plan Esmeralda",
                        CostoMensual = 1200,
                        Moneda = "USD"
                    }
                }

            };
            response = Ok(data);
        }
        catch (Exception ex)
        {
            response = StatusCode(500, "Error interno del servidor");
        }
        return response;
    }
    
}
