using SecretSanta.Console;

try
{
    Console.WriteLine("Starting...");
    ListGenerator.CreateSanatasList(args);
    Console.WriteLine("Complete!");
}
catch (Exception ex)
{
    Console.WriteLine("An error has occured: {0}", ex.ToString());
}
finally
{
    Console.WriteLine("Press enter to quit.");
    Console.ReadLine();
}