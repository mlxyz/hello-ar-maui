using Java.Lang;
using System.Security.Cryptography.X509Certificates;

namespace ArMauiApp.Controls;

public class ArView : View
{
    public ArView()
    {
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();
        Task.Delay(3000).ContinueWith(t => Handler.Invoke("Start"));
    }
}