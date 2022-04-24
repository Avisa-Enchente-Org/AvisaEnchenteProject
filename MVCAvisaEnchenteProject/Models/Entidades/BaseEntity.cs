using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Models.Entidades
{
    public class BaseEntity
    {
        public virtual int Id { get; protected set; }
    }
}
