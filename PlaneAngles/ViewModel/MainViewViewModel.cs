// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="CNC Software, Inc.">
//   Copyright (c) 2017 CNC Software, Inc.
// </copyright>
// <summary>
//  If this project is helpful please take a short survey at ->
//  http://ux.mastercam.com/Surveys/APISDKSupport 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PlaneAngles.ViewModel
{
    using System.Windows.Input;
    using System.ComponentModel;
    using System.Collections.Generic;

    using Mastercam.Database;

    using PlaneAngles.Services;
    using PlaneAngles.Commands;
    using PlaneAngles.DataTypes;
    using PlaneAngles.Resources;

    public class MainViewViewModel : INotifyPropertyChanged
    {
        #region Private Fields

        private readonly IAngleService angleService;

        private readonly ISearchService searchService;

        private List<MCView> views;

        private MCView selectedView;

        private List<AxisCombination> axisCombinations;

        private AxisCombination selectedAxisCombination;

        private string primaryAngle;

        private string secondaryAngle;

        #endregion

        #region Construction

        public MainViewViewModel(IAngleService angleService, ISearchService searchService)
        {
            this.angleService = angleService;
            this.searchService = searchService;

            this.PlaneAngleTitle = UIStrings.WindowTitle;
            this.AxisCombinationLabel = UIStrings.AxisCombinationLabel;

            this.UpdateViewListCommand = new DelegateCommand(this.OnUpdateViewListCommand);

            this.Views = searchService.GetViews();

            this.AxisCombinations = new List<AxisCombination>()
            {
                new AxisCombination()
                {
                    Setup = RotarySetup.AC,
                    CombinationName = UIStrings.ACCombination,
                    PrimaryAxisLabel = UIStrings.CLabel,
                    SecondaryAxisLabel = UIStrings.ALabel
                },
                new AxisCombination()
                {
                    Setup = RotarySetup.BC,
                    CombinationName = UIStrings.BCCombination,
                    PrimaryAxisLabel = UIStrings.CLabel,
                    SecondaryAxisLabel = UIStrings.BLabel       
                }
            };

            this.SelectedAxisCombination = AxisCombinations[0];

            this.PrimaryAngle = SelectedAxisCombination.PrimaryAxisLabel;
            this.SecondaryAngle = SelectedAxisCombination.SecondaryAxisLabel;
        }

        #endregion

        #region Commands

        public ICommand UpdateViewListCommand { get; }

        #endregion

        #region Public Properties

        public string PlaneAngleTitle { get; set; }

        public string AxisCombinationLabel { get; set; }

        public List<MCView> Views
        {
            get => this.views;

            set
            {
                this.views = value;
                OnPropertyChanged(nameof(this.Views));
            }
        }

        public MCView SelectedView
        {
            get => this.selectedView;

            set
            {
                this.selectedView = value;
                if (SelectedAxisCombination != null)
                    SetAngles();
                OnPropertyChanged(nameof(this.SelectedView));
            }
        }

        public List<AxisCombination> AxisCombinations
        {
            get => this.axisCombinations;

            set
            {
                this.axisCombinations = value;
                OnPropertyChanged(nameof(this.AxisCombinations));
            }
        }

        public AxisCombination SelectedAxisCombination
        {
            get => this.selectedAxisCombination;

            set
            {
                this.selectedAxisCombination = value;
                if (SelectedView != null)
                    SetAngles();
                OnPropertyChanged(nameof(this.SelectedAxisCombination));
            }
        }

        public string PrimaryAngle
        {
            get => this.primaryAngle;

            set
            {
                this.primaryAngle = value;
                OnPropertyChanged(nameof(this.PrimaryAngle));
            }
        }

        public string SecondaryAngle
        {
            get => this.secondaryAngle;

            set
            {
                this.secondaryAngle = value;
                OnPropertyChanged(nameof(this.SecondaryAngle));
            }
        }

        #endregion

        #region Private Methods     

        private void OnUpdateViewListCommand(object parameter) => this.Views = searchService.GetViews();
      
        private void SetAngles()
        {
            switch (SelectedAxisCombination.Setup)
            {
                case RotarySetup.AC:
                    var acSetup = angleService.CalculateAngles(SelectedView.ViewMatrix, SelectedAxisCombination.Setup);
                    this.SecondaryAngle = $"{SelectedAxisCombination.SecondaryAxisLabel} {acSetup.SecondaryAngle.ToString(UIStrings.AngleFormat)}";
                    this.PrimaryAngle = $"{SelectedAxisCombination.PrimaryAxisLabel} {acSetup.PrimaryAngle.ToString(UIStrings.AngleFormat)}";
                    break;

                case RotarySetup.BC:
                    var bcSetup = angleService.CalculateAngles(SelectedView.ViewMatrix, SelectedAxisCombination.Setup);
                    this.SecondaryAngle = $"{SelectedAxisCombination.SecondaryAxisLabel} {bcSetup.SecondaryAngle.ToString(UIStrings.AngleFormat)}";
                    this.PrimaryAngle = $"{SelectedAxisCombination.PrimaryAxisLabel} {bcSetup.PrimaryAngle.ToString(UIStrings.AngleFormat)}";
                    break;

                default:
                    break;
            }      
        }

        #endregion

        #region Notify Property Changed

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion
    }

}