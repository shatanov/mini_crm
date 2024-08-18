using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mini_crm.Models
{
    public class Contract
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("CounterpartyId")]
        public LegalPerson Counterparty { get; set; }
        public int CounterpartyId { get; set; }

        [ForeignKey("AuthorizedPersonId")]
        public PhysicalPerson AuthorizedPerson { get; set; }
        public int AuthorizedPersonId { get; set; }
        public decimal ContractAmount { get; set; }
        public ContractStatus Status { get; set; }
        public DateTime SigningDate { get; set; }
    }

    public enum ContractStatus
    {
        Discussed,    // Обсуждается
        NotConcluded, // Не заключен
        Active,       // Действует
        Fulfilled,    // Исполнен
        Suspended,    // Приостановлен
        Terminated    // Расторгнут
    }
}
