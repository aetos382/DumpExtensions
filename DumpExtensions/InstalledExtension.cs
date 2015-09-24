namespace DumpExtensions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Media.Imaging;

    using Microsoft.VisualStudio.ExtensionManager;
    using Microsoft.VisualStudio.ExtensionsExplorer;

    internal class InstalledExtension :
        IVsExtension
    {
        private readonly IInstalledExtension _installedExtension;

        public InstalledExtension(IInstalledExtension extension)
        {
            this._installedExtension = extension;

            this.Name = extension.Header.LocalizedName ?? extension.Header.Name;
            this.Id = extension.Header.Identifier;
            this.Description = extension.Header.LocalizedDescription ?? extension.Header.Description;

            this.SmallThumbnailImage = this.MediumThumbnailImage = this.LoadIcon(extension.Header.Icon);
            this.PreviewImage = this.LoadBitmap(extension.Header.PreviewImage);
        }

        public string Name { get; }

        public string Id { get; }

        public string Description { get; }

        public float Priority { get; }

        public BitmapSource MediumThumbnailImage { get; }

        public BitmapSource SmallThumbnailImage { get; }

        public BitmapSource PreviewImage { get; }

        public bool IsSelected { get; set; }

        private BitmapSource LoadBitmap(string path)
        {
            BitmapSource bitmap = null;

            if (!string.IsNullOrEmpty(path))
            {
                path = Path.Combine(this._installedExtension.InstallPath, path);

                try
                {
                    using (var stream = File.OpenRead(path))
                    {
                        var image = new BitmapImage();
                        image.BeginInit();
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.StreamSource = stream;
                        image.EndInit();

                        bitmap = image;
                        bitmap.Freeze();
                    }
                }
                catch
                {
                }
            }

            return bitmap;
        }

        private BitmapSource LoadIcon(string path)
        {
            var bitmap = this.LoadBitmap(path);

            if (bitmap == null)
            {
                // set default image
            }

            return bitmap;
        }
    }
}
