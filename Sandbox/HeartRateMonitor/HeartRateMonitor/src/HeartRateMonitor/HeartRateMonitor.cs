
//Copyright 2018 Samsung Electronics Co., Ltd
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Tizen.Wearable.CircularUI.Forms;
using Tizen.Wearable.CircularUI.Forms.Renderer;
using FormsCircularUI = Tizen.Wearable.CircularUI.Forms.FormsCircularUI;
using ShellRenderer = Xamarin.Forms.Platform.Tizen.Watch.ShellRenderer;

namespace HeartRateMonitor
{
    class Program : Xamarin.Forms.Platform.Tizen.FormsApplication
    {
        protected override void OnCreate()
        {
            base.OnCreate();

            LoadApplication(new App());
        }

        static void Main(string[] args)
        {
            var app = new Program();
            var customRenderer = new Dictionary<Type, Func<IRegisterable>>()
            {
                { typeof(Shell), ()=> new CircularShellRenderer() },
                { typeof(CirclePage), ()=> new CirclePageRenderer() },
                { typeof(CircleStepper), ()=> new CircleStepperRenderer() },
            };

            var option = new InitializationOptions(app)
            {
                UseMessagingCenter = false,
                PlatformType = PlatformType.Lightweight
            };
            option.UseStaticRegistrar(StaticRegistrarStrategy.StaticRegistrarOnly, customRenderer, true);
            Forms.Init(option);
            
            FormsCircularUI.Init();
            app.Run(args);
        }
    }
}
