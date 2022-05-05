namespace MVCAvisaEnchenteProject.Models.Entidades
{
    public class BaseEntity
    {
        public virtual int Id { get; set; }

        public BaseEntity(int? id)
        {
            if(id.HasValue && id != default(int))
                Id = id.Value;
        }
        public BaseEntity() { }
    }
}
