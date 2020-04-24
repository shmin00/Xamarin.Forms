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
using Alarm.Implements;
using Alarm.Models;
using Alarm.ViewModels;
using Alarm.Views;
using Tizen.Applications;
using Xamarin.Forms;
using Tizen.Wearable.CircularUI.Forms;
using Tizen.Wearable.CircularUI.Forms.Renderer;
using Alarm.Controls;
using Alarm.Tizen.Renderers;

namespace Alarm
{
    class Program : global::Xamarin.Forms.Platform.Tizen.FormsApplication
    {
        App app;
        AlertPageModel _alertPageModel;

        /// <summary>
        /// Called when this application is launched
        /// </summary>
        protected override void OnCreate()
        {
            AlarmNativeHandler.ResourceDir = DirectoryInfo.Resource;
            base.OnCreate();
            app = new App();
            LoadApplication(app);
        }

        protected override void OnPause()
        {
            Console.WriteLine("OnPause");
            if (_alertPageModel != null)
            {
                if(_alertPageModel.AlertSoundState == SoundState.Start)
                {
                    Console.WriteLine("Alert sound state is start, pause alert sound!");
                    _alertPageModel.PauseAlert();
                }
            }

            base.OnPause();

        }

        protected override void OnResume()
        {
            Console.WriteLine("OnResume");
            base.OnResume();

        }

        /// <summary>
        /// Called when this app control event received.
        /// </summary>
        /// <param name="e">AppControlReceivedEventArgs</param>
        async protected override void OnAppControlReceived(AppControlReceivedEventArgs e)
        {
            Console.WriteLine("OnAppControlReceived");
            base.OnAppControlReceived(e);
            AppControl appControl = e.ReceivedAppControl;
            try
            {
                if (appControl.ExtraData.Count() != 0)
                {
                    string AlarmCreatedDate = appControl.ExtraData.Get<string>("AlarmRecord.UniqueIdentifier");
                    Console.WriteLine($"ExtraData.Count() != 0 AlarmCreatedDate:{AlarmCreatedDate}");
                    if (AlarmModel.AlarmRecordDictionary == null)
                    {
                        // Need to retrieve at the page creation time
                        AlarmModel.AlarmRecordDictionary = AlarmPersistentHandler.DeserializeAlarmRecord();
                    }

                    AlarmRecord retrievedRecord;
                    if (AlarmModel.AlarmRecordDictionary != null)
                    {
                        if (AlarmModel.AlarmRecordDictionary.TryGetValue(AlarmCreatedDate, out retrievedRecord))
                        {
                            if (retrievedRecord == null)
                            {
                                Console.WriteLine("[OnAppControlReceived] retrievedRecord is null!!");
                            }

                            Console.WriteLine("retrievedRecord:" + retrievedRecord);
                            if (retrievedRecord != null && retrievedRecord.AlarmState < AlarmStates.Inactive)
                            {
                                _alertPageModel = new AlertPageModel(retrievedRecord);
                                await Shell.Current.Navigation.PushAsync(new AlarmAlertPage(_alertPageModel));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[OnAppControlReceived] Exception - Message: " + ex.Message + ", source: " + ex.Source + ", " + ex.StackTrace);
            }
        }

        static void Main(string[] args)
        {
            var app = new Program();

            var customRenderers = new Dictionary<Type, Func<IRegisterable>>()
            {
                { typeof(Shell), ()=> new ShellRenderer() },
                { typeof(CirclePage), ()=> new CirclePageRenderer() },
                { typeof(CircleDateTimeSelector), ()=> new CircleDateTimeSelectorRenderer() },
                { typeof(Check), ()=> new CheckRenderer() },
                { typeof(CustomImageButton), () => new CustomImageButtonRenderer() }
             };

            var option = new InitializationOptions(app)
            {
                StaticRegistarStrategy = StaticRegistrarStrategy.StaticRegistrarOnly,
                PlatformType = PlatformType.Lightweight,
                CustomHandlers = customRenderers,
                Flags = InitializationFlags.DisableCss
            };
            Forms.Init(option);

            // It's mandatory to initialize Circular UI for using Tizen Wearable Circular UI API
            global::Tizen.Wearable.CircularUI.Forms.FormsCircularUI.Init();
            app.Run(args);
        }
    }
}
