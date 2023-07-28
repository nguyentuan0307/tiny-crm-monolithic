using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using TinyCRM.Domain.Entities.Accounts;
using TinyCRM.Domain.Entities.Contacts;
using TinyCRM.Domain.Entities.Deals;
using TinyCRM.Domain.Entities.Leads;
using TinyCRM.Domain.Entities.ProductDeals;
using TinyCRM.Domain.Entities.Products;
using TinyCRM.Domain.Entities.Roles;
using TinyCRM.Domain.Entities.Users;
using TinyCRM.Domain.Enums;
using TinyCRM.Domain.Interfaces;

namespace TinyCRM.Domain;

public class DataContributor
{
    private readonly IContactRepository _contactRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IProductRepository _productRepository;
    private readonly ILeadRepository _leadRepository;
    private readonly IDealRepository _dealRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DataContributor> _logger;

    private static IEnumerable<Account> _accounts = null!;
    private static IEnumerable<Product> _products = null!;
    private static IEnumerable<Lead> _leads = null!;

    public DataContributor(
        IContactRepository contactRepository,
        IAccountRepository accountRepository,
        IProductRepository productRepository,
        ILeadRepository leadRepository,
        IDealRepository dealRepository,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IUnitOfWork unitOfWork,
        ILogger<DataContributor> logger)
    {
        _contactRepository = contactRepository;
        _accountRepository = accountRepository;
        _productRepository = productRepository;
        _leadRepository = leadRepository;
        _dealRepository = dealRepository;
        _userManager = userManager;
        _roleManager = roleManager;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        try
        {
            if (
                !(await _leadRepository.AnyAsync()) &&
                !(await _dealRepository.AnyAsync()) &&
                !(await _accountRepository.AnyAsync()) &&
                !(await _contactRepository.AnyAsync()) &&
                !(await _productRepository.AnyAsync())
            )
            {
                _logger.LogInformation("Begin seeding data...");

                _accounts = await SeedAccountAsync();
                await SeedContactAsync();
                _products = await SeedProductAsync();
                _leads = await SeedLeadAsync();
                await SeedDealAsync();

                await _unitOfWork.SaveChangeAsync();

                _logger.LogInformation("Seed data successfully!");
            }

            if (!_roleManager.Roles.Any() && !_userManager.Users.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole(Role.SuperAdmin));
                await _roleManager.CreateAsync(new IdentityRole(Role.Admin));
                await _roleManager.CreateAsync(new IdentityRole(Role.User));

                var user = new ApplicationUser()
                {
                    UserName = "superadmin@gmail.com",
                    Email = "superadmin@gmail.com",
                    Name = "superAdmin"
                };

                await _userManager.CreateAsync(user, "@SuperAdmin123");
                await _userManager.AddToRoleAsync(user, Role.SuperAdmin);
                var faker = new Faker<ApplicationUser>()
                    .RuleFor(u => u.UserName, f => f.Internet.UserName())
                    .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.UserName))
                    .RuleFor(u => u.Name, f => f.Name.FullName());

                var users = faker.Generate(10);

                foreach (var applicationUser in users)
                {
                    await _userManager.CreateAsync(applicationUser, "@User123");
                    await _userManager.AddToRoleAsync(applicationUser, Role.User);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Seeding data failed!{ex}");
        }
    }

    private async Task<IEnumerable<Account>> SeedAccountAsync()
    {
        var accounts = new List<Account>();

        for (var i = 1; i <= 10; i++)
        {
            accounts.Add(new Account()
            {
                Name = $"Account {i}",
                Email = $"account{i}@gmail.com",
                Address = $"Address - {i}",
                Phone = i.ToString(),
                TotalSales = 0
            });
        }

        await _accountRepository.AddRangeAsync(accounts);
        return accounts;
    }

    private async Task SeedContactAsync()
    {
        var contacts = new List<Contact>();
        var random = new Random();

        for (var i = 1; i <= 10; i++)
        {
            contacts.Add(new Contact()
            {
                Name = $"Contact {i}",
                Email = $"contact{i}@gmail.com",
                Phone = i.ToString(),
                Account = _accounts.ElementAt(random.Next(0, 9))
            });
        }

        await _contactRepository.AddRangeAsync(contacts);
    }

    private async Task<IEnumerable<Product>> SeedProductAsync()
    {
        var products = new List<Product>();
        var random = new Random();

        for (var i = 1; i <= 10; i++)
        {
            products.Add(new Product()
            {
                Code = $"P-{i}",
                Name = $"Product {i}",
                Price = random.Next(2000),
                Status = random.Next(2) == 1,
                TypeProduct = random.Next(2) == 1 ? TypeProduct.Service : TypeProduct.Physical
            });
        }

        await _productRepository.AddRangeAsync(products);
        return products;
    }

    private async Task<IEnumerable<Lead>> SeedLeadAsync()
    {
        var leads = new List<Lead>();
        var random = new Random();

        for (var i = 1; i <= 20; i++)
        {
            leads.Add(new Lead()
            {
                Title = $"Lead {i}",
                Description = $"Lead {i}",
                SourceLead = GetRandomEnumValue<SourceLead>(),
                StatusLead = GetRandomEnumValue<StatusLead>(),
                Account = _accounts.ElementAt(random.Next(0, 9)),
                EstimatedRevenue = random.Next(5000)
            });
        }

        await _leadRepository.AddRangeAsync(leads);

        return leads;
    }

    private async Task SeedDealAsync()
    {
        var deals = new List<Deal>();
        var leadQualifieds = _leads.Where(x => x.StatusLead == StatusLead.Qualified).ToList();
        var random = new Random();

        for (var i = 1; i <= 10; i++)
        {
            var productDeals = new List<ProductDeal>();

            for (var j = 1; j <= 5; j++)
            {
                productDeals.Add(new ProductDeal()
                {
                    Price = random.Next(1, 100000),
                    Product = _products.ElementAt(random.Next(0, 9)),
                    Quantity = random.Next(1, 10),
                });
            }

            if (leadQualifieds == null) continue;
            deals.Add(new Deal()
            {
                Title = $"Deal {i}",
                Description = $"Deal {i}",
                StatusDeal = random.Next(2) == 1 ? StatusDeal.Open : StatusDeal.Won,
                Lead = leadQualifieds.ElementAt(random.Next(0, leadQualifieds.Count() - 1)),
                ActualRevenue = random.Next(0, 5000),
                ProductDeals = productDeals
            });
        }

        await _dealRepository.AddRangeAsync(deals);
    }

    private static T GetRandomEnumValue<T>() where T : Enum
    {
        var random = new Random();
        var enumValues = Enum.GetValues(typeof(T));
        var randomIndex = random.Next(0, enumValues.Length);
        return (T)enumValues.GetValue(randomIndex)!;
    }
}