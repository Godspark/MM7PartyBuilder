using System;
using MM7ClassCreatorWPF.BaseClasses;
using System.Collections.ObjectModel;
using System.Linq;
using MM7ClassCreatorWPF.Models;

namespace MM7ClassCreatorWPF
{
    public class MainViewModel : PropertyChangedBase
    {
        private ClassSuggestionGenerator _classSuggestionGenerator;
        public ObservableCollection<CharacterClass> AllClasses { get; set; }
        public ObservableCollection<MasteryLevel> AllMasteryLevels { get; set; }
        public ObservableCollection<CharacterSkill> AllSkills { get; set; }
        
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
                Skill = CharacterSkill.MagicAir,
                Mastery = MasteryLevel.None
            });
        }

        private void OnRemoveFilter(object sender)
        {
            SelectedFilterItems.Remove(sender as FilterItem);
        }

        private void OnGenerateClasses()
        {
            ClassSuggestions = new ObservableCollection<ClassSuggestion>(_classSuggestionGenerator.SuggestClasses(SelectedFilterItems));
        }
    }
}
