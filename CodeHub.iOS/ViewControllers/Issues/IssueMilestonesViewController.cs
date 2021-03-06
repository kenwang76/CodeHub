using CodeHub.Core.ViewModels.Issues;
using CodeHub.iOS.Views;
using UIKit;
using System;
using CodeHub.iOS.TableViewSources;
using ReactiveUI;
using System.Reactive.Linq;

namespace CodeHub.iOS.ViewControllers.Issues
{
    public class IssueMilestonesViewController : BaseTableViewController<IssueMilestonesViewModel>
    {
        public IssueMilestonesViewController()
        {
            EmptyView = new Lazy<UIView>(() =>
                new EmptyListView(Octicon.Milestone.ToEmptyListImage(), "There are no milestones."));
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.WhenAnyValue(x => x.ViewModel.Milestones)
                .Select(x => new IssueMilestoneTableViewSource(TableView, x))
                .BindTo(TableView, x => x.Source);
        }

        public static void Show(UIViewController parent, IssueMilestonesViewModel viewModel)
        {
            var viewController = new IssueMilestonesViewController { Title = "Milestones", ViewModel = viewModel };
            parent.NavigationController.PushViewController(viewController, true);
            viewController.ViewModel.DismissCommand.Subscribe(_ => parent.NavigationController.PopToViewController(parent, true));
        }
    }
}

