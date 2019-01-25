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

        private TiltSolution currentTiltSolution;

        #endregion

        #region Construction

        public MainViewViewModel(IAngleService angleService, ISearchService searchService)
        {
            this.angleService = angleService;
            this.searchService = searchService;

            this.PlaneAngleTitle = UIStrings.WindowTitle;
            this.AxisCombinationLabel = UIStrings.AxisCombinationLabel;

            this.PlanesHeader = UIStrings.PlanesToolTipHeader;
            this.PlanesContent = UIStrings.PlanesToolTipContent;
            this.RefreshHeader = UIStrings.RefreshToolTipHeader;
            this.RefreshContent = UIStrings.RefreshToolTipContent;
            this.SwapHeader = UIStrings.SwapToolTipHeader;
            this.SwapContent = UIStrings.SwapToolTipContent;

            this.UpdateViewListCommand = new DelegateCommand(this.OnUpdateViewListCommand);
            this.SwapSolutionCommand = new DelegateCommand(this.OnSwapSolutionCommand);

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

            this.currentTiltSolution = TiltSolution.Positive;

            this.SelectedAxisCombination = AxisCombinations[0];
            this.SelectedView = Views[0];
    }

        #endregion

        #region Commands

        public ICommand UpdateViewListCommand { get; }

        public ICommand SwapSolutionCommand { get; }

        #endregion

        #region Public Properties

        public string PlaneAngleTitle { get; set; }

        public string AxisCombinationLabel { get; set; }

        public string PlanesHeader { get; set; }

        public string PlanesContent { get; set; }

        public string RefreshHeader { get; set; }

        public string RefreshContent { get; set; }

        public string SwapHeader { get; set; }

        public string SwapContent { get; set; }

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
                SetAngles(this.currentTiltSolution);
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
                    SetAngles(this.currentTiltSolution);
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

        private void OnSwapSolutionCommand(object parameter)
        {
            if (this.SelectedView != null)
            {
                switch (this.currentTiltSolution)
                {
                    case TiltSolution.Positive:
                        SetAngles(TiltSolution.Negative);
                        break;

                    case TiltSolution.Negative:
                        SetAngles(TiltSolution.Positive);
                        break;

                    default:
                        break;
                }
            }
        }
            
        private void SetAngles(TiltSolution solution)
        {
            switch (SelectedAxisCombination.Setup)
            {
                case RotarySetup.AC:
                    RotationAngle acSetup = null;

                    if (solution == TiltSolution.Positive)
                    {
                        acSetup = angleService.CalculatePositiveTilt(SelectedView.ViewMatrix, SelectedAxisCombination.Setup);
                    }
                    else
                    {
                        acSetup = angleService.CalculateNegativeTilt(SelectedView.ViewMatrix, SelectedAxisCombination.Setup);
                    }

                    this.SecondaryAngle = $"{SelectedAxisCombination.SecondaryAxisLabel} {acSetup.SecondaryAngle.ToString(UIStrings.AngleFormat)}";
                    this.PrimaryAngle = $"{SelectedAxisCombination.PrimaryAxisLabel} {acSetup.PrimaryAngle.ToString(UIStrings.AngleFormat)}";
                    this.currentTiltSolution = acSetup.Solution;
                    break;

                case RotarySetup.BC:
                    RotationAngle bcSetup = null;

                    if (solution == TiltSolution.Positive)
                    {
                        bcSetup = angleService.CalculatePositiveTilt(SelectedView.ViewMatrix, SelectedAxisCombination.Setup);
                    }
                    else
                    {
                        bcSetup = angleService.CalculateNegativeTilt(SelectedView.ViewMatrix, SelectedAxisCombination.Setup);
                    }
                    
                    this.SecondaryAngle = $"{SelectedAxisCombination.SecondaryAxisLabel} {bcSetup.SecondaryAngle.ToString(UIStrings.AngleFormat)}";
                    this.PrimaryAngle = $"{SelectedAxisCombination.PrimaryAxisLabel} {bcSetup.PrimaryAngle.ToString(UIStrings.AngleFormat)}";
                    this.currentTiltSolution = bcSetup.Solution;
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