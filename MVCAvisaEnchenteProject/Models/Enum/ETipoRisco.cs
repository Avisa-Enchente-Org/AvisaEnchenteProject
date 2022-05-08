using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Models.Enum
{
    public enum ETipoRisco
    {
        [Description("Sem Risco")]
        SemRisco = 0,

        [Description("Baixo Risco")]
        BaixoRisco = 1,

        [Description("Baixo Risco")]
        MedioRisco = 2,

        [Description("Baixo Risco")]
        AltoRisco = 3
    }
}
