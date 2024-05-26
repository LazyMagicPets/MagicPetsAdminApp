
namespace ViewModels;
public interface IBaseAppJS
{
    Task Initialize();
    void OnHello(string helloText);
    Task SayHello();

}
