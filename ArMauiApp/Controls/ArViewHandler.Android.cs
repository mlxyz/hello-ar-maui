using Microsoft.Maui.Handlers;


namespace ArMauiApp.Controls
{
    public partial class ArViewHandler : ViewHandler<ArView, CustomGLSurfaceView>
    {
        
        protected override CustomGLSurfaceView CreatePlatformView()
        {
           return new CustomGLSurfaceView(Context);
        }
        protected override void ConnectHandler(CustomGLSurfaceView platformView)
        {
            base.ConnectHandler(platformView);
            platformView.Start();

        }

        protected override void DisconnectHandler(CustomGLSurfaceView platformView)
        {
            platformView.Dispose();
            base.DisconnectHandler(platformView);
        }
    }
}
