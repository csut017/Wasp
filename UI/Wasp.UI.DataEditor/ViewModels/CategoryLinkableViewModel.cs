using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Wasp.UI.DataEditor.DataModels;
using Data = Wasp.Core.Data;

namespace Wasp.UI.DataEditor.ViewModels
{
    public abstract class CategoryLinkableViewModel
        : ViewModel
    {
        private CollectionView? categoryLinksView;
        private Data.ICategoryLinkable definition;
        private string? filterString;
        private CategoryLink? primaryCategory;

        protected CategoryLinkableViewModel(Data.ICategoryLinkable definition, Main main, ConfigurationItem item)
            : base(main, item)
        {
            this.definition = definition;
            if (definition?.CategoryLinks != null)
            {
                foreach (var link in definition.CategoryLinks)
                {
                    this.SelectedCategoryLinks.Add(link);
                    Main.EnsureCategoryIsNamed(link);
                }
            }
        }

        public List<CategoryLink> CategoryLinks
        {
            get
            {
                var existing = definition.CategoryLinks?
                    .Where(cl => cl.TargetId != null)
                    .ToDictionary(cl => cl.TargetId!) ?? new Dictionary<string, Data.CategoryLink>();
                var list = new List<CategoryLink>(
                    Main.CategoryEntries
                        .Where(ce => ce.Name != null)
                        .Select(ce =>
                        {
                            var link = new CategoryLink(this, ce);
                            if ((ce.Id != null) && existing.TryGetValue(ce.Id, out var cl))
                            {
                                link.Link = cl;
                                if (cl.IsPrimary) this.primaryCategory = link;
                            }
                            return link;
                        }));

                NotifyPropertyChanged(nameof(HasNoPrimary));
                return list;
            }
        }

        public CollectionView CategoryLinksView
        {
            get
            {
                categoryLinksView = (CollectionView)CollectionViewSource.GetDefaultView(CategoryLinks);
                categoryLinksView.SortDescriptions.Add(new SortDescription("DisplayName", ListSortDirection.Ascending));
                categoryLinksView.Filter = FilterCategoryLinks;
                return categoryLinksView;
            }
        }

        public string? FilterString
        {
            get => filterString;
            set
            {
                filterString = value;
                NotifyPropertyChanged();
                categoryLinksView?.Refresh();
            }
        }

        public bool HasNoPrimary
        {
            get => this.PrimaryCategory == null;
            set => this.PrimaryCategory = null;
        }

        public CategoryLink? PrimaryCategory
        {
            get => primaryCategory;
            set
            {
                if (primaryCategory != null) primaryCategory.IsPrimary = false;
                primaryCategory = value;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged(nameof(HasNoPrimary));
            }
        }

        public ObservableCollection<Data.CategoryLink> SelectedCategoryLinks { get; } = new();

        public CollectionView SelectedCategoryLinksView
        {
            get
            {
                var view = (CollectionView)CollectionViewSource.GetDefaultView(SelectedCategoryLinks);
                view.SortDescriptions.Add(new SortDescription("IsPrimary", ListSortDirection.Descending));
                view.SortDescriptions.Add(new SortDescription("DisplayName", ListSortDirection.Ascending));
                return view;
            }
        }

        private bool FilterCategoryLinks(object item)
        {
            if (string.IsNullOrEmpty(filterString)) return true;
            var link = item as CategoryLink;
            return link?.Entry?.Name?.Contains(filterString, System.StringComparison.InvariantCultureIgnoreCase) ?? false;
        }

        public class CategoryLink
            : ViewModelBase
        {
            private Data.CategoryLink? link;

            public CategoryLink(CategoryLinkableViewModel owner, Data.CategoryEntry entry)
            {
                Owner = owner;
                Entry = entry;
            }

            public string? DisplayName
            {
                get => Entry.DisplayName;
            }

            public Data.CategoryEntry Entry { get; }

            public bool IsPrimary
            {
                get => link?.IsPrimary ?? false;
                set
                {
                    if (IsPrimary == value) return;
                    if (value)
                    {
                        if (link == null) IsSelected = true;
                        link!.IsPrimary = true;
                        Owner.PrimaryCategory = this;
                    }
                    else
                    {
                        if (link != null)
                        {
                            link.IsPrimary = false;
                        }
                        Owner.PrimaryCategory = null;
                    }

                    NotifyPropertyChanged();
                }
            }

            public bool IsSelected
            {
                get => link != null;
                set
                {
                    if (IsSelected == value) return;
                    if (value)
                    {
                        var id = Data.NamedEntry.GenerateId();
                        link = new Data.CategoryLink
                        {
                            Id = id,
                            IsHidden = false,
                            IsPrimary = false,
                            Name = Entry.Name,
                            TargetId = Entry.Id,
                        };
                        Owner.Main.EnsureCategoryIsNamed(link);
                        Owner.definition.CategoryLinks ??= new List<Data.CategoryLink>();
                        Owner.definition.CategoryLinks.Add(link);
                        Owner.SelectedCategoryLinks.Add(link);
                    }
                    else
                    {
                        if (link != null)
                        {
                            Owner.definition.CategoryLinks?.Remove(link);
                            Owner.SelectedCategoryLinks.Remove(link);
                            if (link.IsPrimary) Owner.PrimaryCategory = null;
                        }
                        link = null;
                    }
                    NotifyPropertyChanged();
                }
            }

            public Data.CategoryLink? Link
            {
                set => link = value;
            }

            public CategoryLinkableViewModel Owner { get; }
        }
    }
}