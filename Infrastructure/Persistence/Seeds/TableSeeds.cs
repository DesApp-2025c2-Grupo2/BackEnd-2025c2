using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.Persistence.Seeds;

public static class TableSeeds
{

    public static List<SituacionTerapeutica> SituacionesTerapeuticas() => new List<SituacionTerapeutica>()
    {
        new SituacionTerapeutica()
        {
            Nombre = "Diabetes tipo 1",
            Descripcion = "Condición crónica en la que el páncreas produce poca o ninguna insulina.",
            Alta = DateTime.Parse("2025-09-21").Date
        },
        new SituacionTerapeutica()
        {
            Nombre = "Diabetes tipo 2",
            Descripcion = "Condición crónica que afecta la forma en que el cuerpo procesa la glucosa en sangre.",
            Alta = DateTime.Parse("2025-09-21").Date
        },
        new SituacionTerapeutica()
        {
            Nombre = "Hipertensión",
            Descripcion = "Condición en la que la presión arterial está constantemente elevada.",
            Alta = DateTime.Parse("2025-09-21").Date
        },
        new SituacionTerapeutica()
        {
            Nombre = "Asma",
            Descripcion = "Enfermedad crónica que inflama y estrecha las vías respiratorias.",
            Alta = DateTime.Parse("2025-09-21").Date
        },
        new SituacionTerapeutica()
        {
            Nombre = "EPOC",
            Descripcion = "Enfermedad pulmonar obstructiva crónica que dificulta la respiración.",
            Alta = DateTime.Parse("2025-09-21").Date
        },
        new SituacionTerapeutica()
        {
            Nombre = "Depresión",
            Descripcion = "Trastorno del estado de ánimo que causa una sensación persistente de tristeza y pérdida de interés.",
            Alta = DateTime.Parse("2025-09-21").Date
        },
        new SituacionTerapeutica()
        {
            Nombre = "Ansiedad",
            Descripcion = "Trastorno caracterizado por sentimientos de preocupación, ansiedad o miedo que son lo suficientemente fuertes como para interferir con las actividades diarias.",
            Alta = DateTime.Parse("2025-09-21").Date
        },
        new SituacionTerapeutica()
        {
            Nombre = "Obesidad",
            Descripcion = "Condición médica en la que una persona tiene un exceso de grasa corporal que puede afectar negativamente su salud.",
            Alta = DateTime.Parse("2025-09-21").Date
        },
        new SituacionTerapeutica()
        {
            Nombre = "Embarazo",
            Descripcion = "Estado de una mujer que lleva un feto en su útero.",
            Alta = DateTime.Parse("2025-09-21").Date
        },
        new SituacionTerapeutica()
        {
            Nombre = "Cáncer",
            Descripcion = "Conjunto de enfermedades caracterizadas por el crecimiento descontrolado de células anormales en el cuerpo.",
            Alta = DateTime.Parse("2025-09-21").Date
        },
        new SituacionTerapeutica()
        {
            Nombre = "VIH/SIDA",
            Descripcion = "Enfermedad causada por el virus de la inmunodeficiencia humana (VIH) que afecta el sistema inmunológico.",
            Alta = DateTime.Parse("2025-09-21").Date
        },
        new SituacionTerapeutica()
        {
            Nombre = "Hipotiroidismo",
            Descripcion = "Condición en la que la glándula tiroides no produce suficientes hormonas tiroideas.",
            Alta = DateTime.Parse("2025-09-21").Date
        },
        new SituacionTerapeutica()
        {
            Nombre = "Hipertiroidismo",
            Descripcion = "Condición en la que la glándula tiroides produce demasiadas hormonas tiroideas.",
            Alta = DateTime.Parse("2025-09-21").Date
        },
        new SituacionTerapeutica()
        {
            Nombre = "Artritis",
            Descripcion = "Inflamación de las articulaciones que causa dolor y rigidez.",
            Alta = DateTime.Parse("2025-09-21").Date
        },
        new SituacionTerapeutica()
        {
            Nombre = "Osteoporosis",
            Descripcion = "Enfermedad en la que los huesos se vuelven frágiles y más propensos a fracturas.",
            Alta = DateTime.Parse("2025-09-21").Date
        },
        new SituacionTerapeutica()
        {
            Nombre = "Enfermedad renal crónica",
            Descripcion = "Pérdida gradual de la función renal con el tiempo.",
            Alta = DateTime.Parse("2025-09-21").Date
        }
    };

    public static List<Especialidad> Especialidades() => new List<Especialidad>()
    {
        new Especialidad()
        {
            Nombre = "Medicina General",
            Descripcion = "Atención primaria y general de la salud.",
            Alta = DateTime.Parse("2025-09-21").Date
        },
        new Especialidad()
        {
            Nombre = "Pediatría",
            Descripcion = "Atención médica para niños y adolescentes.",
            Alta = DateTime.Parse("2025-09-21").Date
        },
        new Especialidad()
        {
            Nombre = "Ginecología",
            Descripcion = "Atención médica para mujeres, especialmente en relación con el sistema reproductivo.",
            Alta = DateTime.Parse("2025-09-21").Date
        },
        new Especialidad()
        {
            Nombre = "Cardiología",
            Descripcion = "Estudio y tratamiento de las enfermedades del corazón y del sistema circulatorio.",
            Alta = DateTime.Parse("2025-09-21").Date
        },
        new Especialidad()
        {
            Nombre = "Dermatología",
            Descripcion = "Estudio y tratamiento de las enfermedades de la piel.",
            Alta = DateTime.Parse("2025-09-21").Date
        },
        new Especialidad()
        {
            Nombre = "Neurología",
            Descripcion = "Estudio y tratamiento de las enfermedades del sistema nervioso.",
            Alta = DateTime.Parse("2025-09-21").Date
        },
        new Especialidad()
        {
            Nombre = "Psiquiatría",
            Descripcion = "Estudio y tratamiento de los trastornos mentales.",
            Alta = DateTime.Parse("2025-09-21").Date
        },
        new Especialidad()
        {
            Nombre = "Ortopedia",
            Descripcion = "Estudio y tratamiento de las enfermedades y lesiones del sistema musculoesquelético.",
            Alta = DateTime.Parse("2025-09-21").Date
        },
        new Especialidad()
        {
            Nombre = "Oftalmología",
            Descripcion = "Estudio y tratamiento de las enfermedades de los ojos.",
            Alta = DateTime.Parse("2025-09-21").Date
        },
        new Especialidad()
        {
            Nombre = "Otorrinolaringología",
            Descripcion = "Estudio y tratamiento de las enfermedades del oído, nariz y garganta.",
            Alta = DateTime.Parse("2025-09-21").Date
        }
    };

    public static List<PlanMedico> PlanesMedicos() => new List<PlanMedico>()
    {
        new PlanMedico()
        {
            Nombre = "Plan Hierro",
            Descripcion = "La tranquilidad de estar cubierto. Consultas médicas, estudios de diagnóstico básicos y atención de urgencias en la red nacional, con la accesibilidad como prioridad. Ideal para quienes buscan una solución confiable y económica.",
            CostoMensual = 55000,
            Moneda = "ARS",
            Alta = DateTime.Parse("2025-09-21").Date
        },
        new PlanMedico()
        {
            Nombre = "Plan Bronce",
            Descripcion = "Un paso más en bienestar. Atención de especialistas con copagos reducidos, estudios clínicos avanzados y acceso inicial a cobertura de medicamentos. Perfecto para quienes desean cuidar su salud sin preocupaciones.",
            CostoMensual = 85000,
            Moneda = "ARS",
            Alta = DateTime.Parse("2025-09-21").Date
        },
        new PlanMedico()
        {
            Nombre = "Plan Plata",
            Descripcion = "Equilibrio y seguridad. Cobertura integral de consultas, especialistas y estudios, con programas de salud preventiva y beneficios en farmacia. Una opción sólida para familias que priorizan el cuidado diario.",
            CostoMensual = 150000,
            Moneda = "ARS",
            Alta = DateTime.Parse("2025-09-21").Date
        },
        new PlanMedico()
        {
            Nombre = "Plan Oro",
            Descripcion = "Prestigio y amplitud en la cobertura nacional. Incluye maternidad integral, emergencias a domicilio y beneficios en odontología y estética con copagos mínimos. Una elección premium para quienes buscan lo mejor en Argentina.",
            CostoMensual = 280000,
            Moneda = "ARS",
            Alta = DateTime.Parse("2025-09-21").Date
        },
        new PlanMedico()
        {
            Nombre = "Plan Platino",
            Descripcion = "La excelencia al alcance. Todos los beneficios de Oro, pero potenciados: acceso sin límites a especialistas, medicamentos cubiertos al 100%, programas de bienestar integral y cobertura internacional de emergencias. El máximo nivel en planes nacionales.",
            CostoMensual = 420000,
            Moneda = "ARS",
            Alta = DateTime.Parse("2025-09-21").Date
        },
        new PlanMedico()
        {
            Nombre = "Plan Esmeralda",
            Descripcion = "Tu salud, sin fronteras. Acceso a una red internacional de clínicas y hospitales, telemedicina global y programas de bienestar. Una propuesta ideal para quienes viajan con frecuencia y buscan respaldo en todo el mundo.",
            CostoMensual = 1200,
            Moneda = "USD",
            Alta = DateTime.Parse("2025-09-21").Date
        },
        new PlanMedico()
        {
            Nombre = "Plan Diamante",
            Descripcion = "Exclusividad total. Cobertura médica integral, internaciones en clínicas internacionales de primer nivel con mínimos copagos, seguro de vida y acceso prioritario a tecnología médica avanzada. El plan perfecto para quienes lo esperan todo.",
            CostoMensual = 2500,
            Moneda = "USD",
            Alta = DateTime.Parse("2025-09-21").Date
        },
        new PlanMedico()
        {
            Nombre = "Plan Obsidiana",
            Descripcion = "El futuro de la medicina, hoy. Telemedicina ilimitada, estudios genéticos y medicina personalizada, acceso preferencial a tratamientos innovadores y cobertura internacional completa. Obsidiana representa la unión entre tecnología de vanguardia y cuidado humano.",
            CostoMensual = 3800,
            Moneda = "USD",
            Alta = DateTime.Parse("2025-09-21").Date
        },
        new PlanMedico()
        {
            Nombre = "Plan Mythril",
            Descripcion = "La cumbre absoluta del cuidado médico. Concierge médico personal, programas de longevidad y bienestar, acceso a medicina experimental y cobertura mundial sin límites. Mythril es más que un plan de salud: es un estilo de vida, creado para quienes desean vivir más y mejor.",
            CostoMensual = 6000,
            Moneda = "USD",
            Alta = DateTime.Parse("2025-09-21").Date
        }
    };

    public static List<Afiliado> Afiliados() => new List<Afiliado>()
    {
            new Afiliado()
            {
                Id = 1,
                NumeroAfiliado = 100001,
                TitularID = 1,
                PlanMedicoId = 1,
                Alta = DateTime.Parse("2024-01-15").Date,
                Baja = null,
                Integrantes = new List<Persona>()
            },
            new Afiliado()
            {
                Id = 2,
                NumeroAfiliado = 100002,
                TitularID = 3,
                PlanMedicoId = 2,
                Alta = DateTime.Parse("2024-02-20").Date,
                Baja = null,
                Integrantes = new List<Persona>()
            },
            new Afiliado()
            {
                Id = 3,
                NumeroAfiliado = 100003,
                TitularID = 5,
                PlanMedicoId = 3,
                Alta = DateTime.Parse("2024-03-10").Date,
                Baja = DateTime.Parse("2024-06-01").Date,
                Integrantes = new List<Persona>()
            }
        };

    public static List<Persona> Personas() => new List<Persona>()
        {
            // Familia González - Afiliado 100001
            new Persona()
            {
                Id = 1,
                NumeroIntegrante = 1,
                Nombre = "Carlos",
                Apellido = "González",
                FechaNacimiento = DateTime.Parse("1980-05-15").Date,
                Parentesco = Parentesco.Titular,
                AfiliadoId = 1,
                Alta = DateTime.Parse("2024-01-15").Date,
                Baja = null,
                Telefonos = new List<Telefono>
                {
                    new Telefono { Id = 1, Numero = "+5491154879632", PersonaId = 1 },
                    new Telefono { Id = 2, Numero = "+5491154879633", PersonaId = 1 }
                },
                Emails = new List<Email>
                {
                    new Email { Id = 1, Correo = "carlos.gonzalez@email.com", PersonaId = 1 }
                },
                Documentacion = new Documentacion
                {
                    Id = 1,
                    TipoDocumento = TipoDocumento.DocumentoNacionalDeIdentidad,
                    Numero = "30123456",
                    PersonaId = 1
                },
                Direcciones = new List<Direccion>
                {
                    new Direccion
                    {
                        Id = 1,
                        Calle = "Av. Corrientes",
                        Altura = "1234",
                        Piso = "5",
                        Departamento = "A",
                        ProvinciaCiudad = "Buenos Aires",
                        PersonaId = 1
                    }
                }
            },
            new Persona()
            {
                Id = 2,
                NumeroIntegrante = 2,
                Nombre = "María",
                Apellido = "López",
                FechaNacimiento = DateTime.Parse("1982-08-22").Date,
                Parentesco = Parentesco.Conyuge,
                AfiliadoId = 1,
                Alta = DateTime.Parse("2024-01-15").Date,
                Baja = null,
                Telefonos = new List<Telefono>
                {
                    new Telefono { Id = 3, Numero = "+5491163258741", PersonaId = 2 }
                },
                Emails = new List<Email>
                {
                    new Email { Id = 2, Correo = "maria.lopez@email.com", PersonaId = 2 }
                },
                Documentacion = new Documentacion
                {
                    Id = 2,
                    TipoDocumento = TipoDocumento.DocumentoNacionalDeIdentidad,
                    Numero = "28987654",
                    PersonaId = 2
                }
            },

            // Familia Rodríguez - Afiliado 100002
            new Persona()
            {
                Id = 3,
                NumeroIntegrante = 1,
                Nombre = "Ana",
                Apellido = "Rodríguez",
                FechaNacimiento = DateTime.Parse("1975-12-03").Date,
                Parentesco = Parentesco.Titular,
                AfiliadoId = 2,
                Alta = DateTime.Parse("2024-02-20").Date,
                Baja = null,
                Telefonos = new List<Telefono>
                {
                    new Telefono { Id = 4, Numero = "+5491145698723", PersonaId = 3 }
                },
                Emails = new List<Email>
                {
                    new Email { Id = 3, Correo = "ana.rodriguez@email.com", PersonaId = 3 }
                },
                Documentacion = new Documentacion
                {
                    Id = 3,
                    TipoDocumento = TipoDocumento.DocumentoNacionalDeIdentidad,
                    Numero = "25456321",
                    PersonaId = 3
                },
                Direcciones = new List<Direccion>
                {
                    new Direccion
                    {
                        Id = 2,
                        Calle = "Calle Florida",
                        Altura = "567",
                        Piso = "",
                        Departamento = "",
                        ProvinciaCiudad = "Córdoba",
                        PersonaId = 3
                    }
                }
            },
            new Persona()
            {
                Id = 4,
                NumeroIntegrante = 2,
                Nombre = "Juan",
                Apellido = "Rodríguez",
                FechaNacimiento = DateTime.Parse("2010-03-18").Date,
                Parentesco = Parentesco.Hijo_a,
                AfiliadoId = 2,
                Alta = DateTime.Parse("2024-02-20").Date,
                Baja = null,
                Documentacion = new Documentacion
                {
                    Id = 4,
                    TipoDocumento = TipoDocumento.DocumentoNacionalDeIdentidad,
                    Numero = "50123456",
                    PersonaId = 4
                }
            },

            // Afiliado individual - Afiliado 100003 (dado de baja)
            new Persona()
            {
                Id = 5,
                NumeroIntegrante = 1,
                Nombre = "Luis",
                Apellido = "Martínez",
                FechaNacimiento = DateTime.Parse("1990-07-30").Date,
                Parentesco = Parentesco.Titular,
                AfiliadoId = 3,
                Alta = DateTime.Parse("2024-03-10").Date,
                Baja = DateTime.Parse("2024-06-01").Date,
                Telefonos = new List<Telefono>
                {
                    new Telefono { Id = 5, Numero = "+5491156239874", PersonaId = 5 }
                },
                Emails = new List<Email>
                {
                    new Email { Id = 4, Correo = "luis.martinez@email.com", PersonaId = 5 }
                },
                Documentacion = new Documentacion
                {
                    Id = 5,
                    TipoDocumento = TipoDocumento.DocumentoNacionalDeIdentidad,
                    Numero = "37123456",
                    PersonaId = 5
                },
                Direcciones = new List<Direccion>
                {
                    new Direccion
                    {
                        Id = 3,
                        Calle = "Av. Santa Fe",
                        Altura = "2456",
                        Piso = "2",
                        Departamento = "B",
                        ProvinciaCiudad = "Mendoza",
                        PersonaId = 5
                    }
                }
            }
        };
}
