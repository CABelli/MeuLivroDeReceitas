namespace MeuLivroDeReceitas.Domain.EntityGeneric
{
    public abstract class GenericEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime? CreationDate { get; protected set; } = DateTime.UtcNow;

        public DateTime? AlterationDate { get; protected set; }

        public string? CreationUsuarioId { get; protected set; }

        public string? AlterationUsuarioId { get; protected set; }

        public void SetCreationDate(DateTime creationDate) => CreationDate = creationDate;

        public void SetAlterationDate(DateTime alterationDate) => AlterationDate = alterationDate;

        public void SetCreationUsuarioId(string creationUsuarioId) => CreationUsuarioId = creationUsuarioId;

        public void SetAlterationUsuarioId(string alterationUsuarioId) => AlterationUsuarioId = AlterationUsuarioId;
    }
}
