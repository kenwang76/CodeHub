using System;
using UIKit;
using System.Reactive.Linq;
using CoreGraphics;
using CodeHub.Core.ViewModels;
using CodeHub.iOS.TableViewSources;
using CodeHub.iOS.DialogElements;
using CodeHub.iOS.Views;

namespace CodeHub.iOS.ViewControllers
{
    public abstract class BaseDialogViewController<TViewModel> : BaseTableViewController<TViewModel> where TViewModel : class, IBaseViewModel
    {
        protected readonly SlideUpTitleView SlideUpTitle;
        protected readonly ImageAndTitleHeaderView HeaderView;
        private readonly UIView _backgroundHeaderView;
        private DialogTableViewSource _dialogSource;

        protected RootElement Root
        {
            get { return _dialogSource.Root; }
        }

        public override string Title
        {
            get
            {
                return base.Title;
            }
            set
            {
                HeaderView.Text = value;
                SlideUpTitle.Text = value;
                base.Title = value;
                RefreshHeaderView();
            }
        }

        public override UIRefreshControl RefreshControl
        {
            get { return base.RefreshControl; }
            set
            {
                value.Do(x => x.TintColor = Theme.PrimaryNavigationBarTextColor.ColorWithAlpha(0.8f));
                base.RefreshControl = value;
            }
        }

        protected BaseDialogViewController()
            : base(UITableViewStyle.Grouped)
        {
            SlideUpTitle = new SlideUpTitleView(44f) { Offset = 100f };
            NavigationItem.TitleView = SlideUpTitle;

            HeaderView = new ImageAndTitleHeaderView();

            _backgroundHeaderView = new UIView();

            Appearing
                .Where(x => ToolbarItems != null && NavigationController != null)
                .Subscribe(x => NavigationController.SetToolbarHidden(false, x));
            Disappearing
                .Where(x => ToolbarItems != null && NavigationController != null)
                .Subscribe(x => NavigationController.SetToolbarHidden(true, x));
            Disappearing
                .Where(_ => NavigationController != null)
                .Subscribe(_ => NavigationController.NavigationBar.ShadowImage = null);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            NavigationController.NavigationBar.ShadowImage = new UIImage();

            HeaderView.BackgroundColor = NavigationController.NavigationBar.BackgroundColor;
            HeaderView.TextColor = NavigationController.NavigationBar.TintColor;
            HeaderView.SubTextColor = NavigationController.NavigationBar.TintColor.ColorWithAlpha(0.8f);
            (SlideUpTitle.Subviews[0] as UILabel).TextColor = HeaderView.TextColor;
            _backgroundHeaderView.BackgroundColor = HeaderView.BackgroundColor;
        }

        public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
        {
            base.DidRotate(fromInterfaceOrientation);
            TableView.BeginUpdates();
            TableView.TableHeaderView = HeaderView;
            TableView.EndUpdates();
        }

        protected virtual DialogTableViewSource CreateTableViewSource()
        {
            return new DialogTableViewSource(TableView);
        }

        protected void RefreshHeaderView()
        {
            TableView.TableHeaderView = HeaderView;
            TableView.ReloadData();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _dialogSource = CreateTableViewSource();

            _dialogSource.ScrolledObservable.Where(x => x.Y > 0)
                .Where(_ => NavigationController != null)
                .Subscribe(_ => NavigationController.NavigationBar.ShadowImage = null);
            _dialogSource.ScrolledObservable.Where(x => x.Y <= 0)
                .Where(_ => NavigationController != null)
                .Where(_ => NavigationController.NavigationBar.ShadowImage == null)
                .Subscribe(_ => NavigationController.NavigationBar.ShadowImage = new UIImage());
            _dialogSource.ScrolledObservable.Where(_ => SlideUpTitle != null).Subscribe(x => SlideUpTitle.Offset = 108 + 28f - x.Y);

            TableView.TableHeaderView = HeaderView;
            TableView.SectionHeaderHeight = 0;
            TableView.Source = _dialogSource;

            var frame = TableView.Bounds;
            frame.Y = -frame.Size.Height;
            _backgroundHeaderView.Frame = frame;
            _backgroundHeaderView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
            _backgroundHeaderView.Layer.ZPosition = -1f;
            TableView.InsertSubview(_backgroundHeaderView, 0);
        }
    }
}
