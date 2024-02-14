using Microsoft.Maui.Handlers;
using IO.Github.Sceneview.AR;
using Com.Google.Android.Filament.Gltfio;
using IO.Github.Sceneview.Node;
using Dev.Romainguy.Kotlin.Math;
using Kotlin.Jvm.Functions;
using IO.Github.Sceneview.Loaders;
using Kotlin.Coroutines;
using IO.Github.Sceneview.AR.Node;
using Google.AR.Core;

namespace ArMauiApp.Controls
{
    public partial class ArViewHandler : ViewHandler<ArView, ARSceneView>
    {
        FilamentInstance model;

        protected override ARSceneView CreatePlatformView()
        {
            var view = new ARSceneView(Context);
            var url = "https://sceneview.github.io/assets/models/DamagedHelmet.glb";
            var resourceResolver = new ResourceResolverFunction1(url);
            LoadModelInstance(view.ModelLoader, url, resourceResolver).ContinueWith(model =>
            {
                view.PlaneRenderer.Enabled = true;
                view.OnSessionUpdated = new OnSessionUpdatedImpl(view, model.Result);
            });
            
            return view;
        }
        protected override void ConnectHandler(ARSceneView platformView)
        {
            base.ConnectHandler(platformView);
        }

        protected override void DisconnectHandler(ARSceneView platformView)
        {
            platformView.Dispose();
            base.DisconnectHandler(platformView);
        }

        private static async Task<FilamentInstance> LoadModelInstance(ModelLoader modelLoader, string url, IFunction1 resourceResolver)
        {
            var tcs = new TaskCompletionSource<FilamentInstance>();
            var continuation = new Continuation((result) =>
            {
                if (result is FilamentInstance modelInstance)
                {
                    tcs.SetResult(modelInstance);
                }
                else
                {
                    tcs.SetException(new Exception("Failed to load model instance."));
                }
            });
            modelLoader.LoadModelInstance(url, resourceResolver, continuation);
            return await tcs.Task;
        }

        class ResourceResolverFunction1 : Java.Lang.Object, IFunction1
        {
            private readonly string fileLocation;

            public ResourceResolverFunction1(string fileLocation)
            {
                this.fileLocation = fileLocation;
            }
            public Java.Lang.Object Invoke(Java.Lang.Object? objParameter)
            {
                try
                {
                    var parameter = (Java.Lang.String)objParameter;
                    var res = ModelLoader.CompanionField.GetFolderPath(this.fileLocation, parameter.ToString());
                    return res;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        class Continuation : Java.Lang.Object, IContinuation
        {
            private readonly Action<Java.Lang.Object> _onComplete;

            public Continuation(Action<Java.Lang.Object> onComplete)
            {
                _onComplete = onComplete;
            }

            public ICoroutineContext Context => EmptyCoroutineContext.Instance;

            public void ResumeWith(Java.Lang.Object result)
            {
                _onComplete(result);
            }
        }

        public class OnSessionUpdatedImpl : Java.Lang.Object, IFunction2
        {
            private readonly ARSceneView view;
            private bool _placed = false;
            private FilamentInstance model;

            public OnSessionUpdatedImpl(ARSceneView view, FilamentInstance model)
            {
                this.view = view;
                this.model = model;
            }
            public Java.Lang.Object? Invoke(Java.Lang.Object? p0, Java.Lang.Object? p1)
            {
                if (_placed)
                    return null;
                var frame = (Google.AR.Core.Frame)p1;
                var trackables = frame.GetUpdatedTrackables(Java.Lang.Class.ForName("com.google.ar.core.Plane"));
                var planes = trackables.Cast<Plane>();
                var plane = planes.FirstOrDefault(plane => plane.GetType() == Plane.Type.HorizontalUpwardFacing, null);
                if (plane != null)
                {
                    var anchor = plane.CreateAnchor(plane.CenterPose);
                    var modelNode = new ModelNode(model, true, new Java.Lang.Float(1), new Float3(0, -0.5f, 0));
                    var anchorNode = new AnchorNode(view.Engine, anchor, null, null, null, null);
                    anchorNode.AddChildNode(modelNode);
                    view.AddChildNode(anchorNode);
                    _placed = true;
                    return null;
                }
                return null;
            }
        }

        public static void Start(ArViewHandler handler, ArView view, object? _)
        {

        }

    }
}
