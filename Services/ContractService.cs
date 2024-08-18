using mini_crm.Repositories;
using System.Text.Json;


namespace mini_crm.Services
{
    public interface IContractService
    {
        void PrintSumContractsThisYear();
        void PrintSumByLegalPersonInRussia();
        void PrintAuthorizedPersonEmailsThisMonth();
        void PrintUpdatedContractsStatusForOlderPersons();
        void SaveActiveContractsReport();
    }
    public class ContractService : IContractService
    {
        private static IContractRepository _contractRepository;

        public ContractService(IContractRepository contractRepository)
        {
            _contractRepository = contractRepository;
        }
        public void PrintSumContractsThisYear()
        {
            var sum = _contractRepository.GetSumOfContractsThisYear();
            Console.WriteLine($"Сумма всех договоров за текущий год: {sum}");

        }

        public void PrintSumByLegalPersonInRussia()
        {
            var sumInfo = _contractRepository.GetSumByLegalPersonInRussia();
            Console.WriteLine("Id: Company name - Amount");
            foreach (var info in sumInfo)
            {
                Console.WriteLine($"{info.LegalPersonId}: {info.CompanyName} - {info.TotalAmount}");
            }
        }

        public void PrintAuthorizedPersonEmailsThisMonth()
        {
            var emails = _contractRepository.GetAuthorizedPersonEmailsThisMonth();
            Console.WriteLine("e-mail уполномоченных лиц, заключивших договора за последние 30 дней, на сумму больше 40000:");
            foreach (var email in emails)
            {
                Console.WriteLine(email);
            }
        }

        public void PrintUpdatedContractsStatusForOlderPersons()
        {
            var updated = _contractRepository.UpdateContractsStatusForOlderPersons();
            Console.WriteLine($"Обновлено записей: {updated} ");
        }

        public void SaveActiveContractsReport()
        {
            var reportData = _contractRepository.GetActiveContractsReport();
            try
            {
                var json = JsonSerializer.Serialize(reportData, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText("report.json", json);
                Console.WriteLine("Файл отчета(report.json) успешно сохранен. Файл находится в том же каталоге где и исходный файл программы");
            }
            catch (Exception e)
            {
                Console.WriteLine("Произошла ошибка при сохранении");
                throw;
            }

        }
    }
}
