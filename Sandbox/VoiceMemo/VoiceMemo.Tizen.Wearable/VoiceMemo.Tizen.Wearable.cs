/*
 * Copyright (c) 2018 Samsung Electronics Co., Ltd
 *
 * Licensed under the Flora License, Version 1.1 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://floralicense.org/license/
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using Tizen.Applications;
using Tizen.Wearable.CircularUI.Forms;
using Tizen.Wearable.CircularUI.Forms.Renderer;
using VoiceMemo.Services;
using VoiceMemo.Tizen.Wearable.Effects;
using VoiceMemo.Tizen.Wearable.Renderers;
using VoiceMemo.Tizen.Wearable.Services;
using VoiceMemo.Views;
using Xamarin.Forms;
using CircleListView = Tizen.Wearable.CircularUI.Forms.CircleListView;
using FormsCircularUI = Tizen.Wearable.CircularUI.Forms.FormsCircularUI;
using Xamarin.Forms.Internals;

namespace VoiceMemo.Tizen.Wearable
{
    class Program : global::Xamarin.Forms.Platform.Tizen.FormsApplication
    {
        App app;
        /// <summary>
        /// Called when this application is launched
        /// </summary>
        protected override void OnCreate()
        {
            base.OnCreate();
            app = new App();
            LoadApplication(app);
        }

        /// <summary>
        /// Called when this application is terminated
        /// </summary>
        protected override void OnTerminate()
        {
            Console.WriteLine("[Program.OnTerminate] LastUsedID " + AudioRecordService.numbering);
            // Save the index of recording files
            Preference.Set(DeviceInformation.LastUsedID, AudioRecordService.numbering);
            base.OnTerminate();
            app.Terminate();
        }

        static void Main(string[] args)
        {
            var app = new Program();
            var customRenderer = new Dictionary<Type, Func<IRegisterable>>()
            {
                { typeof(Shell), ()=> new ShellRenderer() },
                { typeof(CirclePage), ()=> new CirclePageRenderer() },
                { typeof(CirclePageEx), ()=> new CirclePageRenderer() },
                { typeof(CircleListView), ()=> new CircleListViewRenderer() },
                { typeof(CircleScrollView), ()=> new CircleScrollViewRenderer() },
                { typeof(TwoButtonPage), ()=> new TwoButtonPageRenderer() },
            };

            var option = new InitializationOptions(app)
            {
                PlatformType = PlatformType.Lightweight,
                EffectScopes = new InitializationOptions.EffectScope[]
                {
                    new InitializationOptions.EffectScope
                    {
                        Name = "SEC",
                        Effects = new ExportEffectAttribute[] {
                            new ExportEffectAttribute(typeof(BlendColorEffect), "BlendColorEffect"),
                            new ExportEffectAttribute(typeof(TizenEventPropagationEffect), "TizenEventPropagationEffect"),
                            new ExportEffectAttribute(typeof(TizenItemLongPressEffect), "ItemLongPressEffect"),
                            new ExportEffectAttribute(typeof(TizenStyleEffect), "TizenStyleEffect")
                        }
                    }
                }
            };
            option.UseStaticRegistrar(StaticRegistrarStrategy.StaticRegistrarOnly, customRenderer, true);
            DependencyService.Register<IAudioPlayService, AudioPlayService>();
            DependencyService.Register<IDeviceInformation, DeviceInformation>();
            DependencyService.Register<ILocaleService, LocaleService>();
            DependencyService.Register<IAppDataService, AppDataService>();
            DependencyService.Register<ISpeechToTextService, SpeechToTextService>();
            DependencyService.Register<IMediaContentService, MediaContentService>();
            DependencyService.Register<IUserPermission, UserPermission>();
            DependencyService.Register<IAppTerminator, AppTerminator>();
            DependencyService.Register<IAudioRecordService, AudioRecordService>();
            DependencyService.Register<IGraphicPopup, GraphicPopUpRenderer>();

            Forms.Init(option);
            // It's mandatory to initialize Circular UI for using Tizen Wearable Circular UI API
            FormsCircularUI.Init();
            app.Run(args);
        }
    }
}
