namespace ConsoleApp
{
    internal interface IModule
    {
        string Name { get; }
        void Handle();
    }
}
