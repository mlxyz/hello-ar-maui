﻿using Microsoft.Maui.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
# if ANDROID
using PlatformView = ArMauiApp.Controls.CustomGLSurfaceView;
# else
using PlatformView = System.Object;
#endif
namespace ArMauiApp.Controls
{
    public partial class ArViewHandler
    {
        public static IPropertyMapper<ArView, ArViewHandler> PropertyMapper = new PropertyMapper<ArView, ArViewHandler>(ViewHandler.ViewMapper)
        {
        };

        public static CommandMapper<ArView, ArViewHandler> CommandMapper = new(ViewCommandMapper)
        {
        };
        public ArViewHandler() : base(PropertyMapper, CommandMapper)
        {
        }

    }
}
