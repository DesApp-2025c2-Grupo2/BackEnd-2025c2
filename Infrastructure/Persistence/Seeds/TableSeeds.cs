using Domain.Entities;

namespace Infrastructure.Persistence.Seeds;

public static class TableSeeds
{

    public static List<SituacionTerapeutica> SituacionesTerapeuticas() => new List<SituacionTerapeutica>()
    {
        // Diabetes tipo 1, Diabetes tipo 2, Hipertensión, Asma, EPOC, Depresión, Ansiedad, Obesidad
        // Embarazo, Cáncer, VIH/SIDA, Hipotiroidismo, Hipertiroidismo, Artritis, Osteoporosis, Enfermedad renal crónica
        new SituacionTerapeutica()
        {
            Nombre = "Diabetes tipo 1",
            Descripcion = "Condición crónica en la que el páncreas produce poca o ninguna insulina.",
            Alta = DateTime.Now.Date
        },
        new SituacionTerapeutica()
        {
            Nombre = "Diabetes tipo 2",
            Descripcion = "Condición crónica que afecta la forma en que el cuerpo procesa la glucosa en sangre.",
            Alta = DateTime.Now.Date
        },
        new SituacionTerapeutica()
        {
            Nombre = "Hipertensión",
            Descripcion = "Condición en la que la presión arterial está constantemente elevada.",
            Alta = DateTime.Now.Date
        },
        new SituacionTerapeutica()
        {
            Nombre = "Asma",
            Descripcion = "Enfermedad crónica que inflama y estrecha las vías respiratorias.",
            Alta = DateTime.Now.Date
        },
        new SituacionTerapeutica()
        {
            Nombre = "EPOC",
            Descripcion = "Enfermedad pulmonar obstructiva crónica que dificulta la respiración.",
            Alta = DateTime.Now.Date
        },
        new SituacionTerapeutica()
        {
            Nombre = "Depresión",
            Descripcion = "Trastorno del estado de ánimo que causa una sensación persistente de tristeza y pérdida de interés.",
            Alta = DateTime.Now.Date
        },
        new SituacionTerapeutica()
        {
            Nombre = "Ansiedad",
            Descripcion = "Trastorno caracterizado por sentimientos de preocupación, ansiedad o miedo que son lo suficientemente fuertes como para interferir con las actividades diarias.",
            Alta = DateTime.Now.Date
        },
        new SituacionTerapeutica()
        {
            Nombre = "Obesidad",
            Descripcion = "Condición médica en la que una persona tiene un exceso de grasa corporal que puede afectar negativamente su salud.",
            Alta = DateTime.Now.Date
        },
        new SituacionTerapeutica()
        {
            Nombre = "Embarazo",
            Descripcion = "Estado de una mujer que lleva un feto en su útero.",
            Alta = DateTime.Now.Date
        },
        new SituacionTerapeutica()
        {
            Nombre = "Cáncer",
            Descripcion = "Conjunto de enfermedades caracterizadas por el crecimiento descontrolado de células anormales en el cuerpo.",
            Alta = DateTime.Now.Date
        },
        new SituacionTerapeutica()
        {
            Nombre = "VIH/SIDA",
            Descripcion = "Enfermedad causada por el virus de la inmunodeficiencia humana (VIH) que afecta el sistema inmunológico.",
            Alta = DateTime.Now.Date
        },
        new SituacionTerapeutica()
        {
            Nombre = "Hipotiroidismo",
            Descripcion = "Condición en la que la glándula tiroides no produce suficientes hormonas tiroideas.",
            Alta = DateTime.Now.Date
        },
        new SituacionTerapeutica()
        {
            Nombre = "Hipertiroidismo",
            Descripcion = "Condición en la que la glándula tiroides produce demasiadas hormonas tiroideas.",
            Alta = DateTime.Now.Date
        },
        new SituacionTerapeutica()
        {
            Nombre = "Artritis",
            Descripcion = "Inflamación de las articulaciones que causa dolor y rigidez.",
            Alta = DateTime.Now.Date
        },
        new SituacionTerapeutica()
        {
            Nombre = "Osteoporosis",
            Descripcion = "Enfermedad en la que los huesos se vuelven frágiles y más propensos a fracturas.",
            Alta = DateTime.Now.Date
        },
        new SituacionTerapeutica()
        {
            Nombre = "Enfermedad renal crónica",
            Descripcion = "Pérdida gradual de la función renal con el tiempo.",
            Alta = DateTime.Now.Date
        }
    };
    
    public static List<Especialidad> Especialidades() => new List<Especialidad>()
    {
        new Especialidad()
        {
            Nombre = "Medicina General",
            Descripcion = "Atención primaria y general de la salud.",
            Alta = DateTime.Now.Date
        },
        new Especialidad()
        {
            Nombre = "Pediatría",
            Descripcion = "Atención médica para niños y adolescentes.",
            Alta = DateTime.Now.Date
        },
        new Especialidad()
        {
            Nombre = "Ginecología",
            Descripcion = "Atención médica para mujeres, especialmente en relación con el sistema reproductivo.",
            Alta = DateTime.Now.Date
        },
        new Especialidad()
        {
            Nombre = "Cardiología",
            Descripcion = "Estudio y tratamiento de las enfermedades del corazón y del sistema circulatorio.",
            Alta = DateTime.Now.Date
        },
        new Especialidad()
        {
            Nombre = "Dermatología",
            Descripcion = "Estudio y tratamiento de las enfermedades de la piel.",
            Alta = DateTime.Now.Date
        },
        new Especialidad()
        {
            Nombre = "Neurología",
            Descripcion = "Estudio y tratamiento de las enfermedades del sistema nervioso.",
            Alta = DateTime.Now.Date
        },
        new Especialidad()
        {
            Nombre = "Psiquiatría",
            Descripcion = "Estudio y tratamiento de los trastornos mentales.",
            Alta = DateTime.Now.Date
        },
        new Especialidad()
        {
            Nombre = "Ortopedia",
            Descripcion = "Estudio y tratamiento de las enfermedades y lesiones del sistema musculoesquelético.",
            Alta = DateTime.Now.Date
        },
        new Especialidad()
        {
            Nombre = "Oftalmología",
            Descripcion = "Estudio y tratamiento de las enfermedades de los ojos.",
            Alta = DateTime.Now.Date
        },
        new Especialidad()
        {
            Nombre = "Otorrinolaringología",
            Descripcion = "Estudio y tratamiento de las enfermedades del oído, nariz y garganta.",
            Alta = DateTime.Now.Date
        }
    };
    
    public static List<PlanMedico> PlanesMedicos() => new List<PlanMedico>()
    {
        /*
        🏥 Planes Nacionales

            Plan Hierro - ARS 55000
            La tranquilidad de estar cubierto. Consultas médicas, estudios de diagnóstico básicos y atención de urgencias en la red nacional, con la accesibilidad como prioridad. Ideal para quienes buscan una solución confiable y económica.

            Plan Bronce - ARS 85000
            Un paso más en bienestar. Atención de especialistas con copagos reducidos, estudios clínicos avanzados y acceso inicial a cobertura de medicamentos. Perfecto para quienes desean cuidar su salud sin preocupaciones.

            Plan Plata - ARS 150000
            Equilibrio y seguridad. Cobertura integral de consultas, especialistas y estudios, con programas de salud preventiva y beneficios en farmacia. Una opción sólida para familias que priorizan el cuidado diario.

            Plan Oro - ARS 280000
            Prestigio y amplitud en la cobertura nacional. Incluye maternidad integral, emergencias a domicilio y beneficios en odontología y estética con copagos mínimos. Una elección premium para quienes buscan lo mejor en Argentina.

            Plan Platino - ARS 420000
            La excelencia al alcance. Todos los beneficios de Oro, pero potenciados: acceso sin límites a especialistas, medicamentos cubiertos al 100%, programas de bienestar integral y cobertura internacional de emergencias. El máximo nivel en planes nacionales.

        🌍 Planes Internacionales

            Plan Esmeralda - USD 1200
            Tu salud, sin fronteras. Acceso a una red internacional de clínicas y hospitales, telemedicina global y programas de bienestar. Una propuesta ideal para quienes viajan con frecuencia y buscan respaldo en todo el mundo.

            Plan Diamante - USD 2500
            Exclusividad total. Cobertura médica integral, internaciones en clínicas internacionales de primer nivel con mínimos copagos, seguro de vida y acceso prioritario a tecnología médica avanzada. El plan perfecto para quienes lo esperan todo.

            Plan Obsidiana - USD 3800
            El futuro de la medicina, hoy. Telemedicina ilimitada, estudios genéticos y medicina personalizada, acceso preferencial a tratamientos innovadores y cobertura internacional completa. Obsidiana representa la unión entre tecnología de vanguardia y cuidado humano.

            Plan Mythril - USD 6000
            La cumbre absoluta del cuidado médico. Concierge médico personal, programas de longevidad y bienestar, acceso a medicina experimental y cobertura mundial sin límites. Mythril es más que un plan de salud: es un estilo de vida, creado para quienes desean vivir más y mejor.
         
         */
        new PlanMedico()
        {
            Nombre = "Plan Hierro",
            Descripcion = "La tranquilidad de estar cubierto. Consultas médicas, estudios de diagnóstico básicos y atención de urgencias en la red nacional, con la accesibilidad como prioridad. Ideal para quienes buscan una solución confiable y económica.",
            CostoMensual = 55000,
            Moneda = "ARS",
            Alta = DateTime.Now.Date
        },
        new PlanMedico()
        {
            Nombre = "Plan Bronce",
            Descripcion = "Un paso más en bienestar. Atención de especialistas con copagos reducidos, estudios clínicos avanzados y acceso inicial a cobertura de medicamentos. Perfecto para quienes desean cuidar su salud sin preocupaciones.",
            CostoMensual = 85000,
            Moneda = "ARS",
            Alta = DateTime.Now.Date
        },
        new PlanMedico()
        {
            Nombre = "Plan Plata",
            Descripcion = "Equilibrio y seguridad. Cobertura integral de consultas, especialistas y estudios, con programas de salud preventiva y beneficios en farmacia. Una opción sólida para familias que priorizan el cuidado diario.",
            CostoMensual = 150000,
            Moneda = "ARS",
            Alta = DateTime.Now.Date
        },
        new PlanMedico()
        {
            Nombre = "Plan Oro",
            Descripcion = "Prestigio y amplitud en la cobertura nacional. Incluye maternidad integral, emergencias a domicilio y beneficios en odontología y estética con copagos mínimos. Una elección premium para quienes buscan lo mejor en Argentina.",
            CostoMensual = 280000,
            Moneda = "ARS",
            Alta = DateTime.Now.Date
        },
        new PlanMedico()
        {
            Nombre = "Plan Platino",
            Descripcion = "La excelencia al alcance. Todos los beneficios de Oro, pero potenciados: acceso sin límites a especialistas, medicamentos cubiertos al 100%, programas de bienestar integral y cobertura internacional de emergencias. El máximo nivel en planes nacionales.",
            CostoMensual = 420000,
            Moneda = "ARS",
            Alta = DateTime.Now.Date
        },
        new PlanMedico()
        {
            Nombre = "Plan Esmeralda",
            Descripcion = "Tu salud, sin fronteras. Acceso a una red internacional de clínicas y hospitales, telemedicina global y programas de bienestar. Una propuesta ideal para quienes viajan con frecuencia y buscan respaldo en todo el mundo.",
            CostoMensual = 1200,
            Moneda = "USD",
            Alta = DateTime.Now.Date
        },
        new PlanMedico()
        {
            Nombre = "Plan Diamante",
            Descripcion = "Exclusividad total. Cobertura médica integral, internaciones en clínicas internacionales de primer nivel con mínimos copagos, seguro de vida y acceso prioritario a tecnología médica avanzada. El plan perfecto para quienes lo esperan todo.",
            CostoMensual = 2500,
            Moneda = "USD",
            Alta = DateTime.Now.Date
        },
        new PlanMedico()
        {
            Nombre = "Plan Obsidiana",
            Descripcion = "El futuro de la medicina, hoy. Telemedicina ilimitada, estudios genéticos y medicina personalizada, acceso preferencial a tratamientos innovadores y cobertura internacional completa. Obsidiana representa la unión entre tecnología de vanguardia y cuidado humano.",
            CostoMensual = 3800,
            Moneda = "USD",
            Alta = DateTime.Now.Date
        },
        new PlanMedico()
        {
            Nombre = "Plan Mythril",
            Descripcion = "La cumbre absoluta del cuidado médico. Concierge médico personal, programas de longevidad y bienestar, acceso a medicina experimental y cobertura mundial sin límites. Mythril es más que un plan de salud: es un estilo de vida, creado para quienes desean vivir más y mejor.",
            CostoMensual = 6000,
            Moneda = "USD",
            Alta = DateTime.Now.Date
        }
    };
}
