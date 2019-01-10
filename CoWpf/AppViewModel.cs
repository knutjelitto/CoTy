using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using ReactiveUI;

namespace CoWpf
{
    public class AppViewModel : ReactiveObject
    {
        private string _searchTerm;

        public string SearchTerm
        {
            get => this._searchTerm;
            set => this.RaiseAndSetIfChanged(ref this._searchTerm, value);
        }

        private readonly ObservableAsPropertyHelper<IEnumerable<NugetDetailsViewModel>> _searchResults;
        public IEnumerable<NugetDetailsViewModel> SearchResults => this._searchResults.Value;

        private readonly ObservableAsPropertyHelper<bool> _isAvailable;
        public bool IsAvailable => this._isAvailable.Value;

        public AppViewModel()
        {
            this._searchResults =
                this.WhenAnyValue(x => x.SearchTerm)
                    .Throttle(TimeSpan.FromMilliseconds(800))
                    .Select(term => term?.Trim())
                    .DistinctUntilChanged()
                    .Where(term => !string.IsNullOrWhiteSpace(term))
                    .SelectMany(SearchNuGetPackages)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .ToProperty(this, x => x.SearchResults);

            this._searchResults.ThrownExceptions.Subscribe(error => { });

            this._isAvailable =
                this.WhenAnyValue(x => x.SearchResults)
                    .Select(searchResults => searchResults != null)
                    .ToProperty(this, x => x.IsAvailable);
        }

        private async Task<IEnumerable<NugetDetailsViewModel>> SearchNuGetPackages(string term, CancellationToken token)
        {
            var providers = new List<Lazy<INuGetResourceProvider>>();
            providers.AddRange(Repository.Provider.GetCoreV3()); // Add v3 API support
            var packageSource = new PackageSource("https://api.nuget.org/v3/index.json");
            var source = new SourceRepository(packageSource, providers);

            var filter = new SearchFilter(false);
            var resource = await source.GetResourceAsync<PackageSearchResource>().ConfigureAwait(false);
            var metadata = await resource.SearchAsync(term, filter, 0, 10, null, token).ConfigureAwait(false);
            return metadata.Select(x => new NugetDetailsViewModel(x));
        }
    }
}
