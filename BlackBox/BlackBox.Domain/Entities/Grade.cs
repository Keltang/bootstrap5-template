using Ardalis.GuardClauses;
using BlackBox.Domain.Common; 

namespace BlackBox.Domain.Entities
{

    public class Grade : AuditableEntity<Guid>
    { 
        public string Code { get; private set; }
        public string Name { get; private set; } 

        private Grade(int externalId, string code, string name)
        {
            Id = Guid.NewGuid(); 
            Code = code;
            Name = name; 
        }


        public static Grade Create(int externalId, string code, string name)
        {
            Guard.Against.Default(externalId, nameof(externalId));
            Guard.Against.Default(externalId, nameof(externalId)); 
            Guard.Against.NullOrWhiteSpace(code, nameof(code));
            Guard.Against.NullOrWhiteSpace(name, nameof(name));

            return new Grade(externalId, code, name);
        }

    }
}
