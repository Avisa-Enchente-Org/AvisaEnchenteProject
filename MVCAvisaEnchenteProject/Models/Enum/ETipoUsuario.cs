﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Models.Enum
{
    public enum ETipoUsuario
    {
        [Description("Comum")]
        Comum = 1,

        [Description("Administrador")]
        Admin = 2
    }
}
