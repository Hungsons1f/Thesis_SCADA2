using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Thesis_SCADA.Model;

namespace Thesis_SCADA.ViewModel
{
    public class EventViewModel : BaseViewModel
    {
        #region Properties
        private ObservableCollection<ProcessEvent> _eventList;
        public ObservableCollection<ProcessEvent> EventList { get => _eventList; set { _eventList = value; OnPropertyChanged(); } }

        private bool isFilterInfo = true;
        public bool IsFilterInfo { get => isFilterInfo; set { isFilterInfo = value; OnPropertyChanged(); } }

        private bool isFilterAlarm =true;
        public bool IsFilterAlarm { get => isFilterAlarm; set { isFilterAlarm = value; OnPropertyChanged(); } }

        private bool isFilterError = true;
        public bool IsFilterError { get => isFilterError; set { isFilterError = value; OnPropertyChanged(); } }

        private CollectionView view;
        private bool issortid;
        private bool issorttype;
        private bool issortgroup;
        private bool issortcontent;
        private bool issortsource;
        private bool issorttimeraised;
        private bool issorttimeconfirmed;
        private bool issorttimecleared;
        #endregion

        #region Commands
        public ICommand RefreshCommand { get; set; }
        public ICommand AckAllCommand { get; set; }
        public ICommand LoadedCommand { get; set; }
        public ICommand RefreshViewCommand { get; set; }
        public ICommand SortCommand { get; set; }
        #endregion

        public EventViewModel()
        {
            EventList = new ObservableCollection<ProcessEvent>(DataProvider.Ins.DB.ProcessEvent.OrderByDescending(p => p.Id));

            RefreshCommand = new RelayCommand<ListView>((p) => { return true; }, (p) =>
            {
                EventList = new ObservableCollection<ProcessEvent>(DataProvider.Ins.DB.ProcessEvent.OrderByDescending(x => x.Id));
                view = (CollectionView)CollectionViewSource.GetDefaultView(p.ItemsSource);
                view.Filter -= UserFilter;
                view.Filter += UserFilter;
            });

            AckAllCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                GlobalVar.Ins.AckAllAlarms();
            });

            LoadedCommand = new RelayCommand<ListView>((p) => { return true; }, (p) =>
            {
                view = (CollectionView)CollectionViewSource.GetDefaultView(p.ItemsSource);
                view.Filter -= UserFilter;
                view.Filter += UserFilter;
            });

            RefreshViewCommand = new RelayCommand<ListView>((p) => { return true; }, (p) =>
            {
                CollectionViewSource.GetDefaultView(p.ItemsSource).Refresh();
            });

            SortCommand = new RelayCommand<GridViewColumnHeader>((p) => { return true; }, (p) =>
            {
                bool issort;
                string name;
                switch (p.Name)
                {
                    case "gvchId":
                        issort = issortid;
                        issortid = !issortid;
                        name = "Id";
                        break;
                    case "gvchType":
                        issort = issorttype;
                        issorttype = !issorttype;
                        name = "SeverityLevel";
                        break;
                    case "gvchGroup":
                        issort = issortgroup;
                        issortgroup = !issortgroup;
                        name = "EventClass";
                        break;
                    case "gvchContent":
                        issort = issortcontent;
                        issortcontent = !issortcontent;
                        name = "EventID";
                        break;
                    case "gvchSource":
                        issort = issortsource;
                        issortsource = !issortsource;
                        name = "SourceName";
                        break;
                    case "gvchRaised":
                        issort = issorttimeraised;
                        issorttimeraised = !issorttimeraised;
                        name = "TimeRaised";
                        break;
                    case "gvchConfirmed":
                        issort = issorttimeconfirmed;
                        issorttimeconfirmed = !issorttimeconfirmed;
                        name = "TimeConfirmed";
                        break;
                    case "gvchCleared":
                        issort = issorttimecleared;
                        issorttimecleared = !issorttimecleared;
                        name = "TimeCleared";
                        break;
                    default:
                        issort = true;
                        name = "Id";
                        break;
                }

                if (issort)
                {
                    view.SortDescriptions.Clear();
                    view.SortDescriptions.Add(new SortDescription(name, ListSortDirection.Ascending)); //Sắp xếp theo header
                }
                else
                {
                    view.SortDescriptions.Clear();
                    view.SortDescriptions.Add(new SortDescription(name, ListSortDirection.Descending));
                }
            });
        }

        private bool UserFilter(object item)
        {
            if (IsFilterInfo && !IsFilterAlarm && !IsFilterError)
                return ((item as ProcessEvent).SeverityLevel == 1);
            else if (!IsFilterInfo && IsFilterAlarm && !IsFilterError)
                return ((item as ProcessEvent).SeverityLevel == 2);
            else if (!IsFilterInfo && !IsFilterAlarm && IsFilterError)
                return ((item as ProcessEvent).SeverityLevel == 3);
            else if (IsFilterInfo && IsFilterAlarm && !IsFilterError)
                return ((item as ProcessEvent).SeverityLevel != 3);
            else if (!IsFilterInfo && IsFilterAlarm && IsFilterError)
                return ((item as ProcessEvent).SeverityLevel != 1);
            else if (IsFilterInfo && !IsFilterAlarm && IsFilterError)
                return ((item as ProcessEvent).SeverityLevel != 2);
            else if (IsFilterInfo && IsFilterAlarm && IsFilterError)
                return true;
            else
                return false;
        }
    }
}
