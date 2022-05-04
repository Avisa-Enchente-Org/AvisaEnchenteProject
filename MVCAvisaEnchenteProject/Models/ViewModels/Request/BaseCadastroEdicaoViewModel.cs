using Microsoft.AspNetCore.Mvc.Rendering;
using MVCAvisaEnchenteProject.Infrastructure.Helpers;
using MVCAvisaEnchenteProject.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Models.ViewModels.Request
{
    public class BaseCadastroEdicaoViewModel
    {
        public int Id { get; set; }
        public bool EdicaoModel() => this.Id > 0;
    }
}
