﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.ComTypes;
using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.SimpleChildWindow;
using NathalieInwentaryzacje.Lib.Contracts.Dto;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Templates;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;
using NathalieInwentaryzacje.Main.Interfaces;
using NathalieInwentaryzacje.ViewModels.Common;
using NathalieInwentaryzacje.ViewModels.Records;
using NathalieInwentaryzacje.Views.Records;

namespace NathalieInwentaryzacje.ViewModels
{
    public class MainViewModel : ListScreen<RecordInfo>, IMain
    {
        public MainViewModel()
        {
            LoadData();
        }

        public override void LoadData()
        {
            var records = IoC.Get<IRecordsManager>().GetRecords();

            var context = new ObservableCollection<RecordInfo>();

            foreach (var record in records)
            {
                context.Add(record);
            }

            Context = context;
        }

        public void NewRecord()
        {
            if (WindowManager.ShowDialog(new NewRecordViewModel()) == true)
            {
                LoadData();
            }
        }

        public void CloseApp()
        {
            Application.Current.Shutdown();
        }
    }
}