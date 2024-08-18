using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using mini_crm.DbContext;
using mini_crm.Repositories;
using mini_crm.Services;

class Program
{
    private static IContractService _contractService;

    private static void Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql("Host=localhost;Port=5432;Database=mini_crm;Username=postgres;Password=your_pass;"));


        // Регистрируем репозиторий и сервис
        serviceCollection.AddScoped<IContractRepository, ContractRepository>();
        serviceCollection.AddScoped<ContractService>();

        // Строим провайдер служб
        var serviceProvider = serviceCollection.BuildServiceProvider();

        // Получаем ContractService через DI
        _contractService = serviceProvider.GetRequiredService<ContractService>();

        ShowMenu();
    }

    static void ShowMenu()
    {
        var flag = true;
        while (flag)
        {
            Console.WriteLine("1. Сумма всех заключенных договоров за текущий год");
            Console.WriteLine("2. Сумма заключенных договоров по каждому контрагенту из России");
            Console.WriteLine("3. e-mail уполномоченных лиц с договорами больше 40000 за последние 30 дней");
            Console.WriteLine("4. Изменить статус договора на 'Расторгнут' для физических лиц старше 60 лет");
            Console.WriteLine("5. Создать отчет для физических лиц с договорами по компаниям в Москве");
            Console.WriteLine("exit: для выхода");
            Console.WriteLine("Выберите функцию:");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    _contractService.PrintSumContractsThisYear();
                    break;
                case "2":
                    _contractService.PrintSumByLegalPersonInRussia();
                    break;
                case "3":
                    _contractService.PrintAuthorizedPersonEmailsThisMonth();
                    break;
                case "4":
                    _contractService.PrintUpdatedContractsStatusForOlderPersons();
                    break;
                case "5":
                    _contractService.SaveActiveContractsReport();
                    break;
                case "exit":
                    flag = false;
                    break;
                default:
                    Console.WriteLine("Неверный выбор");
                    break;
            }
            
        }
    }
}
