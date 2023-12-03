namespace Aquantica.DAL.Seeder;

public interface ISeeder
{
    Task SeedIfNeededAsync();
}