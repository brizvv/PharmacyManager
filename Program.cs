// See https://aka.ms/new-console-template for more information

using PharmacyManager;
using PharmacyManager.DataTypes;
using PharmacyManager.Commands;
using PharmacyManager.Database;

Console.WriteLine("Подключение к базе данных");

var dbManger = new DbManager("(localdb)\\mssqllocaldb", "PharmacyDB");

if (!dbManger.ConnectToDb())
{
    Console.WriteLine("Произошла ошибка из-за дальнейшая работа с программой невозможна, нажмите любую кнопку, чтобы выйти");    
    Console.ReadKey();
    Environment.Exit(0);
}

ObjectTypesStorage objectsStorage = new ObjectTypesStorage();
objectsStorage.Register(new Product());
objectsStorage.Register(new Pharmacy());
objectsStorage.Register(new Storage());
objectsStorage.Register(new Shipment());

var commandProcessor = new CommandProcessor(dbManger, objectsStorage);
commandProcessor.RegisterCommand(new DeleteCommand(dbManger));
commandProcessor.RegisterCommand(new ExitCommand(dbManger));
commandProcessor.RegisterCommand(new CreateCommand(dbManger));
commandProcessor.RegisterCommand(new CleanCommand(dbManger));
commandProcessor.RegisterCommand(new ShowCommand(dbManger));
commandProcessor.RegisterCommand(new GetCommand(dbManger));

if (!dbManger.IsDataBaseEmpty(objectsStorage))
{
    var testData = new TestDataCreator(commandProcessor);
    testData.AddTestDataToDatabase();
}

commandProcessor.ProcessCommand("clean");

while (true)
{
    Thread.Sleep(1);
    string command = Console.ReadLine();
    commandProcessor.ProcessCommand(command);
}


