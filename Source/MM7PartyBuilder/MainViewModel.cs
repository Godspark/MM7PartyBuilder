using System;
using MM7ClassCreatorWPF.BaseClasses;
using System.Collections.ObjectModel;
using System.Linq;
using MM7ClassCreatorWPF.Models;
using System.Collections.Generic;

namespace MM7ClassCreatorWPF
{
    public class MainViewModel : PropertyChangedBase
    {
        private ClassSuggestionGenerator _classSuggestionGenerator;
        public ObservableCollection<CharacterClass> AllClasses { get; set; }
        public ObservableCollection<MasteryLevel> AllMasteryLevels { get; set; }
        public ObservableCollection<CharacterSkill> AllSkills { get; set; }
        public ObservableCollection<NumberOfCharacters> AllNumberOfCharacters { get; set; }
        public ObservableCollection<AndOr> AllAndOr { get; set; }

        private ObservableCollection<FilterItem> _selectedFilterItems;
        public ObservableCollection<FilterItem> SelectedFilterItems
        {
            get { return _selectedFilterItems; }
            set
            {
                _selectedFilterItems = value;
                NotifyOnPropertyChanged(() => SelectedFilterItems);
            }
        }
        
        private ObservableCollection<ClassSuggestion> _classSuggestions;
        public ObservableCollection<ClassSuggestion> ClassSuggestions
        {
            get { return _classSuggestions; }
            set
            {
                _classSuggestions = value;
                NotifyOnPropertyChanged(() => ClassSuggestions);
            }
        }

        public Command AddFilterCommand { get; set; }
        public Command RemoveFilterCommand { get; set; }
        public Command GenerateClassesCommand { get; set; }

        public MainViewModel()
        {
            Initialize();

            AddFilterCommand = new Command(p => OnAddFilter());
            RemoveFilterCommand = new Command(OnRemoveFilter);
            GenerateClassesCommand = new Command(p => OnGenerateClasses());

            SelectedFilterItems.CollectionChanged += OnCollectionChanged;
            ClassSuggestions.CollectionChanged += OnCollectionChanged;
            OnAddFilter();
        }

        public void Initialize()
        {
            _classSuggestionGenerator = new ClassSuggestionGenerator();
            AllClasses = new ObservableCollection<CharacterClass>(Enum.GetValues(typeof(CharacterClass)).Cast<CharacterClass>());
            AllMasteryLevels = new ObservableCollection<MasteryLevel>(Enum.GetValues(typeof(MasteryLevel)).Cast<MasteryLevel>());
            AllSkills = new ObservableCollection<CharacterSkill>(Enum.GetValues(typeof(CharacterSkill)).Cast<CharacterSkill>());
            AllNumberOfCharacters = new ObservableCollection<NumberOfCharacters>(Enum.GetValues(typeof(NumberOfCharacters)).Cast<NumberOfCharacters>());
            AllAndOr = new ObservableCollection<AndOr>(Enum.GetValues(typeof(AndOr)).Cast<AndOr>());

            SelectedFilterItems = new ObservableCollection<FilterItem>();
            ClassSuggestions = new ObservableCollection<ClassSuggestion>();
        }

        private void OnCollectionChanged(object sender, EventArgs e)
        {
            NotifyOnPropertyChanged(() => sender);
        }

        private void OnAddFilter()
        {
            SelectedFilterItems.Add(new FilterItem
            {
                IsFirstElementInList = false,
                Skill = CharacterSkill.MagicAir,
                Mastery = MasteryLevel.None,
                NumberOfCharacters = NumberOfCharacters.AtLeast1,
                AndOr = AndOr.And
            });
            if (SelectedFilterItems.Count() == 1)
                SelectedFilterItems[0].IsFirstElementInList = true;
        }

        private void OnRemoveFilter(object sender)
        {
            SelectedFilterItems.Remove(sender as FilterItem);
        }

        private void OnGenerateClasses()
        {
            ClassSuggestions = new ObservableCollection<ClassSuggestion>(_classSuggestionGenerator.SuggestClasses(SelectedFilterItems));
            //ClassSuggestions[0].IsFirstElementInList = true;
        }
    }
}
