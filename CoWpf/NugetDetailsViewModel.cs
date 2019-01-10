using System;
using System.Diagnostics;
using System.Reactive;
using NuGet.Protocol.Core.Types;
using ReactiveUI;

namespace CoWpf
{
    public class NugetDetailsViewModel : ReactiveObject
    {
        private readonly IPackageSearchMetadata _metadata;
        private readonly Uri _defaultUrl;

        public NugetDetailsViewModel(IPackageSearchMetadata metadata)
        {
            this._metadata = metadata;
            this._defaultUrl = new Uri("https://git.io/fAlfh");
            OpenPage = ReactiveCommand.Create(() => { Process.Start(ProjectUrl.ToString()); });
        }

        public Uri IconUrl => this._metadata.IconUrl ?? this._defaultUrl;
        public string Description => this._metadata.Description;
        public Uri ProjectUrl => this._metadata.ProjectUrl;
        public string Title => this._metadata.Title;

        public ReactiveCommand<Unit, Unit> OpenPage { get; }
    }
}